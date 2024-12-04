using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.AI;
using BlueChessDataBase;

public class TeamManager : MonoBehaviour
{
    private CombineSystem combineSystem;
    private SynergyManager synergyManager;

    public Dictionary<string, int> UnitCheck = new Dictionary<string, int>(); //Synergy체크용
    public Dictionary<string, LevelData> UnitLevel = new Dictionary<string, LevelData>(); //Combine용
    public Dictionary<string, List<GameObject>> UnitObject = new Dictionary<string, List<GameObject>>();//

    //----------------FieldObject-------------------//
    public GameObject BattleArea, BenchArea;
    public GameObject HomeTeam, AwayTeam;
    public GameObject UnitLocateController;
    public GameObject GridView;
    private Transform previousParent;
    //-------------------text----------------------//
    public GameObject UnitCapacityObject;
    public TextMeshPro UnitCapacityText;

    //----------------------UI---------------------//
    public GameObject UnitDetailCard;



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
    public void Initialize(SynergyManager synergyManager, CombineSystem combineSystem)
    {
        this.synergyManager = synergyManager;
        this.combineSystem = combineSystem;
    }
    private void Start()
    {
        HomeTeam = this.gameObject;
        UnitListAdd();
        UnitDetailCard.gameObject.SetActive(false);
    }

    public void Update()
    {
        InputSystem();
        UnitCapacityText.text = currentUnitCapacity.ToString() + " / " + maxUnitCapacity.ToString();
        if (!PlayManager.instance.isStart) UnitCapacityObject.SetActive(false);
        else if (PlayManager.instance.isReady && !GameManager.isBattle) UnitCapacityObject.SetActive(true);
        else if (PlayManager.instance.isStart) UnitCapacityObject.SetActive(false);
        else if (GameManager.isBattle) UnitCapacityObject.SetActive(false);
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
        SetBattlePhase();

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
        SetBattlePhase();

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
        UnitCapacityObject.SetActive(true);
    }

    public void SetBattlePhase()
    {
        UnitCapacityObject.SetActive(false);

        foreach (List<GameObject> respawnObjects in UnitObject.Values)
        {
            List<UnitStatus> synergyActive = new List<UnitStatus>();
            foreach (GameObject respawnObject in respawnObjects)
            {
                if (respawnObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Battle"))
                    synergyActive.Add(respawnObject.GetComponent<UnitStatus>());
            }
            //synergyManager.SynergyActive(synergyActive);
        }

    }


    //---------------------------------------------------------------------------------------------//
    public void UnitListAdd()
    {
        foreach (string unitName in GameManager.instance.UnitList)
        {
            UnitCheck.Add(unitName, 0);
        }
    }

    public void UnitListUpdate(string unitName, GameObject unitObject)
    {
        UnitObject[unitName].Add(unitObject);
        combineSystem.CombineListUpdate(unitName);
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
                respawnObject.GetComponent<NavMeshAgent>().enabled = false;
                respawnObject.transform.localPosition = Vector3.zero;
                respawnObject.tag = "Untagged";
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

    //-----------------------------------------------------------------------//
    public void OpenUI(UnitCard UnitCard)
    {
        UnitDetailCard.gameObject.SetActive(true);
        UnitDetailCard.GetComponentInChildren<UnitDetailUI>().CardEnable(UnitCard);
    }

    public void CloseUI()
    {
        UnitDetailCard.gameObject.SetActive(false);
    }

}
