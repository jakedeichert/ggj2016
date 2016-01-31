using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HittableBehaviour))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerBehaviour : MonoBehaviour {
    public int speed = 10;
    public int attackBoost = 1;
    public float friction = 0.95f;

    public WeaponBehaviour weapon;
    public UIController uiController;

    private HittableBehaviour hittable;
    private InventoryBehaviour inventory;
    private Rigidbody2D rBody;
    private SpriteRenderer spriteRenderer;
    private Vector3 movement;
    private bool isChargingWeapon;

	void Start () {
        movement = new Vector3(0,0,0);
        hittable = GetComponent<HittableBehaviour>();
        inventory = GetComponent<InventoryBehaviour>();
        rBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (inventory != null) {
                inventory.UseItemByIndex(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            if (inventory != null) {
                inventory.UseItemByIndex(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            if (inventory != null) {
                inventory.UseItemByIndex(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            if (inventory != null) {
                inventory.UseItemByIndex(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            if (inventory != null) {
                inventory.UseItemByIndex(4);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            if (inventory != null) {
                inventory.UseItemByIndex(5);
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
                weapon.UseWeapon(attackBoost);
                isChargingWeapon = false;
            }
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(10, 0, 0));
        if (weapon != null) {
            Vector3 weaponForward = (mousePos - transform.position).normalized;
            weaponForward.z = 0;
            WeaponBehaviour weaponBehav = weapon.GetComponent<WeaponBehaviour>();
            if (weaponBehav != null) {
                weaponBehav.Direction = weaponForward;
            }
            Vector3 weaponForwardNor = (weaponForward).normalized;
            float angle = Mathf.Atan2(weaponForwardNor.y, weaponForwardNor.x) * Mathf.Rad2Deg;
            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(0, 0, angle);
            weapon.transform.rotation = newRotation;
            if (newRotation.eulerAngles.z > 90 && newRotation.eulerAngles.z < 270) {
                FlipDirection(-1);
            } else {
                FlipDirection(1);
            }
        }

        Vector3 newPos = transform.position + movement * Time.deltaTime;
        rBody.MovePosition(newPos);
        movement.x *= friction;
        movement.y *= friction;
	}

    void FlipDirection(int direction) {
        if (direction >= 0) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }

    void ApplyBuff(BuffInfo buffInfo) {
        switch (buffInfo.buffType) {
            case "Speed":
                speed += buffInfo.buffAmount;
                break;
            case "Health":
                hittable.maxHealth += buffInfo.buffAmount;
                break;
            case "Attack":
                attackBoost += buffInfo.buffAmount;
                break;
            default:
                break;
        }
    }

    void OnDead() {
        Debug.Log("Player is dead!");
        uiController.SendMessage("ResetLevel");
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Item") {
            collider.SendMessage("PickupItem", this);
        }
    }
}
