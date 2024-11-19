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

    Color baseColor;
    Color readyColor;

    void Awake()
    {
        maxCount = 0;
        currentCount = 0;
        baseColor = new Color(0, 225, 255);
        readyColor = Color.yellow;
    }

    void Update()
    {
        float timerGauge = currentCount / maxCount;
        timerBar.fillAmount = timerGauge;
        timerCount.text = ((int)currentCount).ToString();
    }

    public void SetTimer(float maxCount)
    {
        this.maxCount = maxCount;
        this.currentCount = 0;
    }

    public void UpdateTimer(float currentTimer)
    {
        currentCount = currentTimer;
    }

    public void SetReadyColor(bool isChange)
    {
        if (isChange) timerBar.color = readyColor;
        else timerBar.color = baseColor;
    }
}
