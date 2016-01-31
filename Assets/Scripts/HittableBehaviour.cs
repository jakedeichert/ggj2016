using System;
using System.Collections.Generic;
using UnityEngine;

public class HittableBehaviour : MonoBehaviour {
    public int health;
    public int maxHealth = 100;
    bool isDead;

    public void AddHealth(int healthDelta) {
        if (isDead) return;
        health += healthDelta;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (health <= 0) {
            health = 0;
            SendMessage("OnDead");
        }
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
