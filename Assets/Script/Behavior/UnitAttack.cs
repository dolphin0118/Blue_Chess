using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Photon.Pun;

[TaskCategory("Unity/UnitAction")]
public class UnitAttack : UnitAction
{
    public SharedBool isAttack;
    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient) UnitState(State.Attack);
        return TaskStatus.Success;  // 행동이 성공적으로 수행되었음을 반환합니다
    }
}

