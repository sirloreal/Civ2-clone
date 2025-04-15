using Civ2engine.Units;
using Model.Core.GoodyHuts.Outcomes;

namespace Model.Core.GoodyHuts
{
    public class GoodyHut
    {
        private List<GoodyHutOutcome> _outcomes = new();

        public GoodyHut() 
        {
            _outcomes.Add(new GoldOutcome(50));
            // Mercenaries
            // Technology (scrolls)
            // Advanced Tribe (new city)
            // Barbarians
        }

        public void Trigger(Unit unit)
        {
            var outcome = _outcomes[0]; // TODO: Randomly select an outcome
            outcome.ApplyOutcome(unit);
        }
    }
}
