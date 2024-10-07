using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;
    private TeamManager[] TeamManagers;
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this) Destroy(this.gameObject);
        }
        TeamManagers = FindObjectsOfType<TeamManager>();
    }
    public void Update() {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MatchTeam();
        }
    }

    public void MatchTeam() {
        TeamManager Team1 = PlayerManager.instance.playerControllers[0].TeamManager;
        TeamManager Team2 = PlayerManager.instance.playerControllers[1].TeamManager;

        GameObject HomeTeam = Team1.HomeTeam;
        GameObject AwayTeam = Team2.HomeTeam;
        AwayTeam.transform.SetParent(Team1.AwayTeam.transform);
        AwayTeam.transform.localPosition = Vector3.zero;
        AwayTeam.transform.localRotation = Quaternion.identity;
        
        foreach (Transform unit in HomeTeam.transform) {
            unit.tag = "Home";
        }
        foreach (Transform unit in AwayTeam.transform) {
            unit.tag = "Away";
        }

    }
}
