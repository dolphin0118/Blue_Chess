using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MysterySynergy : SynergyBase
{
    int[] synergyTrueDamageValue = new int[2] { 30, 50 };
    public MysterySynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[2] { 1, 2 };
        synergyName = Synergy.Mystery;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {
        if (SynergyCheck(unitStatus.traitSynergy))
        {
            unitStatus.trueDamage += synergyTrueDamageValue[synergyOrder];
        }

    }
}
