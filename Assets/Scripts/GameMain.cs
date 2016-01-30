using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMain : MonoBehaviour {
    public Boid prefabBoid;
    public Transform enemiesHolder;
    List<Boid> allBoids = new List<Boid>();
    int numBoids = 200;
    bool isDebug = true;



    // Use this for initialization
    void Start() {
        // Create boids.
        for (int i = 0; i < numBoids; i++) {
            Boid boid = Instantiate(prefabBoid) as Boid;
            boid.transform.parent = enemiesHolder;
            allBoids.Add(boid);
        }

        reset();
    }

    void OnDrawGizmosSelected() {
        if (isDebug) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(allBoids[0].transform.position, allBoids[0].getNeighborDistance());
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(allBoids[0].transform.position, allBoids[0].getDesiredSeparation());
        }
    }

    void Update() {
        // Update each boid.
        foreach (Boid b in allBoids) {
            b.updateFlock(allBoids);
        }
    }



    void reset() {
        // Randomize position and rotation for forward moving direction.
        foreach (Boid b in allBoids) {
            b.transform.position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0);
            b.location = b.transform.position;
            b.velocity = Vector2.zero;
        }
    }
}
