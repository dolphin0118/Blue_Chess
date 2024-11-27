using System;
using System.Linq;
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
    private List<TeamManager> teamManagers;
    private PlayerController[] playerControllers;
    private PhotonView photonView;
    private bool isMatched = false;
    private Tuple<int, int> match1;
    private Tuple<int, int> match2;

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
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        playerControllers = PlayerManager.instance.playerControllers;

        teamManagers = playerControllers
        .Select(playerController => playerController.TeamManager)
        .ToList();
    }

    public void BattleReadyPhase()
    {
        int[] teamlist = RandomTeamList();
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("BattleReadyRPC", RpcTarget.All, teamlist);
        }
    }

    public void BattlePhase()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("BattlePhaseRPC", RpcTarget.All);
        }
    }

    public void DisarmPhase()
    {
        if (PhotonNetwork.IsMasterClient && isMatched)
        {

            photonView.RPC("DisarmPhaseRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void BattlePhaseRPC()
    {
        GameManager.isBattle = true;
    }

    [PunRPC]
    public void BattleReadyRPC(int[] teamlist)
    {
        MatchTeam(teamlist);
    }

    [PunRPC]
    public void DisarmPhaseRPC()
    {
        RevertTeam();
    }

    public int[] RandomTeamList()
    {
        isMatched = true;
        List<int> list = new List<int>() { 0, 1, 2, 3 };
        var random = new System.Random();
        var randomized = list.OrderBy(x => random.Next());
        int[] teamlist = new int[4];
        int count = 0;
        foreach (var i in randomized)
        {
            teamlist[count] = i;
            count++;
        }

        return teamlist;
    }

    public void MatchTeam(int[] teamlist)
    {
        MatchTeamSetup(teamlist[0], teamlist[1]);
        match1 = new Tuple<int, int>(teamlist[0], teamlist[1]);
        MatchTeamSetup(teamlist[2], teamlist[3]);
        match2 = new Tuple<int, int>(teamlist[2], teamlist[3]);
    }

    private void MatchTeamSetup(int team1, int team2)
    {
        teamManagers[team1].SetHomeTeam("Team" + team1, "Team" + team2);
        teamManagers[team2].SetAwayTeam(teamManagers[team1].AwayTeam.transform, "Team" + team2, "Team" + team1);
        playerControllers[team2].SetAwayViewTarget(playerControllers[team1].AwayViewTarget);
    }


    public void RevertTeam()
    {
        GameManager.isBattle = false;
        isMatched = false;
        foreach (TeamManager teamManager in teamManagers)
        {
            teamManager.RevertTeam();
        }

    }

    public bool IsBattleEnd()
    {
        bool firstMatch = IsBothBattleEnd(match1.Item1, match1.Item2);
        bool secondMatch = IsBothBattleEnd(match2.Item1, match2.Item2);

        if (!firstMatch || !secondMatch) return false;
        return true;
    }

    public bool IsBothBattleEnd(int team1, int team2)
    {
        TeamManager Team1 = teamManagers[team1];
        TeamManager Team2 = teamManagers[team2];

        if (Team1.IsBattleEndCheck() || Team2.IsBattleEndCheck())
        {
            return true;
        }

        return false;
    }

    public void BattleEnd()
    {
        CalculateDamage(match1.Item1, match1.Item2);
        CalculateDamage(match2.Item1, match2.Item2);
    }

    public void CalculateDamage(int team1, int team2)
    {
        TeamManager Team1 = teamManagers[team1];
        TeamManager Team2 = teamManagers[team2];

        int team1RemainUnit = Team1.GetRemainUnit();
        int team2RemainUnit = Team2.GetRemainUnit();

        int damage = Mathf.Abs(team1RemainUnit - team2RemainUnit);

        if (team1RemainUnit > team2RemainUnit)
        {
            playerControllers[team2].GetDamage(damage);
        }
        else if (team1RemainUnit < team2RemainUnit)
        {
            playerControllers[team1].GetDamage(damage);
        }
        else
        {
            int drawDamage = 3;
            playerControllers[team1].GetDamage(drawDamage);
            playerControllers[team2].GetDamage(drawDamage);
        }
    }
}
