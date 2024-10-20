using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UnitConditional : Conditional
{
    protected UnitManager unitManager;
    protected UnitController unitController;
    

    public override void OnStart()
    {
        unitManager = this.gameObject.GetComponent<UnitManager>();
        unitController = this.gameObject.GetComponent<UnitController>();
    }
}

[TaskCategory("Unity/UnitConditional")]
public class CanBattle : UnitConditional
{
    bool isBattle = false;
    public override TaskStatus OnUpdate()
    {
        if (GameManager.isBattle)
        {
            unitManager.BattlePhase();
            return TaskStatus.Failure;
        }
        else
        {
            unitManager.DisarmPhase();
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
