using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Item {
    public int id;
}

public class ItemBehaviour : MonoBehaviour {
    public Item item;

    void PickupItem(PlayerBehaviour playerBehav) {
        playerBehav.SendMessage("AddItem", item);
        Destroy(this.gameObject);
    }
}
