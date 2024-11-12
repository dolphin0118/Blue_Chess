using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Photon.Pun;

public class UnitConditional : Conditional
{
    protected UnitManager unitManager;
   // public PhotonView photonView;

    protected bool isBattle;
    protected bool isAttack;

    public override void OnStart()
    {
        unitManager = this.gameObject.GetComponent<UnitManager>();

    }

    public void BattlePhase()
    {
        unitManager.BattlePhase();
        isBattle = GameManager.isBattle;
    }

    public void DisarmPhase()
    {
        unitManager.DisarmPhase();
        isBattle = GameManager.isBattle;
    }

    public void IsCanAttack()
    {
        unitManager.IsCanAttack();
    }

    public void GetCanAttack() {
        isAttack = unitManager.GetCanAttack();
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanBattle : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (GameManager.isBattle && PhotonNetwork.IsMasterClient)
        {
            BattlePhase();
            return TaskStatus.Failure;
        }
        else if (GameManager.isBattle) {
            return TaskStatus.Failure;
        }
        // else if (!GameManager.isBattle && PhotonNetwork.IsMasterClient)
        // {
        //     DisarmPhase();
        //     return TaskStatus.Success;
        // }
        return isBattle ? TaskStatus.Failure : TaskStatus.Success;

    }
}


[TaskCategory("Unity/UnitConditional")]
public class CanAttack : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient) IsCanAttack();
        GetCanAttack();
        
        return isAttack ? TaskStatus.Success : TaskStatus.Failure;
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanMove : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient) IsCanAttack();
        GetCanAttack();

        return !isAttack? TaskStatus.Success : TaskStatus.Failure;
    }
}
