using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
public class TeamManager : MonoBehaviour {
    public static TeamManager instance = null;
    bool[] charaCheck = new bool[100]; 
    GameObject[] Chara_s;  
    CharaInfo[] CharaInfo_s;
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
        Chara_s = GameObject.FindGameObjectsWithTag("Player");
        CharaInfo_s = GetComponentsInChildren<CharaInfo>(); 
        int Delete_Count = 0;
        for(int i = 0; i < CharaInfo_s.Length; i++) {
            int Player_Code = CharaInfo_s[i].Player_Code;
            if(Compare_Code == Player_Code) {
                if(Delete_Count >= 2) {
                    CharaInfo_s[i].Player_Level += 1;
                    CharaInfo_s[i].Player_Code += Level_gap;
                    CharaInfo_s[i].Init();
                    break;
                }
                else {
                    Delete_Count++;
                    Destroy(Chara_s[i]);
                    //SpawnSystem.instance.Destroy_Chara(Chara_s[i]);
                }
            }
        }
    }
    public void SynersyChara_s() {  
        Chara_s = GameObject.FindGameObjectsWithTag("Player");
        CharaInfo_s = GetComponentsInChildren<CharaInfo>();
        charaCheck = Enumerable.Repeat(false , 100).ToArray();
        SynergyManager.instance.SynergyList.Clear();
        
        for (int i = 0; i < CharaInfo_s.Length; i++) {
            int Player_Code = CharaInfo_s[i].Player_Code;
            while(Player_Code > 100) Player_Code -= 100;
            if(!charaCheck[Player_Code]) {
                charaCheck[Player_Code] = true;
                SynergyManager.instance.SynergyList.Add(CharaInfo_s[i].CharaSynergy);
            }
        }

    }

    public void BattleChara_s() {
        Chara_s = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < Chara_s.Length; i++) {
            TileBase Chara_Tile = Chara_s[i].GetComponent<CharaLocate>().Player_Tile();
            if(Chara_Tile.name == MapManager.instance.BattleTile.name) {
                Chara_s[i].GetComponent<CharaController>().isBattle = true;
            }
            else Chara_s[i].GetComponent<CharaController>().isBattle = false;
        }      
    }
    public void BenchChara_s() {
        
    }

    void Update(){
        SynersyChara_s();
        if(Input.GetKeyDown(KeyCode.Space)) {
            BattleChara_s();
        }
    }
}
