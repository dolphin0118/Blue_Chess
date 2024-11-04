using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public TeamManager TeamManager;
    public SynergyManager synergyManager;
    public UIManager UIManager;
    public UnitCombine unitCombine;
    private SpawnSystem spawnSystem;
    public PhotonView photonView;
    public GameObject viewTarget;
    [SerializeField] public int playerCode = 0;
        bool IsConnected = false;
    private void Awake()
    {
        TeamManager = GetComponentInChildren<TeamManager>();
        synergyManager = GetComponentInChildren<SynergyManager>();
        unitCombine = GetComponentInChildren<UnitCombine>();
        UIManager = GetComponentInChildren<UIManager>();
        spawnSystem = GetComponentInChildren<SpawnSystem>();
        photonView = GetComponent<PhotonView>();
        spawnSystem.Initialize(TeamManager, synergyManager, unitCombine, photonView);
    }

    private void Update() {
        UpdateView();
    }

    public void UpdateView() {
        
        if(photonView.IsMine && !IsConnected && !PhotonNetwork.IsMasterClient) {
            PlayerManager.instance.playerViewCode = playerCode;
            IsConnected = true;
        }

        if(PlayerManager.instance.playerViewCode == playerCode) {
            UIManager.SetUIActive(true);
            UpdateViewTarget();
            if(photonView.IsMine) {
                PlayerManager.instance.playerViewCode = playerCode;
                UIManager.SetShopUIActive(true);  
            }
            else  {
                UIManager.SetShopUIActive(false);
            }
        }       
        else  {
            UIManager.SetUIActive(false);
            UIManager.SetShopUIActive(false);
        }
    }

    public void UpdateViewTarget() {
        Camera.main.GetComponent<FollowCam>().viewTarget = this.viewTarget.transform;
    }

}
