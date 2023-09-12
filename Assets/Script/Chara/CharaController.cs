using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class State {
    public enum state {
        Idle,
        Move,
        Attack,
        Die
    }
}

public class CharaController : MonoBehaviour {
    Animator anim;
    CharaAstar Astar;
    CharaInfo  charainfo;
    GameObject Target_Enemy = null;
    State.state _state = State.state.Idle;
    static public Vector3Int Infinity = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    public bool isBattle = false;

    private float Player_Speed;
    private float Player_Range;
    private float Player_Attack_count;
  
    void Awake(){
        anim = GetComponent<Animator>();
        Astar = GetComponent<CharaAstar>();
        charainfo = GetComponent<CharaInfo>();
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
        Player_State();
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
        else if(isBattle) {
            _state = State.state.Move;
            anim.SetBool("Idle", false);
            Astar.MovePath();
        }
    }

    void Move_state() {
        if(Target_Enemy == null) {
            _state = State.state.Idle;
            anim.SetBool("Move", false);
            return;
        }
        else {
            anim.SetBool("Move", true);
        }

        if(PlayerToTarget() <= Player_Range) {
            _state = State.state.Attack;
            anim.SetBool("Attack", true);
            anim.SetBool("Move", false); 
            Astar.StopPath();
            return;
        }
    }

    void Attack_State() {
         if(PlayerToTarget() > Player_Range) {
            _state = State.state.Move;
            anim.SetBool("Attack", false);
            anim.SetBool("Move", true); 
            Astar.MovePath();
            return;
        }
        
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime > Player_Attack_count) {
            //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Player_Attack_count += 1.0f;
            Target_Enemy.GetComponent<CharaInfo>().Player_Hp-=10;
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
