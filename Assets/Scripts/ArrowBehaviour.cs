using System;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {
    public float speed = 20.0f;
    public float lifetime = 4.0f;

    private float timeElapsed;

    void Update() {
        transform.position += transform.right * speed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= lifetime) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        HittableBehaviour hittable = collider.transform.GetComponent<HittableBehaviour>();
        if (hittable != null) {
            hittable.Damage(10);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
    }
}
