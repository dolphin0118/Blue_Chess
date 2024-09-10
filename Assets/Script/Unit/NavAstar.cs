using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class NavAstar : MonoBehaviour
{
    private NavMeshAgent charaNav;
    private GameObject targetObject;
    private string targetTag;

    void Start()
    {
        charaNav = this.GetComponent<NavMeshAgent>();
        charaNav.speed = 1f;
        if (this.transform.tag == "Friendly") targetTag = "Enemy";
        else if (this.transform.tag == "Enemy") targetTag = "Friendly";
    }
    private void Update()
    {
        if (!GameManager.isBattle)
        {
            NavStop();
        }
    }
    public void NavStart()
    {
        targetObject = this.GetComponent<UnitController>().GetTarget();
        if (targetObject == null) return;
        else
        {
            charaNav.SetDestination(targetObject.transform.position);
        }
    }

    public void NavStop()
    {
        charaNav.ResetPath();
    }
}

