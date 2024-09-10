using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
public class TeamManager : MonoBehaviour {
    public static TeamManager instance = null;
    public static Dictionary<string, bool> UnitCheck= new Dictionary<string, bool>();
    public static Dictionary<string, LevelData> UnitLevel = new Dictionary<string, LevelData>();
    public static Dictionary<string, List<GameObject>> UnitObject = new Dictionary<string, List<GameObject>>();

    private GameObject BattleArea;
    private GameObject BenchArea;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }     
        BattleArea = GameObject.FindGameObjectWithTag("BattleArea");
        BenchArea = GameObject.FindGameObjectWithTag("BenchArea");
    }

    public void UnitListAdd(string Name) {UnitCheck.Add(Name, false);}

    public void UnitRelocation() {
        
    }

    
}
