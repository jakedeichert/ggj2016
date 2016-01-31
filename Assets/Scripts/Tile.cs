using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public int gridX = 0, gridY = 0;

    public bool isWall = true;
    public bool empty = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetGridPos(int _x, int _y) {
        gridX = _x;
        gridY = _y;
    }
}
