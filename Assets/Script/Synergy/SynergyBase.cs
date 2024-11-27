using System.Collections;
using System.Collections.Generic;
using BlueChessDataBase;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Linq;


public abstract class SynergyBase
{
    public Synergy synergyName { get; set; }
    public int[] synergyStack { get; set; }
    public int synergyCount { get; set; }
    public int synergyOrder { get; set; }

    protected List<UnitStatus> unitStatuses;

    public abstract void Init();

    public virtual void StatSetup() { }

    public virtual void SynergyActive(List<UnitStatus> unitStatuses)
    {
        this.unitStatuses = unitStatuses;
        foreach (UnitStatus unitStatus in unitStatuses)
        {
            SynergyApply(unitStatus);
        }

    }

    public bool SynergyCheck(Synergy checkSynergy)
    {
        if (checkSynergy == this.synergyName) return true;
        return false;
    }

    public abstract void SynergyApply(UnitStatus unitStatus);

    public int SynergyStackCheck()
    {
        int synergyStackMax = synergyStack.Length - 1;
        int synergyStackCount = 0;
        if (synergyCount == 0)
        {
            synergyOrder = -1;
            return -1;
        }

        while (synergyStackCount <= synergyStackMax)
        {
            if (synergyStack[synergyStackCount] <= synergyCount)
            {
                synergyStackCount++;
                continue;
            }
            break;
        }
        synergyOrder = synergyStackCount;
        return synergyStackCount;
    }

    public int SynergyStackCheck(int a)
    {
        int synergyStackMax = synergyStack.Length;
        int synergyStackCount = -1;
        for (int i = 0; i < synergyStackMax; i++)
        {
            if (synergyCount == 0) break;
            if (synergyStack[i] <= synergyCount) synergyStackCount++;
        }
        synergyOrder = synergyStackCount;
        return synergyStackCount;
    }
}
