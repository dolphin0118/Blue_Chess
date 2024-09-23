using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SpawnSystem : MonoBehaviour
{
    public static SpawnSystem instance = null;
    public UnitCard[] UnitCards;
    bool[] checkSlot = new bool[9];
    private GameObject BenchArea;
    public TeamManager teamManager;
    public UnitCombine unitCombine;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this) Destroy(this.gameObject);
        }
        unitCombine = PlayerManager.instance.playerController[0].unitCombine;
        teamManager = PlayerManager.instance.playerController[0].TeamManager;
        BenchArea = teamManager.BenchArea;
        UnitCards = Resources.LoadAll<UnitCard>("Scriptable");
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

                GameObject UnitClone = Instantiate(UnitCard.UnitPrefab, prefabPos, Quaternion.identity);
                UnitClone.transform.SetParent(BenchArea.transform.GetChild(i));
                UnitClone.gameObject.tag = "Friendly";
                UnitClone.GetComponent<UnitManager>().Initialize(teamManager, unitCombine, UnitCard);
                break;
            }
        }
    }
}
