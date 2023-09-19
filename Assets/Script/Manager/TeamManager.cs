using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
public class TeamManager : MonoBehaviour {
    public static TeamManager instance = null;
    bool[] charaCheck = new bool[100]; 
    GameObject[] Chara_s;  
    int Level_gap = 100;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }     
    }

    public void Combine_Chara_s(int Compare_Code) {
        Chara_s = GameObject.FindGameObjectsWithTag("Friendly");
        int Delete_Count = 0;
        for(int i = 0; i < Chara_s.Length; i++) {
            CharaInfo getCharaInfo = Chara_s[i].GetComponent<CharaInfo>(); 
            int Player_Code = getCharaInfo.Player_Code;
            if(Compare_Code == Player_Code) {
                if(Delete_Count >= 2) {
                    getCharaInfo.Player_Level += 1;
                    getCharaInfo.Player_Code += Level_gap;
                    getCharaInfo.Init();
                    break;
                }
                else {
                    Delete_Count++;
                    Destroy(Chara_s[i]);
                }
            }
        }
    }
    
    public void SynersyChara_s() {  
        GameObject battleCharas = GameObject.FindGameObjectWithTag("BattleArea");
        int charaCount = battleCharas.transform.childCount;
        charaCheck = Enumerable.Repeat(false , 100).ToArray();
        SynergyManager.instance.SynergyList.Clear();
        
        for (int i = 0; i < charaCount; i++) {
            CharaInfo charaInfo = battleCharas.transform.GetChild(i).GetComponent<CharaInfo>();
            int Player_Code = charaInfo.Player_Code;
            while(Player_Code > 100) Player_Code -= 100;
            if(!charaCheck[Player_Code]) {
                charaCheck[Player_Code] = true;
                SynergyManager.instance.SynergyList.AddRange(charaInfo.CharaSynergy);
            }
        }

    }

    void Update(){
        SynersyChara_s();
    }
}
