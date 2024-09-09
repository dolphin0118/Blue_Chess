using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Unity/UnitAction")]
public class UnitAttack : Action
{
    public SharedBool isAttack; 
    public override TaskStatus OnUpdate()
    {
        this.GetComponent<UnitManager>().UnitState(State.Attack);
        Debug.Log("isAttack State");
        return TaskStatus.Success;  // 행동이 성공적으로 수행되었음을 반환합니다
    }
}

