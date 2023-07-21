using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaCombine : MonoBehaviour {
    static public Dictionary<int, int> CharaList = new Dictionary<int, int>();
    int Chara_Number = 100;
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
            if(CharaList[i] >= 3) {
                TeamManager.instance.Delete_Chara_s(i);//i 코드를 가진 캐릭을 2개 삭제, 1개 레벨 + 1
                int Player_count = 0;
                CharaList.TryGetValue(i, out Player_count);
                CharaList[i] = Player_count - 3;
            }
        }
    }


}
