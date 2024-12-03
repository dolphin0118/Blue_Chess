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
    private List<Tuple<int, int>> matchTeam;

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
        List<int> list = new List<int>() { };

        for (int i = 0; i < playerControllers.Length; i++)
        {
            if (playerControllers[i].gameObject.activeSelf)
            {
                list.Add(i);
            }
        }

        //var random = new System.Random();  
        var randomized = list.OrderBy(x => new System.Random().Next());
        int[] teamlist = new int[list.Count];

        int count = 0;
        foreach (var randomNum in randomized)
        {
            teamlist[count] = randomNum;
            count++;
        }

        return teamlist;
    }

    public void MatchTeam(int[] teamlist)
    {
        matchTeam = new List<Tuple<int, int>>();
        MatchTeamSetup(teamlist[0], teamlist[1]);
        matchTeam.Add(new Tuple<int, int>(teamlist[0], teamlist[1]));

        if (teamlist.Length >= 4)
        {
            MatchTeamSetup(teamlist[2], teamlist[3]);
            matchTeam.Add(new Tuple<int, int>(teamlist[2], teamlist[3]));
        }

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
        foreach (Tuple<int, int> team in matchTeam)
        {
            bool currentMatch = IsBothBattleEnd(team);
            if (!currentMatch) return false;
        }
        return true;
    }

    public bool IsBothBattleEnd(Tuple<int, int> teams)
    {
        TeamManager Team1 = teamManagers[teams.Item1];
        TeamManager Team2 = teamManagers[teams.Item2];

        if (Team1.IsBattleEndCheck() || Team2.IsBattleEndCheck())
        {
            return true;
        }

        return false;
    }

    public void BattleEnd()
    {
        if (PhotonNetwork.IsMasterClient && isMatched)
        {
            photonView.RPC("BattleEndRPC", RpcTarget.All);
        }

    }

    [PunRPC]
    public void BattleEndRPC()
    {
        foreach (Tuple<int, int> team in matchTeam)
        {
            CalculateDamage(team);
        }

    }

    public void CalculateDamage(Tuple<int, int> teams)
    {
        TeamManager Team1 = teamManagers[teams.Item1];
        TeamManager Team2 = teamManagers[teams.Item2];

        int team1RemainUnit = Team1.GetRemainUnit();
        int team2RemainUnit = Team2.GetRemainUnit();

        int damage = Mathf.Abs(team1RemainUnit - team2RemainUnit);

        if (team1RemainUnit > team2RemainUnit)
        {
            playerControllers[teams.Item2].GetDamage(damage);
        }
        else if (team1RemainUnit < team2RemainUnit)
        {
            playerControllers[teams.Item1].GetDamage(damage);
        }
        else
        {
            int drawDamage = 2;
            playerControllers[teams.Item1].GetDamage(drawDamage);
            playerControllers[teams.Item2].GetDamage(drawDamage);
        }
    }
}
