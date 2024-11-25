using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlueChessDataBase;
using BehaviorDesigner.Runtime;
using Unity.VisualScripting;
using Photon.Pun;


public class UnitAnimator : MonoBehaviour
{
    Animator animator;
    UnitAstar unitAstar;
    UnitStatus unitStatus;
    UnitController unitController;
    bool isAttack = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        unitAstar = GetComponent<UnitAstar>();
        unitStatus = GetComponent<UnitStatus>();
        unitController = GetComponent<UnitController>();
    }
    void Update()
    {
        animator.SetFloat("AttackSpeed", unitStatus.currentATKSpeed);
        AttackMotion();
        if(GameManager.isBattle) animator.SetBool("isBattle", true);
        else animator.SetBool("isBattle", false);
    }

    public void NoneState()
    {
        animator.SetBool("Idle", true);
    }

    public void IdleState()
    {
        //현재 상태
        animator.SetBool("Idle", true);

        //다른 상태 취소
        animator.SetBool("Move", false);
        animator.SetBool("Attack", false);
    }
    public void MoveState()
    {
        unitAstar.NavStart();
        //현재 상태
        animator.SetBool("Move", true);
        animator.SetBool("Idle", false);
        animator.SetBool("Attack", false);
    }

    public void AttackState()
    {
        //현재 상태
        unitAstar.NavStop();
        animator.SetBool("Attack", true);

        animator.SetBool("Move", false);
        animator.SetBool("Idle", false);


    }

    public void AttackMotion()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f &&
            !isAttack)
        {
            unitController.AttackTarget(); 
            isAttack = true;
        }
         if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing"))
        {
            isAttack = false;
        }
    }

}
