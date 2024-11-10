using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatusUI : MonoBehaviour
{
    public Sprite[] Level_sprites;
    UnitStatus unitStatus;
    Transform cam;

    [SerializeField] Image hpImage;
    [SerializeField] Image mpImage;
    [SerializeField] Image levelImage;

    void Start()
    {
        cam = Camera.main.transform;    
        unitStatus = transform.GetComponentInParent<UnitStatus>();
        HPSetup();
        MPSetup();
    }
    void HPSetup() {
       // hpImage = transform.GetComponent<Image>();
        //hpGague.rectTransform.SetSiblingIndex(1);
        hpImage.fillAmount = 1.0f;
        if (hpImage == null) {
           Debug.LogError("Hp_front object not found");
        }
    }

    void MPSetup() {
        //mpImage = transform.GetComponent<Image>();
        mpImage.fillAmount = 1.0f;
        if (mpImage == null) {
           Debug.LogError("Hp_front object not found");
        }
    }
    
    void LevelSetup() {

    }

    void Update()
    {
        
    }
}
