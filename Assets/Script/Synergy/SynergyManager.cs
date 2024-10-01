using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.Events;

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
        SynergyUIInvoke();
        SynergySort();
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
        }
    }

    void SynergyUIInvoke() {
        for(int i = 0; i < synergyBase.Count; i++) {
            int synergyCount = synergyBase[i].SynergyStackCheck();
            if(synergyCount >= 0) {
                synergyUI[i].ChangeActive(true);
                synergyUI[i].ChangeMaterial(synergyCount); 
            }
            else {
                synergyUI[i].ChangeActive(false);
                synergyUI[i].ChangeMaterial(0);
            }
        }
    }
    
    void SynergySort() {  
        List<int> OrderList = new List<int>();
        for(int i = 0 ; i < synergyBase.Count; i++) {
            int orderIndex = synergyBase[i].synergyOrder;
            OrderList.Add(orderIndex);
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
}
