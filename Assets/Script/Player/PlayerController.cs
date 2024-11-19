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
    public GameObject HomeViewTarget;
    public GameObject AwayViewTarget;
    private GameObject ViewTarget;

    [SerializeField] public int playerCode = 0;
    public string playerName { get; set; }
    public int playerLevel { get; set; }
    public int playerHp { get; set; }
    public int playerGold { get; set; }
    public int maxUnitCapacity { get; set; }

    private int[] levelEXP = new int[] { 2, 2, 6, 10, 20, 36 };
    private int EXP = 0;

    private void Awake()
    {
        TeamManager = GetComponentInChildren<TeamManager>();
        synergyManager = GetComponentInChildren<SynergyManager>();
        unitCombine = GetComponentInChildren<UnitCombine>();
        UIManager = GetComponentInChildren<UIManager>();
        spawnSystem = GetComponentInChildren<SpawnSystem>();
        photonView = GetComponent<PhotonView>();
        spawnSystem.Initialize(TeamManager, synergyManager, unitCombine, photonView);

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
            UIManager.SetUIActive(true);
            UpdateViewTarget();
            if (photonView.IsMine)
            {
                PlayerManager.instance.playerViewCode = playerCode;
                UIManager.SetShopUIActive(true);
            }
            else
            {
                UIManager.SetShopUIActive(false);
                //UIManager.SetShopUIActive(true);
            }
        }
        else
        {
            UIManager.SetUIActive(false);
            UIManager.SetShopUIActive(false);
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

    public void getGold()
    {
        int basicGold = 5;
        int winGold = 1;

        int totalGold = basicGold;
        playerGold += totalGold;
    }
    //전투 종료시 호출
    public void BattleEXP()
    {
        int BattleEXPValue = 2;
        EXP += BattleEXPValue;
    }

    //Button으로 호출
    public void BuyEXP()
    {
        int buyEXPValue = 4;
        if (playerGold >= 4)
        {
            EXP += buyEXPValue;
            playerGold -= 4;
        }

    }

    public Tuple<int, int> getEXP()
    {
        return new Tuple<int, int>(EXP, levelEXP[playerLevel]);
    }

    public void GetDamage(int damage)
    {
        playerHp -= damage;
    }

}
