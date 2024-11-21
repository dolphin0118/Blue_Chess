using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueChessDataBase
{
    [Serializable]
    public class UnitData
    {
        public string Name = "";
        public Synergy schoolSynergy;
        public Synergy traitSynergy;
        public int UnitPrice;
    }

}