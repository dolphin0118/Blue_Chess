using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using JetBrains.Annotations;
using TMPro;

public class TeamManager : MonoBehaviour
{
    public Dictionary<string, int> UnitCheck = new Dictionary<string, int>(); //Synergy체크용
    public Dictionary<string, LevelData> UnitLevel = new Dictionary<string, LevelData>(); //Combine용
    public Dictionary<string, List<GameObject>> UnitObject = new Dictionary<string, List<GameObject>>();//

    public GameObject BattleArea, BenchArea;
    public GameObject HomeTeam, AwayTeam;
    public GameObject UnitLocateController;
    public GameObject GridView;
    public GameObject UnitCapacityObject;
    public TextMeshPro UnitCapacityText;

    private Transform previousParent;
    public bool isAwayTeam { get; private set; }

    public int maxUnitCapacity;
    public int currentUnitCapacity;
    public string targetTag;

    private void Awake()
    {
        AreaSetup();
        previousParent = this.transform.parent;
        isAwayTeam = false;
        maxUnitCapacity = 0;
        currentUnitCapacity = 0;
    }

    private void Start()
    {
        HomeTeam = this.gameObject;
        UnitListAdd();
    }

    public void Update()
    {
        InputSystem();
        UnitCapacityText.text = currentUnitCapacity.ToString() + " / " + maxUnitCapacity.ToString();
        if (GameManager.isBattle) UnitCapacityObject.SetActive(false);
        else UnitCapacityObject.SetActive(true);
    }

    private void InputSystem()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            UnitDeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnitRespawnAll();
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

    public void SetHomeTeam(string tag, string targetTag)
    {
        this.targetTag = targetTag;
        foreach (List<GameObject> respawnObjects in UnitObject.Values)
        {
            foreach (GameObject respawnObject in respawnObjects)
            {
                if (respawnObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Battle"))
                    respawnObject.tag = tag;
            }
        }
    }

    public void SetAwayTeam(Transform AwayTeam, string tag, string targetTag)
    {
        this.targetTag = targetTag;
        HomeTeam.transform.SetParent(AwayTeam);
        HomeTeam.transform.localPosition = Vector3.zero;
        HomeTeam.transform.localRotation = Quaternion.identity;
        isAwayTeam = true;

        foreach (List<GameObject> respawnObjects in UnitObject.Values)
        {
            foreach (GameObject respawnObject in respawnObjects)
            {
                if (respawnObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Battle"))
                    respawnObject.tag = tag;
            }
        }
    }

    public void RevertTeam()
    {
        if (this.isAwayTeam)
        {
            isAwayTeam = false;
            HomeTeam.transform.SetParent(previousParent);
            HomeTeam.transform.localPosition = Vector3.zero;
            HomeTeam.transform.localRotation = Quaternion.identity;
        }
        UnitRespawnAll();
    }

    //---------------------------------------------------------------------------------------------//
    public void UnitListAdd()
    {
        foreach (string unitName in GameManager.instance.UnitList)
        {
            UnitCheck.Add(unitName, 0);
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

    public void UnitRespawnAll()
    {
        foreach (List<GameObject> respawnObjects in UnitObject.Values)
        {
            foreach (GameObject respawnObject in respawnObjects)
            {
                respawnObject.SetActive(true);
                respawnObject.transform.localPosition = Vector3.zero;
                Debug.Log(respawnObject.name);
            }
        }

    }
    //------------------------------------------------------------------//
    public void AddUnitCapacity()
    {
        currentUnitCapacity++;
    }
    public void RemoveUnitCapacity()
    {
        currentUnitCapacity--;
    }
    public bool IsCanLocateBattleArea()
    {
        if (currentUnitCapacity >= maxUnitCapacity) return false;
        return true;
    }
    //-----------------------------------------------------------------//

    public bool IsBattleEndCheck()
    {
        foreach (List<GameObject> respawnObjects in UnitObject.Values)
        {
            foreach (GameObject respawnObject in respawnObjects)
            {
                if (respawnObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Battle"))
                {
                    if (respawnObject.activeSelf)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public int GetRemainUnit()
    {
        int remainCount = 0;
        foreach (List<GameObject> respawnObjects in UnitObject.Values)
        {
            foreach (GameObject respawnObject in respawnObjects)
            {
                if (respawnObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Battle"))
                {
                    if (respawnObject.activeSelf)
                    {
                        remainCount++;
                    }
                }
            }
        }
        return remainCount;
    }

    public void SetBattleResult(bool result, int remainCount)
    {

    }

}
