using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SynergyDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI synergyCount;
    [SerializeField] TextMeshProUGUI synergyName;
    [SerializeField] TextMeshProUGUI synergyStack;
    SynergyUI synergyUI;
    SynergyBase synergyBase;

    void Start() {
        synergyUI = GetComponentInParent<SynergyUI>();
    }

    
    void Update()
    {
        synergyBase = synergyUI.synergyBase;
        this.synergyCount.text = synergyBase.synergyCount.ToString();
        this.synergyName.text = synergyBase.synergyName.ToString();
    
        string synergyStackConvert = "";
        for(int i = 0; i < synergyBase.synergyStack.Length; i++) {
            synergyStackConvert += synergyBase.synergyStack[i].ToString();
            if(i < synergyBase.synergyStack.Length - 1) synergyStackConvert += " /";
        }

        this.synergyStack.text = synergyStackConvert;
    }
}
