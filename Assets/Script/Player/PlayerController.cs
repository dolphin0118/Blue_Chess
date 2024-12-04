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
    //-----------------------------Manager-----------------------------------//
    [NonSerialized] public TeamManager TeamManager;
    [NonSerialized] public SynergyManager synergyManager;
    [NonSerialized] public UIManager UIManager;

    //------------------------------System----------------------------------//
    [NonSerialized] public CombineSystem combineSystem;
    [NonSerialized] private SpawnSystem spawnSystem;
    [NonSerialized] public PhotonView photonView;

    //-------------------------CameraTarget---------------------------------//
    public GameObject HomeViewTarget;
    public GameObject AwayViewTarget;
    private GameObject ViewTarget;

    //------------------------------Data------------------------------------//
    private PlayerData playerData;

    [SerializeField] public int playerCode = 0;

    private void Awake()
    {
        TeamManager = GetComponentInChildren<TeamManager>();
        synergyManager = GetComponentInChildren<SynergyManager>();
        UIManager = GetComponentInChildren<UIManager>();

        combineSystem = GetComponentInChildren<CombineSystem>();
        spawnSystem = GetComponentInChildren<SpawnSystem>();
        photonView = GetComponent<PhotonView>();

        playerData = GetComponent<PlayerData>();

        TeamManager.Initialize(synergyManager, combineSystem);
        spawnSystem.Initialize(TeamManager, synergyManager);
        combineSystem.Initialize(TeamManager);
        playerData.Initialize(TeamManager);

    }

    private void Update()
    {
        TeamManager.maxUnitCapacity = playerData.maxUnitCapacity;
        UpdateView();
        IsPlayerDie();
    }

    public void UpdateView()
    {
        if (!PlayManager.instance.isStart)
        {
            UIManager.SetShopUIActive(false);
            UIManager.SetSynergyUIActive(false);
            UpdateViewTarget();
            return;
        }

        if (photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
            PlayerManager.instance.playerViewCode = playerCode;
        }

        if (PlayerManager.instance.playerViewCode == playerCode)
        {
            UpdateViewTarget();
            if (photonView.IsMine)
            {
                //PlayerManager.instance.playerViewCode = playerCode;
                UIManager.SetShopUIActive(true);
                UIManager.SetSynergyUIActive(true);
            }
            else
            {
                UIManager.SetShopUIActive(false);
                UIManager.SetSynergyUIActive(false);
            }
        }
        else
        {
            //UpdateViewTarget();
            UIManager.SetShopUIActive(false);
            UIManager.SetSynergyUIActive(false);
        }
    }

    public void SetAwayViewTarget(GameObject AwayViewTarget)
    {
        this.AwayViewTarget = AwayViewTarget;
    }

    public void UpdateViewTarget()
    {
        if (TeamManager.isAwayTeam)
        {
            Camera.main.GetComponent<FollowCam>().viewTarget = ViewTarget.transform;
            Camera.main.GetComponent<FollowCam>().isAwayViewTarget = true;
        }
        else
        {
            ViewTarget = this.HomeViewTarget;
            Camera.main.GetComponent<FollowCam>().viewTarget = ViewTarget.transform;
            Camera.main.GetComponent<FollowCam>().isAwayViewTarget = false;
        }

    }
    public void GetDamage(int damage)
    {
        playerData.GetDamage(damage);
    }

    public void IsPlayerDie()
    {
        if (playerData.playerHp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
