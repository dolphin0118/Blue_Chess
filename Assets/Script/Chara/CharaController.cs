using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;
using BlueChessDataBase;

public class CharaController : MonoBehaviour {
    Animator anim;
    NavAstar navAstar;
    CharaInfo charainfo;
    CharaData charaData;
    CharaStat charaStat;
    CharaLocate charaLocate;
    GameObject targetEnemy;
    State state;
    NavMeshAgent charaNav;
    static public Vector3Int Infinity = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    public bool isBattle = false;

    void Awake(){
        anim = GetComponent<Animator>();
        navAstar = GetComponent<NavAstar>();
        charainfo = GetComponent<CharaInfo>();
        charaNav = GetComponent<NavMeshAgent>();
        charaLocate = GetComponent<CharaLocate>();
        charaData = charainfo.charaData;
        charaStat = charainfo.charaStat;
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
            if(PlayerToTarget() <= charaStat.Range) {
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
            if(PlayerToTarget() > charaStat.Range) {
                state = State.Move;
                anim.SetBool("Attack", false);
                anim.SetBool("Move", true); 
                return;
            }
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing")&&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime > charaStat.ATKSpeed) {
                charaStat.ATKSpeed += 1.0f;
                targetEnemy.GetComponent<CharaInfo>().charaStat.HP-=10;
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

    float PlayerToTarget() {
        Vector3 Player_distance = gameObject.transform.position;
        Vector3 Target_distance = targetEnemy.transform.position;
        float Distance = Vector3.Distance(Player_distance, Target_distance);
        return Distance;
    }

    public GameObject Set_Target(string targetTag) {
        string _targetTag = targetTag;
        GameObject[] Target_Enemys = GameObject.FindGameObjectsWithTag(_targetTag);
        Vector3 Target_distance;
        float min_distance = float.MaxValue;
        for(int i = 0; i < Target_Enemys.Length; i++) {
            Vector3 Player_distance = this.transform.position;
            Target_distance = Target_Enemys[i].transform.position;
            float current_distance = Vector3.Distance(Player_distance, Target_distance);
            if(min_distance > current_distance) {
                targetEnemy = Target_Enemys[i];
                min_distance = current_distance;
            }
        }
        return targetEnemy;
    }
}
