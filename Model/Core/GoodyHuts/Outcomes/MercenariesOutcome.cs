using Civ2engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core.GoodyHuts.Outcomes
{
    internal class MercenariesOutcome : GoodyHutOutcome
    {
        public string Description => "You have discovered a friendly tribe of skilled mercenaries.";
        public override void ApplyOutcome(Unit unit)
        {
            throw new NotImplementedException();
        }
    }
}
