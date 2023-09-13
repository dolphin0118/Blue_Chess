using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class SynergyManager : MonoBehaviour {

    public static SynergyManager instance = null;
    public List<Synergy> SynergyList = Enum.GetValues(typeof(Synergy)).Cast<Synergy>().ToList();
    public GameObject[] SynergyObjects;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else{
            if (instance != this) Destroy(this.gameObject);
        }
        //SynergyObjects = GetComponentsInChildren<GameObject>();
        for(int i = 0; i < SynergyObjects.Length; i++) SynergyObjects[i].gameObject.SetActive(false);
    }
    void Update() {
        SynergyGeneral();
        SynergySorting();
    }
    
    void SynergySorting() {
        
    }

    void SynergyGeneral() {
        AbydosSynergy();
        MillenniumSynergy();
    }

    void AbydosSynergy() {
        int[] synergyStack = {0, 1, 2, 3};
        int synergyCheck = synergyStack.Length - 1;
        bool isSynergy = false;
        Synergy abydosSynergy = Synergy.Abydos;
        int synergyCnt = 0;

        for(int i = 0; i < SynergyList.Count; i++) {
            if(SynergyList[i] == abydosSynergy) {
                synergyCnt++;
            }
        }
        while(synergyCheck > 0) {
            if(synergyStack[synergyCheck] > synergyCnt) {
                synergyCheck--;
                
            }
            else {
                isSynergy = true;
                break;
            }
        }

        if(!isSynergy) return;
        else if(isSynergy) {
            GameObject abydosCell = GameObject.FindGameObjectWithTag("Abydos");
            abydosCell.SetActive(true);
            abydosCell.GetComponent<SynergyUi>().ChangeMaterial(synergyCnt);
            return;
        }
    }
    void MillenniumSynergy() {
        int[] synergyStack = { 0, 1, 2, 3 };
        int synergyCheck = synergyStack.Length - 1;
        bool isSynergy = false;
        Synergy millenniumSynergy = Synergy.millennium;
        int synergyCnt = 0;

        for (int i = 0; i < SynergyList.Count; i++)
        {
            if (SynergyList[i] == millenniumSynergy)
            {
                synergyCnt++;
            }
        }
        while (synergyCheck > 0)
        {
            if (synergyStack[synergyCheck] > synergyCnt)
            {
                synergyCheck--;

            }
            else
            {
                isSynergy = true;
                break;
            }
        }

        if (!isSynergy) return;
        else if (isSynergy)
        {
            GameObject abydosCell = GameObject.FindGameObjectWithTag("Millennium");
            abydosCell.SetActive(true);
            abydosCell.GetComponent<SynergyUi>().ChangeMaterial(synergyCnt);
            return;
        }
    }
}
