using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStatusUI : MonoBehaviour
{
    private UnitStatus unitStatus;
    private TeamManager teamManager;

    [SerializeField] Sprite[] Level_sprites;
    [SerializeField] Image hpImage;
    [SerializeField] Image mpImage;
    [SerializeField] Image levelImage;

    private Transform cam;
    Color homeTeamColor;
    Color awayTeamColor;

    void Start()
    {
        cam = Camera.main.transform;
    }

    public void Initialize(TeamManager teamManager, UnitStatus unitStatus)
    {
        this.teamManager = teamManager;
        this.unitStatus = unitStatus;
        Setup();
    }

    void Setup()
    {
        homeTeamColor = new Color(0, 255, 0); // Home Color
        awayTeamColor = new Color(255, 255, 0); // Away Color

        hpImage.fillAmount = 1.0f;
        if (hpImage == null)
        {
            Debug.LogError("Hp_front object not found");
        }
        mpImage.fillAmount = 1.0f;
        if (mpImage == null)
        {
            Debug.LogError("Hp_front object not found");
        }
    }

    void Update()
    {
        HPUpdate();
        MPUpdate();
        LevelUpdate();
        transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
    }

    void HPUpdate()
    {
        float gagueValue = unitStatus.currentHP / unitStatus.HP;
        hpImage.fillAmount = gagueValue;
        hpImage.color = SetTeamColor();
    }

    void MPUpdate()
    {
        float gagueValue = unitStatus.currentMP / unitStatus.MP;
        mpImage.fillAmount = gagueValue;
    }

    void LevelUpdate()
    {
        int UnitLevel = unitStatus.Level;
        levelImage.sprite = Level_sprites[UnitLevel - 1];
    }

    Color SetTeamColor()
    {
        if (teamManager.isAwayTeam)
        {
            return awayTeamColor;
        }
        else return homeTeamColor;

    }

}
