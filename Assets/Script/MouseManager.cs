using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MouseManager : MonoBehaviour {
    public Tilemap tilemap;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Select_Tile();
    }

    void Select_Tile() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            Vector3Int tilepos = tilemap.LocalToCell(hit.point);
            Vector3 pos = tilemap.GetCellCenterLocal(tilepos);
            transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
        }

    }
}
