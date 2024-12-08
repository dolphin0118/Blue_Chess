using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;
    private PhotonView photonView;

    [SerializeField] StageUI stageUI;
    [SerializeField] GameObject lobbyUI;
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera lobbyCamera;


    public const float WaitTime = 20f;
    public const float BattleTime = 30f;
    public const float OverTime = 30f;
    public const float ReadyTime = 5;

    public bool isReady = false;
    public bool isStart = false;

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
        SwitchToLobbyCamera();
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        StartCoroutine(StartGameCheck());
    }
    
    void Update()
    {
        bool isEnd = false;
        if (isEnd) StopAllCoroutines();
    }

    [PunRPC]
    public void SwitchToMainCamera()
    {
        mainCamera.enabled = true;
        lobbyCamera.enabled = false;
        lobbyUI.SetActive(false);
        isStart = true;
    }

    public void SwitchToLobbyCamera()
    {
        mainCamera.enabled = false;
        lobbyCamera.enabled = true;
    }

    IEnumerator StartGameCheck()
    {
        while (!PhotonNetwork.IsConnected || !PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isReady = true;
    }

    public void StartGame()
    {
        if (isReady)
        {
            photonView.RPC("SwitchToMainCamera", RpcTarget.All);
            StartCoroutine(DisarmState());
        }
    }

    IEnumerator DisarmReadyState()
    {
        float elapsedTime = 0f;
        stageUI.SetTimer(ReadyTime, true);
        //BattleManager.instance.DisarmReadyPhase();
        while (elapsedTime < ReadyTime)
        {
            elapsedTime += Time.deltaTime; // 매 프레임의 시간 합산
            float Timer = ReadyTime - elapsedTime;
            if (Timer <= 0) Timer = 0f;
            yield return null;
        }
        StartCoroutine(DisarmState());
    }

    IEnumerator DisarmState()
    {
        float elapsedTime = 0f;

        stageUI.SetTimer(WaitTime, false);
        BattleManager.instance.DisarmPhase();

        while (elapsedTime < WaitTime)
        {
            elapsedTime += Time.deltaTime;
            float Timer = WaitTime - elapsedTime;
            if (Timer <= 0) Timer = 0f;

            yield return null;
        }

        StartCoroutine(BattleReadyState());
    }

    IEnumerator BattleReadyState()
    {
        float elapsedTime = 0f;

        stageUI.SetTimer(ReadyTime, true);
        BattleManager.instance.BattleReadyPhase();

        while (elapsedTime < ReadyTime)
        {
            elapsedTime += Time.deltaTime; // 매 프레임의 시간 합산
            float Timer = ReadyTime - elapsedTime;
            if (Timer <= 0) Timer = 0f;

            yield return null;
        }
        StartCoroutine(BattleState());
    }

    IEnumerator BattleState()
    {
        float elapsedTime = 0f;
        bool isBattleEnd = false;
        bool isOverTime = false;

        stageUI.SetTimer(BattleTime, false);
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
                break;
            }

            float Timer = BattleTime - elapsedTime;
            if (Timer <= 0) Timer = 0f;

            isBattleEnd = BattleManager.instance.IsBattleEnd(); // End체크

            yield return null;
        }
        BattleManager.instance.BattleEnd();
        StartCoroutine(DisarmReadyState());
    }

}
