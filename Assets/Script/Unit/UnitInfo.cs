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
        public float MR ;
        public float ATKSpeed;
        public float Range;
        public void LevelAdd() {
            Level++;
        }
    }
}


public class UnitInfo : MonoBehaviour
{
    public UnitCard UnitCard { get; set; }
    public UnitData UnitData { get; set; }
    public UnitStat unitStat { get; set; }
    public UnitStatus unitStatus;
    public Synergy[] UnitSynergy { get; set; }
    private UnitCombine unitCombine;
    private TeamManager teamManager;
    
    public void Initialize(TeamManager teamManager, UnitCombine unitCombine, UnitCard UnitCard) {
        this.teamManager = teamManager;
        this.unitCombine = unitCombine;
        this.UnitCard = UnitCard;
        this.UnitData = UnitCard.UnitData;
        this.unitStat = UnitCard.UnitStat;
        Debug.Log(unitStat.Level);
        unitStatus = GetComponent<UnitStatus>();
        unitStatus.Initialize(unitStat);
        LevelInit();

    }

    void LevelInit()
    {
        if (this.transform.tag == "Friendly")
        {
            if (!teamManager.UnitLevel.ContainsKey(UnitData.Name))
            {
                teamManager.UnitLevel.Add(UnitData.Name, new LevelData());
            }
            LevelData levelData = teamManager.UnitLevel[UnitData.Name];

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
            teamManager.UnitLevel[UnitData.Name] = levelData;

            if (!teamManager.UnitObject.ContainsKey(UnitData.Name))
            {
                teamManager.UnitObject[UnitData.Name] = new List<GameObject>();
            }
            teamManager.UnitObject[UnitData.Name].Add(transform.gameObject);
        }
        unitCombine.CombineListUpdate(UnitData.Name);
    }

    public void SynergyAdd()
    {
        if (!teamManager.UnitCheck[UnitData.Name])
        {
            teamManager.UnitCheck[UnitData.Name] = true;
            Synergy traitSynergy = UnitData.traitSynergy;
            Synergy schoolSynergy = UnitData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]++;
            SynergyManager.instance.synergyCount[schoolSynergy]++;
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyAdd);
    }

    public void SynergyRemove()
    {
        if (teamManager.UnitCheck[UnitData.Name])
        {
            teamManager.UnitCheck[UnitData.Name] = false;
            Synergy traitSynergy = UnitData.traitSynergy;
            Synergy schoolSynergy = UnitData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]--;
            SynergyManager.instance.synergyCount[schoolSynergy]--;
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyRemove);
    }
}
