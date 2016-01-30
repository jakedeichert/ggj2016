using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour{
    public GameObject itemMenu;

    void Start() {

    }

    public void BringUpItems(bool expression) {
        itemMenu.SetActive(expression);
    }
}
