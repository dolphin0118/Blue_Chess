using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TeamManager : MonoBehaviour {
    public static TeamManager instance = null;
    GameObject[] Chara_s;  
    CharaInfo[] CharaInfo_s;
    int Level_gap = 100;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
                    SpawnManager.instance.Destroy_Chara(Chara_s[i]);
                }
            }
        }
    }

    public void Battle_Chara_s() {
        Chara_s = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < Chara_s.Length; i++) {
            TileBase Chara_Tile = Chara_s[i].GetComponent<CharaLocate>().Player_Tile();
            if(Chara_Tile.name == MapManager.instance.BattleTile.name) {
                Chara_s[i].GetComponent<CharaController>().isBattle = true;
            }
            else Chara_s[i].GetComponent<CharaController>().isBattle = false;
        }      
    }
    
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)) {
            Battle_Chara_s();
        }
    }
}
