using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using BehaviorDesigner.Runtime;

public class SpawnSystem : MonoBehaviour
{
    public TeamManager teamManager;
    public SynergyManager synergyManager;
    public UnitCombine unitCombine;
    public UnitCard[] UnitCards;
    private Dictionary<string, UnitCard> unitDictionary = new Dictionary<string, UnitCard>();

    private bool[] checkSlot = new bool[9];
    private GameObject BenchArea;
    private PhotonView photonView;

    void Awake()
    {
        UnitCards = Resources.LoadAll<UnitCard>("Scriptable");
        UnitDictionarySetup();
        
    }   

    private void UnitDictionarySetup() {
        foreach (var unit in UnitCards) {
            unitDictionary.Add(unit.Name,unit);
        }
    }

    public void Initialize(TeamManager teamManager, SynergyManager synergyManager, UnitCombine unitCombine, PhotonView photonView)
    {
        this.teamManager = teamManager;
        this.synergyManager = synergyManager;
        this.unitCombine = unitCombine;
        this.BenchArea = this.teamManager.BenchArea;
        //this.photonView = photonView;
        this.photonView = this.GetComponent<PhotonView>();
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

    public void SpawnUnit(string unitName) {
        UnitCard currentUnit = unitDictionary[unitName];
        //GameObject UnitClone = Instantiate(currentUnit.UnitPrefab, Vector3.zero, Quaternion.identity);
        GameObject UnitClone = ObjectPoolManager.instance.multiPool[unitName].Get();
        for (int i = 0; i < checkSlot.Length; i++) {
            if (!checkSlot[i]) {
                UnitClone.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                UnitSetup(UnitClone, currentUnit, i);  // 슬롯에 유닛 설정
                break;
            }
        }
        photonView.RPC("SpawnUnitRPC", RpcTarget.OthersBuffered, unitName);
    }

    [PunRPC]
    public void SpawnUnitRPC(string unitName) {
        UnitCard currentUnit = unitDictionary[unitName];
        //GameObject UnitClone = Instantiate(currentUnit.UnitPrefab, Vector3.zero, Quaternion.identity);
        GameObject UnitClone = ObjectPoolManager.instance.multiPool[unitName].Get();
        for (int i = 0; i < checkSlot.Length; i++) {
            if (!checkSlot[i]) {
                UnitSetup(UnitClone, currentUnit, i);  // 슬롯에 유닛 설정
                break;
            }
        }
    }

    public void SpawnUnitIncludePhotonView(string unitName) {
        GameObject UnitClone = PhotonNetwork.Instantiate("Unit/Prefab/" + unitName, Vector3.zero, Quaternion.identity);
        PhotonView unitPhotonView = UnitClone.GetComponent<PhotonView>();
        if (this.photonView.IsMine) // 오브젝트 생성자가 소유자임
        {
            unitPhotonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
        }
        photonView.RPC("SetupUnitOnAllClients", RpcTarget.All, unitName, unitPhotonView.ViewID);
    }

    [PunRPC]
    public void SetupUnitOnAllClients(string unitName, int viewID) {
        this.isSpawnable();
        PhotonView unitPhotonView = PhotonView.Find(viewID);
        if (unitPhotonView == null) {
            Debug.LogError("UnitClone을 찾을 수 없습니다.");
            return;
        }

        GameObject UnitClone = unitPhotonView.gameObject;
        UnitCard currentUnit = unitDictionary[unitName];

        // 빈 슬롯에 유닛 배치
        for (int i = 0; i < checkSlot.Length; i++) {
            if (!checkSlot[i]) {
                UnitSetup(UnitClone, currentUnit, i);  // 슬롯에 유닛 설정
                break;
            }
        }
    }


    public void UnitSetup(GameObject UnitClone, UnitCard UnitCard, int parent)
    {
        UnitClone.transform.SetParent(BenchArea.transform.GetChild(parent));
        UnitClone.transform.localPosition = Vector3.zero;
        UnitClone.gameObject.tag = "Friendly";
        UnitClone.GetComponent<UnitManager>().Initialize(teamManager, synergyManager, unitCombine, UnitCard);
    }


}
