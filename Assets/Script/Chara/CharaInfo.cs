using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using System;
using BlueChessDataBase;

namespace BlueChessDataBase {
    [Serializable]
    public class CharaData
    {
        public string Name = "";
        public AttackType attackType;
        public Synergy schoolSynergy;
        public Synergy traitSynergy;
        public int charaPrice;

    }

    [Serializable]
    public class CharaStat
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


public class CharaInfo : MonoBehaviour { 
    public CharaCard charaCard {get; set;}
    public CharaData charaData {get; set;}
    public CharaStat charaStat {get; set;}
    public Synergy[] CharaSynergy {get; set;}
    private CharaCombine charaCombine;

    void Start() {
        charaCombine = gameObject.AddComponent<CharaCombine>();
        LevelInit();
    }

    public void CharaDataSetup(CharaCard charaCard) {
        this.charaCard = charaCard;
        this.charaData = charaCard.charaData;
        this.charaStat = charaCard.charaStat;
    }

    void ChangeArea() {

    }

    void LevelInit() {
        if(this.transform.tag == "Friendly") {
            if(!TeamManager.CharaLevel.ContainsKey(charaData.Name)) {
                TeamManager.CharaLevel.Add(charaData.Name, new LevelData());
            }
            LevelData levelData = TeamManager.CharaLevel[charaData.Name];
            switch(charaStat.Level){
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
            TeamManager.CharaLevel[charaData.Name] = levelData;

            if(!TeamManager.CharaObject.ContainsKey(charaData.Name)) {
                TeamManager.CharaObject[charaData.Name] = new List<GameObject>();
            }
            TeamManager.CharaObject[charaData.Name].Add(transform.gameObject);
        }
        charaCombine.CombineListUpdate(charaData.Name);
    }

    public void SynergyAdd() {
        if(!TeamManager.CharaCheck[charaData.Name]) {
            TeamManager.CharaCheck[charaData.Name] = true;
            Synergy traitSynergy = charaData.traitSynergy;
            Synergy schoolSynergy = charaData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]++;
            SynergyManager.instance.synergyCount[schoolSynergy]++;           
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyAdd);
    }

    public void SynergyRemove() {    
        if(TeamManager.CharaCheck[charaData.Name]) {
            TeamManager.CharaCheck[charaData.Name] = false;
            Synergy traitSynergy = charaData.traitSynergy;
            Synergy schoolSynergy = charaData.schoolSynergy;
            SynergyManager.instance.synergyCount[traitSynergy]--;
            SynergyManager.instance.synergyCount[schoolSynergy]--;           
        }
        SynergyManager.instance.synergyEvent.RemoveListener(SynergyRemove);
    }
}
