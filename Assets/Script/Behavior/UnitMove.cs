using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;


public class UnitAction : Action
{
    protected UnitManager unitManager;

    public override void OnStart()
    {
        unitManager = GetComponent<UnitManager>();
    }


    public void UnitState(State state)
    {
        unitManager.UnitState(state);
    }
}

[TaskCategory("Unity/UnitAction")]
public class UnitMove : UnitAction
{
    public SharedBool isMove;  // Behavior Designer에서 설정한 SharedBool 변수
    public override TaskStatus OnUpdate()
    {
        if (PhotonNetwork.IsMasterClient) UnitState(State.Move);
        return TaskStatus.Success;  // 행동이 성공적으로 수행되었음을 반환합니다.
    }
}

