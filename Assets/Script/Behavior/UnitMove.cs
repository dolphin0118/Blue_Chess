using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Unity/UnitAction")]
public class UnitMove : Action
{
    public SharedBool isMove;  // Behavior Designer에서 설정한 SharedBool 변수
    public override TaskStatus OnUpdate()
    {
        if (isMove.Value)  // SharedBool 값이 true일 때
        {
            // Idle 상태의 행동을 수행합니다.
            Debug.Log("Move State");
            return TaskStatus.Success;  // 행동이 성공적으로 수행되었음을 반환합니다.
        }

        return TaskStatus.Failure;  // SharedBool 값이 false일 때, 실패로 반환합니다.
    }
}

[TaskCategory("Unity/UnitAction")]
public class CanMove : Conditional
{
    public UnitAnimator playerManager;
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
        // return TaskStatus.Failure;
    }
}