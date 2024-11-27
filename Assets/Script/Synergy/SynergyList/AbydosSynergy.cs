using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AbydosSynergy : SynergyBase
{
    int[] synergyATKValue = new int[3] { 50, 100, 200 };

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
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {
        if (SynergyCheck(unitStatus.schoolSynergy))
        {
            unitStatus.synergyStat.ATK += synergyATKValue[synergyOrder];
        }
    }

}
