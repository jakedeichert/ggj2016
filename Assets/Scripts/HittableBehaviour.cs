using System;
using System.Collections.Generic;
using UnityEngine;

public class HittableBehaviour : MonoBehaviour {
    public int health;

    public void AddHealth(int healthDelta) {
        health += healthDelta;
        if (health <= 0) {
            health = 0;
            SendMessage("OnDead");
        }
        Debug.Log("Health: " + health);
    }

    public void Damage(int healthDelta) {
        healthDelta = Math.Abs(healthDelta);
        AddHealth(-healthDelta);
    }

    public void Replenish(int healthDelta) {
        healthDelta = Math.Abs(healthDelta);
        AddHealth(healthDelta);
    }
}
