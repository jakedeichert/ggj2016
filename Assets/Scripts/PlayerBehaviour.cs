using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
    Vector3 movement;
    public float speed = 10;

	void Start () {
        movement = new Vector3(0,0,0);
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            movement.x = -speed;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            movement.x = speed;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            movement.y = speed;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            movement.y = -speed;
        }

        transform.position += movement * Time.deltaTime;
        movement.x *= 0.95f;
        movement.y *= 0.95f;
	}
}
