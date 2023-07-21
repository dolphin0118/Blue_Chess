using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleManager : MonoBehaviour {
    public static  bool isBattle = false;
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)) {
            isBattle = true;
            Debug.Log("Battle");
        }
    }
}
