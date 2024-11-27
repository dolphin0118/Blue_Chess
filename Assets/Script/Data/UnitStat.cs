using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueChessDataBase
{
    [Serializable]
    public class UnitStat
    {
        public int Level = 1;
        public string Name;
        public float HP;
        public float MP;
        public float ATK;
        public float AP;
        public float AR;
        public float MR;
        public float ATKSpeed;
        public float Range;
        public AttackType attackType;

        public void Reset()
        {

        }
    }
}