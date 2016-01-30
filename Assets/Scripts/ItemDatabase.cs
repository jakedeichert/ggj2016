using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemEntry {
    public Sprite sprite;
    public string name;
    public int id;
}

public class ItemDatabase : MonoBehaviour {
    public List<ItemEntry> itemDatabase;
    public Sprite invalidItemSprite;
    public string invalidItemName = "None";

    void Awake() {
        if (itemDatabase == null)
            itemDatabase = new List<ItemEntry>();
    }

    public Sprite FindItemSprite(int id) {
        if (id < 0 || id >= itemDatabase.Count) return invalidItemSprite;
        for (int i = 0; i < itemDatabase.Count; i++) {
            if (itemDatabase[i].id == id) {
                return itemDatabase[i].sprite;
            }
        }
        return invalidItemSprite;
    }

    public string FindItemName(int id) {
        if (id < 0 || id >= itemDatabase.Count) return invalidItemName;
        for (int i = 0; i < itemDatabase.Count; i++) {
            if (itemDatabase[i].id == id) {
                return itemDatabase[i].name;
            }
        }
        return invalidItemName;
    }
}
