using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;

public class UnitLocate : ObjectControll {
    [SerializeField] GameObject BattleArea;
    private Tilemap tilemap;
    private Quaternion benchRotate, battleRotate;
  
    int battleLayer, benchLayer, unitLayer;

    void Start() {
        tilemap = GameManager.instance.tilemap;
        benchRotate = Quaternion.Euler(-20, 180,0);
        battleRotate = Quaternion.Euler(0, 0, 0);
        unitLayer= 1 << LayerMask.NameToLayer("Unit");
        battleLayer = 1 << LayerMask.NameToLayer("Battle");
        benchLayer = 1 << LayerMask.NameToLayer("Bench");
    }

    void OnMouseUp() {
        CheckLayer();
        OutLayer();
        Destroy(ObjectHitPosition);
    }

    public void ForceLocate() {
        if(ObjectHitPosition != null) {
            this.transform.SetParent(previousParent.transform);
            Destroy(ObjectHitPosition);
        }
    }

    public override void OnObjectControll() {
        base.OnObjectControll();
        SynergyManager.instance.synergyEvent.AddListener(GetComponent<UnitInfo>().SynergyRemove);
        SynergyManager.instance.synergyEvent.Invoke();

    }

    public override void OnObjectMove() {
        base.OnObjectMove();
    }
    void CheckLayer() {
        GameObject swapUnit = null;
        bool isSwap = false;
        
        Vector3 currentPos = this.transform.position;
        Vector3Int tilePos = tilemap.LocalToCell(currentPos);
        Vector3 checkPos = tilemap.GetCellCenterLocal(tilePos);
        checkPos = new Vector3(checkPos.x, 0.1f, checkPos.z);
        //유닛 to 유닛 Swap
        if (Physics.CheckSphere(checkPos, 0.1f, unitLayer)) {
            isSwap = true;
            Collider[] hitColliders = Physics.OverlapSphere(checkPos, 0.1f, unitLayer);
            swapUnit = hitColliders[0].transform.gameObject;
            this.transform.position = swapUnit.transform.position;
            swapUnit.transform.position = new Vector3(previousPos.x, 0.1f, previousPos.z);        
            Debug.Log("Swap");
        }
        //벤치레이어일 경우
        RaycastHit hitLayer;
        if (Physics.Raycast(currentPos, Vector3.down, out hitLayer, Mathf.Infinity, benchLayer)){
            GameObject hitObject = hitLayer.collider.transform.gameObject;
            if (hitObject.transform.childCount == 0) {     
                previousParent = hitObject;  
            }
            else if(isSwap){
                hitObject.transform.GetChild(0).SetParent(previousParent.transform);     
                previousParent.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 0, 0);
                previousParent = hitObject;
            }
            Debug.Log("Bench");
        }
        //배틀레이어일 경우
        else if(Physics.Raycast(currentPos, Vector3.down, out hitLayer, Mathf.Infinity, battleLayer)) {
            //현재 타일이 배틀레이어 내부의 유효한 타일인지
            if(!CheckBattleTile()) {this.transform.position = previousPos; return;}

            GameObject hitObject = hitLayer.collider.transform.gameObject;
            this.transform.position = checkPos;
            if(!isSwap) {
                TeamManager.instance.UnitLocateDelete(CheckTilePoition(previousPos));
                TeamManager.instance.UnitLocateSave(CheckTilePoition(this.gameObject), this.gameObject);
            }
            if(isSwap && swapUnit != null) {
                TeamManager.instance.UnitLocateSwap(CheckTilePoition(this.gameObject), CheckTilePoition(swapUnit));
                swapUnit.transform.SetParent(previousParent.transform);
            }
            previousParent = hitObject;
            Debug.Log(tilePos);
            Debug.Log("battle");
        }
        else {
            this.transform.position = previousPos;
        }
    }

    void OutLayer() {
        this.transform.SetParent(previousParent.transform);
        Vector3 currentPos = this.transform.position;
        if (Physics.Raycast(currentPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, benchLayer)) {
            Vector3 hitPos = hit.transform.position;
            transform.localPosition = new Vector3(0, 0.1f, 0);
            this.transform.rotation = benchRotate;
        }
        else if(Physics.Raycast(currentPos, Vector3.down, out hit, Mathf.Infinity, battleLayer)){
            this.transform.SetParent(TeamManager.instance.BattleArea.transform);
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(this.transform.position));
            transform.position = new Vector3(pos.x, 0.1f, pos.z);
            this.transform.rotation = battleRotate;
            SynergyManager.instance.synergyEvent.AddListener(this.transform.GetComponent<UnitInfo>().SynergyAdd);
            SynergyManager.instance.synergyEvent.Invoke();
        }
        else {
            Vector3 pos = tilemap.GetCellCenterLocal(tilemap.LocalToCell(previousPos));
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
            transform.localPosition = new Vector3(0, transform.position.y, 0);
        }
    }
    
    Vector2Int CheckTilePoition(Vector3 checkPos) {
        //row -3 -2 -1 0 1 2 3
        //col -3 -4 -5 -6
        Vector3Int tilePos = tilemap.LocalToCell(checkPos);
        int row = tilePos.x + 3;
        int col = tilePos.y * -1 - 3;
        Vector2Int convertTilePos = new Vector2Int(row, col);

        return convertTilePos;
    }
    
    Vector2Int CheckTilePoition(GameObject checkUnit) {
        //row -3 -2 -1 0 1 2 3
        //col -3 -4 -5 -6
        Vector3Int tilePos = tilemap.LocalToCell(checkUnit.transform.position);
        int row = tilePos.x + 3;
        int col = tilePos.y * -1 - 3;
        Vector2Int convertTilePos = new Vector2Int(row, col);

        return convertTilePos;
    }

    bool CheckBattleTile() {
        Vector2Int tilePos = CheckTilePoition(this.gameObject);
        int row = tilePos.x;
        int col = tilePos.y;
        //bool isUnitLocate = TeamManager.instance.IsUnitLocate(row, col);
        if(row > 6 || row < 0||col > 3 || col < 0) return false;
        return true;
    }
}
  