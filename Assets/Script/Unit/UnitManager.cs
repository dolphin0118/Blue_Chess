using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class UnitManager : MonoBehaviour, IPunObservable
{

    private PhotonView pv;
    private TeamManager teamManager;
    private SynergyManager synergyManager;

    private UnitCard unitCard;
    private UnitInfo unitInfo;
    private UnitLocate unitLocate;
    private UnitController unitController;
    private UnitStatus unitStatus;
    private UnitItem unitItem;
    private UnitAnimator unitAnimator;
    private UnitCombine unitCombine;

    public string unitOwner;
    private Transform currTr;

    private void Awake()
    {
        BindComponent();
    }
    private void Update()
    {
        if (!pv.IsMine)
        {
            // this.transform.position = Vector3.Lerp(this.transform.position, currTr.position, Time.deltaTime * 10.0f);
            // this.transform.rotation = Quaternion.Lerp(this.transform.rotation, currTr.rotation, Time.deltaTime * 10.0f);
        }

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.position);
        }
        else
        {
            currTr.position = (Vector3)stream.ReceiveNext();
            currTr.rotation = (Quaternion)stream.ReceiveNext();
        }

    }
    public void BattlePhase()
    {
        unitLocate.ForceLocate();
        unitLocate.enabled = false;
        unitController.OnBattle();
    }

    public void DisarmPhase()
    {
        UnitState(State.Idle);
        unitController.OnDisarm();
        unitLocate.enabled = true;
    }

    public void Initialize(TeamManager teamManager, SynergyManager synergyManager, UnitCombine unitCombine, UnitCard unitCard)
    {
        this.unitCard = unitCard;
        this.teamManager = teamManager;
        this.synergyManager = synergyManager;
        this.unitCombine = unitCombine;

        this.unitCombine.Initialize(teamManager);
        unitStatus.Initialize(unitCard.UnitStat);
        unitInfo.Initialize(teamManager, synergyManager, unitCombine, unitCard.UnitData, unitStatus);
        unitLocate.Initialize(teamManager, synergyManager);
    }

    void BindComponent()
    {
        pv = GetComponent<PhotonView>();
        unitInfo = GetComponent<UnitInfo>();
        unitLocate = GetComponent<UnitLocate>();
        unitController = GetComponent<UnitController>();
        unitStatus = GetComponent<UnitStatus>();
        unitItem = GetComponentInChildren<UnitItem>();
        unitAnimator = GetComponent<UnitAnimator>();
    }

    public void UnitState(State state)
    {
        switch (state)
        {
            case State.Attack:
                unitAnimator.AttackState();
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

    bool UnitCheckRay(string type)
    {
        RaycastHit hitRay;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitRay);

        if (hitRay.transform.tag == type)
        {
            return true;
        }
        else return false;
    }

    //----------------------------------------//
    void OnMouseOver()
    {
        if (!pv.IsMine) return;
        if (Input.GetMouseButtonDown(0))
        { //Left
            unitLocate.OnUnitControll();
        }
        else if (Input.GetMouseButtonDown(1))
        { //Right
            //UIManager.instance.OpenUI(unitCard);
        }
    }

    void OnMouseDrag()
    {
        unitLocate.OnUnitMove();
    }

    void OnMouseExit()
    {
        //UIManager.instance.CloseUI();
    }
    //------------------------------------------//

    public void AddItem(ItemAsset item)
    {
        unitItem.AddItem(item);
    }


}
