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
    Vector3 previous_pos, current_pos;

    void Start() {
        cam = Camera.main.transform;
        tilemap = MapManager.instance.tilemap;
        BattleTile = MapManager.instance.BattleTile;
        BenchTile = MapManager.instance.BenchTile;
    }
    void Update() {
        Player_Rotate();
    }

    Vector3 tile_pos() {
        Vector3 Player_pos = this.transform.position;
        Vector3Int tile_pos = tilemap.LocalToCell(Player_pos);
        Player_pos = tilemap.GetCellCenterLocal(tile_pos);
        return Player_pos;
    }

    void Player_Rotate() {
        current_pos = this.transform.position;
        Vector3Int current_tilepos = tilemap.LocalToCell(current_pos);
        Vector3Int previous_tilepos = tilemap.LocalToCell(previous_pos);
        TileBase UnderTile = tilemap.GetTile(current_tilepos);
        TileBase previous_UnderTile = tilemap.GetTile(previous_tilepos);
        
        if(UnderTile != null && UnderTile.name == BenchTile.name) {
            transform.rotation = Quaternion.Euler(-20, 180, 0);
            MapManager.instance.isBench[current_tilepos.x + 4] = true;
        }
        else if(UnderTile != null && UnderTile.name == BattleTile.name) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if(previous_UnderTile.name == BenchTile.name) MapManager.instance.isBench[previous_tilepos.x + 4] = false;
    }

    void OnMouseUp() {
        this.transform.SetParent(previousParent.transform);
        CorrectPos();
        Player_Rotate();     
        Destroy(ObjectHitPosition);
    }

    void CorrectPos() {
        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = -this.transform.up;
        Vector3 pos = tile_pos();
        transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
    }
    void Check_Tile() {
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
    }

}
