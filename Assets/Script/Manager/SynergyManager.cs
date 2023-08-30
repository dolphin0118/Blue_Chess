using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoBehaviour {
    public static SynergyManager instance = null;
    public static Dictionary<string, int> SynergyList = new Dictionary<string, int>();
    

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else{
            if (instance != this) Destroy(this.gameObject);
        }
    }

    void AbydosSynergy() {
        int[] synergyStack = {1, 2, 5};
        int synergyCheck = synergyStack.Length;
        bool isSynergy = false;
        while(synergyCheck > 0) {
            if(synergyStack[synergyCheck] > SynergyList["Abydos"]) {
                synergyCheck--;
                break;
            }
            else isSynergy = true;
        }
        if(!isSynergy) return;
        else if(isSynergy) {
            GameObject[] AbydosChara = GameObject.FindGameObjectsWithTag("Abydos");
            for(int i = 0; i < AbydosChara.Length; i++) {
                AbydosChara[i].GetComponent<CharaInfo>().Player_Hp *= 1.5f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
