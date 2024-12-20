using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public static PhotonInit instance;

    public InputField playerInput;
    public Button chattingBtn;


    bool isGameStart = false;
    bool isLoggIn = false;
    bool isReady = false;
    string playerName = "";
    string connectionState = "";
    public string chatMessage;
    Text chatText;
    ScrollRect scroll_rect = null;
    PhotonView pv;

    Text connectionInfoText;

    [Header("LobbyCanvas")] public GameObject LobbyCanvas;
    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public InputField RoomInput;
    public InputField RoomPwInput;
    public Toggle PwToggle;
    public GameObject PwPanel;
    public GameObject PwErrorLog;
    public GameObject PwConfirmBtn;
    public GameObject PwPanelCloseBtn;
    public InputField PwCheckIF;
    public bool LockState = false;
    public string privateroom;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Button CreateRoomBtn;
    public int hashtablecount;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple, roomnumber;

    private void Awake()
    {

        PhotonNetwork.GameVersion = "MyFps 1.0";
        PhotonNetwork.ConnectUsingSettings();

        if (GameObject.Find("ChatText") != null)
            chatText = GameObject.Find("ChatText").GetComponent<Text>();

        if (GameObject.Find("Scroll View") != null)
            scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();

        if (GameObject.Find("ConnectionInfoText") != null)
            connectionInfoText = GameObject.Find("ConnectionInfoText").GetComponent<Text>();

        connectionState = "������ ������ ���� ��...";

        if (connectionInfoText)
            connectionInfoText.text = connectionState;

        DontDestroyOnLoad(gameObject);
    }

    public static PhotonInit Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if (instance == null)
                    Debug.Log("no singleton obj");
            }

            return instance;
        }
    }



    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby");
    }

    public void Connect()
    {

        if (PhotonNetwork.IsConnected && isReady)
        {
            connectionState = "�뿡 ����...";
            if (connectionInfoText)
                connectionInfoText.text = connectionState;

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);


            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionState = "";
            if (connectionInfoText)
                connectionInfoText.text = connectionState;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionState = "No Room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("No Room");
    }



    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        connectionState = "Finish make a room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("Finish make a room");

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("OnCreateRoomFailed:" + returnCode + "-" + message);
    }



    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        connectionState = "Joined Room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("Joined Room");
        isLoggIn = true;
        PlayerPrefs.SetInt("LogIn", 1);

        PhotonNetwork.LoadLevel("Main");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LogIn", 0);
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("LogIn") == 1)
            isLoggIn = true;

        if (isGameStart == false && SceneManager.GetActiveScene().name == "SampleScene" && isLoggIn == true)
        {
            Debug.Log("Update :" + isGameStart + ", " + isLoggIn);
            isGameStart = true;
            if (GameObject.Find("ChatText") != null)
                chatText = GameObject.Find("ChatText").GetComponent<Text>();

            if (GameObject.Find("Scroll View") != null)
                scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();

            if (GameObject.Find("InputFieldChat") != null)
                playerInput = GameObject.Find("InputFieldChat").GetComponent<InputField>();

            if (GameObject.Find("ChattingButton") != null)
            {
                chattingBtn = GameObject.Find("ChattingButton").GetComponent<Button>();
                chattingBtn.onClick.AddListener(SetPlayerName);
            }


            StartCoroutine(CreatePlayer());
        }
    }

    IEnumerator CreatePlayer()
    {
        while (!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }

    private void OnGUI()
    {
        GUILayout.Label(connectionState);
    }

    public void SetPlayerName()
    {
        Debug.Log(playerInput.text + "�� �Է� �ϼ̽��ϴ�!" + isGameStart + ", " + isLoggIn);

        if (isGameStart == false && isLoggIn == false)
        {
            playerName = playerInput.text;
            playerInput.text = string.Empty;
            Debug.Log("connect �õ�!" + isGameStart + ", " + isLoggIn);
            Connect();

        }
        else
        {
            chatMessage = playerInput.text;
            playerInput.text = string.Empty;
            pv.RPC("ChatInfo", RpcTarget.All, chatMessage);
        }

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToServer");
        isReady = true;
    }

    #region �� ���� �� ���� ���� �޼���
    public void CreateRoomBtnOnClick()
    {
        MakeRoomPanel.SetActive(true);
    }

    public void OKBtnOnClick()
    {
        MakeRoomPanel.SetActive(false);
    }


    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 20) : RoomInput.text,
               new RoomOptions { MaxPlayers = 20 });
        LobbyPanel.SetActive(false);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        connectionState = "������ ������ ���� ��...";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        isGameStart = false;
        isLoggIn = false;
        PlayerPrefs.SetInt("LogIn", 0);
    }


    public void CreateNewRoom()
    {

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            {"password", RoomPwInput.text}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 20) : "*" + RoomInput.text,
                roomOptions);
        }

        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 20) : RoomInput.text,
                new RoomOptions { MaxPlayers = 20 });
        }

        MakeRoomPanel.SetActive(false);
        //LobbyCanvas.SetActive(false);

    }

    public void MyListClick(int num)
    {

        if (num == -2)
        {
            --currentPage;
            MyListRenewal();
        }
        else if (num == -1)
        {
            ++currentPage;
            MyListRenewal();
        }

        else if (myList[multiple + num].CustomProperties["password"] != null)
        {
            PwPanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            MyListRenewal();

        }

    }

    public void RoomPw(int number)
    {
        switch (number)
        {
            case 0:
                roomnumber = 0;
                break;
            case 1:
                roomnumber = 1;
                break;
            case 2:
                roomnumber = 2;
                break;
            case 3:
                roomnumber = 3;
                break;

            default:
                break;
        }


    }

    public void EnterRoomWithPW()
    {
        if ((string)myList[multiple + roomnumber].CustomProperties["password"] == PwCheckIF.text)
        {
            PhotonNetwork.JoinRoom(myList[multiple + roomnumber].Name);
            MyListRenewal();
            PwPanel.SetActive(false);
        }

        else
        {
            StartCoroutine("ShowPwWrongMsg");
        }


    }

    IEnumerator ShowPwWrongMsg()
    {
        if (!PwErrorLog.activeSelf)
        {
            PwErrorLog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            PwErrorLog.SetActive(false);
        }
    }

    void MyListRenewal()
    {
        maxPage = (myList.Count % CellBtn.Length == 0)
            ? myList.Count / CellBtn.Length
            : myList.Count / CellBtn.Length + 1;

        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text =
                (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
        }

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate:" + roomList.Count);
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }

        MyListRenewal();

    }
    #endregion


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            PlayerManager.instance.AssignPlayer(PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }
}
