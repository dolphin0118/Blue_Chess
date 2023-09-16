using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaInfo : MonoBehaviour { 
    public Synergy[] CharaSynergy;
    public float Player_Attack_count = 0.5f;

    public float Player_Hp = 100;
    public float Player_Mp = 100;
    public float Player_ATK = 50;
    
    public float Player_Speed = 1.0f;
    public float Player_Range = 1.0f;

    public int Player_Level = 0;
    public int Player_Code = 1;

    void Start() {
        Init();
    }

    public void Init() {
        LevelInit();
        StatInit();
    }

    void LevelInit() {
        int Player_count = 0;
        CharaCombine.CharaList.TryGetValue(Player_Code, out Player_count);
        CharaCombine.CharaList[Player_Code] = Player_count + 1;
    }

    void StatInit() {
        if(Player_Level != 0) {
            Player_Hp += Player_Hp * 0.8f;
            Player_ATK += Player_ATK * 0.5f;
        }
    }

}
