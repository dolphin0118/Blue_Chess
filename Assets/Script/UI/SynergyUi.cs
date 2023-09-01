using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SynergyUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler  {
    Color initColor;
    GameObject tooltip;
    bool mouseOn;
    
    void Start() {
        initColor = this.GetComponent<Image>().color;
        tooltip = this.transform.Find("SynergyTooltip").gameObject;
        tooltip.SetActive(false);
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
