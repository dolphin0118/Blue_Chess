using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BlueChessDataBase;



public class UnitAnimator : MonoBehaviour {
    static public Vector3Int Infinity = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    Animator anim;
    NavAstar navAstar;
    CharaInfo charainfo;
    CharaLocate charaLocate;
    NavMeshAgent charaNav;
    
    public bool isBattle = false;
    private GameObject[] targetEnemys;
    private GameObject targetEnemy;
    private State state;

    void Awake(){
        anim = GetComponent<Animator>();
        navAstar = GetComponent<NavAstar>();
        charainfo = GetComponent<CharaInfo>();
        charaNav = GetComponent<NavMeshAgent>();
        charaLocate = GetComponent<CharaLocate>();
        Init();
    }
    
    void Init() {
        charaNav.enabled = false;
        targetEnemy = null;
        state = State.Idle;
    }

    void Update() {
        if(Input.GetKey(KeyCode.Q)) isBattle = true;     
    }

    void FixedUpdate() {
        //PlayerDieCheck();
        Player_State();
    }

    void PlayerDieCheck() {
        //if(charaData.charaHp <= 0) state = State.Die;
    }

    void Player_State(){
        switch(state) {
            case State.Idle :
                Idle_state();
                break;
            case State.Move :
                Move_state();
                break;
            case State.Attack :
                Attack_State();
                break;
            case State.Die :
                Die_state();
                break;
        }
    }

    void Idle_state() {
        if(!isBattle) {
            state = State.Idle;
            anim.SetBool("Idle", true);
        }
        else if(isBattle&&charaLocate.isBattleLayer) {
            state = State.Move;
            anim.SetBool("Idle", false);
            charaNav.enabled = true;
        }
        else return;
    }

    void Move_state() {
        navAstar.NavStart();
        if(targetEnemy == null) {
            state = State.Idle;
            anim.SetBool("Move", false);
            return;
        }
        else {
            anim.SetBool("Move", true);
            if(CalcDistance() <= charainfo.charaStat.Range) {
                state = State.Attack;
                anim.SetBool("Attack", true);
                anim.SetBool("Move", false); 
                navAstar.NavStop();
                return;
            }
            else return;
        }

    }

    void Attack_State() {
        if(targetEnemy != null) {
            if(CalcDistance() > charainfo.charaStat.Range) {
                SetTarget();
                if(CalcDistance() > charainfo.charaStat.Range) {
                    state = State.Move;
                    anim.SetBool("Attack", false);
                    anim.SetBool("Move", true); 
                }
                return;
            }
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing")&&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime > charainfo.charaStat.ATKSpeed) {
                charainfo.charaStat.ATKSpeed += 1.0f;
                targetEnemy.GetComponent<CharaStatus>().Hit(10);
            }
        }   
        else if(targetEnemy == null){
            state = State.Move;
            anim.SetBool("Attack", false);
            anim.SetBool("Move", true); 
            return;
        }

    }

    void Die_state() {
        Destroy(this.gameObject);
    }

    float CalcDistance() {
        Vector3 playerDistance = gameObject.transform.position;
        Vector3 targetDistance = targetEnemy.transform.position;
        float Distance = Vector3.Distance(playerDistance, targetDistance);
        return Distance;
    }

    public void SetTargetList(string targetTag) {
        GameObject[] targetEnemys = GameObject.FindGameObjectsWithTag(targetTag);//최초의 타겟 리스트 생성
        return;
    }

    public void SetTarget() {
        Vector3 targetDistance;
        float min_distance = float.MaxValue;
        for(int i = 0; i < targetEnemys.Length; i++) {
            Vector3 playerDistance = this.transform.position;
            targetDistance = targetEnemys[i].transform.position;
            float current_distance = Vector3.Distance(playerDistance, targetDistance);
            if(min_distance > current_distance) {
                targetEnemy = targetEnemys[i];
                min_distance = current_distance;
            }
        }
        return;
    }

}
