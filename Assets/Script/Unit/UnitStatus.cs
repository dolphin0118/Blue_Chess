using System;
using System.Collections;
using System.Collections.Generic;
using BlueChessDataBase;
using JetBrains.Annotations;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public UnitStat unitStat;
    // public Item;
    public int Level {get; set;}
    public float MaxHP {get; set;}
    public float HP {get; set;}
    public float MP {get; set;}
    public float AR {get; set;}
    public float MR {get; set;}
    public float ATK {get; set;}
    public float ATKSpeed{get; set;}
    public float Speed {get; set;}
    public float Range {get; set;}
    
    void Start()
    {
        unitStat = GetComponent<UnitInfo>().UnitStat;
        Init();
    }

    void Init() {
        Level = unitStat.Level;
        MaxHP = unitStat.HP;
        HP = unitStat.HP;
        MP = unitStat.MP;
        ATK = unitStat.ATK;
        ATKSpeed = unitStat.ATKSpeed;
        AR = unitStat.AR;
        MR = unitStat.MR;
        //Speed = unitStat.Speed;
        Range = unitStat.Range;
    }

    void Update()
    {
        Calculate();//비전투시
    }

    void Calculate() {
        HPCalc();
        ATKCalc();
    }

    void HPCalc() {
        HP = (unitStat.HP * Level);
    }

    void ATKCalc() {
        ATK = unitStat.ATK + (Level * 0.3f);
    }

    public void Hit(float Damage) {
        HP -= Damage;
    }

}
