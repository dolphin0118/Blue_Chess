using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class SRTSynergy : SynergyBase
{
    private int BattleCount = 0;
    private int[] synergyATKValue = { 10, 30 };
    private float[] synergyATKSpeedValue = { 0.05f, 0.15f };
    public SRTSynergy()
    {
        Init();
    }
    public override void Init()
    {
        synergyStack = new int[2] { 2, 3 };
        synergyName = Synergy.SRT;
        synergyCount = 0;
        synergyOrder = -1;
    }

    public override void SynergyActive(List<UnitStatus> unitStatuses)
    {
        BattleCount++;
        base.SynergyActive(unitStatuses);
    }

    public override void SynergyApply(UnitStatus unitStatus)
    {
        if (SynergyCheck(unitStatus.schoolSynergy))
        {
            unitStatus.synergyStat.ATK += synergyATKValue[synergyOrder] * BattleCount;
            unitStatus.synergyStat.ATKSpeed += synergyATKSpeedValue[synergyOrder] * BattleCount;
        }
    }


}

