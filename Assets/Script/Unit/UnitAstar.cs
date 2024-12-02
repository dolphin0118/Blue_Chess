using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using TMPro.Examples;

public class UnitAstar : MonoBehaviour
{
    private NavMeshAgent unitNav;
    private GameObject targetObject;
    private bool isRunning;

    void Start()
    {
        unitNav = this.GetComponent<NavMeshAgent>();
        unitNav.speed = 1f;
    }

    private void Update()
    {
        if (!GameManager.isBattle)
        {
            //NavStop();
        }
    }

    public void NavStart()
    {
        targetObject = this.GetComponent<UnitController>().GetTarget();
        if (targetObject == null || !targetObject.activeSelf) return;
        else
        {
            unitNav.SetDestination(targetObject.transform.position);
            isRunning = true;
        }
    }

    public void NavStop()
    {
        if (unitNav == null) return;
        if (isRunning) unitNav.ResetPath();
        else return;
    }
}

