using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaCombine : MonoBehaviour {
    static public Dictionary<int, int> CharaList = new Dictionary<int, int>();
    int Chara_Number = 100;
    int Combine_Count = 3;
    int Chara_MaxLevel = 3;

    void Start() {
        MapInit();
    }

    void Update() {
        MapUpdate();
    }

    void MapInit() {
        for(int i = 1; i <= Chara_Number * Chara_MaxLevel; i++) {
            CharaList.Add(i, 0);
        }
    }

    void MapUpdate() {
        for(int i = 1; i <= CharaList.Count; i++) {   
            if(CharaList[i] >= Combine_Count) {
                TeamManager.instance.Combine_Chara_s(i);//i 코드를 가진 캐릭을 2개 삭제, 1개 레벨 + 1
                int Player_Count = 0;
                CharaList.TryGetValue(i, out Player_Count);
                CharaList[i] = Player_Count - Combine_Count; 
            }
        }
    }


}
