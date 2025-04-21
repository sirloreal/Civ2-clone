using Civ2engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core.GoodyHuts.Outcomes
{
    internal class AbandonedVillageOutcome : GoodyHutOutcome
    {
        public string Name => "Abandoned Village";
        public string Description => "Weeds grow in empty ruins.  This village has long\r\nbeen abandoned.";
        public override void ApplyOutcome(Unit unit)
        {
            throw new NotImplementedException();
        }
    }
}
