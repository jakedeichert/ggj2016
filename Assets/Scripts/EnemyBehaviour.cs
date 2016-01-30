using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour {
    public Transform playerTarget;
    public List<EnemyBehaviour> allEnemies;
    Vector2 velocity;
    bool shouldSeek;
    float maxVelocity;
    float avoidanceRadius;
    float seekDistance;
    float mass;


	void Start() {
        velocity = Vector2.zero;
        shouldSeek = true;
        avoidanceRadius = 3f;
        maxVelocity = 5.0f;
        seekDistance = 15.0f;
        mass = 20.0f;
	}
	
	void Update () {
        float distance = Vector3.Distance(playerTarget.transform.position, transform.position);
        if (distance < seekDistance && distance > 2) {
            seek(playerTarget.position);
        } else {
            velocity = Vector2.zero;
        }
    }

    void seek(Vector3 target) {
        Vector2 dir = target - transform.position;

        Vector2 desiredVelocity = dir.normalized * maxVelocity;
        Vector2 steering = desiredVelocity - velocity;
        steering += collisionAvoidance(new Vector2(0, 0), 1f, dir);
        steering += collisionAvoidance(new Vector2(0, 0), 1f, Quaternion.Euler(0,0, 60f) * dir);
        steering += collisionAvoidance(new Vector2(0, 0), 1f, Quaternion.Euler(0, 0, -60f) * dir);
        float maxForce = 1000.0f;
        steering = truncate(steering, maxForce);
        steering = steering / mass;

        velocity = truncate(velocity + steering, maxVelocity); 

        move();
    }

    Vector2 collisionAvoidance(Vector2 rayOriginOffset, float rayLength, Vector2 dir) {
        Vector2 avoidance = Vector2.zero;

        for (int i = 0; i < allEnemies.Count; i++) {
            if (this != allEnemies[i]) {
                float distance = Vector2.Distance(transform.position, allEnemies[i].transform.position);
                if (distance < avoidanceRadius) {
                    float maxAvoidForce = 3.0f;
                    avoidance.x = transform.position.x + velocity.x - allEnemies[i].transform.position.x;
                    avoidance.y = transform.position.y + velocity.y - allEnemies[i].transform.position.y;
                    avoidance.Normalize();
                    avoidance *= maxAvoidForce;
                }
            }
        }

        return avoidance;
    }

    Vector3 truncate(Vector3 v, float max) {
        return (v.magnitude > max) ? v.normalized * max : v;
    }


    void move() {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
