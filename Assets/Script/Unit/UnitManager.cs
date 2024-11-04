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
    private PhotonView photonView;

    public string unitOwner;
    private bool isUnitControll;

    private void Awake()
    {
        isUnitControll = false;
        BindComponent();
    }

    private void Update()
    {

    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(isUnitControll) return;
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
           // this.transform.SetParent((Transform)stream.ReceiveNext());
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
        unitInfo = GetComponent<UnitInfo>();
        unitLocate = GetComponent<UnitLocate>();
        unitController = GetComponent<UnitController>();
        unitStatus = GetComponent<UnitStatus>();
        unitItem = GetComponentInChildren<UnitItem>();
        unitAnimator = GetComponent<UnitAnimator>();
        photonView = GetComponent<PhotonView>();
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
        //Left
        if (Input.GetMouseButtonDown(0))
        { 
            isUnitControll = true;
            unitLocate.OnUnitControll();
            //photonView.RPC("OnUnitControll", RpcTarget.AllBufferedViaServer);
        }
        //Right
        else if (Input.GetMouseButtonDown(1))
        { 
            //UIManager.instance.OpenUI(unitCard);
        }
    }

    void OnMouseDrag()
    {
        unitLocate.OnUnitMove();
    }

    void OnMouseUp()
    {
        isUnitControll = false;
        unitLocate.OnUnitUpdate();
        //photonView.RPC("OnUnitUpdate", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    public void OnUnitControll() {
        unitLocate.OnUnitControll();
    }

    [PunRPC]
    public void OnUnitUpdate() {
        unitLocate.OnUnitUpdate();
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
