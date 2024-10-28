using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.Events;
using Unity.VisualScripting;

public class SynergyManager : MonoBehaviour {

    public Dictionary<Synergy, int> synergyCount;
    public UnityEvent synergyEvent;
    private List<GameObject> SynergyUIGameObject;
    private List<SynergyUI> synergyUI;
    private List<SynergyBase> synergyBase;
    
    void Awake() { 
        SynergyInit();
    }

    void Update() {
        SynergyUpdate();
    }

    public void SynergyUpdate() {
        SynergyCounter();
        //SynergySort();
        SynergySort("a");
    } 

    void SynergyInit() {
        synergyBase = new List<SynergyBase>();//Class바인딩
        var synergyClasses = Assembly.GetAssembly(typeof(SynergyBase)).GetTypes()
                            .Where(t => t.IsSubclassOf(typeof(SynergyBase))).ToList();
        foreach(var type in synergyClasses){
            var instance = Activator.CreateInstance(type) as SynergyBase;
            this.synergyBase.Add(instance);
        }

        synergyCount = new Dictionary<Synergy, int>();//Synergy 카운트
        Array synergyEnumList = Enum.GetValues(typeof(Synergy));
        foreach (Synergy synergyEnum in synergyEnumList) {
            synergyCount.Add(synergyEnum, 0);
        }

        SynergyUIGameObject = new List<GameObject>();
        synergyUI = new List<SynergyUI>();
        for(int i = 0; i < synergyBase.Count; i++) {
            SynergyUIGameObject.Add(this.transform.GetChild(i).gameObject);
            synergyUI.Add(SynergyUIGameObject[i].GetComponent<SynergyUI>());
            synergyUI[i].Init(synergyBase[i]);
        }
    }

    void SynergyCounter() {
        for(int i = 0; i < synergyBase.Count; i++) {
            Synergy currentSynergy = synergyBase[i].synergyName;
            synergyBase[i].synergyCount = synergyCount[currentSynergy];
            synergyBase[i].SynergyStackCheck();
        }
    }

    void SynergyUIActive(int order, int stack) {
        if(stack >= 0) {
            synergyUI[order].ChangeActive(true);
            synergyUI[order].ChangeMaterial(stack); 
        }
        else {
            synergyUI[order].ChangeActive(false);
            synergyUI[order].ChangeMaterial(0);
        }
    }

    void SynergySort() {  
        List<int> OrderList = new List<int>();
        for(int i = 0 ; i < synergyBase.Count; i++) {
            OrderList.Add(synergyBase[i].synergyOrder);
        }
        OrderList = OrderList.OrderByDescending(x => x).ToList();

        for(int i = 0; i < synergyUI.Count; i++) {
            if(synergyBase[i].synergyOrder == -1) {
                int maxOrder = synergyUI.Count;
                synergyUI[i].ChangeOrder(maxOrder);
                continue;
            }
            int nindex = OrderList.FindIndex(x => x == synergyBase[i].synergyOrder);
            synergyUI[i].ChangeOrder(nindex);
            OrderList[nindex] = 0;
        }
    }

    void SynergySort(string a) {  
        synergyBase = synergyBase.OrderByDescending(x => x.synergyOrder).
        ThenByDescending(x => x.synergyCount).ToList();

        for(int i = 0; i < synergyBase.Count; i++) {
            int nindex = synergyUI.FindIndex(x => x.synergyName == synergyBase[i].synergyName.ToString());
            int order = synergyBase[i].synergyOrder;
            synergyUI[nindex].ChangeOrder(i);
            SynergyUIActive(nindex, order);
        }
    }

}
