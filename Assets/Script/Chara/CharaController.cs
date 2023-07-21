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
    GameObject Target_Enemy = null;
    State.state _state = State.state.Idle;

    [SerializeField]float Player_Speed = 1.0f;
    [SerializeField]float Player_Range = 1.0f;
    private float Attack_count;

    void Awake(){
       anim = GetComponent<Animator>();
       Attack_count = GetComponent<CharaInfo>().Player_Attack_count;
    }

    void Update() {
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
        if(!BattleManager.isBattle) {
            _state = State.state.Idle;
            anim.SetBool("Idle", true);
        }
        else if(BattleManager.isBattle) {
            _state = State.state.Move;
            anim.SetBool("Idle", false);
            anim.SetBool("Move", true);
        }
    }

    void Move_state() {
        Set_Target();
        Vector3 Player_distance = gameObject.transform.position;
        Vector3 Target_distance = Target_Enemy.transform.position;
        float Distance = Vector3.Distance(Player_distance, Target_distance);

        if(Distance <= Player_Range) {
            _state = State.state.Attack;
            anim.SetBool("Attack", true);
            anim.SetBool("Move", false); 
            return;
        }
        else {
            transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(Target_distance - Player_distance), Time.deltaTime);
            transform.position = Vector3.MoveTowards(Player_distance, Target_distance, 1f * Time.deltaTime);
        }
    }

    void Attack_State() {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_ing")&&anim.GetCurrentAnimatorStateInfo(0).normalizedTime > Attack_count) {
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Attack_count += 1.0f;
            Target_Enemy.GetComponent<CharaInfo>().Player_Hp-=10;
        }
    }

    void Die_state() {
        Destroy(this.gameObject);
    }

    void Set_Target() {
        GameObject[] Target_Enemys = GameObject.FindGameObjectsWithTag("Away");
        float min_distance = float.MaxValue;
        for(int i = 0; i < Target_Enemys.Length; i++) {
            Vector3 Player_distance = this.transform.position;
            Vector3 Target_distance = Target_Enemys[i].transform.position;
            float current_distance = Vector3.Distance(Player_distance, Target_distance);
            if(min_distance > current_distance) {
                Target_Enemy = Target_Enemys[i];
                min_distance = current_distance;
            }
        }
    }

    void Attack_Enemy() {
        if(Target_Enemy!= null) {
            
        }
    }
}
