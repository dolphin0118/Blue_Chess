using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class SynergyUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    
    [SerializeField] Image SynergyImage;
    [SerializeField] GameObject tooltip;
    private CanvasGroup synergyActive;
    private Image backgroundImage;
    private Material baseMater;
    private Color baseColor;
    private TooltipUI tooltipUI;
    

    public bool isActive {get; set;}
    public Material[] maters;
    public SynergyBase synergyBase {get; set;}
    public string synergyName;

    private void Awake() {
        synergyActive = this.GetComponent<CanvasGroup>();
        tooltipUI = this.GetComponentInChildren<TooltipUI>();
        backgroundImage = this.GetComponent<Image>();
        baseColor = backgroundImage.color;
        backgroundImage.material = baseMater;
    }

    private void Start() {
        tooltipInit();
        tooltipUI.Init();
    }

    private void tooltipInit() {
        tooltip = this.transform.Find("SynergyTooltip").gameObject;
        baseMater = maters[0];
        string symbolPath = "Chara/Symbol/";
        SynergyImage.sprite = Resources.Load(symbolPath + synergyName,typeof(Sprite)) as Sprite;
        
    }

    public void Init(SynergyBase synergyBase) {
        this.synergyBase = synergyBase;
        synergyName = synergyBase.synergyName.ToString();
    }

    public void ChangeOrder(int orderIndex) {
        this.transform.SetSiblingIndex(orderIndex);
    }

    public void ChangeActive(bool isActive) {
        this.isActive = isActive;
        if(this.isActive) {
            synergyActive.alpha = 1;
        }
        else synergyActive.alpha = 0;
    }

    public void ChangeMaterial(int materValue) {
        this.GetComponent<Image>().material = maters[materValue];
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(isActive) {
            this.GetComponent<Image>().color = Color.grey;
            tooltip.SetActive(true);
        }
        else return;

    }

    public void OnPointerExit(PointerEventData eventData) {
        if(isActive) {
            this.GetComponent<Image>().color = baseColor;
            tooltip.SetActive(false);
        }
        else return;
    }
}
