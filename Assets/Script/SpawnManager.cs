using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public GameObject[] Chara_List;
    void Start() {
        
    }

    void Spawn_Chara() {
        bool isSpawn = MapManager.instance.Check_Bench();
        if(!isSpawn) return;
        for(int i = 0; i < MapManager.instance.Bench_length; i++) {
            
        }
     }
    void Update()
    {
        
    }
}
