using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public override void SynergyApply(UnitStatus unitStatus)
    {

    }
}

