using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using BlueChessDataBase;
using Unity.VisualScripting;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine.Rendering;

public class UnitController : MonoBehaviour
{
    NavAstar navAstar;
    NavMeshAgent navMeshAgent;
    UnitInfo Unitinfo;
    UnitData UnitData;
    UnitStat UnitStat;
    UnitLocate UnitLocate;

    private GameObject[] targetEnemys;
    private GameObject targetEnemy { get; set; }
    private string targetTag { get; set; }

    void Awake()
    {
        navAstar = GetComponent<NavAstar>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Unitinfo = GetComponent<UnitInfo>();
        UnitLocate = GetComponent<UnitLocate>();
        UnitData = Unitinfo.UnitData;
        UnitStat = Unitinfo.UnitStat;
        navMeshAgent.enabled = false;
        targetEnemy = null;
    }

    void Update()
    {
        SetTargetTag();
        //if(GameManager.isBattle) IsTargetNull();
    }

    public void BattlePhase()
    {
        navMeshAgent.enabled = true;
        SetTargetTag();
        SetTargetList();
        SetTarget();
    }

    public void DisarmPhase()
    {
        navMeshAgent.enabled = false;
        targetEnemy = null;
    }

    //player to target 거리 체크
    public float CalcDistance()
    {
        if(targetEnemy == null) return 0; 
        Vector3 playerDistance = gameObject.transform.position;
        Vector3 targetDistance = targetEnemy.transform.position;
        float Distance = Vector3.Distance(playerDistance, targetDistance);
        return Distance;
    }

    public bool CheckAttackRange()
    {
        if (CalcDistance() > Unitinfo.UnitStat.Range)
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


    private GameObject SetTarget()
    {
        float minDistance = float.MaxValue;
        if (targetEnemys == null)
        {
            Debug.Log(this.transform.name);
            return null;
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
        return targetEnemy;
    }

    public GameObject GetTarget()
    {
        return targetEnemy;
    }

    public void SetTargetTag()
    {
        string teamName = this.transform.root.name;
        if (teamName == "Home_Team")
        {
            this.transform.tag = "Home";
            targetTag = "Away";
        }
        else
        {
            this.transform.tag = "Away";
            targetTag = "Home";
        }
    }

    public void AttackTarget()
    {
        targetEnemy.GetComponent<UnitStatus>().Hit(10);
    }
}
