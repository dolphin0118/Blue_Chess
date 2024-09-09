using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Unity/UnitAction")]
public class UnitIdle : Action
{
    public SharedBool isIdle;  // Behavior Designer에서 설정한 SharedBool 변수
    public override TaskStatus OnUpdate()
    {
        this.GetComponent<UnitManager>().UnitState(State.Idle);
        return TaskStatus.Success;  // 행동이 성공적으로 수행되었음을 반환합니다.
    }
}