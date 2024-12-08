using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TooltipUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private RectTransform parentRectTransform;
    private Vector2 targetPosition;
    private VisualElement root;
    private VisualElement panel;
    private SynergyUI synergyUI;
    
    void Awake() {
        synergyUI = GetComponentInParent<SynergyUI>();
        parentRectTransform = GetComponentInParent<RectTransform>();
        uiDocument = GetComponent<UIDocument>();
    }

    public void Init() {
        uiDocument.visualTreeAsset = Resources.Load("UI/SynergyTooltip/"+synergyUI.synergyName) as VisualTreeAsset;
        targetPosition = new Vector2(200, 200) + parentRectTransform.anchoredPosition;
        root = uiDocument.rootVisualElement;
        panel = root.Q<VisualElement>("Root_Panel");
        if (panel != null) {
            panel.style.marginLeft = 200;
            panel.style.marginTop = 100;
        }
        else {
            Debug.Log("Panel not found."+synergyUI.synergyName);
        }      
        this.gameObject.SetActive(false);
    }

    private void OnEnable() {
        PositionSetup();
    }

    public void PositionSetup() {
        root = uiDocument.rootVisualElement;
        panel = root.Q<VisualElement>("Root_Panel");
        panel.style.marginLeft = 300;
        panel.style.marginTop = 100;
    }
}
