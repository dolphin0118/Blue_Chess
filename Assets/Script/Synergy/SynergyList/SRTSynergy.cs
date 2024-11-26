using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class SRTSynergy : SynergyBase
{

    public SRTSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 2, 3, 4 };
        synergyName = Synergy.SRT;
        synergyCount = 0;
        synergyOrder = -1;
    }
    public override void SynergyActive(List<UnitStatus> unitStatuses)
    {
        base.SynergyActive(unitStatuses);
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {

    }
}

