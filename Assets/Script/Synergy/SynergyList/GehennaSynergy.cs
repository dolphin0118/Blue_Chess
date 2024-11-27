using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GehennaSynergy : SynergyBase
{
    public GehennaSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[3] { 1, 2, 3 };
        synergyName = Synergy.Gehenna;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void StatSetup()
    {

    }
    public override void SynergyApply(UnitStatus unitStatus)
    {
        if (SynergyCheck(Synergy.Gehenna))
        {
            switch (synergyOrder)
            {
                case 0:
                    unitStatus.synergyStat.ATK += 50;
                    unitStatus.synergyStat.HP += 100;
                    break;
                case 1:
                    unitStatus.synergyStat.ATK -= 100;
                    unitStatus.synergyStat.HP -= 100;
                    break;
                case 2:
                    unitStatus.synergyStat.ATK -= 500;
                    unitStatus.synergyStat.HP -= 500;
                    break;

            }
        }
    }

}

