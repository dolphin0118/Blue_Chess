using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GameManager : MonoBehaviour {
    public static GameManager instance = null; 
    public static  bool isBattle = false;
    public Tilemap tilemap;
    private void Awake() {
        if (instance == null) {
            instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject); 
        }
    }

}
