using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Boid : MonoBehaviour {
    float neighborDistance;
    float desiredSeparation;
    // Maximum speed.
    float maxSpeed = 0.1f;
    // Maximum steering force.
    float maxForce = 0.12f;
    //float tWander = 10.0f;
    Vector2 acceleration;
    public Vector2 velocity;
    public Vector2 location;

    Vector2 limit(Vector2 v, float max) {
        Vector2 l = v;
        if (l.magnitude > max) {
            l.Normalize();
            l *= max;
        }
        return l;
    }


    void Start() {
        acceleration = new Vector2(0, 0);
        velocity = new Vector2(Random.Range(0, 2) * 2 - 1, Random.Range(0, 2) * 2 - 1);
        location = new Vector2(transform.position.x, transform.position.y);
    }

    void Update() {
    }


    void seek(Vector2 target) {
        acceleration += steer(target);
    }


    void arrive(Vector2 target) {
        acceleration += steer(target);
    }


    public void updateFlock(List<Boid> boids) {
        neighborDistance = 12.0f;
        desiredSeparation = 5.0f;

        Vector2 s = separate(boids);
        Vector2 a = align(boids);
        Vector2 c = cohere(boids);

        s *= 3.2f;
        a *= 1.8f;
        c *= 1.0f;

        acceleration += s;
        acceleration += a;
        acceleration += c;

        velocity += acceleration;

        velocity = limit(velocity, maxSpeed);

        location += velocity;
        transform.position = location;



        //} else if (isFollowLeader) {
        //    tWander += Time.deltaTime;
        //    if (tWander >= 2.0f) {
        //        arrive(new Vector2(Random.Range(0.0f, 120.0f), Random.Range(0.0f, 120.0f)));
        //        velocity = acceleration;
        //        velocity *= 5.0f;
        //        tWander = 0;
        //    }
        //    location += velocity;
        //    transform.position = location;

        acceleration = new Vector2(0, 0);
    }


    // Separate the boids.
    Vector2 separate(List<Boid> boids) {
        Vector2 sum = new Vector2(0, 0);
        int numCloseBoids = 0;

        // Check if any other boids are too close.
        for (int i = 0; i < boids.Count; i++) {
            Boid b = boids[i];

            float distance = Vector2.Distance(location, b.location);

            // Find close boids.
            if ((distance > 0) && (distance < desiredSeparation)) {
                Vector2 difference = location - b.location;
                difference.Normalize();
                difference /= distance;
                sum += difference;
                numCloseBoids++;
            }
        }

        // Calculate average based on number of boids.
        if (numCloseBoids > 0) {
            sum /= (float)numCloseBoids;
        }

        return sum;
    }


    // Align the boids.
    Vector2 align(List<Boid> boids) {
        Vector2 sum = new Vector2(0, 0);
        int numCloseBoids = 0;

        for (int i = 0; i < boids.Count; i++) {
            Boid b = boids[i];
            float distance = Vector2.Distance(location, b.location);

            // Find close boids.
            if ((distance > 0) && (distance < neighborDistance)) {
                sum += b.velocity;
                numCloseBoids++;
            }

        }

        // Calculate the average velocity of all nearby boids.
        if (numCloseBoids > 0) {
            sum /= (float)numCloseBoids;
            sum = limit(sum, maxForce);
        }

        return sum;
    }


    // Cohere the boids together.
    Vector2 cohere(List<Boid> boids) {
        Vector2 sum = new Vector2(0, 0);
        int numCloseBoids = 0;

        for (int i = 0; i < boids.Count; i++) {
            Boid b = boids[i];
            float distance = Vector2.Distance(location, b.location);

            // Find close boids.
            if ((distance > 0) && (distance < neighborDistance)) {
                sum += b.location;
                numCloseBoids++;
            }
        }

        // Calculate the average based on the nearby boids.
        if (numCloseBoids > 0) {
            sum /= (float)numCloseBoids;
            // Steer the boids in the direction of the sum.
            return steer(sum);
        }

        return sum;
    }



    // Calculate the steering velocity based on a target.
    Vector2 steer(Vector2 target) {
        Vector2 steer;
        Vector2 desired = target - location;
        float distance = desired.magnitude;

        if (distance > 0) {
            desired.Normalize();
            steer = desired - velocity;
            steer = limit(steer, maxForce);
        } else {
            steer = new Vector2(0, 0);
        }
        return steer;
    }



    public float getNeighborDistance() {
        return neighborDistance;
    }


    public float getDesiredSeparation() {
        return desiredSeparation;
    }

}
