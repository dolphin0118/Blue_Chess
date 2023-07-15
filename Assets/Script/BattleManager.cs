using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleManager : MonoBehaviour {
    Tilemap tilemap;
    TileBase BattleTile;
    TileBase[] BenchTile;
    int Bench_length = 9;
    void Start() {

    }
    void Init() {
        for (int i = 0; i < Bench_length; i++) {
            Vector3Int tilepos = new Vector3Int(-4, i, 0);
            BenchTile[i] = tilemap.GetTile(tilepos);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    bool Check_Tile(Vector3 get_transform) {
        Vector3Int tilepos = tilemap.LocalToCell(get_transform);
        TileBase UnderTile = tilemap.GetTile(tilepos);
        if(UnderTile == BattleTile) return true;
        else return false;
    }
}
