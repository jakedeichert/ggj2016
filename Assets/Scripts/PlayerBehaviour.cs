﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HittableBehaviour))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerBehaviour : MonoBehaviour {
    public int speed = 10;
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

        if (Input.GetKeyDown(KeyCode.E)) {
            if (inventory != null) {
                inventory.UseItemByIndex(0);
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
            Vector3 norTar = (weapon.transform.localPosition).normalized;
            float angle = Mathf.Atan2(norTar.y, norTar.x) * Mathf.Rad2Deg;
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
            default:
                break;
        }
    }

    void OnDead() {
        Debug.Log("Player is dead!");
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Item") {
            collider.SendMessage("PickupItem", this);
        }
    }
}
