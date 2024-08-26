
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int Level1 = 0;
    public int Level2 = 0;
    public int Level3 = 0;
}

public class CharaCombine : MonoBehaviour {
    const int CombineCount = 3;

    public void CombineListUpdate(string charaName) {
        LevelData levelData = TeamManager.CharaLevel[charaName];
        int combineLevel = 0;

        if(levelData.Level1 >= CombineCount) {
            combineLevel = 1;
            CombineChara(charaName, combineLevel);//LEVEL_1의 같은 이름 캐릭을 2개 삭제, 1개 레벨 + 1
            TeamManager.CharaLevel[charaName].Level1 = levelData.Level1 - CombineCount; 
            TeamManager.CharaLevel[charaName].Level2 = levelData.Level2 + 1; 
        }
        if(levelData.Level2 >= CombineCount) {
            combineLevel = 2;
            CombineChara(charaName, combineLevel);//LEVEL_2의 같은 이름 캐릭을 2개 삭제, 1개 레벨 + 1
            TeamManager.CharaLevel[charaName].Level2 = levelData.Level2 - CombineCount; 
            TeamManager.CharaLevel[charaName].Level3 = levelData.Level3 + 1; 
        }
    }

    public void CombineChara(string charaName, int combineLevel) {
        int Delete_Count = 0;

        for(int i = TeamManager.CharaObject[charaName].Count - 1; i >= 0; i--) {
            CharaInfo currentCharaInfo = TeamManager.CharaObject[charaName][i].GetComponent<CharaInfo>(); 
                if(currentCharaInfo.charaStat.Level == combineLevel) {
                    if(Delete_Count >= 2) {
                        currentCharaInfo.charaStat.Level++;
                        break;
                    }
                    else {
                        Delete_Count++;
                        Destroy(TeamManager.CharaObject[charaName][i]);
                        TeamManager.CharaObject[charaName].Remove(TeamManager.CharaObject[charaName][i]);
                    }
                }
        }

    }
    
}
