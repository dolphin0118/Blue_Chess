using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    [SerializeField] TextMeshProUGUI GoldText;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI EXPText;
    [SerializeField] PlayerController playerController;
    int gold;
    int level;
    int EXP;
    int maxEXP;

    void Start() {
        
    }

    void Update() {
        gold = playerController.playerGold;
        level = playerController.playerLevel;
        Tuple<int, int> EXPTuple = playerController.getEXP();
        EXP = EXPTuple.Item1;
        maxEXP = EXPTuple.Item2;
        GoldText.text = gold.ToString();
        LevelText.text = level.ToString()+"레벨";
        EXPText.text = EXP.ToString() + "/"+maxEXP.ToString();
    }

    
    public void AddExp() {
         
    }

}
