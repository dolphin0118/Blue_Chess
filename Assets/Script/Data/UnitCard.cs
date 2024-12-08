using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueChessDataBase;

[GoogleSheet.Core.Type.UGS(typeof(AttackType))]
public enum AttackType {
    Explosion,
    Penetrate,
    Mystery
}

[GoogleSheet.Core.Type.UGS(typeof(AttackType))]
public enum DefenseType {
    Explosion,
    Penetrate,
    Mystery
}

[GoogleSheet.Core.Type.UGS(typeof(Synergy))]
public enum Synergy {
    Abydos,
    Gehenna,
    Trinity,
    Millennium,
    SRT,
    Engineer,
    Sniper,
    Slayer,
    Guardian,
    Mystery,
    Vanguard,
    Commander,
}

[GoogleSheet.Core.Type.UGS(typeof(Synergy))]
public enum SchoolSynergy {
    Abydos,
    Gehenna,
    Trinity,
    Millennium,
    SRT,
}

[GoogleSheet.Core.Type.UGS(typeof(Synergy))]
public enum TraitSynergy {
    Engineer,
    Sniper,
    Slayer,
    Guardian,
    Mystery,
    Vanguard,
    Commander,
}

public enum State {
    None,
    Idle,
    Move,
    Attack,
    Die
}


[CreateAssetMenu]
public class UnitCard : ScriptableObject {
    public GameObject UnitPrefab;
    public Sprite UnitMemorial;
    public UnitData UnitData;
    public UnitStat UnitStat;
    public string Name;
}
