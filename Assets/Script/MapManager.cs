using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour {
    public Tilemap tilemap;
    public TileBase BattleTile;
    public TileBase BenchTile;

    public TileBase[] BenchTiles;
    public bool[] isBench;
    public int Bench_length = 9;

    public static MapManager instance = null;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }
    }
    void Bench_Init() {

        for (int i = 0; i < Bench_length; i++){
            Vector3Int tilepos = new Vector3Int(i - 4 , i, 0);
            BenchTiles[i] = tilemap.GetTile(tilepos);
            isBench[i] = false;
        }
    }

    public bool Check_Bench() {
        for(int i = 0; i < isBench.Length; i++) {
            if(isBench[i]) return true;
            else continue;
        }
        return false;
    }

    void Update() {
        
    }
}
