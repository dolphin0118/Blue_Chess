using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    GameObject charaObject;
    GameObject symbolObject;
    bool isBattleTile;
    void Start() {
        symbolObject = GetComponent<GameObject>();
        string symbolName = symbolObject.tag;
        SynergyManager.SynergyList[symbolName] += 1;
        //isBattleTile = charaObject.GetComponent<CharaLocate>().isBattleTile;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
