using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonInit1 : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");

        // 방이 없으면 방을 생성하거나 기존 방에 참가
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8; //최대 8명의 플레이어가 참가 가능
        PhotonNetwork.JoinOrCreateRoom("TestRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("No Room");
        PhotonNetwork.CreateRoom("MyRoom");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Finish make a room");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");
        if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient)
        {
            PlayerManager.instance.AssignPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
            PlayerManager.instance.playerViewCode = 1;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("ENTER");
        PlayerManager.instance.AssignPlayer(newPlayer.ActorNumber); // 새로운 플레이어가 들어오면 소유권을 할당
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    // private void OnEnable()
    // {
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // private void OnDisable()
    // {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "Main")
    //     {
    //         PlayerManager.instance.AssignPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
    //     }
    // }

}
