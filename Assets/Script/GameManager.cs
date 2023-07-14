using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; 
    public static  bool isBattle = false;
    private void Awake() {
        if (instance == null) {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject); 
        }
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)) {
            isBattle = true;
            Debug.Log("Battle");
        }
    }
}
