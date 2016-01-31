using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour {
    public Item item;

    void PickupItem(PlayerBehaviour playerBehav) {
        playerBehav.SendMessage("AddItem", item);
        Destroy(this.gameObject);
    }
}
