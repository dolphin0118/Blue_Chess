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
    [NonSerialized] public TeamManager TeamManager;
    [NonSerialized] public SynergyManager synergyManager;
    [NonSerialized] public UIManager UIManager;

    [NonSerialized] public CombineSystem combineSystem;
    [NonSerialized] private SpawnSystem spawnSystem;
    [NonSerialized] public PhotonView photonView;

    public GameObject HomeViewTarget;
    public GameObject AwayViewTarget;
    private GameObject ViewTarget;

    [SerializeField] public int playerCode = 0;
    public string playerName { get; set; }
    public int playerLevel { get; set; }
    public int playerHp { get; set; }
    public int playerGold { get; set; }
    public int maxUnitCapacity { get; set; }

    public int[] levelEXP = new int[] { 2, 2, 6, 10, 20, 36 };
    public int EXP = 0;

    private void Awake()
    {
        TeamManager = GetComponentInChildren<TeamManager>();
        synergyManager = GetComponentInChildren<SynergyManager>();
        UIManager = GetComponentInChildren<UIManager>();

        combineSystem = GetComponentInChildren<CombineSystem>();
        spawnSystem = GetComponentInChildren<SpawnSystem>();
        photonView = GetComponent<PhotonView>();

        TeamManager.Initialize(combineSystem);
        spawnSystem.Initialize(TeamManager, synergyManager);
        combineSystem.Initialize(TeamManager);


        playerName = "Player" + playerCode;
        playerLevel = 3;
        playerHp = 20;
        playerGold = 20;
        maxUnitCapacity = playerLevel;

    }

    private void Update()
    {
        maxUnitCapacity = playerLevel;
        TeamManager.maxUnitCapacity = maxUnitCapacity;
        UpdateView();
        UpdateLevel();
    }

    public void UpdateView()
    {
        if (photonView.IsMine && !PhotonNetwork.IsMasterClient)
        {
            PlayerManager.instance.playerViewCode = playerCode;
        }

        if (PlayerManager.instance.playerViewCode == playerCode)
        {
            UpdateViewTarget();
            if (photonView.IsMine)
            {
                PlayerManager.instance.playerViewCode = playerCode;
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

    private void UpdateLevel()
    {
        if (EXP >= levelEXP[playerLevel])
        {
            EXP -= levelEXP[playerLevel];
            playerLevel++;
        }
    }

    //전투 종료시 호출
    public void BattleEXP()
    {
        int BattleEXPValue = 2;
        EXP += BattleEXPValue;
    }

    //Button으로 호출

    public Tuple<int, int> GetEXP()
    {
        return new Tuple<int, int>(EXP, levelEXP[playerLevel]);
    }

    public void GetDamage(int damage)
    {
        playerHp -= damage;
    }

}
