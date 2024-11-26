using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public override void SynergyActive(List<UnitStatus> unitStatuses)
    {
        base.SynergyActive(unitStatuses);

    }

    public override void SynergyApply(UnitStatus unitStatus)
    {

    }
}

