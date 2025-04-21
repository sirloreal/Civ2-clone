using Civ2engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core.GoodyHuts.Outcomes
{
    internal class NomadsOutcome : GoodyHutOutcome
    {
        public string Name => "Nomads";
        public string Description => "You discover a band of wandering nomads.\r\nThey agree to join your tribe.";
        public override void ApplyOutcome(Unit unit)
        {
            throw new NotImplementedException();
        }
    }
}
