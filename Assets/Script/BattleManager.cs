using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleManager : MonoBehaviour {
    Tilemap tilemap;
    TileBase BattleTile;
    TileBase BenchTile;

    void Start()
    {
        
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
