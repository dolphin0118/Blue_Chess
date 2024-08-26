using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitItem : MonoBehaviour
{
    List<ItemAsset> items;
    Image[] itemImages;

    void Start()
    {
        itemImages = GetComponentsInChildren<Image>();
        items = new List<ItemAsset>();
    }

    // Update is called once per frame
    void Update() {
        SetViewItems();
    }

    public void SetViewItems() {
        for(int i = 0; i < items.Count ; i++) {
            Debug.Log(items[i].ItemImage);
            itemImages[i].sprite = items[i].ItemImage;
        }
    }

    public void AddItem(ItemAsset getItem) {
        if(items.Count < 3) {
            items.Add(getItem);
        }
    }

    private void OnDestroy() {
        foreach (var item in items) {
            Debug.Log(item.Name);
            ItemManager.instance.AddItem(item);
        }
        
    }
}
