using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [SerializeField] Image timerBar;
    [SerializeField] TextMeshProUGUI timerCount;
    float maxCount;
    float currentCount;

    void Awake() {
        maxCount = 0;
        currentCount = 0;
    }

    void Update()
    {
        float timerGauge = currentCount/maxCount;
        timerBar.fillAmount = timerGauge;
        timerCount.text = ((int)currentCount).ToString();
    }
    
    public void SetTimer(float maxCount) {
        this.maxCount = maxCount;
        this.currentCount = 0;
    }

    public void UpdateTimer(float currentTimer) {
        currentCount = currentTimer;
    }
}
