using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SynergyUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler  {
    Color initColor;
    GameObject tooltip;
    Material initMater;
    RectTransform synergyObject;
    CanvasGroup synergyActive;
    bool mouseOn;
    public Material[] maters;
    void Start() {
        synergyActive = this.GetComponent<CanvasGroup>();
        synergyObject = this.GetComponent<RectTransform>();
        initColor = this.GetComponent<Image>().color;
        tooltip = this.transform.Find("SynergyTooltip").gameObject;
        tooltip.SetActive(false);
        initMater = maters[0];
        this.GetComponent<Image>().material = initMater;
        synergyActive.alpha = 1;
    }

    public void ChangeOrder(int orderIndex) {
        synergyObject.SetSiblingIndex(orderIndex);
    }

    public void ChangeActive(bool isActive) {
        if(isActive) {
            synergyActive.alpha = 1;
        }
        else synergyActive.alpha = 0;
    }
    public void ChangeMaterial(int materValue) {
        this.GetComponent<Image>().material = maters[materValue];
    }

    public void OnPointerEnter(PointerEventData eventData) {
        this.GetComponent<Image>().color = Color.grey;
        mouseOn = true;
        tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        this.GetComponent<Image>().color = initColor;
        mouseOn = false;
        tooltip.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {

    }

    
}
