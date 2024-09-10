using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance = null; 
    public GameObject[] ItemSlots;

    private void Awake() {
        if (instance == null) {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject); 
        }
         
    }

    private void Start() {
        SetItemSlots();       
    }

    private void SetItemSlots() {
        ItemSlots = new GameObject[7];
        for (int i = 0; i < this.transform.childCount; i++)
        {
            ItemSlots[i] = this.transform.GetChild(i).gameObject;
        }
    }
    public void AddItem(ItemAsset item) {
        foreach (var slot in ItemSlots) {
            if(slot.transform.childCount == 0) {
                GameObject itemPrefab = Instantiate(item.ItemPrefab);
                itemPrefab.transform.SetParent(slot.transform, false);
                return;
            }
        }
    }


}