using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentItemUIBehaviour : MonoBehaviour {
    public Image backImage;
    public Image frontImage;
    public InventoryBehaviour inventoryBehav;
    public ItemDatabase itemDatabase;

    void Start() {
        if (inventoryBehav != null) {
            inventoryBehav.InventoryChangeEvent.AddListener(Refresh);
        }
    }

    void Refresh() {
        if (inventoryBehav != null && backImage != null && frontImage != null && itemDatabase != null) {
            frontImage.sprite = itemDatabase.FindItemSprite(inventoryBehav.CurrItemID);
        }
    }
}