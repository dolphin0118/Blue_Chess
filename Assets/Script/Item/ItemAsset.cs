using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ItemData {
    public string Name;
    public float HP;
    public float MP;
    public float AR;
    public float MR;
}

[CreateAssetMenu]
public class ItemAsset : ScriptableObject {
    public GameObject ItemPrefab;
    public Sprite ItemImage;
    public ItemData ItemData;
    public string Name;
}

