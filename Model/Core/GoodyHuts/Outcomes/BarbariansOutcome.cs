using Civ2engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core.GoodyHuts.Outcomes
{
    internal class BarbariansOutcome : GoodyHutOutcome
    {
        public string Name => "Barbarians";
        public string Description => "You have unleashed a horde of barbarians!";
        public override void ApplyOutcome(Unit unit)
        {
            throw new NotImplementedException();
        }
    }
}
