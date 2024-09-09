using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitLocate : ObjectControll {
    Tilemap tilemap;
    Collider UnitCollier;
    Vector3 currentPos;
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
        OutLayer();
        Destroy(ObjectHitPosition);
    }

    public override void OnObjectControll() {
        base.OnObjectControll();
        SynergyManager.instance.synergyEvent.AddListener(GetComponent<UnitInfo>().SynergyRemove);
        SynergyManager.instance.synergyEvent.Invoke();

    }

    public override void OnObjectMove() {
        base.OnObjectMove();
    }

    void OutLayer() {
        this.transform.SetParent(previousParent.transform);
        int battleLayer = 1 << LayerMask.NameToLayer("Battle");
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
            SynergyManager.instance.synergyEvent.AddListener(this.transform.GetComponent<UnitInfo>().SynergyAdd);
            SynergyManager.instance.synergyEvent.Invoke();
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
        int BenchLayer = 1 << LayerMask.NameToLayer("Bench");
        int BattleLayer = 1 << LayerMask.NameToLayer("Battle");
        if (Physics.Raycast(currentPos, Vector3.down, out hit, Mathf.Infinity, BenchLayer)){
            GameObject hitBench = hit.collider.transform.gameObject;
            if (hitBench.transform.childCount == 0) {
                previousParent = hitBench;
            }
            else {
                hitBench.transform.GetChild(0).SetParent(previousParent.transform);     
                previousParent.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 0, 0);
                previousParent = hitBench;
            }
            Debug.Log("Bench");
        }
        else if(Physics.Raycast(currentPos, Vector3.down, Mathf.Infinity, BattleLayer)) {
            int UnitLayer = 1 << LayerMask.NameToLayer("Unit");
            Vector3 checkPos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(currentPos));
            checkPos = new Vector3(checkPos.x, 0.1f, checkPos.z);
            
            if (Physics.CheckSphere(checkPos, 0.1f, UnitLayer)) {
                Collider[] hitColliders = Physics.OverlapSphere(checkPos, 0.1f, UnitLayer);
                GameObject hitUnit = hitColliders[0].transform.gameObject;
                this.transform.position = hitUnit.transform.position;
                hitUnit.transform.position = new Vector3(previousPos.x, 0.1f, previousPos.z);
                Debug.Log("Swap");
            }
            else {
                this.transform.position = checkPos;
                Debug.Log("battle");
            }
        }
    }
}
  
        //   else if(Physics.Raycast(currentPos, Vector3.down, Mathf.Infinity, BattleLayer)) {
        //     int layerUnit = 1 << LayerMask.NameToLayer("Unit");
        //     Vector3 checkPos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(currentPos));
        //     checkPos = new Vector3(checkPos.x, -1.0f, checkPos.z);
        //     if(Physics.Raycast(checkPos,Vector3.up, out hit, Mathf.Infinity, layerUnit)) {
        //         if(hit.collider.gameObject == this.gameObject) return;
        //         else Debug.Log("battle");
        //         GameObject hitUnit = hit.collider.transform.gameObject;
        //         this.transform.position = hitUnit.transform.position;
        //         hitUnit.transform.position = new Vector3(previousPos.x, 0.1f, previousPos.z);
              
        //     }
        // }