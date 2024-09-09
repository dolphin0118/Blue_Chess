using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    private UnitInfo unitInfo;
    private UnitLocate UnitLocate;
    private UnitController UnitController;
    private UnitStatus UnitStatus;
    private UnitItem unitItem;
    private UnitAnimator unitAnimator;

    void Start() {
        Init();
    }
    
    void Init() {
        Binding();
    }

    void Binding() {
        unitInfo = GetComponent<UnitInfo>();
        UnitLocate = GetComponent<UnitLocate>();
        UnitController = GetComponent<UnitController>();
        UnitStatus = GetComponent<UnitStatus>();
        unitItem = GetComponentInChildren<UnitItem>();
        unitAnimator = GetComponent<UnitAnimator>();
    }


    public void UnitState(State state) {
        switch (state) {
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

    bool UnitCheckRay(string type) {
        RaycastHit hitRay;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitRay);

        if(hitRay.transform.tag == type) {
            return true;
        }
        else return false;
    }

//----------------------------------------//
    void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)){ //Left
            UnitLocate.OnObjectControll();
        }
        else if (Input.GetMouseButtonDown(1)){ //Right
            UIManager.instance.OpenUI(unitInfo.UnitCard);
        }
    }

    void OnMouseDrag() {
        UnitLocate.OnObjectMove();
    }

    void OnMouseExit() {
        UIManager.instance.CloseUI();
    }
//------------------------------------------//

    public void AddItem(ItemAsset item) {
        unitItem.AddItem(item);
    }

}
