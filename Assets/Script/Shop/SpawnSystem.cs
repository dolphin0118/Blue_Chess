using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SpawnSystem : MonoBehaviour
{
    public TeamManager teamManager;
    public SynergyManager synergyManager;
    public UnitCombine unitCombine;

    public UnitCard[] UnitCards;
    bool[] checkSlot = new bool[9];
    private GameObject BenchArea;

    
    void Awake()
    {
        UnitCards = Resources.LoadAll<UnitCard>("Scriptable");
    }

    public void Initialize(TeamManager teamManager, SynergyManager synergyManager, UnitCombine unitCombine) {
        this.teamManager = teamManager;
        this.synergyManager = synergyManager;
        this.unitCombine = unitCombine;
        BenchArea = this.teamManager.BenchArea;
    }

    public bool isSpawnable()
    {
        bool isSpawn = false;
        for (int i = 0; i < checkSlot.Length; i++)
        {
            if (BenchArea.transform.GetChild(i).childCount == 0)
            {
                checkSlot[i] = false;
                isSpawn = true;
            }
            else checkSlot[i] = true;
        }
        return isSpawn;
    }

    public void SpawnUnit(UnitCard UnitCard)
    {
        for (int i = 0; i < checkSlot.Length; i++)
        {
            if (!checkSlot[i])
            {
                Vector3 spawnPos = new Vector3(-4, 0.1f, -6);
                Vector3 prefabPos = new Vector3(spawnPos.x + i, spawnPos.y, spawnPos.z);
                Vector3Int tilepos = GameManager.instance.tilemap.LocalToCell(prefabPos);
                prefabPos = GameManager.instance.tilemap.GetCellCenterLocal(tilepos);
                prefabPos = new Vector3(prefabPos.x, 0.1f, prefabPos.z);

                GameObject UnitClone = Instantiate(UnitCard.UnitPrefab, Vector3.zero, Quaternion.identity);
                UnitClone.transform.SetParent(BenchArea.transform.GetChild(i));
                UnitClone.transform.localPosition = Vector3.zero;
                UnitClone.gameObject.tag = "Friendly";
                UnitClone.GetComponent<UnitManager>().Initialize(teamManager, synergyManager, unitCombine, UnitCard);
                break;
            }
        }
    }
}
