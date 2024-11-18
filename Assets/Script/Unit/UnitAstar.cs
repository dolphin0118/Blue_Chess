using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using TMPro.Examples;

public class UnitAstar : MonoBehaviour
{
    private NavMeshAgent charaNav;
    private GameObject targetObject;
    private bool isRunning;

    void Start()
    {
        charaNav = this.GetComponent<NavMeshAgent>();
        charaNav.speed = 1f;
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
            isRunning = true;
        }
    }

    public void NavStop()
    {
        if (charaNav == null) return;
        if (isRunning) charaNav.ResetPath();
        else return;
    }
}

