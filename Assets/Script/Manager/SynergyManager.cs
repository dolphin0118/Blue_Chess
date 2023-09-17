using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
public class SynergyManager : MonoBehaviour {

    public static SynergyManager instance = null;
    public List<Synergy> SynergyList = Enum.GetValues(typeof(Synergy)).Cast<Synergy>().ToList();
    List<SynergyAll> synergyAll;
    public List<GameObject> SynergyObjects;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else{
            if (instance != this) Destroy(this.gameObject);
        }
        
        SynergyInit();
    }

    void Update() {
        SynergyGeneral();
    }

    void SynergyInit() {
        for(int i = 0; i < SynergyObjects.Count; i++) {
            SynergyObjects[i] = this.transform.GetChild(i).gameObject;
            SynergyObjects[i].gameObject.SetActive(false);
        }
        synergyAll = new List<SynergyAll>();
        synergyAll.Add(new AbydosSynergy());
        synergyAll.Add(new MillenniumSynergy());
        synergyAll.Add(new MysterySynergy());
        synergyAll.Add(new SlayerSynergy());
    }

    void SynergyGeneral() {
        SynergyCounter();
        SynergyInvoke(); 
    }

    void SynergyCounter() {
        for(int i = 0; i < synergyAll.Count; i++) synergyAll[i].ResetCount();

        for(int i = 0; i < SynergyList.Count; i++) {
            string tempSynergy = Convert.ToString(SynergyList[i]);
            int nindex = synergyAll.FindIndex(x => x.synergyTag == tempSynergy);
            if(nindex != -1) {
                synergyAll[nindex].AddCount();
            }
        }
    }

    void SynergyDisable() {
        for(int i = 0; i < SynergyObjects.Count; i++) {
            SynergyObjects[i].SetActive(false);
            SynergyObjects[i].GetComponent<SynergyUi>().ChangeMaterial(0);
        }
    }
    void SynergyInvoke() {
        SynergyDisable();
        for(int i = 0; i < synergyAll.Count; i++) {
            int synergyCnt = synergyAll[i].SynergyInvoke();
            if(synergyCnt != -1) {
                int nindex = SynergyObjects.FindIndex(x => x.tag == synergyAll[i].synergyTag);
                SynergyObjects[nindex].SetActive(true);
                SynergyObjects[nindex].GetComponent<SynergyUi>().ChangeMaterial(synergyAll[i].synergyCount);
            }
        }
    }
}
