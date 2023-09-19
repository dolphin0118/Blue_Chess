using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class NavAstar : MonoBehaviour {
    private NavMeshAgent charaNav;
    private GameObject Target_Object;  
    private string targetTag;

    void Start() {
        charaNav = this.GetComponent<NavMeshAgent>();
        charaNav.speed = 1f;
        if (this.transform.tag == "Friendly") targetTag = "Enemy";
        else if (this.transform.tag == "Enemy") targetTag = "Friendly";
    }

    public void NavStart() {
        Target_Object = this.GetComponent<CharaController>().Set_Target(targetTag);
        if(Target_Object == null) return;
        else {
            charaNav.SetDestination(Target_Object.transform.position);
        }
    }

    public void NavStop() {
        charaNav.ResetPath();
    }
}

 