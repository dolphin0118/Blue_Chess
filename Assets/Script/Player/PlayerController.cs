using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public TeamManager TeamManager;
    public UnitCombine unitCombine;
    private void Awake()
    {
        TeamManager = GetComponentInChildren<TeamManager>();
        unitCombine = GetComponentInChildren<UnitCombine>();
    }

}
