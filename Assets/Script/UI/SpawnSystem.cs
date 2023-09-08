using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SpawnSystem : MonoBehaviour{
    public static SpawnSystem  instance = null;
    public Chara[] Chara_List;
    private Vector3 Spawn_Pos = new Vector3(-4, 0, -6);
    bool[] checkSlot = new bool[5];
    GameObject teamObject;
    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }
        teamObject = GameObject.FindGameObjectWithTag("Home");
    }
    void Update() {
        for(int i = 0; i < checkSlot.Length; i++) {
            if(teamObject.transform.GetChild(i).childCount == 0) {
                checkSlot[i] = false;
            }
            else checkSlot[i] = true;
        }
    }
    public void Spawn_Chara(GameObject Chara_Prefab) {
        for(int i = 0; i < checkSlot.Length; i++) {
            if(!checkSlot[i]) {
                Vector3 Prefab_Pos = new Vector3(Spawn_Pos.x + i, Spawn_Pos.y, Spawn_Pos.z);
                Vector3Int tilepos = MapManager.instance.tilemap.LocalToCell(Prefab_Pos);
                Prefab_Pos = MapManager.instance.tilemap.GetCellCenterLocal(tilepos);
                Prefab_Pos = new Vector3(Prefab_Pos.x, 0.1f, Prefab_Pos.z);
                GameObject Chara_Clone = Instantiate(Chara_Prefab,Prefab_Pos, Quaternion.identity);//prefab 소환
                Chara_Clone.transform.SetParent(teamObject.transform.GetChild(i));
                break;
            }
        }
    }

    public void Destroy_Chara(GameObject Chara_Object) {
        Vector3Int tilepos = MapManager.instance.tilemap.LocalToCell(Chara_Object.transform.position);
        MapManager.instance.Bench_seat(tilepos.x, false);
        Destroy(Chara_Object);
    }
}
