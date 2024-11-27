using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SniperSynergy : SynergyBase
{
    int[] synergyRangeValue = { 1, 2, 3 };

    public SniperSynergy()
    {
        Init();
    }

    public override void Init()
    {
        synergyStack = new int[3] { 2, 3, 4 };
        synergyName = Synergy.Sniper;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {
        if (SynergyCheck(unitStatus.traitSynergy))
        {
            unitStatus.synergyStat.Range += synergyRangeValue[synergyOrder];
        }

    }
}