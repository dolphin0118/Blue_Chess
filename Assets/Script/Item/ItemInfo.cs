using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class ItemInfo : MonoBehaviour
{
    public ItemAsset itemAsset;
    public GameObject ItemPrefab;
    public Sprite ItemImage;
    public ItemData ItemData;
    public string Name;

    void Start() {
        Name = this.transform.name;
        Name = Name.Replace("(Clone)","");
        string path = "Assets/Resources/Item/Scriptable/"+Name+".asset";
        itemAsset = AssetDatabase.LoadAssetAtPath<ItemAsset>(path);
    }

    void Update()
    {
        
    }
}
