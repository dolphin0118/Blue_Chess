using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using Photon.Pun;
using Photon.Realtime;

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

    public void Initialize(TeamManager teamManager, SynergyManager synergyManager, UnitCombine unitCombine)
    {
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

    public void aSpawnUnit(UnitCard UnitCard)
    {
        for (int i = 0; i < checkSlot.Length; i++)
        {
            if (!checkSlot[i])
            {
                GameObject UnitClone = Instantiate(UnitCard.UnitPrefab, Vector3.zero, Quaternion.identity);
                UnitSetup(UnitClone, UnitCard, i);
                PhotonView photonView = UnitClone.GetComponent<PhotonView>();
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber); // targetPlayerId로 소유권 이동
                break;
            }
        }
    }
    public void SpawnUnit(UnitCard UnitCard)
    {
        for (int i = 0; i < checkSlot.Length; i++)
        {
            if (!checkSlot[i])
            {
                GameObject UnitClone = PhotonNetwork.Instantiate("Unit/Prefab/" + UnitCard.UnitPrefab.name, Vector3.zero, Quaternion.identity);
                PhotonView photonView = UnitClone.GetComponent<PhotonView>();
                //photonView.RPC("UnitSetup", RpcTarget.All, UnitClone, UnitCard, i);
                UnitSetup(UnitClone, UnitCard, i);
                if (photonView.IsMine) // 오브젝트 생성자가 소유자임
                {
                    photonView.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber); // targetPlayerId로 소유권 이동
                }
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
