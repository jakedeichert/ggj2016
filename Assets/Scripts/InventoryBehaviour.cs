using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryEntry {
    public Item item;
    public int quantity;
}

public class InventoryBehaviour : MonoBehaviour {
    private List<InventoryEntry> inventoryList;
    private int currItemIndex;

    public ItemDispatcher itemDispatcher;

    private UnityEvent inventoryChangeEvent;

    public UnityEvent InventoryChangeEvent {
        get {
            return inventoryChangeEvent;
        }
    }

    public int CurrItemIndex {
        get {
            return currItemIndex;
        }
    }

    void Awake() {
        inventoryList = new List<InventoryEntry>();
        inventoryChangeEvent = new UnityEvent();
    }

    void Start() {
        currItemIndex = -1;
    }

    public void AddItem(Item item) {
        //Looks to see if item exists in list
        for (int i = 0; i < inventoryList.Count; i++) {
            if (inventoryList[i].item.id == item.id) {
                //It exists already, just increment quantity by 1
                inventoryList[i].quantity++;
                return;
            }
        }
        //If there's no item like this in the list, create a new entry for it and set quantity to 1
        InventoryEntry inEntry = new InventoryEntry();
        inEntry.item = item;
        inEntry.quantity = 1;
        inventoryList.Add(inEntry);
        //If this is the first item player gets, select it automatically
        if (inventoryList.Count == 1) {
            SelectItem(0);
        }
        inventoryChangeEvent.Invoke();
    }

    public InventoryEntry[] GetInventory() {
        return inventoryList.ToArray();
    }

    public void SelectItem(int index) {
        currItemIndex = index;
    }

    public void UseItem(int index) {
        if (index < 0 || index >= inventoryList.Count) return;
        InventoryEntry useEntry = inventoryList[index];
        int useID = useEntry.item.id;
        if (useEntry.quantity > 0) {
            itemDispatcher.CallItemAction(useID, transform);
            useEntry.quantity--;
            if (useEntry.quantity <= 0) {
                RemoveItem(index);
            } else {
                inventoryChangeEvent.Invoke();
            }
        }
    }

    public void RemoveItem(int index) {
        inventoryList.RemoveAt(index);
        if (inventoryList.Count <= 0) {
            currItemIndex = -1;
        }
        inventoryChangeEvent.Invoke();
    }
}
