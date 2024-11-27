using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class VanguardSynergy : SynergyBase
{
    int[] synergyHPValue = { 500, 1000 };
    int[] synergyMRValue = { 50, 100 };
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
        if (unitStatus.traitSynergy == Synergy.Vanguard)
        {
            unitStatus.synergyStat.HP += synergyHPValue[synergyOrder] * 2;
            unitStatus.synergyStat.MR += synergyMRValue[synergyOrder] * 2;
        }
        else
        {
            unitStatus.synergyStat.HP += synergyHPValue[synergyOrder];
            unitStatus.synergyStat.MR += synergyMRValue[synergyOrder];
        }
    }
}

