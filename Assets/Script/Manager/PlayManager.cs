using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;
    [SerializeField] StageUI stageUI;

    public const float WaitTime = 20f;
    public const float BattleTime = 30f;
    public const float OverTime = 30f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this) Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(StartGame());
        }

    }

    IEnumerator StartGame()
    {
        while (!PhotonNetwork.IsConnected)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(DisarmState());

    }

    IEnumerator DisarmState()
    {
        float elapsedTime = 0f;
        stageUI.SetTimer(WaitTime);
        BattleManager.instance.DisarmPhase();

        while (elapsedTime < WaitTime)
        {
            elapsedTime += Time.deltaTime;
            float Timer = WaitTime - elapsedTime;
            if (Timer <= 0) Timer = 0f;
            stageUI.UpdateTimer(Timer);

            yield return null;
        }

        StartCoroutine(BattleState());
    }

    IEnumerator BattleState()
    {
        float elapsedTime = 0f;
        bool isBattleEnd = false;
        bool isOverTime = false;
        stageUI.SetTimer(BattleTime);
        BattleManager.instance.BattlePhase();

        while (elapsedTime < BattleTime && !isBattleEnd)
        {
            elapsedTime += Time.deltaTime; // 매 프레임의 시간 합산
            if (elapsedTime > BattleTime && !isOverTime) //전투 시간 오버시
            {
                isOverTime = true;
                elapsedTime = 0f;
            }
            else if (elapsedTime > OverTime && isOverTime) //오버시간 끝
            {
                GameManager.isBattle = false;
                BattleManager.instance.ForceBattleEnd();
                break;
            }

            float Timer = BattleTime - elapsedTime;
            if (Timer <= 0) Timer = 0f;
            stageUI.UpdateTimer(Timer);

            isBattleEnd = BattleManager.instance.IsBattleEnd(); // End체크

            yield return null;
        }

        StartCoroutine(DisarmState());
    }

    void Update()
    {
        bool isEnd = false;
        if (isEnd) StopAllCoroutines();
    }
}
