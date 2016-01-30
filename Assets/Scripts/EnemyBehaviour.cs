using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    public Transform playerTarget;
    Vector3 velocity;
    float maxVelocity;
    float seekDistance;
    float mass;


	void Start() {
        velocity = Vector3.zero;
        maxVelocity = 10.0f;
        seekDistance = 10.0f;
        mass = 20.0f;
        Debug.Log(playerTarget.transform.position.y);
	}
	
	void Update () {
        float distance = Vector3.Distance(playerTarget.transform.position, transform.position);
        if (distance < seekDistance && distance > 2) {
            seek(playerTarget.position);
        }
    }

    void seek(Vector3 target) {
        Vector3 dir = target - transform.position;
        dir.z = 0;

        Vector3 desiredVelocity = dir.normalized * maxVelocity;
        Vector3 steering = desiredVelocity - velocity;
        //steering += collisionAvoidance(new Vector3(0, 3, 0), 1.6f, dir);
        //steering += collisionAvoidance(new Vector3(0, 3, 0) + self.transform.right * 2.5f, 1.3f, dir);
        //steering += collisionAvoidance(new Vector3(0, 3, 0) - self.transform.right * 2.5f, 1.3f, dir);
        float maxForce = 1000.0f;
        steering = truncate(steering, maxForce);
        steering = steering / mass;

        velocity = truncate(velocity + steering, maxVelocity); 
        velocity.z = 0;
        Debug.Log(velocity.magnitude);

        move();
    }

    //Vector3 collisionAvoidance(Vector3 rayOriginOffset, float rayLengthScale, Vector3 dir) {
    //    Vector3 avoidance = Vector3.zero;
    //    float rayLength = 10.0f * rayLengthScale;

    //    if (rayLength > 0) {
    //        float maxAvoidForce = 120.0f;
    //        RaycastHit hit;
    //        Ray ray = new Ray(self.transform.position + rayOriginOffset, dir);
    //        Debug.DrawRay(ray.origin, ray.direction * rayLength);

    //        if (Physics.Raycast(ray, out hit, rayLength)) {
    //            if (hit.collider.name == "tree collider") {
    //                avoidance.x = ray.origin.x + velocity.x - hit.transform.position.x;
    //                avoidance.z = ray.origin.z + velocity.z - hit.transform.position.z;
    //                avoidance.Normalize();
    //                avoidance *= maxAvoidForce;
    //            }
    //        }
    //    }

    //    return avoidance;
    //}

    Vector3 truncate(Vector3 v, float max) {
        return (v.magnitude > max) ? v.normalized * max : v;
    }


    void move() {
        transform.position += velocity * Time.deltaTime;
    }
}
