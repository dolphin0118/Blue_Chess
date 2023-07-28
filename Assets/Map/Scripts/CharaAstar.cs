using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAstar : MonoBehaviour {

    List<Vector3Int> Tile_Pos;
    public List<List<bool>> isBattle;
    public Block[,] blocks;

    Vector2Int Coord_Lerp() {
        Vector3Int pos = this.GetComponent<CharaLocate>().Player_Tilepos();
        int Tile_x = 4;
        int Tile_y = 6;
        int Lerp_x = pos.x + Tile_x;
        int Lerp_y = pos.y + Tile_y;
        Vector2Int Lerp_pos = new Vector2Int(Lerp_x, Lerp_y);
        return Lerp_pos;
    }

    bool Exists(int x, int y) {
        if(isBattle[x][y]) return true;
        else return false;
    }

    void TileCheck() {
        isBattle = MapManager.instance.isBattle;
        Vector2Int target = Coord_Lerp();
        List<Block> arounds = new List<Block>();
        if (Exists(target.x - 1, target.y - 1)) {
            Block block = blocks[target.x - 1, target.y - 1];
            arounds.Add(block);
        }
        if (Exists(target.x, target.y - 1))
        {
            Block block = blocks[target.x, target.y - 1];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y - 1))
        {
            Block block = blocks[target.x + 1, target.y - 1];
            arounds.Add(block);
        }

        if (Exists(target.x - 1, target.y))
        {
            Block block = blocks[target.x - 1, target.y];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y))
        {
            Block block = blocks[target.x + 1, target.y];
            arounds.Add(block);
        }

        if (Exists(target.x - 1, target.y + 1))
        {
            Block block = blocks[target.x - 1, target.y + 1];
            arounds.Add(block);
        }
        if (Exists(target.x, target.y + 1))
        {
            Block block = blocks[target.x, target.y + 1];
            arounds.Add(block);
        }
        if (Exists(target.x + 1, target.y + 1))
        {
            Block block = blocks[target.x + 1, target.y + 1];
            arounds.Add(block);
        }
    }
    void Update() {
        
    }
}
