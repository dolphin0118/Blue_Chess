using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Photon.Pun;

public class UnitConditional : Conditional
{
    protected UnitManager unitManager;
    protected UnitController unitController;
    public PhotonView photonView;

    protected bool isBattle;
    protected bool isAttack;

    public override void OnStart()
    {
        unitManager = this.gameObject.GetComponent<UnitManager>();
        unitController = this.gameObject.GetComponent<UnitController>();
        photonView = this.gameObject.GetComponent<PhotonView>();

        isBattle = false;
        isAttack = false;

    }

    [PunRPC]
    public void BattlePhase()
    {
        isBattle = true;
        unitManager.BattlePhase();
    }

    [PunRPC]
    public void DisarmPhase()
    {
        isBattle = false;
        unitManager.DisarmPhase();
    }

    [PunRPC]
    public void AttackRangeCheck()
    {
        isAttack = unitController.CheckAttackRange();
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanBattle : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (GameManager.isBattle && PhotonNetwork.IsMasterClient)
        {
            // unitManager.BattlePhase();
            photonView.RPC("BattlePhase", RpcTarget.All);
            return TaskStatus.Failure;
        }
        else if (!GameManager.isBattle && PhotonNetwork.IsMasterClient)
        {
            // unitManager.DisarmPhase();
            photonView.RPC("DisarmPhase", RpcTarget.All);
            return TaskStatus.Success;
        }
        return isBattle ? TaskStatus.Failure : TaskStatus.Success;

    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanAttack : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient) photonView.RPC("AttackRangeCheck", RpcTarget.All);

        return isAttack ? TaskStatus.Success : TaskStatus.Failure;
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanMove : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient) photonView.RPC("AttackRangeCheck", RpcTarget.All);

        return !isAttack ? TaskStatus.Success : TaskStatus.Failure;
    }
}
