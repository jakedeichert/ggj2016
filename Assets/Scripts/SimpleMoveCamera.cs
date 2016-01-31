using UnityEngine;
using System.Collections;

public class SimpleMoveCamera : MonoBehaviour {

    Vector2 preMouse;

	// Use this for initialization
	void Start () {
        preMouse = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButton(0)) {
            Vector2 deltaMouse = (Vector2)(Input.mousePosition) - preMouse;
            Camera.main.gameObject.transform.Translate(deltaMouse * 0.1f);
        }

        preMouse = Input.mousePosition;
	}
}
