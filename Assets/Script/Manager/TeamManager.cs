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

    const int row = 8; const int col = 4; 
    public GameObject[,] unitLocate = new GameObject[row, col];
    public GameObject BattleArea;
    public GameObject BenchArea;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        } 

        foreach(Transform child in this.transform) {
            if(child.name == "BattleArea") {
                BattleArea = child.gameObject;
            }
            else if(child.name == "BenchArea") {
                BenchArea = child.gameObject;
            }
        }     
    }

    public void UnitLocateSave(Vector2Int UnitPos, GameObject Unit) {
        unitLocate[UnitPos.x, UnitPos.y] = Unit;
        
    }

    public void UnitLocateSwap(Vector2Int Unit1Pos, Vector2Int Unit2Pos) {
        (unitLocate[Unit1Pos.x, Unit1Pos.y], unitLocate[Unit2Pos.x, Unit2Pos.y]) = 
        (unitLocate[Unit2Pos.x, Unit2Pos.y], unitLocate[Unit1Pos.x, Unit1Pos.y]);
    }

    public void UnitListAdd(string Name) {UnitCheck.Add(Name, false);}

    public void UnitLocateAdd(GameObject Unit) {

    }

    public void UnitRelocation() {
        
    }


    
}
