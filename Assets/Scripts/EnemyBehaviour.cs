using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    public Transform playerTarget;
    Vector3 movement;


	void Start() {
        Debug.Log(playerTarget.transform.position.y);
	}
	
	void Update () {
        float distance = Vector3.Distance(playerTarget.transform.position, transform.position);

        if (distance < 10) {
            Debug.Log("close" + distance);

            float speed = 3.0f;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, step);
        }
    }
}
