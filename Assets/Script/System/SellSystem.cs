using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;
using BlueChessDataBase;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellSystem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //-----------------Component--------------------//
    private TeamManager teamManager;
    private PlayerData playerData;

    //----------------Bind Data---------------------//
    [SerializeField] Image panelImage;
    [SerializeField] TextMeshProUGUI goldText;
    //---------------------------------//
    private Color initColor;
    private bool isCanSell = false;
    
    private void Awake() {
        initColor = panelImage.color;
    }

    public void Initialize(TeamManager teamManager, PlayerData playerData) {
        this.teamManager = teamManager;
        this.playerData = playerData;
    }

    private void Update() {
        
        bool isControllUnit = teamManager.isControllUnit;
        GameObject controllUnit = teamManager.controllUnit;
        UnitInfo controllUnitInfo = controllUnit.GetComponent<UnitInfo>();

        if(controllUnit) {
            int unitSellGold = controllUnitInfo.unitData.UnitPrice - 1;
            goldText.text = "판매 금액 : " + unitSellGold.ToString();
        }
        
        if(isCanSell && isControllUnit && Input.GetMouseButtonUp(0)) {
            SellUnit(controllUnit);
        }
        else if(isControllUnit && Input.GetMouseButtonUp(0)) {
            teamManager.isControllUnit = false;
        }
    }

    void SellUnit(GameObject controllUnit) {
        
        if(controllUnit != null) {
            UnitInfo controllUnitInfo = controllUnit.GetComponent<UnitInfo>();

            string unitName = controllUnitInfo.unitData.Name;
            int unitSellGold = controllUnitInfo.unitData.UnitPrice - 1;

            ObjectPoolManager.instance.multiPool[unitName].Release(controllUnit);
            teamManager.UnitObject[unitName].Remove(controllUnit);
            
            playerData.AddGold(unitSellGold);
            isCanSell = false;
            teamManager.isControllUnit = false;
            panelImage.color = initColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isCanSell = true;
        panelImage.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCanSell = false;
        panelImage.color = initColor;
    }


}
