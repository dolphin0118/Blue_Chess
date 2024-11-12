using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;


public class UnitAction : Action
{

    protected UnitAnimator unitAnimator;
    protected UnitManager unitManager;
    public PhotonView photonView;

    public override void OnStart()
    {
        unitAnimator = GetComponent<UnitAnimator>();
        unitManager = GetComponent<UnitManager>();
    }

    [PunRPC]
    public void UnitStateRPC(string state)
    {
        State stateValue;
        if (System.Enum.TryParse(state, out stateValue))
        {
            UnitState(stateValue);
        }
        else
        {
            UnitState(State.None);
            Debug.LogError("Not State type.");
        }

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
        if (PhotonNetwork.IsMasterClient) photonView.RPC("UnitStateRPC", RpcTarget.All, "Move");

        return TaskStatus.Success;  // 행동이 성공적으로 수행되었음을 반환합니다.
    }
}

