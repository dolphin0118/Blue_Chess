using System;
using System.Collections;
using System.Collections.Generic;
using BlueChessDataBase;
using GoogleSheet.Type;
using JetBrains.Annotations;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    private UnitStat unitStat;
    // public Item;
    public int Level { get; set; }

    public float HP { get; private set; }
    public float MP { get; private set; }
    public float AR { get; private set; }
    public float MR { get; private set; }
    public float ATK { get; private set; }
    public float ATKSpeed { get; private set; }
    public float Speed { get; private set; }
    public float Range { get; private set; }

    public float currentHP { get; set; }
    public float currentMP { get; set; }
    public float currentAR { get; set; }
    public float currentMR { get; set; }
    public float currentATK { get; set; }
    public float currentATKSpeed { get; set; }
    public float currentRange { get; set; }

    public AttackType attackType { get; set; }

    public void Initialize(UnitStat unitStat)
    {
        this.unitStat = unitStat;
        Level = 1;
        SetupStatus();
    }

    public void SetupStatus()
    {
        HP = unitStat.HP;
        MP = unitStat.MP;
        ATK = unitStat.ATK;
        ATKSpeed = unitStat.ATKSpeed;
        AR = unitStat.AR;
        MR = unitStat.MR;
        Range = unitStat.Range * 2;

        currentHP = HP;
        currentMP = MP;
        currentAR = AR;
        currentMR = MR;
        currentATK = ATK;
        currentATKSpeed = ATKSpeed;
        currentRange = Range;

        attackType = unitStat.attackType;
        CalulateStatus();
    }

    void Update()
    {
        //CalulateStatus();//비전투시
    }

    public void CalulateStatus()
    {
        HPCalc();
        MPCalc();
        ATKCalc();
    }


    void HPCalc()
    {
        float LevelHPValue = unitStat.HP * 0.8f;
        HP = this.HP + (Level * LevelHPValue);
        currentHP = HP;
    }

    void MPCalc()
    {

    }

    void ATKCalc()
    {
        //공격 데미지 계산 공식 // 유닛 기본 공격력 + (Level * Level당 계수) * 100/(100 + 방어력 or 마법방어력)
        //감산을 위한 * 100/(100 + 방어력 or 마법방어력) 계산은 피격받은 유닛의 내부에서 결정
        //추가 구현 필요사항 => 현재 공격 타입이 물리 or 마법인지 전달 필요 
        //공격력은 레벨 * 0.33 추가 부여

        float LevelATKValue = unitStat.ATK * 0.33f;
        ATK = unitStat.ATK + (Level * LevelATKValue);
    }

    public void Hit(float Damage, AttackType otherType)
    {
        float attackConstantValue = CalculateAttackConstant(attackType, otherType);
        currentHP -= Damage * attackConstantValue;
    }

    private float CalculateAttackConstant(AttackType myType, AttackType otherType)
    {
        float weakValue = 1.25f;
        float normalValue = 1.0f;
        float strongValue = 0.75f;

        if (myType == otherType)
        {
            return normalValue;
        }
        else
        {
            float returnValue = 0;
            switch (myType)
            {
                case AttackType.Explosion:
                    if (otherType == AttackType.Mystery) returnValue = weakValue;
                    else if (otherType == AttackType.Penetrate) returnValue = strongValue;
                    break;
                case AttackType.Penetrate:
                    if (otherType == AttackType.Explosion) returnValue = weakValue;
                    else if (otherType == AttackType.Mystery) returnValue = strongValue;
                    break;
                case AttackType.Mystery:
                    if (otherType == AttackType.Penetrate) returnValue = weakValue;
                    else if (otherType == AttackType.Explosion) returnValue = strongValue;
                    break;
            }
            return returnValue;
        }
    }

    public bool IsUnitDead()
    {
        if (currentHP <= 0) return true;
        else return false;
    }
}
