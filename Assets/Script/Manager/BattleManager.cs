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

            photonView.RPC("RevertTeam", RpcTarget.All);
        }
    }

    [PunRPC]
    public void BattlePhaseRPC()
    {
        MatchTeam();
    }

    [PunRPC]
    public void DisarmPhaseRPC()
    {
        RevertTeam();

    }

    public void MatchTeam()
    {
        isMatched = true;
        var list = new List<int>() { 0, 1, 2, 3 };
        var random = new System.Random();
        var randomized = list.OrderBy(x => random.Next());
        int[] teamlist = new int[4];
        int count = 0;
        foreach (var i in randomized)
        {
            teamlist[count] = i;
            count++;
        }

        MatchTeamSet(teamlist[0], teamlist[1]);
        match1 = new Tuple<int, int>(teamlist[0], teamlist[1]);
        MatchTeamSet(teamlist[2], teamlist[3]);
        match2 = new Tuple<int, int>(teamlist[2], teamlist[3]);
        GameManager.isBattle = true;
    }

    private void MatchTeamSet(int team1, int team2)
    {
        TeamManager Team1 = teamManagers[team1];
        TeamManager Team2 = teamManagers[team2];
        match1 = new Tuple<int, int>(0, 1);

        Team1.SetHomeTeam("Team" + team1, "Team" + team2);
        Team2.SetAwayTeam(Team1.AwayTeam.transform, "Team" + team2, "Team" + team1);
    }

    [PunRPC]
    public void RevertTeam()
    {
        isMatched = false;
        foreach (TeamManager teamManager in teamManagers)
        {
            teamManager.RevertTeam();
        }
        GameManager.isBattle = false;
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

    public void ForceBattleEnd()
    {
        ForceBattleEnd(match1.Item1, match1.Item2);
        ForceBattleEnd(match2.Item1, match2.Item2);
    }

    public void ForceBattleEnd(int team1, int team2)
    {
        TeamManager Team1 = teamManagers[team1];
        TeamManager Team2 = teamManagers[team2];

        int team1RemainUnit = Team1.GetRemainUnit();
        int team2RemainUnit = Team2.GetRemainUnit();

        int damage = Mathf.Abs(team1RemainUnit - team2RemainUnit);

        if (team1RemainUnit > team2RemainUnit)
        {
            playerControllers[team1].GetDamage(damage);
        }
        else if (team1RemainUnit < team2RemainUnit)
        {
            playerControllers[team2].GetDamage(damage);
        }
        else
        {
            int drawDamage = 3;
            playerControllers[team1].GetDamage(drawDamage);
            playerControllers[team2].GetDamage(drawDamage);
        }
    }
}
