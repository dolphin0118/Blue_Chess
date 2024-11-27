using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class MillenniumSynergy : SynergyBase
{
    int[] synergyARValue = new int[3] { 30, 50, 100 };
    int[] synergyMRValue = new int[3] { 30, 50, 100 };

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

    public override void SynergyApply(UnitStatus unitStatus)
    {
        if (SynergyCheck(unitStatus.schoolSynergy))
        {
            unitStatus.synergyStat.AR += synergyARValue[synergyOrder];
            unitStatus.synergyStat.MR += synergyMRValue[synergyOrder];
        }
    }
}

