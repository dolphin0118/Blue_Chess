using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using BlueChessDataBase;
using Unity.Profiling;
using Unity.VisualScripting;

namespace BlueChessDataBase
{
    [Serializable]
    public class UnitData
    {
        public string Name = "";
        public AttackType attackType;
        public Synergy schoolSynergy;
        public Synergy traitSynergy;
        public int UnitPrice;

    }

    [Serializable]
    public class UnitStat
    {
        public int Level = 1;
        public string Name;
        public float HP;
        public float MP;
        public float ATK;
        public float AP;
        public float AR;
        public float MR;
        public float ATKSpeed;
        public float Range;
        public void LevelAdd()
        {
            Level++;
        }
    }
}


public class UnitInfo : MonoBehaviour
{
    public UnitData unitData;
    public UnitStatus unitStatus;
    private UnitCombine unitCombine;
    private TeamManager teamManager;

    public void Initialize(TeamManager teamManager, UnitCombine unitCombine, UnitData unitData, UnitStatus unitStatus)
    {
        this.teamManager = teamManager;
        this.unitCombine = unitCombine;
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
            teamManager.UnitObject[unitData.Name].Add(transform.gameObject);
        }
        unitCombine.CombineListUpdate(unitData.Name);
    }

    public void SynergyAdd()
    {
        if (!teamManager.UnitCheck[unitData.Name])
        {
            teamManager.UnitCheck[unitData.Name] = true;
            Synergy traitSynergy = unitData.traitSynergy;
            Synergy schoolSynergy = unitData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]++;
            SynergyManager.instance.synergyCount[schoolSynergy]++;
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyAdd);
    }

    public void SynergyRemove()
    {
        if (teamManager.UnitCheck[unitData.Name])
        {
            teamManager.UnitCheck[unitData.Name] = false;
            Synergy traitSynergy = unitData.traitSynergy;
            Synergy schoolSynergy = unitData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]--;
            SynergyManager.instance.synergyCount[schoolSynergy]--;
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyRemove);
    }
}
