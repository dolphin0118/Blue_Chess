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
public enum SubSynergy {
    Engineer,
    Sniper,
    Slayer,
    guardian,
    mystery
}

public class State {
    public enum state {
        Idle,
        Move,
        Attack,
        Die
    }
}

[CreateAssetMenu]
public class Chara : ScriptableObject{
    public GameObject Chara_Prefab;
    public Sprite Chara_Card;
    public string CharaSynergy;
}
