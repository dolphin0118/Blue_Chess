using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TrinitySynergy : SynergyBase
{
    public TrinitySynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Trinity;
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
            SynergyApply(unitStatus);
        }
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {
        switch (unitStatus.unitName)
        {
            case "Mika":
                unitStatus.synergyStat.ATK = 50;
                break;
            case "Koharu":
                unitStatus.synergyStat.AR = 50;
                unitStatus.synergyStat.MR = 50;
                break;
            case "Kazusa":
                unitStatus.synergyStat.ATKSpeed = 50;
                break;
        }

    }
}
