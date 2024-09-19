using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSystem : MonoBehaviour {
    UnitStatus UnitStatus;
    Transform cam;
    Image hpGague;

    void Start(){   
        cam = Camera.main.transform;    
        UnitStatus = transform.GetComponentInParent<UnitStatus>();
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
        hpGague.fillAmount = 1;//charaStatus.MaxHP / charaStatus.HP;
    }
}
