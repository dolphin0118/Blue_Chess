using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Synergy{
    Abydos,
    Gehenna,
    Trinity,
    millennium,
    guardian,
}

[CreateAssetMenu]
public class Chara : ScriptableObject{
    public GameObject Chara_Prefab;
    public Sprite Chara_Card;
    public string CharaSynergy;
}
