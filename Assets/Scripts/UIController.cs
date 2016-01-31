using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour{
    public ItemListUIBehaviour itemListUI;
    public CurrentItemUIBehaviour currItemUI;

    public GameMain gameMain;

    void Start() {

    }

    //public void BringUpItems(bool expression) {
    //    if (itemListUI == null) return;
    //    itemListUI.gameObject.SetActive(expression);
    //    itemListUI.Populate();
    //}

    void ResetLevel() {
        gameMain.SendMessage("RestartLevel");
    }
}
