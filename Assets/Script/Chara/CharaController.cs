using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class CharaController : MonoBehaviour {
    Animator anim;
    CharaAstar Astar;
    NavAstar navAstar;
    CharaInfo  charainfo;
    CharaLocate charaLocate;
    GameObject Target_Enemy = null;
    State.state _state = State.state.Idle;
    NavMeshAgent charaNav;
    static public Vector3Int Infinity = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    public bool isBattle = false;

    private float Player_Speed;
    private float Player_Range;
    private float Player_Attack_count;
    private float Player_Hp;

    void Awake(){
        anim = GetComponent<Animator>();
        Astar = GetComponent<CharaAstar>();
        navAstar = GetComponent<NavAstar>();
        charainfo = GetComponent<CharaInfo>();
        charaNav = GetComponent<NavMeshAgent>();
        charaLocate = GetComponent<CharaLocate>();
        charaNav.enabled = false;
        Init();
    }
    void Init() {
        Player_Speed = charainfo.Player_Speed;
        Player_Range = charainfo.Player_Range;
        Player_Attack_count = charainfo.Player_Attack_count;
    }

    void Update() {
        if(Input.GetKey(KeyCode.Q)) isBattle = true;     
    }

    void FixedUpdate() {
        PlayerDieCheck();
        Player_State();
    }
    void PlayerDieCheck() {
        Player_Hp = charainfo.Player_Hp;
        if(Player_Hp <= 0) _state = State.state.Die;
    }
    void Player_State(){
        switch(_state) {
            case State.state.Idle :
                Idle_state();
            break;
            case State.state.Move :
                Move_state();
            break;
            case State.state.Attack :
                Attack_State();
            break;
            case State.state.Die :
                Die_state();
            break;
        }
    }

    void Idle_state() {
        if(!isBattle) {
            _state = State.state.Idle;
            anim.SetBool("Idle", true);
        }
        else if(isBattle&&charaLocate.isBattleLayer) {
            _state = State.state.Move;
            anim.SetBool("Idle", false);
            charaNav.enabled = true;
        }
        else return;
    }

    void Move_state() {
        navAstar.NavStart();
        if(Target_Enemy == null) {
            _state = State.state.Idle;
            anim.SetBool("Move", false);
            return;
        }
        else {
            anim.SetBool("Move", true);
            if(PlayerToTarget() <= Player_Range) {
                _state = State.state.Attack;
                anim.SetBool("Attack", true);
                anim.SetBool("Move", false); 
                navAstar.NavStop();
                return;
            }
            else return;
        }

    }

    void Attack_State() {
        if(Target_Enemy != null) {
            if(PlayerToTarget() > Player_Range) {
                _state = State.state.Move;
                anim.SetBool("Attack", false);
                anim.SetBool("Move", true); 
                return;
            }
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing")&&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime > Player_Attack_count) {
                Player_Attack_count += 1.0f;
                Target_Enemy.GetComponent<CharaInfo>().Player_Hp-=10;
            }
        }   
        else if(Target_Enemy == null){
            _state = State.state.Move;
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
        Vector3 Target_distance = Target_Enemy.transform.position;
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
                Target_Enemy = Target_Enemys[i];
                min_distance = current_distance;
            }
        }
        return Target_Enemy;
    }
}
