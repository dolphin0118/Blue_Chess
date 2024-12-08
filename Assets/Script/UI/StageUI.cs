using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class StageUI : MonoBehaviour
{
    [SerializeField] Image timerBar;
    [SerializeField] TextMeshProUGUI timerCount;
    PhotonView photonView;

    float maxCount;
    float currentCount;

    Color baseColor;
    Color readyColor;

    void Awake()
    {
        maxCount = 30;
        currentCount = 0;
        baseColor = new Color(0, 225, 255);
        readyColor = Color.yellow;
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        float timerGauge = currentCount / maxCount;
        timerBar.fillAmount = timerGauge;
        timerCount.text = ((int)currentCount).ToString();
    }


    public void SetTimer(float maxCount, bool isChange)
    {
        photonView.RPC("SetTimerRPC", RpcTarget.All, maxCount, isChange);
    }

    [PunRPC]
    public void SetTimerRPC(float maxCount, bool isChange) {
        this.maxCount = maxCount;
        this.currentCount = maxCount;
        SetReadyColor(isChange);
        StopCoroutine(UpdateTimer());
        StartCoroutine(UpdateTimer());
    }

    public IEnumerator UpdateTimer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < maxCount)
        {
            elapsedTime += Time.deltaTime; // 매 프레임의 시간 합산
            float Timer = maxCount - elapsedTime;
            if (Timer <= 0) {
                Timer = 0f;
            }
            currentCount = Timer;
            yield return null;
        }
        
    }

    public void SetReadyColor(bool isChange)
    {
        if (isChange) timerBar.color = readyColor;
        else timerBar.color = baseColor;
    }


}
