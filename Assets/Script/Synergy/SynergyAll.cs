using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SynergyAll{

    public int[] synergyStack{get; set;}
    public string synergyTag{get; set;}
    public int synergyCount{get; set;}
    public int synergyOrder{get; set;}
    public virtual void Init() {}
    public void SetCount(int synergyCount) {this.synergyCount = synergyCount;}
    public void AddCount() {this.synergyCount++;}
    public void ResetCount(){this.synergyCount = 0; }

    public int SynergyInvoke() {
        bool isSynergy = false;
        int synergyStackCount = synergyStack.Length - 1;
        while(synergyStackCount > 0) {
            if(synergyStack[synergyStackCount] > synergyCount) {
                synergyStackCount--;
            }
            else {
                isSynergy = true;
                break;
            }
        }
        if(!isSynergy) {
            synergyOrder = 0;
            return -1;
        }
        else {
            synergyOrder = synergyStackCount;
            return synergyStackCount;
        }
    }

}

//1
public class AbydosSynergy : SynergyAll {
    public AbydosSynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Abydos"; 
        synergyCount = 0;       
    }
}
//2
public class MillenniumSynergy : SynergyAll {
    public MillenniumSynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Millennium";     
        synergyCount = 0;     
    }
}
//3
public class GehennaSynergy : SynergyAll {
    public GehennaSynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Gehenna";     
        synergyCount = 0;      
    }
}
//4
public class TrinitySynergy : SynergyAll {
    public TrinitySynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Trinity";     
        synergyCount = 0;      
    }
}
//5
public class SniperSynergy : SynergyAll {
    public SniperSynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Sniper";     
        synergyCount = 0;      
    }
}
//6
public class SlayerSynergy : SynergyAll {
    public SlayerSynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Slayer";     
        synergyCount = 0;      
    }
}
//7
public class MysterySynergy : SynergyAll {
    public MysterySynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Mystery";     
        synergyCount = 0;      
    }
}
//8
public class GuardianSynergy : SynergyAll {
    public GuardianSynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Guardian";     
        synergyCount = 0;      
    }
}
//9
public class EngineerSynergy : SynergyAll {
    public EngineerSynergy() { 
        Init();
    }
    public override void Init() {
        synergyStack = new int[4]{0, 1, 2, 3};
        synergyTag = "Engineer";     
        synergyCount = 0;      
    }
}