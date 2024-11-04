using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UI_Card UIcard{get; set;}
    [SerializeField]private GameObject CharaDetailUI;
    [SerializeField]private GameObject SynergyUI;
    [SerializeField]private GameObject ShopUI;
    [SerializeField]private GameObject UserUI;

    void Awake() {
        UIcard = GetComponentInChildren<UI_Card>();
        MatchUI();
    }
    void MatchUI() {
  
    }

    public void OpenUI(UnitCard UnitCard) {
        UIcard.gameObject.SetActive(true);
        UIcard.CardEnable(UnitCard);
    }

    public void CloseUI() {
        UIcard.gameObject.SetActive(false);
    }

    public void SetUIActive(bool isActive) {
        if(isActive) {
            CharaDetailUI.SetActive(true);
            SynergyUI.SetActive(true);
        }
        else {
            CharaDetailUI.SetActive(false);
            SynergyUI.SetActive(false);
        }
    }

    public void SetShopUIActive(bool isActive) {
        if(isActive) {
            ShopUI.SetActive(true);
        }
        else {
            ShopUI.SetActive(false);
        }

    }
}
