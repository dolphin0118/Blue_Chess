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
        synergyStack = new int[2] { 1, 2 };
        synergyName = Synergy.Commander;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void SynergyActive(List<UnitStatus> unitStatuses)
    {
        base.SynergyActive(unitStatuses);
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {
        unitStatus.synergyStat.HP += 10 * synergyStack[synergyOrder];
        unitStatus.synergyStat.AR += 10 * synergyStack[synergyOrder];
        unitStatus.synergyStat.MR += 10 * synergyStack[synergyOrder];
        unitStatus.synergyStat.ATK += 10 * synergyStack[synergyOrder];
        unitStatus.synergyStat.ATKSpeed += 0.01f * synergyStack[synergyOrder];
    }
}