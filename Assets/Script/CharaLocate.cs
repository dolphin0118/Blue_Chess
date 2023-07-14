using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharaLocate : MonoBehaviour {
    [SerializeField] Tilemap tilemap;
    [SerializeField] TileBase BattleTile;
    [SerializeField] TileBase BenchTile;
    Transform cam;
    RaycastHit hitRay, hitLayerMask;
    GameObject ObjectHitPosition;
    Vector3 previous_pos, current_pos;

    void Awake() {
        cam = Camera.main.transform;
    }

    void Player_Rotate() {
        Vector3 pos = this.transform.position;
        Vector3Int tilepos = tilemap.LocalToCell(pos);
        TileBase UnderTile = tilemap.GetTile(tilepos);
        if(UnderTile != null && UnderTile.name == BenchTile.name) {
            transform.rotation = Quaternion.Euler(-20, 180, 0);
            Debug.Log("Bench");
            Debug.Log(tilepos);
        }
        else if(UnderTile != null && UnderTile.name == BattleTile.name) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Battle");
            Debug.Log(tilepos);
        }
    }

    void OnMouseUp() {
        this.transform.parent = null;
        CorrectPos();
        Player_Rotate();
        Destroy(ObjectHitPosition);
    }

    void CorrectPos() {
        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = -this.transform.up;
        Vector3Int tilemapCell = Vector3Int.zero;
        Vector3 pos = this.transform.position;
        Vector3Int tilepos = tilemap.LocalToCell(pos);
        pos = tilemap.GetCellCenterLocal(tilepos);
        transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
    }

    void OnMouseDown() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitRay)) {
            ObjectHitPosition = new GameObject("HitPosition");
            ObjectHitPosition.transform.position = hitRay.point;
            this.transform.SetParent(ObjectHitPosition.transform);
            previous_pos = this.transform.position;
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

    void Chara_transfer(){

    }

}
