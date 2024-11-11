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

    private GameObject[] targetEnemys;
    private GameObject targetEnemy { get; set; }
    private string targetTag { get; set; }

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        UnitStatus = GetComponent<UnitStatus>();
        photonView = GetComponent<PhotonView>();

        navMeshAgent.enabled = false;
        targetEnemy = null;
    }

    void Update()
    {
        //if(GameManager.isBattle) IsTargetNull();
    }

    [PunRPC]
    public void OnBattle()
    {
        navMeshAgent.enabled = true;
        SetTargetTag();
        SetTargetList();
        SetTarget();
    }

    public void OnDisarm()
    {
        navMeshAgent.enabled = false;
        targetEnemy = null;
    }

    //player to target 거리 체크
    public float CalcDistance()
    {
        if (targetEnemy == null) return 0;
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
    private void IsTargetNull()
    {
        if (targetEnemy == null)
        {
            SetTarget();
        }
        return;
    }

    public void SetTargetList()
    {
        targetEnemys = GameObject.FindGameObjectsWithTag(targetTag);//최초의 타겟 리스트 생성
        return;
    }


    private void SetTarget()
    {
        float minDistance = float.MaxValue;
        if (targetEnemys == null)
        {
            Debug.Log(this.transform.name);
            return;
        }
        for (int i = 0; i < targetEnemys.Length; i++)
        {
            if (targetEnemys[i] == null) continue;
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

    public void SetTargetTag()
    {
        if (this.transform.tag == "Home")
        {
            targetTag = "Away";
        }
        else targetTag = "Home";
    }

    public void Attack()
    {
        photonView.RPC("AttackTarget", RpcTarget.All);
    }

    [PunRPC]
    public void AttackTarget()
    {
        targetEnemy.GetComponent<UnitManager>().OnHitDamage(UnitStatus.currentATK, UnitStatus.attackType);
    }
}
