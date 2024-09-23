using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;
    public static TeamManager TeamManager = new TeamManager();
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

