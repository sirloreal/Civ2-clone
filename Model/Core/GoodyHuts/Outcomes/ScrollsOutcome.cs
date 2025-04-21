using Civ2engine.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core.GoodyHuts.Outcomes
{
    internal class ScrollsOutcome : GoodyHutOutcome
    {
        public string Name => "Scrolls";
        public string Description => "You have discovered scrolls of ancient wisdom.";
        public override void ApplyOutcome(Unit unit)
        {
            throw new NotImplementedException();
        }
    }
}
