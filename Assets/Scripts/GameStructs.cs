using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Item {
    public int id;
    public bool isPermanent;
}

public class InventoryEntry {
    public Item item;
    public int invenID;
    public int quantity;
}

public struct BuffInfo {
    public string buffType;
    public int buffAmount;
}
