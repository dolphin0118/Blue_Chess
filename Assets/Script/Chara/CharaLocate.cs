using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharaLocate : MonoBehaviour {
    Tilemap tilemap;
    Collider charaCollier;
    GameObject ObjectHitPosition, previousParent;

    RaycastHit hitRay, hitLayerMask;
    Vector3 currentPos,previousPos;
    Quaternion benchRotate, battleRotate;
    public bool isBattleLayer {get; set;}

    [SerializeField] GameObject BattleArea;

    void Start() {
        tilemap = GameManager.instance.tilemap;
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
    void OnMouseUp() {
        CheckLayer();
        outLayer();
        Destroy(ObjectHitPosition);
    }

    void OnMouseDown() {
        previousPos = this.transform.position;
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
        int layerMask = 1 << LayerMask.NameToLayer("Stage");
        if(Physics.Raycast(ray, out hitLayerMask, Mathf.Infinity, layerMask)) {
            float H = Camera.main.transform.position.y;
            float h = ObjectHitPosition.transform.position.y;
            Vector3 newPos = (hitLayerMask.point * (H - h) + Camera.main.transform.position * h) / H;
            ObjectHitPosition.transform.position = newPos;
        }
    }
    void outLayer() {
        this.transform.SetParent(previousParent.transform);
        int battleLayer= 1 << LayerMask.NameToLayer("Battle");
        int benchLayer = 1 << LayerMask.NameToLayer("Bench");
        currentPos = this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(currentPos, Vector3.down, out hit, Mathf.Infinity, benchLayer)) {
            Vector3 hitPos = hit.transform.position;
            transform.localPosition = new Vector3(0, 0.1f, 0);
            this.transform.rotation = benchRotate;
        }
        else if(Physics.Raycast(currentPos, Vector3.down, out hit, Mathf.Infinity, battleLayer)){
            this.transform.SetParent(GameObject.FindGameObjectWithTag("BattleArea").transform);
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(this.transform.position));
            transform.position = new Vector3(pos.x, 0.1f, pos.z);
            this.transform.rotation = battleRotate;
            isBattleLayer = true;
        }
        else {
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(previousPos));
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
            transform.localPosition = new Vector3(0, transform.position.y, 0);
        }
    }
    void CheckLayer() {
        currentPos = this.transform.position;
        RaycastHit hit;
        int layerBench = 1 << LayerMask.NameToLayer("Bench");
        int layerBattle = 1 << LayerMask.NameToLayer("Battle");
        if (Physics.Raycast(currentPos, Vector3.down, out hit, Mathf.Infinity, layerBench)){
            GameObject hitBench = hit.collider.transform.gameObject;
            if (hitBench.transform.childCount == 0) {
                previousParent = hitBench;
            }
            else {
                hitBench.transform.GetChild(0).SetParent(previousParent.transform);     
                previousParent.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 0, 0);
                previousParent = hitBench;
            }
        }
        else if(Physics.Raycast(currentPos, Vector3.down, Mathf.Infinity, layerBattle)) {
            int layerChara = 1 << LayerMask.NameToLayer("Chara");
            Vector3 checkPos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(currentPos));
            checkPos = new Vector3(checkPos.x, -1.0f, checkPos.z);
            if(Physics.Raycast(checkPos,Vector3.up, out hit, Mathf.Infinity, layerChara)) {
                if(hit.collider.gameObject == this.gameObject) return;
                else Debug.Log("battle");
                GameObject hitChara = hit.collider.transform.gameObject;
                this.transform.position = hitChara.transform.position;
                hitChara.transform.position = new Vector3(previousPos.x, 0.1f, previousPos.z);
              
            }
        }
    }
}
  