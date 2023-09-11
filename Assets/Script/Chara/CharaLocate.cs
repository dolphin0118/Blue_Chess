using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharaLocate : MonoBehaviour {
    Tilemap tilemap;
    TileBase BattleTile, BenchTile;
    Transform cam;
    RaycastHit hitRay, hitLayerMask;
    GameObject ObjectHitPosition, previousParent;
    Vector3 previous_pos;

    void Start() {
        cam = Camera.main.transform;
        tilemap = MapManager.instance.tilemap;
        BattleTile = MapManager.instance.BattleTile;
        BenchTile = MapManager.instance.BenchTile;
        Player_Rotate();
    }

    public Vector3Int Player_Tilepos() {
        Vector3 Player_pos = this.transform.position;
        Vector3Int tile_pos = tilemap.LocalToCell(Player_pos);
        return tile_pos;
    }

    public TileBase Player_Tile() {
        TileBase Player_tile = tilemap.GetTile(Player_Tilepos());
        return Player_tile;
    }

    bool Check_Tile(){
        TileBase UnderTile = Player_Tile();
        if(UnderTile != null && UnderTile.name == BenchTile.name) {
            return false;
        }
        else if(UnderTile != null &&UnderTile.name == BattleTile.name) {
            return true;
        }
        else return false;
    }
    
    void Player_Rotate() {
        Vector3Int current_tilepos = Player_Tilepos();
        Vector3Int previous_tilepos = tilemap.LocalToCell(previous_pos);
        TileBase current_UnderTile = Player_Tile();
        TileBase previous_UnderTile = tilemap.GetTile(previous_tilepos);
        bool isBattleTile = Check_Tile();
        if(!isBattleTile) {
            this.transform.rotation = Quaternion.Euler(-20, 180, 0);
            MapManager.instance.Bench_seat(current_tilepos.x, true);
        }
        else if(isBattleTile) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        //if(previous_UnderTile.name == BenchTile.name) MapManager.instance.Bench_seat(previous_tilepos.x, false);
    }

    void OnMouseUp() {
        this.transform.SetParent(previousParent.transform);
        TileBase UnderTile = Player_Tile();
        int layerMask = 1 << LayerMask.NameToLayer("Bench") | 1 << LayerMask.NameToLayer("Battle");
        Vector3 charaPos = this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(charaPos, Vector3.down, out hit, Mathf.Infinity, layerMask)) {
            Vector3 hitPos = hit.transform.position;
            transform.position = new Vector3((int)hitPos.x, transform.position.y, (int)hitPos.z);
            Player_Rotate();
            Destroy(ObjectHitPosition);
        }
        else {
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(previous_pos));
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
            Destroy(ObjectHitPosition);
        }
    }

    void OnMouseDown() {
        previous_pos = this.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitRay)) {
            ObjectHitPosition = new GameObject("HitPosition");
            previousParent= this.transform.parent.gameObject;
            ObjectHitPosition.transform.position = hitRay.point;
            this.transform.SetParent(ObjectHitPosition.transform);   
        }
        
    }

    void OnMouseDrag() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        int layerMask = 1 << LayerMask.NameToLayer("Stage");
        if(Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask))
        {
            float H = Camera.main.transform.position.y;
            float h = ObjectHitPosition.transform.position.y;

            Vector3 newPos 
            	= (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;

            ObjectHitPosition.transform.position = newPos;
        }
        BenchLayer();
    }
    void BenchLayer() {
        Vector3 charaPos = this.transform.position;
        RaycastHit hit;
        int layerMask = 1 << LayerMask.NameToLayer("Bench");
        if (Physics.Raycast(charaPos, Vector3.down, out hit, Mathf.Infinity, layerMask)) {
            GameObject hitBench = hit.collider.transform.gameObject;
            if (hitBench.transform.childCount == 0) {
                previousParent = hitBench;
            }
            else {
                hitBench.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 0, 0);
                hitBench.transform.GetChild(0).SetParent(previousParent.transform);     
                previousParent = hitBench;
            }
        }

    }
}
  