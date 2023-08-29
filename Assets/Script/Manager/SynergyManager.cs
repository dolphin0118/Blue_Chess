using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyManager : MonoBehaviour {
    static public Dictionary<string, int> SynergyList = new Dictionary<string, int>();
    void Start()
    {
        
    }

    void AbydosSynergy() {
        int[] synergyStack = {1, 2, 5};
        int synergyCheck = synergyStack.Length;
        bool isSynergy = false;
        while(synergyCheck > 0) {
            if(synergyStack[synergyCheck] > SynergyList["Abydos"]) {
                synergyCheck--;
                return;
            }
            else isSynergy = true;
        }
        if(!isSynergy) return;
        else if(isSynergy) {
            GameObject[] AbydosChara = GameObject.FindGameObjectsWithTag("Abydos");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
