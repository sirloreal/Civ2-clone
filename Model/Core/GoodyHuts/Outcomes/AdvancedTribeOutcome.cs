using Civ2engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core.GoodyHuts.Outcomes
{
    internal class AdvancedTribeOutcome : GoodyHutOutcome
    {
        public string Name => "Advanced Tribe";
        public string Description => "You have discovered an advanced tribe.";

        public override void ApplyOutcome(Unit unit)
        {
            throw new NotImplementedException();
        }
    }
}
