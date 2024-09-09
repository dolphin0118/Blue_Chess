using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public UI_Card UIcard{get; set;}

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else{
            if (instance != this) Destroy(this.gameObject);
        }    
    }

    public void OpenUI(UnitCard UnitCard) {
        UIcard.gameObject.SetActive(true);
        UIcard.CardEnable(UnitCard);
    }

    public void CloseUI() {
        UIcard.gameObject.SetActive(false);
    }
}
