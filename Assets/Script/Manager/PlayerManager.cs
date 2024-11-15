using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;
    public GameObject[] players;
    public PlayerController[] playerControllers;
    private GameObject holdPlayer;
    private FollowCam followCam;
    public int playerViewCode = 1;
    const int playerCount = 8;

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
        playerControllers = FindObjectsOfType<PlayerController>();
     
    }
    void Start()
    {


    }

    IEnumerator AssignPlayer()
    {
        // 모든 플레이어 Prefab을 순회하며 아직 소유자가 없는 것을 찾음
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            
            // PhotonView의 소유자가 없는 경우 (0은 소유자가 없다는 의미)
            if (photonView.Owner == null)
            {

                // 소유권을 현재 로컬 플레이어로 변경
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} is assigned to {player.name}");
                // 배정 후 더 이상 반복할 필요가 없으므로 종료
                break;
            }
        }
        yield return null;
    }

    public void AssignPlayer(int ActorNumber)
    {
        // 모든 플레이어 Prefab을 순회하며 아직 소유자가 없는 것을 찾음
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            
            // PhotonView의 소유자가 없는 경우 (0은 소유자가 없다는 의미)
            if (photonView.Owner == null)
            {
                // 소유권을 해당 로컬 플레이어로 변경
                photonView.TransferOwnership(ActorNumber);
                Debug.Log($"Player {ActorNumber} is assigned to {player.name}");
                // 배정 후 더 이상 반복할 필요가 없으므로 종료
                break;
            }
        }
    }

    private void Update()
    {
        PlayerView();
    }

    void PlayerView()
    {
        for (int i = 1; i <= playerCount; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                playerViewCode = i;
            }
        }
    }

    public void SetPlayerView(int viewCode) {
        playerViewCode = viewCode;
    }

}

