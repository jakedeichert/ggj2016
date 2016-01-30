using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
    public int health = 100;
    public float speed = 10;
    public float friction = 0.95f;

    public WeaponBehaviour weapon;

    private Vector3 movement;

	void Start () {
        movement = new Vector3(0,0,0);
	}
	
	void Update () {
        if (Input.GetKey(KeyCode.A)) {
            movement.x = -speed;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement.x = speed;
        }
        if (Input.GetKey(KeyCode.W)) {
            movement.y = speed;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement.y = -speed;
        }

        if (Input.GetMouseButtonDown(0)) {
            if (weapon != null) {
                weapon.UseWeapon();
            }
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(10, 0, 0));
        if (weapon != null) {
            Vector3 weaponForward = (mousePos - transform.position).normalized;
            weapon.transform.position = transform.position + (weaponForward * 4);
        }

        transform.position += movement * Time.deltaTime;
        movement.x *= friction;
        movement.y *= friction;
	}
}
