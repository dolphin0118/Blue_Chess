using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;
    public PlayerController[] playerControllers;
    private GameObject holdPlayer;
    private FollowCam followCam;
    public int playerViewCode = 0;
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

    private void Update()
    {
        PlayerView();
    }

    void PlayerView()
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                playerViewCode = i;
            }
        }
    }

    void Match()
    {

    }
}

