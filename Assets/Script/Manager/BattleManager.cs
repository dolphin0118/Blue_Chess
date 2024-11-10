using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;
    private TeamManager[] teamManagers;
    private PhotonView photonView;
    private bool isMatched = false;
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
        photonView = GetComponent<PhotonView>();
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.M))
        {
            photonView.RPC("MatchTeam", RpcTarget.All);
        }

        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.N) && isMatched)
        {
            photonView.RPC("RevertTeam", RpcTarget.All);
        }
    }

    [PunRPC]
    public void MatchTeam()
    {
        isMatched = true;
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

        Team1.SetHomeTeam();
        Team2.SetAwayTeam(Team1.AwayTeam.transform);
    }

    [PunRPC]
    public void RevertTeam() {
        
        isMatched = false;
        foreach (PlayerController currentController  in PlayerManager.instance.playerControllers) {
             currentController.TeamManager.RevertTeam();
        }
    }


}
