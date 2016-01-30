using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HealthUIBehaviour : MonoBehaviour {
    public HittableBehaviour playerHittable;
    Text uiText;

    void Start() {
        uiText = GetComponent<Text>();
    }

    void Update() {
        uiText.text = "Health: " + playerHittable.health;
    }
}
