using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using BlueChessDataBase;

namespace BlueChessDataBase {
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
        public string Name = "";
        public float HP = 0;
        public float MP = 0;
        public float ATK = 0;
        public float AP = 0;
        public float AR = 0;
        public float MR = 0;
        public float ATKSpeed = 0.0f;
        public float Range = 0.0f;
    }
}


public class UnitInfo : MonoBehaviour { 
    public UnitCard UnitCard {get; set;}
    public UnitData UnitData {get; set;}
    public UnitStat UnitStat {get; set;}
    public Synergy[] UnitSynergy {get; set;}
    private UnitCombine UnitCombine;

    void Start() {
        UnitCombine = gameObject.AddComponent<UnitCombine>();
        LevelInit();
    }

    public void UnitDataSetup(UnitCard UnitCard) {
        this.UnitCard = UnitCard;
        this.UnitData = UnitCard.UnitData;
        this.UnitStat = UnitCard.UnitStat;
    }

    void ChangeArea() {

    }

    void LevelInit() {
        if(this.transform.tag == "Friendly") {
            if(!TeamManager.UnitLevel.ContainsKey(UnitData.Name)) {
                TeamManager.UnitLevel.Add(UnitData.Name, new LevelData());
            }
            LevelData levelData = TeamManager.UnitLevel[UnitData.Name];
            switch(UnitStat.Level){
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
            TeamManager.UnitLevel[UnitData.Name] = levelData;

            if(!TeamManager.UnitObject.ContainsKey(UnitData.Name)) {
                TeamManager.UnitObject[UnitData.Name] = new List<GameObject>();
            }
            TeamManager.UnitObject[UnitData.Name].Add(transform.gameObject);
        }
        UnitCombine.CombineListUpdate(UnitData.Name);
    }

    public void SynergyAdd() {
        if(!TeamManager.UnitCheck[UnitData.Name]) {
            TeamManager.UnitCheck[UnitData.Name] = true;
            Synergy traitSynergy = UnitData.traitSynergy;
            Synergy schoolSynergy = UnitData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]++;
            SynergyManager.instance.synergyCount[schoolSynergy]++;           
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyAdd);
    }

    public void SynergyRemove() {    
        if(TeamManager.UnitCheck[UnitData.Name]) {
            TeamManager.UnitCheck[UnitData.Name] = false;
            Synergy traitSynergy = UnitData.traitSynergy;
            Synergy schoolSynergy = UnitData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]--;
            SynergyManager.instance.synergyCount[schoolSynergy]--;           
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyRemove);
    }
}
