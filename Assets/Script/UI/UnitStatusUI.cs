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
        Setup();
    }
    void Setup() {
        hpImage.fillAmount = 1.0f;
        if (hpImage == null) {
           Debug.LogError("Hp_front object not found");
        }
        mpImage.fillAmount = 1.0f;
        if (mpImage == null) {
           Debug.LogError("Hp_front object not found");
        }
    }


    void HPUpdate() {
        float gagueValue =  unitStatus.currentHP / unitStatus.HP;
        hpImage.fillAmount = gagueValue;
    }

    void MPUpdate() {
        float gagueValue = unitStatus.currentMP / unitStatus.MP;
        mpImage.fillAmount = gagueValue;
    }

    void LevelSetup() {
        int UnitLevel = GetComponentInParent<UnitInfo>().unitStatus.Level;
        levelImage.sprite = Level_sprites[UnitLevel - 1];
    }
    void Update() {
        swapSprite();
        HPUpdate();
        MPUpdate();
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);

    }

    void swapSprite() {
        int UnitLevel = GetComponentInParent<UnitInfo>().unitStatus.Level;
        levelImage.sprite = Level_sprites[UnitLevel - 1];
    }

}
