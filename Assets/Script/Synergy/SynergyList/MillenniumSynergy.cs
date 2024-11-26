using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MillenniumSynergy : SynergyBase
{
    public MillenniumSynergy()
    {
        Init();
    }

    public override void Init()
    {
        synergyStack = new int[3] { 2, 3, 4 };
        synergyName = Synergy.Millennium;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void StatSetup()
    {

    }

    public override void SynergyApply(UnitStatus unitStatus)
    {

    }
}

