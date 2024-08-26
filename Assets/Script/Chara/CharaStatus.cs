using System;
using System.Collections;
using System.Collections.Generic;
using BlueChessDataBase;
using JetBrains.Annotations;
using UnityEngine;

public class CharaStatus : MonoBehaviour
{
    public CharaStat charaStat;
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
        charaStat = GetComponent<CharaInfo>().charaStat;
        Init();
    }

    void Init() {
        Level = charaStat.Level;
        MaxHP = charaStat.HP;
        HP = charaStat.HP;
        MP = charaStat.MP;
        ATK = charaStat.ATK;
        ATKSpeed = charaStat.ATKSpeed;
        AR = charaStat.AR;
        MR = charaStat.MR;
        //Speed = charaStat.Speed;
        Range = charaStat.Range;
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
        HP = (charaStat.HP * Level);
    }

    void ATKCalc() {
        ATK = charaStat.ATK + (Level * 0.3f);
    }

    public void Hit(float Damage) {
        HP -= Damage;
    }

}
