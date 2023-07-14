using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpSystem : MonoBehaviour {
    Transform cam;
    Image Hp_Gague;
    void Awake(){   
        cam = Camera.main.transform;
        Hp_Gague = GetComponentInChildren<Image>();
    }
    void Update() {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        Hp_Gague.rectTransform.SetAsLastSibling();
        Hp_Gague.fillAmount = transform.GetComponentInParent<CharaInfo>().Player_Hp / 100f;
    }
}
