
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;

public class LevelData
{
    public int Level1 = 0;
    public int Level2 = 0;
    public int Level3 = 0;
}

public class UnitCombine : MonoBehaviour
{
    private TeamManager TeamManager;
    const int CombineCount = 3;

    public void CombineListUpdate(string UnitName)
    {
        LevelData levelData = TeamManager.UnitLevel[UnitName];
        int combineLevel = 0;

        if (levelData.Level1 >= CombineCount)
        {
            combineLevel = 1;
            CombineUnit(UnitName, combineLevel);//LEVEL_1의 같은 이름 캐릭을 2개 삭제, 1개 레벨 + 1
            TeamManager.UnitLevel[UnitName].Level1 = levelData.Level1 - CombineCount;
            TeamManager.UnitLevel[UnitName].Level2 = levelData.Level2 + 1;
        }
        if (levelData.Level2 >= CombineCount)
        {
            combineLevel = 2;
            CombineUnit(UnitName, combineLevel);//LEVEL_2의 같은 이름 캐릭을 2개 삭제, 1개 레벨 + 1
            TeamManager.UnitLevel[UnitName].Level2 = levelData.Level2 - CombineCount;
            TeamManager.UnitLevel[UnitName].Level3 = levelData.Level3 + 1;
        }
    }

    public void CombineUnit(string UnitName, int combineLevel)
    {
        int Delete_Count = 0;

        for (int i = TeamManager.UnitObject[UnitName].Count - 1; i >= 0; i--)
        {
            UnitInfo currentUnitInfo = TeamManager.UnitObject[UnitName][i].GetComponent<UnitInfo>();
            if (currentUnitInfo.UnitStat.Level == combineLevel)
            {
                if (Delete_Count >= 2)
                {
                    currentUnitInfo.UnitStat.Level++;
                    break;
                }
                else
                {
                    Delete_Count++;
                    Destroy(TeamManager.UnitObject[UnitName][i]);
                    TeamManager.UnitObject[UnitName].Remove(TeamManager.UnitObject[UnitName][i]);
                }
            }
        }

    }

}
