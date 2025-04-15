using Civ2engine.Units;

namespace Model.Core.GoodyHuts.Outcomes
{
    internal class GoldOutcome : GoodyHutOutcome
    {
        private readonly int _amount;
        internal GoldOutcome(int amount)
        {
            _amount = amount;
        }
        internal override void ApplyOutcome(Unit unit)
        {
            unit.Owner.Money += _amount;

            // TODO: For debugging - remove later
            Console.WriteLine($"Player {unit.Owner.Id} found {_amount} gold!");
        }
    }
}
