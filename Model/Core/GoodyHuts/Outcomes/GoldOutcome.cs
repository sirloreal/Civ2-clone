using Civ2engine.Units;
using System.Diagnostics.Metrics;

namespace Model.Core.GoodyHuts.Outcomes
{
    public class GoldOutcome : GoodyHutOutcome
    {
        public string Description => $"You have discovered valuable metal deposits worth {_amount} gold.";
        private readonly int _amount;
        public GoldOutcome(int amount)
        {
            _amount = amount;
        }
        public override void ApplyOutcome(Unit unit)
        {
            unit.Owner.Money += _amount;

            // TODO: For debugging - remove later
            Console.WriteLine($"Player {unit.Owner.Id} found {_amount} gold!");
        }
    }
}
