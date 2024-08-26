using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CharaManager : MonoBehaviour {
    private CharaInfo charaInfo;
    private CharaLocate charaLocate;
    private CharaController charaController;
    private CharaStatus charaStatus;
    private UnitItem unitItem;

    void Start() {
        Init();
    }
    
    void Init() {
        Binding();
        Setup();
    }

    void Binding() {
        charaInfo = GetComponent<CharaInfo>();
        charaLocate = GetComponent<CharaLocate>();
        charaController = GetComponent<CharaController>();
        charaStatus = GetComponent<CharaStatus>();
        unitItem = GetComponentInChildren<UnitItem>();
    }


    void Setup() {
        
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
            charaLocate.OnObjectControll();
        }
        else if (Input.GetMouseButtonDown(1)){ //Right
            UIManager.instance.OpenUI(charaInfo.charaCard);
        }
    }

    void OnMouseDrag() {
        charaLocate.OnObjectMove();
    }

    void OnMouseExit() {
        UIManager.instance.CloseUI();
    }
//------------------------------------------//

    public void AddItem(ItemAsset item) {
        unitItem.AddItem(item);
    }

}
