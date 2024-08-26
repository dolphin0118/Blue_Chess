using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SpawnSystem : MonoBehaviour {
    public static SpawnSystem instance = null;
    [NonSerialized] public CharaCard[] charaCards;
    bool[] checkSlot = new bool[9];
    GameObject BenchArea;
    
    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            if (instance != this) Destroy(this.gameObject);
        }
        BenchArea = GameObject.FindGameObjectWithTag("BenchArea");
        charaCards = Resources.LoadAll<CharaCard>("Scriptable");
    }


    public bool isSpawnable() {
        bool isSpawn = false;
        for(int i = 0; i < checkSlot.Length; i++) {
            if(BenchArea.transform.GetChild(i).childCount == 0) {
                checkSlot[i] = false;
                isSpawn = true;
            }
            else checkSlot[i] = true;
        }
        return isSpawn;
    }

    public void SpawnChara(CharaCard charaCard) {
        for(int i = 0; i < checkSlot.Length; i++) {
            if(!checkSlot[i]) {
                Vector3 spawnPos = new Vector3(-4, 0.1f, -6);
                Vector3 prefabPos = new Vector3(spawnPos.x + i, spawnPos.y, spawnPos.z);
                Vector3Int tilepos = GameManager.instance.tilemap.LocalToCell(prefabPos);
                prefabPos = GameManager.instance.tilemap.GetCellCenterLocal(tilepos);
                prefabPos = new Vector3(prefabPos.x, 0.1f, prefabPos.z);
                
                GameObject CharaClone = Instantiate(charaCard.CharaPrefab, prefabPos, Quaternion.identity);
                CharaClone.transform.SetParent(BenchArea.transform.GetChild(i));
                CharaClone.gameObject.tag = "Friendly";    
                CharaClone.GetComponent<CharaInfo>().CharaDataSetup(charaCard);
                break;
            }
        }
    }
}
