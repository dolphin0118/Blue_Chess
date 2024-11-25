using System.Collections;
using System.Collections.Generic;
using BlueChessDataBase;
using UnityEngine;
using UnityEngine.PlayerLoop;



public class SynergyBase
{
    public Synergy synergyName { get; set; }
    public int[] synergyStack { get; set; }
    public int synergyCount { get; set; }
    public int synergyOrder { get; set; }
    protected UnitStat synergyStat;

    public virtual void Init() { }
    public virtual void StatSetup() { }
    public virtual void SynergyActive(UnitStatus unitStatus)
    {
        unitStatus.synergyStat = synergyStat;
    }

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

}

//School
public class AbydosSynergy : SynergyBase
{
    public AbydosSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Abydos;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void StatSetup()
    {
        synergyStat.HP = 100;

    }
}

public class MillenniumSynergy : SynergyBase
{
    public MillenniumSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 2, 3, 4 };
        synergyName = Synergy.Millennium;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void StatSetup()
    {
        synergyStat.HP = 100;

    }

}

public class GehennaSynergy : SynergyBase
{
    public GehennaSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Gehenna;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void StatSetup()
    {
        synergyStat.HP = 100;

    }

}

public class TrinitySynergy : SynergyBase
{
    public TrinitySynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Trinity;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void StatSetup()
    {
        synergyStat.HP = 100;

    }

}

public class SRTSynergy : SynergyBase
{
    public SRTSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.SRT;
        synergyCount = 0;
        synergyOrder = -1;
    }
}


//Trait
public class SniperSynergy : SynergyBase
{
    public SniperSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Sniper;
        synergyCount = 0;
        synergyOrder = -1;
    }
}

public class SlayerSynergy : SynergyBase
{
    public SlayerSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Slayer;
        synergyCount = 0;
        synergyOrder = -1;
    }
}

public class MysterySynergy : SynergyBase
{
    public MysterySynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Mystery;
        synergyCount = 0;
        synergyOrder = -1;
    }
}

public class GuardianSynergy : SynergyBase
{
    public GuardianSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Guardian;
        synergyCount = 0;
        synergyOrder = -1;
    }
}

public class EngineerSynergy : SynergyBase
{
    public EngineerSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Engineer;
        synergyCount = 0;
        synergyOrder = -1;
    }
}

public class VanguardSynergy : SynergyBase
{
    public VanguardSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Vanguard;
        synergyCount = 0;
        synergyOrder = -1;
    }
}

public class CommanderSynergy : SynergyBase
{
    public CommanderSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Commander;
        synergyCount = 0;
        synergyOrder = -1;
    }
}