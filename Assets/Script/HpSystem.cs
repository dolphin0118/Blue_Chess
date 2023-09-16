using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpSystem : MonoBehaviour {
    List<GameObject> Hp_Objects;
    Transform cam;
    Image Hp_Gague;
    void Awake(){   
        Hp_Objects = new List<GameObject>();
        cam = Camera.main.transform;      
        Hp_Gague = GetComponentInChildren<Image>();
        Init();
    }
    void Init() {
        for(int i = 0; i < transform.childCount; i++) {
            Hp_Objects.Add(this.transform.GetChild(i).gameObject);
            Hp_Objects[i].GetComponent<Image>().rectTransform.SetSiblingIndex(i);
        }
    }
    void Update() {
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        Hp_Gague.rectTransform.SetAsLastSibling();
        Hp_Gague.fillAmount = transform.GetComponentInParent<CharaInfo>().Player_Hp / 100f;
    }
}
