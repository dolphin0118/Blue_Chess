using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    private Tuple<int, int> Match_1;
    private Tuple<int, int> Match_2;
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
        if(GameManager.isBattle) {

        }
    }

    public void BattlePhase() {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MatchTeam", RpcTarget.All);
        }
    }

    public void DisarmPhase() {
        if (PhotonNetwork.IsMasterClient && isMatched)
        {
            photonView.RPC("RevertTeam", RpcTarget.All);
        }
    }


    [PunRPC]
    public void MatchTeam()
    {
        isMatched = true;
        int team1 = UnityEngine.Random.Range(0, 4);
        int team2 = 0;
        while (team1 != team2) team2 = UnityEngine.Random.Range(0, 4);
        MatchTeamSet(team1, team2);
        GameManager.isBattle = true;
    }
    
    private void MatchTeamSet(int team1, int team2)
    {
        TeamManager Team1 = PlayerManager.instance.playerControllers[0].TeamManager;
        TeamManager Team2 = PlayerManager.instance.playerControllers[1].TeamManager;
        Match_1 = new Tuple<int, int>(0, 1);
        // TeamManager Team1 = PlayerManager.instance.playerControllers[team1].TeamManager;
        // TeamManager Team2 = PlayerManager.instance.playerControllers[team2].TeamManager;

        Team1.SetHomeTeam();
        Team2.SetAwayTeam(Team1.AwayTeam.transform);
    }

    [PunRPC]
    public void RevertTeam() {     
        isMatched = false;
        GameManager.isBattle = false;
        foreach (PlayerController currentController in PlayerManager.instance.playerControllers) {
             currentController.TeamManager.RevertTeam();
        }   
    }

    public bool IsBattleEnd() {
        foreach (PlayerController currentController in PlayerManager.instance.playerControllers) {
            bool currentCheck = currentController.TeamManager.IsBattleEndCheck();
            if(!currentCheck) return false;
        }   
        return true;
    }

    public bool IsBothBattleEnd(int team1, int team2) {
        TeamManager Team1 = PlayerManager.instance.playerControllers[0].TeamManager;
        TeamManager Team2 = PlayerManager.instance.playerControllers[1].TeamManager;

        if(Team1.IsBattleEndCheck() || Team1.IsBattleEndCheck());

        return true;
    }
}
