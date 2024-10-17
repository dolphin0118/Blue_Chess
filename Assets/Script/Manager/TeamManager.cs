using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using JetBrains.Annotations;

public class TeamManager : MonoBehaviour
{
    const int row = 8; const int col = 4;
    public Dictionary<string, bool> UnitCheck = new Dictionary<string, bool>();
    public Dictionary<string, LevelData> UnitLevel = new Dictionary<string, LevelData>();
    public Dictionary<string, List<GameObject>> UnitObject = new Dictionary<string, List<GameObject>>();
    public Vector3 LerpPos;
    public GameObject[,] unitLocate = new GameObject[row, col];
    public GameObject BattleArea, BenchArea;
    public GameObject HomeTeam, AwayTeam;
    public GameObject UnitLocateController;
    private void Awake() {
        AreaSetup();
    }

    private void Start() {
        LerpPos = this.transform.parent.position;
        HomeTeam = this.gameObject;
        UnitListAdd();
    }

    public void Update(){
        InputSystem();
    }

    private void InputSystem() {
        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (unitLocate[i, j] != null)
                    {
                        Debug.Log(i + " " + j + ",");
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            UnitDeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnitRelocateAll();
        }
    }

    public void AreaSetup()
    {
        foreach (Transform child in this.transform)
        {
            if (child.name == "BattleArea")
            {
                BattleArea = child.gameObject;
            }
            else if (child.name == "BenchArea")
            {
                BenchArea = child.gameObject;
            }
        }
    }
   
    public void UnitLocateSave(Vector2Int UnitPos, GameObject Unit)
    {
        unitLocate[UnitPos.x, UnitPos.y] = Unit;
    }
    public void UnitLocateDelete(Vector2Int UnitPos)
    {
        if (UnitPos.x < 0 || UnitPos.x > row || UnitPos.y < 0 || UnitPos.y > col) return;
        unitLocate[UnitPos.x, UnitPos.y] = null;
    }
    public void UnitLocateSwap(Vector2Int Unit1Pos, Vector2Int Unit2Pos)
    {
        (unitLocate[Unit1Pos.x, Unit1Pos.y], unitLocate[Unit2Pos.x, Unit2Pos.y]) =
        (unitLocate[Unit2Pos.x, Unit2Pos.y], unitLocate[Unit1Pos.x, Unit1Pos.y]);
    }

    public bool UnitLocateCheck(Vector2Int UnitPos)
    {
        if (unitLocate[UnitPos.x, UnitPos.y] == null) return true;
        else return false;
    }

    public void UnitListAdd() { 
        foreach(string unitName in GameManager.instance.UnitList) {
            UnitCheck.Add(unitName, false); 
        }
            
     }

    public void UnitDeleteAll()
    {
        foreach (Transform child in BattleArea.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Unit"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void UnitRelocateAll()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (unitLocate[i, j] != null)
                {
                    GameObject respawnObject = unitLocate[i, j];
                    respawnObject.SetActive(true);
                    respawnObject.transform.SetParent(BattleArea.transform);
                    respawnObject.transform.localPosition = UnitPositionConvert(i, j);

                }
            }
        }
    }

    public Vector3 UnitPositionConvert(int row, int col)
    {
        int convertRow = row - 3;
        int convertCol = (col + 3) * -1;
        Vector3Int unitPos = new Vector3Int(convertRow, convertCol, 0);
        Vector3 unitPosition = GameManager.instance.tilemap.CellToLocal(unitPos);
        Debug.Log(unitPos + " " + unitPosition);
        return unitPosition;
    }


}
