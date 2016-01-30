using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    public Transform playerTarget;
    Vector2 velocity;
    bool shouldSeek = true;
    float maxVelocity;
    float seekDistance;
    float mass;


	void Start() {
        velocity = Vector2.zero;
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

        if (rayLength > 0) {
            float maxAvoidForce = 3.0f;
            Ray2D ray = new Ray2D((Vector2)transform.position + rayOriginOffset, dir);
            Debug.DrawRay(ray.origin, ray.direction * rayLength);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, rayLength);
            if (hit) {
                if (hit.collider.name == "skeleton") {
                    avoidance.x = ray.origin.x + velocity.x - hit.transform.position.x;
                    avoidance.y = ray.origin.y + velocity.y - hit.transform.position.y;
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
