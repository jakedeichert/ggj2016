using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HittableBehaviour))]
public class PlayerBehaviour : MonoBehaviour {
    public float speed = 10;
    public float friction = 0.1f;

    public WeaponBehaviour weapon;
    private HittableBehaviour hittable;
    private InventoryBehaviour inventory;
    private Vector3 movement;
    private bool isChargingWeapon;

	void Start () {
        movement = new Vector3(0,0,0);
        hittable = GetComponent<HittableBehaviour>();
        inventory = GetComponent<InventoryBehaviour>();
        //TODO remove
        Item potion;
        potion.id = 0;
        if (inventory != null) inventory.AddItem(potion);
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

        if (Input.GetKeyDown(KeyCode.E)) {
            if (inventory != null) {
                inventory.UseItem(0);
            }
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

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Item") {
            collider.SendMessage("PickupItem", this);
        }
    }
}
