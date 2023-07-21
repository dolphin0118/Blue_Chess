using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {
    public static TeamManager instance = null;
    GameObject[] Chara_s;
    CharaInfo[] CharaInfo_s;    

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }     
    }

    public void Delete_Chara_s(int Compare_Code) {
        Chara_s = GameObject.FindGameObjectsWithTag("Player");
        CharaInfo_s = GetComponentsInChildren<CharaInfo>(); 
        int Delete_Count = 0;
        for(int i = 0; i < CharaInfo_s.Length; i++) {
            int Player_Code = CharaInfo_s[i].Player_Code;
            if(Compare_Code == Player_Code) {
                if(Delete_Count >= 2) {
                    CharaInfo_s[i].Player_Level += 1;
                    CharaInfo_s[i].Player_Code += 100;
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
    void Update(){
        
    }
}
