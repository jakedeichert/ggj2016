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
}
