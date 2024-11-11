using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

public class AttackItem : ItemInfo
{
    protected AttackType ConvertType;
    virtual public void SetConvertType() {; }
    virtual public void UseItem(UnitStatus unitStatus) {; }
}

public class ConvertExplosionItem : AttackItem
{
    override public void SetConvertType()
    {
        ConvertType = AttackType.Explosion;
    }
}
