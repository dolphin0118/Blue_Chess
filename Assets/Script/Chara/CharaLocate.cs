using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharaLocate : MonoBehaviour {
    Tilemap tilemap;
    RaycastHit hitRay, hitLayerMask;
    GameObject ObjectHitPosition, previousParent;
    Vector3 previous_pos;
    [SerializeField] GameObject BattleArea;
    Quaternion benchRotate,battleRotate;
    
    void Start() {
        tilemap = MapManager.instance.tilemap;
        benchRotate = Quaternion.Euler(-20, 180,0);
        battleRotate = Quaternion.Euler(0, 0, 0);
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
    public bool LayerCheck() {
        int benchLayer = 1 << LayerMask.NameToLayer("Bench");
        int battleLayer = 1 << LayerMask.NameToLayer("Battle");
        if(Physics.Raycast(this.transform.position, Vector3.down, Mathf.Infinity, benchLayer)) {
            return false;
        }
        else if(Physics.Raycast(this.transform.position, Vector3.down, Mathf.Infinity, battleLayer)){
            return true;
        }
        else return false;
    }   

    void OnMouseUp() {
        this.transform.SetParent(previousParent.transform);
        int battleLayer= 1 << LayerMask.NameToLayer("Battle");
        int benchLayer = 1 << LayerMask.NameToLayer("Bench");
        Vector3 charaPos = this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(charaPos, Vector3.down, out hit, Mathf.Infinity, benchLayer)) {
            Vector3 hitPos = hit.transform.position;
            transform.localPosition = new Vector3(0, 0.1f, 0);
            this.transform.rotation = benchRotate;
        }
        else if(Physics.Raycast(charaPos, Vector3.down, out hit, Mathf.Infinity, battleLayer)){
            this.transform.SetParent(GameObject.FindGameObjectWithTag("BattleArea").transform);
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(this.transform.position));
            transform.position = new Vector3(pos.x, 0.1f, pos.z);
            this.transform.rotation = battleRotate;
        }
        else {
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(previous_pos));
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
            transform.localPosition = new Vector3(0, transform.position.y, 0);
        }
        Destroy(ObjectHitPosition);
    }

    void OnMouseDown() {
        previous_pos = this.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitRay)) {
            ObjectHitPosition = new GameObject("HitPosition");
            previousParent= this.transform.parent.gameObject;
            ObjectHitPosition.transform.position = hitRay.point;
            this.transform.SetParent(ObjectHitPosition.transform);   
            this.transform.localPosition = new Vector3(0, 0.1f, 0);
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
        Debug.DrawRay(charaPos, Vector3.down * Mathf.Infinity, Color.green);

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
  