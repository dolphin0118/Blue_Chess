using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

public class ItemBullet : ItemBase
{
    public AttackType attackType { get; set; }

    public override void UseItem(UnitStatus unitStatus)
    {
        base.UseItem(unitStatus);
        unitStatus.attackType = attackType;
    }
}