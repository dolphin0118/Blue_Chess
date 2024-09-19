using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SelectTile: MonoBehaviour {
    public static SelectTile instance = null;
    public Tilemap tilemap;
    public Vector3Int tilePos{get;set;}

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }    
    }

    void Update(){
        TilePosition();
    }

    void TilePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            tilePos = tilemap.LocalToCell(hit.point);
            Vector3 pos = tilemap.GetCellCenterLocal(tilePos);
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
            //Debug.Log(tilePos);
        }
    }
    //이 타일의 좌표를 vector3.up의 레이로 쏴서 맞은 캐릭터 객체 불러오기
}
