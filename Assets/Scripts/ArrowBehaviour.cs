using System;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour {
    public float speed = 20.0f;
    public float lifetime = 2.0f;

    private float timeElapsed;

    void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= lifetime) {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Hit something!");
        HittableBehaviour hittable = collider.transform.GetComponent<HittableBehaviour>();
        if (hittable != null) {
            hittable.Damage(10);
        }
        Destroy(this.gameObject);
    }
}
