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
    bool mouseOn;
    public Material[] maters;
    void Start() {
        initColor = this.GetComponent<Image>().color;
        initMater = maters[0];
        this.GetComponent<Image>().material = initMater;
        tooltip = this.transform.Find("SynergyTooltip").gameObject;
        tooltip.SetActive(false);
    }

    void Update() {
        
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
