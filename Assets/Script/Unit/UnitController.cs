using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlueChessDataBase;
using Photon.Pun;

public class UnitController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private UnitStatus UnitStatus;
    private PhotonView photonView;
    private TeamManager teamManager;

    private GameObject[] targetEnemys;
    private GameObject targetEnemy { get; set; }
    private string targetTag { get; set; }
    [SerializeField] GameObject muzzleFlash;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        UnitStatus = GetComponent<UnitStatus>();
        photonView = GetComponent<PhotonView>();

        navMeshAgent.enabled = false;
        targetEnemy = null;
        muzzleFlash.SetActive(false);
    }
    public void Initialize(TeamManager teamManager)
    {
        this.teamManager = teamManager;
    }

    void Update()
    {
        if (GameManager.isBattle) {
            IsTargetNull();
            if(IsFindTarget()) {
                Vector3 direction = (targetEnemy.transform.position - this.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
            }  
        }
    }

    public void OnBattle()
    {
        navMeshAgent.enabled = true;
        SetTargetTag();
        SetTargetList();
        SetTarget();
        UnitStatus.CalulateStatus();
    }

    public void OnDisarm()
    {
        navMeshAgent.enabled = false;
        targetEnemy = null;
        muzzleFlash.SetActive(false);
    }

    //player to target 거리 체크
    public float CalcDistance()
    {
        if (targetEnemy == null || !targetEnemy.activeSelf) return float.MaxValue;
        Vector3 playerDistance = gameObject.transform.position;
        Vector3 targetDistance = targetEnemy.transform.position;
        float Distance = Vector3.Distance(playerDistance, targetDistance);
        return Distance;
    }

    public bool CheckAttackRange()
    {
        if (CalcDistance() > UnitStatus.currentRange)
        {
            return false;
        }
        return true;
    }

    public void SetTargetList()
    {
        targetEnemys = GameObject.FindGameObjectsWithTag(targetTag);//최초의 타겟 리스트 생성
        return;
    }


    private void SetTarget()
    {
        float minDistance = float.MaxValue;
        targetEnemy = null;
        if (targetEnemys == null)
        {
            return;
        }
        for (int i = 0; i < targetEnemys.Length; i++)
        {
            if (targetEnemys[i] == null || !targetEnemys[i].activeSelf) continue;
            Vector3 playerDistance = this.transform.position;
            Vector3 targetDistance = targetEnemys[i].transform.position;
            float currentDistance = Vector3.Distance(playerDistance, targetDistance);
            if (minDistance > currentDistance)
            {
                targetEnemy = targetEnemys[i];
                minDistance = currentDistance;
            }
        }
    }


    private void IsTargetNull()
    {
        if (targetEnemy == null || !targetEnemy.activeSelf)
        {
            //Debug.Log("Target Null: "+ this.transform.name);
            SetTarget();
        }
        return;
    }

    public bool IsFindTarget()
    {
        if (targetEnemy == null || !targetEnemy.activeSelf) return false;
        return true;
    }

    public GameObject GetTarget()
    {
        return targetEnemy;
    }

    public void SetTargetTag()
    {
        targetTag = teamManager.targetTag;
    }

    public void AttackTarget()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("AttackTargetRPC", RpcTarget.All);
        }

    }

    [PunRPC]
    public void AttackTargetRPC()
    {
        if (targetEnemy == null || !targetEnemy.activeSelf) {
            SetTarget();
            return;
        }
        //Debug.Log(this.transform.name + " =>" + targetEnemy.name + ": " + UnitStatus.currentATK);
        targetEnemy.GetComponent<UnitManager>().OnHitDamage(UnitStatus.currentATK, UnitStatus.attackType);
        StartCoroutine(ShowMuzzleFlash());
    }

     private IEnumerator ShowMuzzleFlash()
    {
        if(muzzleFlash != null) {
            muzzleFlash.SetActive(true); // MuzzleFlash 활성화
            yield return new WaitForSeconds(0.05f);          // 추가로 1프레임 대기 (총 2프레임)
            muzzleFlash.SetActive(false); // MuzzleFlash 비활성화
        }
        yield return null;  
    }
}
