using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using BlueChessDataBase;
using Unity.Profiling;
using Unity.VisualScripting;

public class UnitInfo : MonoBehaviour
{
    private TeamManager teamManager;
    private SynergyManager synergyManager;
    public UnitData unitData;
    public UnitStatus unitStatus;

    public void Initialize(TeamManager teamManager, SynergyManager synergyManager, UnitData unitData, UnitStatus unitStatus)
    {
        this.teamManager = teamManager;
        this.synergyManager = synergyManager;
        this.unitData = unitData;
        this.unitStatus = unitStatus;
        LevelInit();
    }

    void LevelInit()
    {
        if (this.transform.tag == "Friendly")
        {
            if (!teamManager.UnitLevel.ContainsKey(unitData.Name))
            {
                teamManager.UnitLevel.Add(unitData.Name, new LevelData());
            }
            LevelData levelData = teamManager.UnitLevel[unitData.Name];

            switch (unitStatus.Level)
            {
                case 1:
                    levelData.Level1++;
                    break;
                case 2:
                    levelData.Level2++;
                    break;
                case 3:
                    levelData.Level3++;
                    break;
                default:
                    break;
            }
            teamManager.UnitLevel[unitData.Name] = levelData;

            if (!teamManager.UnitObject.ContainsKey(unitData.Name))
            {
                teamManager.UnitObject[unitData.Name] = new List<GameObject>();
            }
            teamManager.UnitListUpdate(unitData.Name, transform.gameObject);
        }

    }

    public void SynergyAdd()
    {

        if (teamManager.UnitCheck[unitData.Name] == 0)
        {
            Synergy traitSynergy = unitData.traitSynergy;
            Synergy schoolSynergy = unitData.schoolSynergy;
            synergyManager.synergyCount[traitSynergy]++;
            synergyManager.synergyCount[schoolSynergy]++;
        }
        teamManager.UnitCheck[unitData.Name]++;
    }

    public void SynergyRemove()
    {
        if (teamManager.UnitCheck[unitData.Name] > 0)
        {
            teamManager.UnitCheck[unitData.Name]--;
            if (teamManager.UnitCheck[unitData.Name] == 0)
            {
                Synergy traitSynergy = unitData.traitSynergy;
                Synergy schoolSynergy = unitData.schoolSynergy;
                synergyManager.synergyCount[traitSynergy]--;
                synergyManager.synergyCount[schoolSynergy]--;
            }
        }
    }

}
