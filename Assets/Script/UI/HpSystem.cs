using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSystem : MonoBehaviour {
    UnitStatus unitStatus;
    Transform cam;
    Image hpGague;
    Image mpGague;

    void Start(){   
        cam = Camera.main.transform;    
        unitStatus = transform.GetComponentInParent<UnitStatus>();
        hpGague = transform.GetComponent<Image>();
        //hpGague.rectTransform.SetSiblingIndex(1);
        hpGague.fillAmount = 0.5f;
        if (hpGague == null) {
           Debug.LogError("Hp_front object not found");
        }
    }

    void Init() {

    }
    void Update() {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        float gagueValue =  unitStatus.currentHP / unitStatus.HP;
        hpGague.fillAmount = gagueValue;
        
    }
}
