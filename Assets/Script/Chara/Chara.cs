using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Synergy {
    Abydos,
    Gehenna,
    Trinity,
    millennium,
}
 [CreateAssetMenu]
public class Chara : ScriptableObject{
    public GameObject Chara_Prefab;
    public Sprite Chara_Card;
    public string CharaSynergy;
}
