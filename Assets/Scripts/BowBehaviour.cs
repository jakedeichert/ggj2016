using System;
using System.Collections.Generic;
using UnityEngine;

public class BowBehaviour : WeaponBehaviour {
    public GameObject arrow;
    public Transform player;

    public override void UseWeapon() {
        player = transform.parent; //TODO figure out if weapon will know about player after all
        GameObject arrowClone = (GameObject)GameObject.Instantiate(arrow, transform.position, Quaternion.identity);
        arrowClone.transform.forward = (transform.position - player.position).normalized;
    }

    void Update() {
        Vector3 norTar = (transform.localPosition).normalized;
        float angle = Mathf.Atan2(norTar.y, norTar.x) * Mathf.Rad2Deg;
        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = newRotation;
    }
}
