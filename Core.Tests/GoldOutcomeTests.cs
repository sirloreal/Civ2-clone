using Civ2engine;
using Civ2engine.Units;
using Model.Core.GoodyHuts.Outcomes;

namespace Core.Tests
{
    public class GoldOutcomeTests
    {
        [Fact]
        public void ApplyOutcome_IncreasesOwnerMoneyBySpecifiedAmount()
        {
            // Arrange
            var initialMoney = 100;
            var goldAmount = 50;
            var owner = new Civilization { Money = initialMoney };
            var unit = new Unit { Owner = owner };
            var goldOutcome = new GoldOutcome(goldAmount);

            // Act
            goldOutcome.ApplyOutcome(unit);

            // Assert
            Assert.Equal(initialMoney + goldAmount, owner.Money);
        }
    }
}