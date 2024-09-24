using System.Collections;
using System.Collections.Generic;
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
    public void MatchTeam() {
        
    }
}
