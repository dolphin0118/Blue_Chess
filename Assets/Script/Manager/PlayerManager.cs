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
    public PlayerController[] playerController;
    private GameObject holdPlayer;
    public int playerCode = 0;
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
        playerController = FindObjectsOfType<PlayerController>();
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
                playerCode = playerCount;
            }
        }
    }
}

