using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour {
    public Tilemap tilemap;
    public TileBase BattleTile;
    public TileBase BenchTile;

    public List<List<bool>> isBattle = new List<List<bool>>();
    public List<TileBase> BenchTiles;
    public List<bool> isBench;

    public int Battle_width = 10;
    public int Battle_height = 10;
    public int Bench_length = 8;
    public int tile_gap = 4;

    public static MapManager instance = null;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }
        Bench_Init();
        Battle_Init();
    }

    void FixedUpdate() {  
        Battle_seat();
    }

    void Bench_Init() {
        for (int i = 0; i < Bench_length; i++){
            Vector3Int tilepos = new Vector3Int(i - 4 , i, 0);
            BenchTiles.Add(tilemap.GetTile(tilepos));
            isBench.Add(false);
        }
    }

    void Battle_Init() {
        for(int i = 0; i < Battle_height; i++) {
            isBattle.Add(new List<bool>());
            for(int j = 0; j < Battle_width; j++) {
                Vector3Int tilepos = new Vector3Int(j - 4, i - 6, 0);
                isBattle[i].Add(false);
            }
        }
    }

    Vector2Int Coord_Lerp(Vector3Int pos) {
        int Tile_x = 4;
        int Tile_y = 6;
        int Lerp_x = pos.x + Tile_x;
        int Lerp_y = pos.y + Tile_y;
        Vector2Int Lerp_pos = new Vector2Int(Lerp_x, Lerp_y);
        return Lerp_pos;
    }

    public void Battle_seat() {
        if(GameObject.FindGameObjectWithTag("Home").gameObject.transform.childCount > 0) {
            CharaLocate[] Chara_pos = GameObject.FindGameObjectWithTag("Home").GetComponentsInChildren<CharaLocate>(); 
            for(int i = 0; i < Chara_pos.Length; i++) {
                Vector2Int current_pos = Coord_Lerp(Chara_pos[i].Player_Tilepos());
                if(current_pos.x >=0 && current_pos.y >= 0) {
                    isBattle[current_pos.x][current_pos.y] = true;            
                }
            }
        }
    }


    public void Bench_seat(int Bench_num, bool ischeck) {
        int Bench_Num = Bench_num + tile_gap;
        isBench[Bench_Num] = ischeck;
    }

    public bool Check_Bench() {
        for(int i = 0; i < isBench.Count; i++) {
            if(!isBench[i]) return true;
            else continue;
        }
        return false;
    }
}
