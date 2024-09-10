using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UnitConditional : Conditional
{
    protected UnitController unitController;

    public override void OnStart()
    {
        unitController = this.gameObject.GetComponent<UnitController>();
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanBattle : UnitConditional
{
    bool isBattle = false;
    public override TaskStatus OnUpdate()
    {
        if (GameManager.isBattle && !isBattle)
        {
            isBattle = true;
            unitController.BattlePhase();
            return TaskStatus.Failure;
        }

        else
        {
            unitController.DisarmPhase();
            return TaskStatus.Success;
        }

    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanAttack : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (unitController.CheckAttackRange())
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanMove : UnitConditional
{
    public override TaskStatus OnUpdate()
    {
        if (!unitController.CheckAttackRange())
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}
