using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public TeamManager TeamManager;
    public SynergyManager synergyManager;
    public UIManager UIManager;
    public UnitCombine unitCombine;
    private SpawnSystem spawnSystem;
    public PhotonView photonView;
    public GameObject viewTarget;
    [SerializeField] private int playerCode = 0;

    private void Awake()
    {
        TeamManager = GetComponentInChildren<TeamManager>();
        synergyManager = GetComponentInChildren<SynergyManager>();
        unitCombine = GetComponentInChildren<UnitCombine>();
        UIManager = GetComponentInChildren<UIManager>();
        spawnSystem = GetComponentInChildren<SpawnSystem>();
        
        spawnSystem.Initialize(TeamManager, synergyManager, unitCombine);

    }
    private void Update() {
        SetView();
    }

    public void SetView() {
        //photonView.isMine;
        if(playerCode == PlayerManager.instance.playerViewCode) {
            UIManager.SetUIActive(true);
            SetViewTarget();
        }
        else
        {
            UIManager.SetUIActive(false);
        }
    }

    public void SetViewTarget() {
        Camera.main.GetComponent<FollowCam>().viewTarget = this.viewTarget.transform;
    }

}
