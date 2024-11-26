using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public override void SynergyActive(List<UnitStatus> unitStatuses)
    {
        base.SynergyActive(unitStatuses);
        foreach (UnitStatus unitStatus in unitStatuses)
        {
            unitStatus.synergyStat.HP += 50;
        }
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {

    }
}