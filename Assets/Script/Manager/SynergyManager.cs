using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
public class SynergyManager : MonoBehaviour {

    public static SynergyManager instance = null;
    public List<Synergy> SynergyList = Enum.GetValues(typeof(Synergy)).Cast<Synergy>().ToList();
    public List<GameObject> SynergyObjects;
    List<SynergyAll> synergyAll;
    List<SynergyUi> synergyUi;

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
        synergyUi = new List<SynergyUi>();
        for(int i = 0; i < SynergyObjects.Count; i++) {
            SynergyObjects[i] = this.transform.GetChild(i).gameObject;
            synergyUi.Add(SynergyObjects[i].GetComponent<SynergyUi>());
        }

        synergyAll = new List<SynergyAll>();
        synergyAll.Add(new AbydosSynergy());
        synergyAll.Add(new MillenniumSynergy());
        synergyAll.Add(new GehennaSynergy());
        synergyAll.Add(new TrinitySynergy());
        synergyAll.Add(new MysterySynergy());
        synergyAll.Add(new SniperSynergy());
        synergyAll.Add(new SlayerSynergy());
        synergyAll.Add(new GuardianSynergy());
        synergyAll.Add(new EngineerSynergy());
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
            synergyUi[i].ChangeActive(false);
            synergyUi[i].ChangeMaterial(0);
        }
    }

    void SynergyInvoke() {
        SynergyDisable();
        for(int i = 0; i < synergyAll.Count; i++) {
            int synergyCnt = synergyAll[i].SynergyInvoke();
            if(synergyCnt != -1) {
                int nindex = SynergyObjects.FindIndex(x => x.tag == synergyAll[i].synergyTag);
                synergyUi[i].ChangeActive(true);
                synergyUi[i].ChangeMaterial(synergyAll[i].synergyCount);
            }
        }
        SynergySort();
    }
    
    void SynergySort() {  
        List<int> OrderList = new List<int>();
        for(int i = 0 ; i <synergyAll.Count; i++) {
            int orderIndex = synergyAll[i].synergyOrder;
            OrderList.Add(orderIndex);
        }
        OrderList = OrderList.OrderByDescending(x => x).ToList();
        for(int i = 0; i < synergyUi.Count; i++) {
            if(synergyAll[i].synergyOrder == 0) {
                int maxOrder = synergyUi.Count;
                synergyUi[i].ChangeOrder(maxOrder);
                continue;
            }
            int nindex = OrderList.FindIndex(x => x == synergyAll[i].synergyOrder);
            synergyUi[i].ChangeOrder(nindex);
            OrderList[nindex] = 0;
        }
    }
}
