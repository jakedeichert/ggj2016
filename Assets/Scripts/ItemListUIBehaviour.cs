using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemListUIBehaviour : MonoBehaviour {
    List<Image> itemBoxList;
    List<Image> itemImageList;
    public InventoryBehaviour inventoryBehav;
    public ItemDatabase itemDatabase;

    public GameObject baseItemBoxPrefab;
    public GameObject baseItemImagePrefab;

    void Awake() {
        itemBoxList = new List<Image>();
        itemImageList = new List<Image>();
    }

    public void Populate() {
        //Grab the inventory array from inventory behaviour
        InventoryEntry[] inventoryArr = inventoryBehav.GetInventory();
        //Make sure there's enough boxes allocated for each item in inventory
        if (itemBoxList.Count > inventoryArr.Length) {
            //Remove excess boxes
            for (int i = 0; i < itemBoxList.Count; i++) {
                GameObject.Destroy(itemBoxList[i].gameObject);
                GameObject.Destroy(itemImageList[i].gameObject);
            }
            itemBoxList.RemoveRange(inventoryArr.Length, itemBoxList.Count - inventoryArr.Length);
            itemImageList.RemoveRange(inventoryArr.Length, itemBoxList.Count - inventoryArr.Length);
        } else if (itemBoxList.Count < inventoryArr.Length) {
            //Add more boxes to accomodate more inventory items than before
            for (int i = 0; i < inventoryArr.Length - itemBoxList.Count; i++) {
                GameObject itemBox = GameObject.Instantiate<GameObject>(baseItemBoxPrefab);
                GameObject itemImage = GameObject.Instantiate<GameObject>(baseItemImagePrefab);
                itemImage.transform.SetParent(itemBox.transform);
                itemBox.transform.SetParent(transform);
                Debug.Log("Item box width: " + itemBox.GetComponent<Image>().sprite.rect.width);
                itemBox.transform.position = new Vector3(32 + (itemBox.GetComponent<Image>().sprite.rect.width * i), 32, 0);
                itemBoxList.Add(itemBox.GetComponent<Image>());
                itemImageList.Add(itemImage.GetComponent<Image>());
            }
        }
        //Grab all infos
        for (int i = 0; i < inventoryArr.Length; i++) {
            itemImageList[i].sprite = itemDatabase.FindItemSprite(inventoryArr[i].item.id);
        }
    }

    void Update() {

    }
}
