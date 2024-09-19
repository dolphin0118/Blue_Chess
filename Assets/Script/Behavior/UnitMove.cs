using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;


public class UnitAction : Action {

    protected UnitAnimator unitAnimator;
    protected UnitManager unitManager;

    public override void OnStart() {
        unitAnimator = GetComponent<UnitAnimator>();
        unitManager = GetComponent<UnitManager>();
    }
}

[TaskCategory("Unity/UnitAction")]
public class UnitMove : UnitAction
{
    public SharedBool isMove;  // Behavior Designer에서 설정한 SharedBool 변수
    public override TaskStatus OnUpdate() {
        unitManager.UnitState(State.Move);
        Debug.Log("Move State");
        return TaskStatus.Success;  // 행동이 성공적으로 수행되었음을 반환합니다.
    }
}

