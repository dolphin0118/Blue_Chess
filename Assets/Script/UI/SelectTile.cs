using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SelectTile: MonoBehaviour {
    public Tilemap tilemap;
    public Vector3 tilePos{get;set;}
    void Update(){
        Select_Tile();
    }

    void Select_Tile() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            Vector3Int tilepos = tilemap.LocalToCell(hit.point);
            Vector3 pos = tilemap.GetCellCenterLocal(tilepos);
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
            tilePos = this.transform.position;    
        }
    }
    //이 타일의 좌표를 vector3.up의 레이로 쏴서 맞은 캐릭터 객체 불러오기
}
