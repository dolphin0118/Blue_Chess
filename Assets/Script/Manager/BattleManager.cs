using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;
    private TeamManager[] teamManagers;
    private PhotonView photonView;

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
        teamManagers = FindObjectsOfType<TeamManager>();
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.M))
        {
            photonView.RPC("MatchTeam", RpcTarget.All);
        }
    }

    [PunRPC]
    public void MatchTeam()
    {
        int team1 = Random.Range(0, 4);
        int team2 = 0;
        while (team1 != team2) team2 = Random.Range(0, 4);
        MatchTeamSet(team1, team2);
    }

    private void MatchTeamSet(int team1, int team2)
    {
        TeamManager Team1 = PlayerManager.instance.playerControllers[0].TeamManager;
        TeamManager Team2 = PlayerManager.instance.playerControllers[1].TeamManager;

        // TeamManager Team1 = PlayerManager.instance.playerControllers[team1].TeamManager;
        // TeamManager Team2 = PlayerManager.instance.playerControllers[team2].TeamManager;

        GameObject HomeTeam = Team1.HomeTeam;
        GameObject AwayTeam = Team2.HomeTeam;
        AwayTeam.transform.SetParent(Team1.AwayTeam.transform);
        AwayTeam.transform.localPosition = Vector3.zero;
        AwayTeam.transform.localRotation = Quaternion.identity;

        foreach (Transform unit in HomeTeam.transform)
        {
            unit.tag = "Home";
        }
        foreach (Transform unit in AwayTeam.transform)
        {
            unit.tag = "Away";
        }
    }
}
