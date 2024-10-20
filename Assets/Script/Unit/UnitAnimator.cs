using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlueChessDataBase;
using BehaviorDesigner.Runtime;


public class UnitAnimator : MonoBehaviour
{
    static public Vector3Int Infinity = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    Animator animator;
    UnitAstar unitAstar;
    UnitStatus unitStatus;
    UnitController unitController;
    void Awake()
    {
        animator = GetComponent<Animator>();
        unitAstar = GetComponent<UnitAstar>();
        unitStatus = GetComponent<UnitStatus>();
        unitController = GetComponent<UnitController>();
    }

    public void NoneState() {
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

    public void HitDetection()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime > unitStatus.ATKSpeed)
        {
            //unitInfo.UnitStat.ATKSpeed += 1.0f;   
        }
    }

}
