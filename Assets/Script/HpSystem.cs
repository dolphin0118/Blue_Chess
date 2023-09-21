using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpSystem : MonoBehaviour {
    Transform[] statusObjects;
    Transform cam;
    Image hpGague;
    void Awake(){   
        cam = Camera.main.transform;    
        statusObjects = GetComponentsInChildren<Transform>();   
        Init();
    }

    void Init() {
        for(int i = 0; i < transform.childCount; i++) {
            if(statusObjects[i].tag == "Hp") {
                hpGague = statusObjects[i].GetComponent<Image>();
                hpGague.rectTransform.SetSiblingIndex(1);

            }
            else if(statusObjects[i].tag == "Level") statusObjects[i].GetComponent<Image>().rectTransform.SetAsLastSibling();
        }

    }
    void Update() {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        hpGague.fillAmount = transform.GetComponentInParent<CharaInfo>().Player_Hp / 100f;
    }
}
