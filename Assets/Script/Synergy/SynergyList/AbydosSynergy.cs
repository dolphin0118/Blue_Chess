using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AbydosSynergy : SynergyBase
{
    public AbydosSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 2, 3, 4 };
        synergyName = Synergy.Abydos;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void StatSetup()
    {

    }

    public override void SynergyActive(List<UnitStatus> unitStatuses)
    {
        base.SynergyActive(unitStatuses);
        foreach (UnitStatus unitStatus in unitStatuses)
        {
            if (unitStatus.schoolSynergy == Synergy.Abydos)
            {
                SynergyApply(unitStatus);
            }
        }
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {
        switch (synergyOrder)
        {
            case 0:
                unitStatus.synergyStat.HP += 50;
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

}
