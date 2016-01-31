using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour {
    protected float charge = 0.6f;
    private const float MAX_CHARGE = 5.0f;
    protected Vector3 direction;

    public Vector3 Direction {
        get {
            return direction;
        }
        set {
            direction = value;
        }
    }

    public void AddCharge(float chargeDelta) {
        if (charge < MAX_CHARGE) {
            charge += Time.deltaTime;
            if (charge >= MAX_CHARGE) {
                PlayChargedAnimation();
            }
        }
    }
    public virtual void UseWeapon() {
        UnchargeWeapon();
    }
    public void UnchargeWeapon() {
        charge = 0.6f;
        StopChargedAnimation();
    }
    protected virtual void PlayChargedAnimation() { }
    protected virtual void StopChargedAnimation() { }
}
