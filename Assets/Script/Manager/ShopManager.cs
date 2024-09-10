using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    public static ShopManager instance = null; 
    public int Gold {get; set;}
    private SpawnSystem spawnSystem;
    
    private void Awake() {
        if (instance == null) {
            instance = this; 
            //DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject); 
        }
    }

  
    void Start() {
        
    }

    void Update() {
        
    }

    
    public void RerollChara() {
        //spawnSystem.SpawnChara();
    }

    public void AddExp() {
         
    }

}
