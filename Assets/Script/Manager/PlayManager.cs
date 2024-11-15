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

    private void Start() {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame() {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DisarmState());
    }

    IEnumerator DisarmState() {
        while(Time.deltaTime < WaitTime) {

            
        }
        yield return null;
        StartCoroutine(BattleState());
    }

    IEnumerator BattleState() {
        while(Time.deltaTime < BattleTime) {

            
        }
        yield return null;
        StartCoroutine(DisarmState());
    }

    void Update()
    {
        bool isEnd = false;
        if(isEnd) StopAllCoroutines();
    }
}
