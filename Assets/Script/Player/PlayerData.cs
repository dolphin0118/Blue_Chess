using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    PlayerController playerController;

    public string playerName { get; set; }
    public int playerLevel { get; set; }
    public int playerHp { get; set; }
    public int playerGold { get; set; }
    public int maxUnitCapacity { get; set; }

    public int[] levelEXP = new int[] { 2, 2, 6, 10, 20, 36 };
    public int EXP = 0;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        Setup();
    }

    void Setup()
    {
        playerName = "Player" + playerController.playerCode;
        playerLevel = 3;
        playerHp = 20;
        playerGold = 20;
        maxUnitCapacity = playerLevel;
    }

    public void Update()
    {
        UpdateLevel();
    }


    private void UpdateLevel()
    {
        if (EXP >= levelEXP[playerLevel])
        {
            EXP -= levelEXP[playerLevel];
            playerLevel++;
        }
    }

    public void AddGold()
    {
        int basicGold = 5;
        int totalGold = basicGold;
        playerGold += totalGold;
    }
    public int GetGold()
    {
        return playerGold;
    }

    public void PayGold(int unitPrice)
    {
        playerGold -= unitPrice;

    }

    //전투 종료시 호출
    public void BattleEXP()
    {
        int BattleEXPValue = 2;
        EXP += BattleEXPValue;
    }

    public void BuyEXP()
    {
        EXP += 4;
    }

    //Button으로 호출
    public Tuple<int, int> GetEXP()
    {
        return new Tuple<int, int>(EXP, levelEXP[playerLevel]);
    }

    public void GetDamage(int damage)
    {
        playerHp -= damage;
    }

}
