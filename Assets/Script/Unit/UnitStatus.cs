using System;
using System.Collections;
using System.Collections.Generic;
using BlueChessDataBase;
using JetBrains.Annotations;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    private UnitStat unitStat;
    // public Item;
    public int Level { get; set; }
    public float maxHP { get; set; }
    public float currentHP { get; set; }
    public float maxMP { get; set; }
    public float MP { get; set; }
    public float AR { get; set; }
    public float MR { get; set; }
    public float ATK { get; set; }
    public float ATKSpeed { get; set; }
    public float Speed { get; set; }
    public float Range { get; set; }


    public void Initialize(UnitStat unitStat)
    {
        this.unitStat = unitStat;
        Level = 1;
        maxHP = unitStat.HP;
        currentHP = unitStat.HP;
        MP = unitStat.MP;
        ATK = unitStat.ATK;
        ATKSpeed = unitStat.ATKSpeed;
        AR = unitStat.AR;
        MR = unitStat.MR;
        Range = unitStat.Range;
    }

    void SetStatus()
    {
        maxHP = unitStat.HP;
        currentHP = unitStat.HP;
        maxMP = unitStat.MP;
        MP = unitStat.MP;
        ATK = unitStat.ATK;
        ATKSpeed = unitStat.ATKSpeed;
        AR = unitStat.AR;
        MR = unitStat.MR;
        Range = unitStat.Range;
    }

    void Update()
    {
        Calculate();//비전투시
    }

    void ResetStatus()
    {

    }

    void Calculate()
    {
        HPCalc();
        ATKCalc();
    }

    void HPCalc()
    {
        maxHP = (unitStat.HP * Level);
    }

    void ATKCalc()
    {
        ATK = unitStat.ATK + (Level * 0.3f);
    }

    public void Hit(float Damage)
    {
        currentHP -= Damage;
    }

}
