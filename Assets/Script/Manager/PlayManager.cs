using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    public const float WaitTime = 20f;
    public const float BattleTime = 30f;
    public const float InjuryTime = 30f;

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
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        bool isStart = false;
        if (isStart)
        {

            StartCoroutine(DisarmState());
        }
        yield return new WaitForSeconds(0.5f);

    }

    IEnumerator DisarmState()
    {
        float elapsedTime = 0f;

        while (elapsedTime < WaitTime)
        {
            elapsedTime += Time.deltaTime; // 매 프레임의 시간 합산
            yield return null;
        }

        StartCoroutine(BattleState());
    }

    IEnumerator BattleState()
    {
        float elapsedTime = 0f;

        while (elapsedTime < BattleTime)
        {
            elapsedTime += Time.deltaTime;
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
