using System;
using System.Collections.Generic;
using UnityEngine;

public class BowBehaviour : WeaponBehaviour {
    public GameObject arrow;
    public Transform player;
    float angle;

    public override void UseWeapon(int attackBoost = 1) {
        player = transform.parent; //TODO figure out if weapon will know about player after all
        GameObject arrowClone = (GameObject)Instantiate(arrow, transform.position, Quaternion.identity);
        arrowClone.transform.right = direction;
        ArrowBehaviour arrowBehav = arrowClone.GetComponent<ArrowBehaviour>();
        arrowBehav.speed = 20.0f * charge;
        arrowBehav.attackBoost = attackBoost;
        base.UseWeapon();
    }

    void Update() {
        
    }

    protected override void PlayChargedAnimation() {
        //Debug.Log("Will play bow's charged animation.");
    }

    protected override void StopChargedAnimation() {
        //Debug.Log("Will stop charged animation.");
    }
}
