using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using BehaviorDesigner.Runtime;

public class UnitManager : MonoBehaviour, IPunObservable
{
    //------------------Manager------------------//
    private PhotonView photonView;
    private TeamManager teamManager;
    private SynergyManager synergyManager;
    private UIManager UIManager;
    private CombineSystem combineSystem;
    //------------------Controll------------------//

    private UnitLocate unitLocate;
    private UnitController unitController;
    private UnitAstar unitAstar;
    private UnitAnimator unitAnimator;

    //-----------------Data----------------------//
    private UnitStatus unitStatus;
    private UnitStatusUI unitStatusUI;
    private UnitItem unitItem;
    private UnitInfo unitInfo;
    private UnitCard unitCard;

    private State currentState;
    private bool isUnitControll;
    public bool isCanAttack { get; private set; }
    public bool isFindTarget { get; private set; }

    private void Awake()
    {
        isCanAttack = false;
        isFindTarget = false;
        isUnitControll = true;
        currentState = State.None;
        BindComponent();
    }

    void BindComponent()
    {
        photonView = GetComponent<PhotonView>();

        unitInfo = GetComponent<UnitInfo>();
        unitLocate = GetComponent<UnitLocate>();
        unitController = GetComponent<UnitController>();
        unitAstar = GetComponent<UnitAstar>();

        unitStatus = GetComponent<UnitStatus>();
        unitItem = GetComponentInChildren<UnitItem>();
        unitAnimator = GetComponent<UnitAnimator>();

        unitStatusUI = GetComponentInChildren<UnitStatusUI>();
    }

    public void Initialize(TeamManager teamManager, SynergyManager synergyManager, UnitCard unitCard)
    {
        this.teamManager = teamManager;
        this.synergyManager = synergyManager;
        this.unitCard = unitCard;

        this.unitStatus.Initialize(synergyManager, unitCard);
        this.unitInfo.Initialize(teamManager, synergyManager, unitCard.UnitData, unitStatus);
        this.unitLocate.Initialize(teamManager, synergyManager);
        this.unitController.Initialize(teamManager);
        this.unitStatusUI.Initialize(teamManager, unitStatus);
    }

    private void Update()
    {
        if (GameManager.isBattle && unitStatus.IsUnitDead())
        {
            photonView.RPC("UnitDisable", RpcTarget.All);
        }
    }

    private void OnEnable()
    {
        //unitStatus.SetupStatus();
    }

    [PunRPC]
    public void UnitDisable()
    {
        this.transform.localPosition = Vector3.zero;
        this.transform.gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        return;
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
            // stream.SendNext(this.transform.parent);
        }
        else
        {
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
            //  this.transform.SetParent((Transform)stream.ReceiveNext());
        }

    }
    ///-------------------------------------------------------------------------------------//
    public void BattlePhase()
    {
        photonView.RPC("BattlePhaseRPC", RpcTarget.All);
    }

    [PunRPC]
    public void BattlePhaseRPC()
    {
        isUnitControll = false;
        unitLocate.ForceLocate();
        if (this.transform.parent.gameObject.layer == LayerMask.NameToLayer("Battle"))
        {
            unitLocate.enabled = false;
            unitController.OnBattle();

        }
    }

    public void DisarmPhase()
    {
        photonView.RPC("DisarmPhaseRPC", RpcTarget.All);
    }

    [PunRPC]
    public void DisarmPhaseRPC()
    {
        isUnitControll = true;
        UnitState(State.Idle);
        unitLocate.enabled = true;
        unitStatus.SetupStatus();

    }

    public void IsCanAttack()
    {
        bool isCheckRange = unitController.CheckAttackRange();
        if (isCanAttack != isCheckRange)
            photonView.RPC("IsCanAttackRPC", RpcTarget.All, isCheckRange);
    }

    [PunRPC]
    public void IsCanAttackRPC(bool isCheckRange)
    {
        isCanAttack = isCheckRange;
    }

    public void IsFindTarget()
    {
        bool isCheckTarget = unitController.IsFindTarget();
        if (isFindTarget != isCheckTarget)
            photonView.RPC("IsFindTargetRPC", RpcTarget.All, isCheckTarget);
    }

    [PunRPC]
    public void IsFindTargetRPC(bool isCheckTarget)
    {
        isFindTarget = isCheckTarget;
    }


    public void UnitState(State state)
    {
        if (currentState != state)
            photonView.RPC("UnitStateRPC", RpcTarget.All, state);
    }

    [PunRPC]
    public void UnitStateRPC(State state)
    {
        currentState = state;
        switch (state)
        {
            case State.Attack:
                unitAnimator.AttackState();
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                break;
            case State.Move:
                unitAnimator.MoveState();
                break;
            case State.Idle:
                unitAnimator.IdleState();
                break;
            case State.None:
                unitAnimator.NoneState();
                break;
        }
    }

    //-------------------------------마우스 컨트롤-----------------------------------//
    void OnMouseOver()
    {
        //Left
        if (Input.GetMouseButtonDown(0) && isUnitControll && photonView.IsMine)
        {
            unitLocate.OnUnitControll();
            teamManager.GridView.SetActive(true);
        }
        //Right
        else if (Input.GetMouseButtonDown(1))
        {
            teamManager.OpenUI(unitCard);
        }
    }

    void OnMouseDrag()
    {
        if (isUnitControll && photonView.IsMine)
            unitLocate.OnUnitMove();
    }

    void OnMouseUp()
    {
        if (isUnitControll && photonView.IsMine)
        {
            unitLocate.OnUnitUpdate();
            teamManager.GridView.SetActive(false);
        }

    }


    void OnMouseExit()
    {
        teamManager.CloseUI();
    }
    //------------------------------------------//

    public void OnHitDamage(float Damage, AttackType otherType)
    {
        unitStatus.Hit(Damage, otherType);
    }


    public void AddItem(ItemAsset item)
    {
        unitItem.AddItem(item);
    }


}
