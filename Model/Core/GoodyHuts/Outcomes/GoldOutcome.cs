using Civ2engine.Units;

namespace Model.Core.GoodyHuts.Outcomes
{
    public class GoldOutcome : GoodyHutOutcome
    {
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
