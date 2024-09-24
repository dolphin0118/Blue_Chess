using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private UnitCard unitCard;
    private UnitInfo unitInfo;
    private UnitLocate unitLocate;
    private UnitController unitController;
    private UnitStatus unitStatus;
    private UnitItem unitItem;
    private UnitAnimator unitAnimator;
    private TeamManager TeamManager;
    private UnitCombine unitCombine;

    private void Awake()
    {
        BindComponent();
    }

    void Start()
    {

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

    public void Initialize(TeamManager TeamManager, UnitCombine unitCombine, UnitCard unitCard)
    {
        this.unitCard = unitCard;
        this.TeamManager = TeamManager;
        this.unitCombine = unitCombine;
        this.unitCombine.Initialize(TeamManager);
        unitStatus.Initialize(unitCard.UnitStat);
        unitInfo.Initialize(TeamManager, unitCombine, unitCard.UnitData, unitStatus);
        unitLocate.Initialize(TeamManager);
    }

    void BindComponent()
    {

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
        if (Input.GetMouseButtonDown(0))
        { //Left
            unitLocate.OnObjectControll();
        }
        else if (Input.GetMouseButtonDown(1))
        { //Right
            UIManager.instance.OpenUI(unitCard);
        }
    }

    void OnMouseDrag()
    {
        unitLocate.OnObjectMove();
    }

    void OnMouseExit()
    {
        UIManager.instance.CloseUI();
    }
    //------------------------------------------//

    public void AddItem(ItemAsset item)
    {
        unitItem.AddItem(item);
    }

}
