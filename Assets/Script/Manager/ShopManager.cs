using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    //-----------------------Component---------------------------//
    private TeamManager teamManager;
    private PlayerData playerData;

    //-----------------------Bind Data----------------------------//
    [SerializeField] TextMeshProUGUI GoldText;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI EXPText;
    
    [SerializeField] GameObject sellSystem;
    [SerializeField] GameObject spawnSystem;

    private int playerGold;
    int level;
    int EXP;
    int maxEXP;
    bool isReroll;

    void Start()
    {
        sellSystem.SetActive(false);
        isReroll = true;
    }

    void Update()
    {
        DataUpdate();
        SystemActive();
    }

    public void Initialize(TeamManager teamManager, PlayerData playerData) {
        this.teamManager = teamManager;
        this.playerData = playerData;

        sellSystem.GetComponent<SellSystem>().Initialize(teamManager, playerData);
        
    }

    void DataUpdate()
    {
        playerGold = playerData.playerGold;
        level = playerData.playerLevel;

        Tuple<int, int> EXPTuple = playerData.GetEXP();
        EXP = EXPTuple.Item1;
        maxEXP = EXPTuple.Item2;

        GoldText.text = playerGold.ToString();
        LevelText.text = level.ToString() + "레벨";
        EXPText.text = EXP.ToString() + "/" + maxEXP.ToString();
    }

    public void BuyEXP()
    {
        int buyEXPValue = 4;
        if (playerGold >= 4)
        {
            playerData.EXP += buyEXPValue;
            playerData.playerGold -= 4;
        }

    }

    public void Reroll()
    {
        if (isReroll && playerGold >= 2)
        {
            playerData.playerGold -= 2;
            StartCoroutine(CardReroll());
        }
    }

    public void SystemActive() {
        if(teamManager.isControllUnit) {
            sellSystem.SetActive(true);
            spawnSystem.SetActive(false);
        }
        else {
            sellSystem.SetActive(false);
            spawnSystem.SetActive(true);
        }
    }

    IEnumerator CardReroll()
    {
        isReroll = false;
        SpawnCard[] spawnCards = GetComponentsInChildren<SpawnCard>();
        foreach (SpawnCard spawnCard in spawnCards) spawnCard.CardReroll();
        yield return new WaitForSeconds(0.5f);
        isReroll = true;
    }

}
