using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SelectTile: MonoBehaviour {
    public Tilemap tilemap;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    void Update(){
        TilePosition();
    }

    void TilePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int battleLayer = 1 << LayerMask.NameToLayer("Battle");
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, battleLayer)) {
            spriteRenderer.enabled = true;
            Vector3Int tilePos = tilemap.LocalToCell(hit.point);
            Vector3 pos = tilemap.GetCellCenterLocal(tilePos);
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
        }
        else spriteRenderer.enabled = false;
    }
    //이 타일의 좌표를 vector3.up의 레이로 쏴서 맞은 캐릭터 객체 불러오기
}
