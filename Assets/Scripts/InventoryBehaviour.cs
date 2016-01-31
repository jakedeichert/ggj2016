using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryBehaviour : MonoBehaviour {
    private List<InventoryEntry> inventoryList;
    private int currItemIndex;

    private int lastInvenIDAssigned;

    public ItemDispatcher itemDispatcher;

    private UnityEvent inventoryChangeEvent;

    public int CurrItemID {
        get {
            if (inventoryList != null && currItemIndex >= 0 && currItemIndex < inventoryList.Count) {
                return inventoryList[currItemIndex].item.id;
            }
            return -1;
        }
    }

    public UnityEvent InventoryChangeEvent {
        get {
            return inventoryChangeEvent;
        }
    }

    void Awake() {
        inventoryList = new List<InventoryEntry>();
        inventoryChangeEvent = new UnityEvent();
    }

    void Start() {
        lastInvenIDAssigned = 0;
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
        inEntry.invenID = lastInvenIDAssigned;
        lastInvenIDAssigned++;
        inventoryList.Add(inEntry);
        //If this is the first item player gets, select it automatically
        if (inventoryList.Count == 1) {
            SelectItem(0);
        }
        //If it's a permanent, use it immediately
        if (item.isPermanent) {
            UseItem(inEntry.invenID);
        }
        //Notify listeners
        inventoryChangeEvent.Invoke();
    }

    public InventoryEntry[] GetInventory() {
        return inventoryList.ToArray();
    }

    public void SelectItem(int index) {
        currItemIndex = index;
    }

    public void BindItem(int invenID, KeyCode keyCode) {

    }

    void UseItem(int invenID) {
        InventoryEntry useEntry = null;
        int index = -1;
        //Look for the index of the inventory entry
        for (int i = 0; i < inventoryList.Count; i++) {
            if (inventoryList[i].invenID == invenID) {
                useEntry = inventoryList[i];
                index = i;
            }
        }
        if (useEntry == null) return;
        //Get ID, check quantity and pass to dispatcher
        int useID = useEntry.item.id;
        if (useEntry.quantity > 0) {
            itemDispatcher.CallItemAction(useID, transform);
            //If non-permanent, remove a quantity from item
            if (!useEntry.item.isPermanent) {
                useEntry.quantity--;
                if (useEntry.quantity <= 0) {
                    RemoveItem(index);
                } else {
                    inventoryChangeEvent.Invoke();
                }
            }
        }
    }

    public void UseItemByIndex(int index) {
        if (index < 0 || index >= inventoryList.Count || inventoryList[index].item.isPermanent) return;
        UseItem(inventoryList[index].invenID);
    }

    public void RemoveItem(int index) {
        inventoryList.RemoveAt(index);
        if (inventoryList.Count <= 0) {
            currItemIndex = -1;
        }
        inventoryChangeEvent.Invoke();
    }
}
