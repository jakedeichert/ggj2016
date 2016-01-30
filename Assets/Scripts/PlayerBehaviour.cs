using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
    public int health = 100;
    public float speed = 10;
    public float friction = 0.95f;

    public WeaponBehaviour weapon;
    private Vector3 movement;
    private bool isChargingWeapon;

	void Start () {
        movement = new Vector3(0,0,0);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ignore Player"), LayerMask.NameToLayer("Player"));
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

        if (weapon != null) {
            if (Input.GetMouseButtonDown(0)) {
                isChargingWeapon = true;
            }

            if (Input.GetMouseButton(0) && isChargingWeapon) {
                weapon.AddCharge(1.0f * Time.deltaTime);
            }

            if (Input.GetMouseButtonUp(0) && isChargingWeapon) {
                weapon.UseWeapon();
                isChargingWeapon = false;
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
