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


}

[TaskCategory("Unity/UnitConditional")]
public class CanBattle : UnitConditional
{
    bool isAlreadyBattle = false;
    bool isAlreadyDisarm = false;
    public override TaskStatus OnUpdate()
    {
        if (GameManager.isBattle && PhotonNetwork.IsMasterClient && !isAlreadyBattle)
        {
            isAlreadyBattle = true;
            isAlreadyDisarm = false;
            BattlePhase();
            return TaskStatus.Failure;
        }
        else if (!GameManager.isBattle && PhotonNetwork.IsMasterClient && !isAlreadyDisarm)
        {
            isAlreadyBattle = false;
            isAlreadyDisarm = true;
            DisarmPhase();
            return TaskStatus.Success;
        }
        return GameManager.isBattle ? TaskStatus.Failure : TaskStatus.Success;

    }
}


[TaskCategory("Unity/UnitConditional")]
public class CanAttack : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IsCanAttack();
        }
        isAttack = unitManager.isCanAttack;
        return isAttack ? TaskStatus.Success : TaskStatus.Failure;
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanMove : UnitConditional
{
    protected bool isFindTarget;

    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            unitManager.IsCanAttack();
            unitManager.IsFindTarget();

        }
        isAttack = unitManager.isCanAttack;
        isFindTarget = unitManager.isFindTarget;

        if (!isFindTarget)
        {
            return TaskStatus.Failure;
        }
        return !isAttack ? TaskStatus.Success : TaskStatus.Failure;

    }
}
