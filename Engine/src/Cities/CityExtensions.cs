using System;
using System.Collections.Generic;
using System.Linq;
using Civ2engine.Enums;
using Civ2engine.MapObjects;
using Civ2engine.Units;
using Model.Constants;
using Model.Core;

namespace Civ2engine
{
    public static class CityExtensions
    {
        public static void CalculateOutput(this City city, int government, IGame game)
        {
            var orgLevel = city.GetOrganizationLevel(game.Rules);

            var lowOrganisation = orgLevel < 1;
            // var hasSuperMarket = city.ImprovementExists(ImprovementType.Supermarket);
            // var hasSuperhighways = city.ImprovementExists(ImprovementType.Superhighways);

            var totalFood = 0;
            var totalSheilds = 0;
            var totalTrade = 0;

            city.WorkedTiles.ForEach(t =>
            {
                totalFood += t.GetFood(lowOrganisation);
                totalSheilds += t.GetShields(lowOrganisation);
                totalTrade += t.GetTrade(orgLevel);
            });


            city.Support = city.SupportedUnits.Count(u => u.NeedsSupport);


            var distance = ComputeDistanceFactor(city, government, game);
            if (distance == 0)
            {
                city.Waste = 0;
                city.Corruption = 0;
            }
            else
            {
                distance *= game.CurrentMap.ScaleFactor;
                var gov = (int)(city.WeLoveKingDay ? government + 1 : government);

                var corruptionTenTenFactor = 15d / (4 + gov);
                var wasteTenTenFactor = 15d / (4 + gov * 4);

                // https://apolyton.net/forum/miscellaneous/archives/civ2-strategy-archive/62524-corruption-and-waste
                var corruption = totalTrade * Math.Min(32, distance) * corruptionTenTenFactor / 100;

                var waste = (totalSheilds - city.Support) * Math.Min(16, distance) * wasteTenTenFactor / 100;

                var corruptionReduction =
                    city.Improvements.Sum(i => i.Effects.GetValueOrDefault(Effects.ReduceCorruption));
                if (corruptionReduction > 0)
                {
                    var modifier = corruptionReduction / 100d;
                    waste *= modifier;
                    corruption *= modifier;
                }

                //TODO: Trade route to capital


                city.Waste = (int)Math.Floor(waste);
                city.Corruption = (int)Math.Floor(corruption);
            }

            city.TotalProduction = totalSheilds;
            city.Trade = totalTrade - city.Corruption;
            city.Production = totalSheilds - city.Support - city.Waste;
            city.FoodConsumption = city.Size * game.Rules.Cosmic.FoodEatenPerTurn +
                                   city.SupportedUnits.Count(u => u.AIrole == AIroleType.Settle) *
                                   game.Rules.Governments[government].SettlersConsumption;
            city.FoodProduction = totalFood;
            city.SurplusHunger = totalFood - city.FoodConsumption;



            city.Pollution = CalculatePollution(city);
        }

        private static int CalculatePollution(City city)
        {
            var smokestackPoints = 0;

            if (!city.Improvements.Any(i => i.Effects.ContainsKey(Effects.EliminateIndustrialPollution)))
            {
                var modifier = 1 + city.Improvements
                                   .Where(i => i.Effects.ContainsKey(Effects.IndustrialPollutionModifier))
                                   .Sum(i => i.Effects[Effects.IndustrialPollutionModifier]) +
                               city.Owner.GlobalEffects.GetValueOrDefault(
                                   Effects.IndustrialPollutionModifier, 0);
                if (modifier > 0)
                {
                    smokestackPoints = Math.Max((city.Production / modifier) - 20, 0);
                }
            }

            if (!city.Improvements.Any(i => i.Effects.ContainsKey(Effects.EliminatePopulationPollution)))
            {
                var sum = city.Improvements
                    .Where(i => i.Effects.ContainsKey(Effects.PopulationPollutionModifier))
                    .Sum(i => i.Effects[Effects.PopulationPollutionModifier]);
                var modifier = sum +
                               city.Owner.GlobalEffects.GetValueOrDefault(
                                   Effects.PopulationPollutionModifier, 0);
                if (modifier > 0)
                {
                    smokestackPoints += city.Size * modifier / 4;
                }
            }

            return smokestackPoints;
            // [Industrial Pollution + Pop. Pollution]
            // Industrial Pollution = (Shield (Civ2).png Production generated by city / Modifier) - 20
            // Modifier = 2 if city has Hydro Plant or Nuclear Plant;
            // Modifier = 3 if city has a Recycling Center;
            // Industrial Pollution = 0; if city has a Solar Plant;
            // Population Pollution = (City Size x Pollution Modifier.) / 4
            // Pollution Modifiers = 0.0 by default
            //     +1 with Industrialization;
            // +1 with Automobile;
            // +1 with Mass Production;
            // +1 with Plastics;
            // +1 with Sanitation NOT discovered after Industrialization;
            // -1 with Environmentalism
            //     -1 with Solar Plant in the city.
            //     Population Pollution = 0; if city has Mass Transit
        }

        public static void SetUnitSupport(this City city, Government government)
        {
            var freeSupport = city.FreeSupport(government);
            var supportFreeTypes = government.UnitTypesAlwaysFree;
            city.SupportedUnits.ForEach(unit =>
            {
                unit.NeedsSupport = !unit.FreeSupport(supportFreeTypes) && freeSupport > 0;
                freeSupport--;
            });
        }

        private static int FreeSupport(this City city, Government government)
        {
            var support = government.NumberOfFreeUnitsPerCity;
            return support == -1 ? city.Size : support;
        }

        private static double ComputeDistanceFactor(City city, int governmentIndex, IGame game)
        {
            if (city.ImprovementExists(Effects.Capital)) return 0; //Capital is always at 0 distance  

            var government = game.Rules.Governments[governmentIndex];
            double distance = government.Distance;
            if (distance >= 0)
                return distance; // if distance is fixed for govt (Communism, Fundamentalism or Democracy)


            distance =
                city.Owner.Cities.Where(c => c.ImprovementExists(Effects.Capital))
                    .Select(c => Utilities.DistanceTo(c, city.Location)).OrderBy(v => v)
                    .FirstOrDefault(game.MaxDistance);

            if (government.Level < 1)
            {
                distance += (int)game.DifficultyLevel;
            }


            return distance;
        }

        public static bool SellImprovement(this City city, Improvement improvement)
        {
            if (city.ImprovementSold)
            {
                return false;
            }
            //Since effects are computed by checking improvements removing improvement removes all effects

            city.OrderedImprovements.Remove(improvement.Type);
            city.ImprovementSold = true;
            return true;
        }

        public static void AddImprovement(this City city, Improvement improvement) =>
            city.OrderedImprovements.Add(improvement.Type, improvement);

        public static bool ImprovementExists(this City city, int improvement) =>
            city.OrderedImprovements.ContainsKey(improvement);

        public static bool ImprovementExists(this City city, Effects improvement) =>
            city.OrderedImprovements.Values.Any(i => i.Effects.ContainsKey(improvement));

        public static void ShrinkCity(this City city, Game game)
        {
            city.Size -= 1;
            city.AutoRemoveWorkersDistribution(game.Rules);
            city.CalculateOutput(city.Owner.Government, game);

            game.TriggerMapEvent(MapEventType.UpdateMap, new List<Tile> { city.Location });
        }

        public static void GrowCity(this City city, IGame game)
        {
            city.Size += 1;

            city.AutoAddDistributionWorkers(game.Rules); // Automatically add a workers on a tile
            city.CalculateOutput(city.Owner.Government, game);

            game.TriggerMapEvent(MapEventType.UpdateMap, new List<Tile> { city.Location });
        }

        public static void ResetFoodStorage(this City city, int foodRows)
        {
            city.FoodInStorage = 0;


            var totalStorage = city.GetFoodStorage();

            if (totalStorage == 0) return;

            var maxFood = (city.Size + 1) * foodRows;
            city.FoodInStorage += maxFood * totalStorage / 100;
        }

        public static int GetFoodStorage(this City city)
        {
            var storageBuildings = city.Improvements
                .Where(i => i.Effects.ContainsKey(Effects.FoodStorage))
                .Select(b => b.Effects[Effects.FoodStorage]).ToList();

            if (storageBuildings.Count <= 0) return 0;

            var totalStorage = storageBuildings.Sum();
            if (totalStorage is > 100 or < 0)
            {
                totalStorage = storageBuildings.Where(v => v is >= 0 and <= 100).Max();
            }

            return totalStorage;
        }

        public static void AutoRemoveWorkersDistribution(this City city, Rules gameRules)
        {
            //TODO: remove scuentists & taxmen first
            var tiles = city.WorkedTiles.Where(t => t != city.Location);

            var organization = city.GetOrganizationLevel(gameRules);

            var unworked = tiles.OrderBy(t =>
                t.GetFood(organization == 0) + t.GetShields(organization == 0) +
                t.GetTrade(organization)).First();

            city.WorkedTiles.Remove(unworked);
        }

        public static void AutoAddDistributionWorkers(this City city, Rules gameRules)
        {
            // First determine how many workers are to be added
            int workersToBeAdded = city.Size + 1 - city.WorkedTiles.Count;

            var organization = city.GetOrganizationLevel(gameRules);
            
            var lowOrganization = organization == 0;

            // Make a list of tiles where you can add workers
            var tilesToAddWorkersTo = new List<Tile>();

            var tileValue = new List<double>();
            foreach (var tile in city.Location.CityRadius().Where(t =>
                         t.WorkedBy == null && t.IsVisible(city.OwnerId) &&
                         !t.UnitsHere.Any<Unit>(u => u.Owner != city.Owner && u.AttackBase > 0) && t.CityHere == null))
            {
                var food = tile.GetFood(lowOrganization) * 1.5;
                var shields = tile.GetShields(lowOrganization);
                var trade = tile.GetTrade(organization) * 0.5;

                var total = food + shields + trade;
                var insertionIndex = tilesToAddWorkersTo.Count;
                for (; insertionIndex > 0; insertionIndex--)
                {
                    if (tileValue[insertionIndex - 1] >= total)
                    {
                        break;
                    }
                }

                if (insertionIndex == tilesToAddWorkersTo.Count)
                {
                    if (insertionIndex >= workersToBeAdded) continue;

                    tilesToAddWorkersTo.Add(tile);
                    tileValue.Add(total);
                }
                else
                {
                    tilesToAddWorkersTo.Insert(insertionIndex, tile);
                    tileValue.Insert(insertionIndex, total);
                }
            }

            foreach (var tile in tilesToAddWorkersTo.Take(workersToBeAdded))
            {
                tile.WorkedBy = city;
            }
        }

        public static int GetPopulation(this City city)
        {
            var population = 0;
            for (int i = 1; i <= city.Size; i++)
                population += i * 10000;
            return population;
        }

        public static int GetOrganizationLevel(this City city, Rules rules)
        {
            var baseLevel = rules.Governments[city.Owner.Government].Level;
            return city.WeLoveKingDay ? baseLevel + 1 : baseLevel;
        }

        public static PeopleType[] GetPeopleTypes(this City city)
        {
            var size = city.Size;
            var people = new PeopleType[size];
            // Unhappy
            int additUnhappy = size - 6; // Without units & improvements present, 6 people are content
            additUnhappy -=
                Math.Min(city.Location.UnitsHere.Count, 3); // Each new unit in city -> 1 less unhappy (up to 3 max)

            var contentImprovements = city.Improvements.Where(i => i.Effects.ContainsKey(Effects.ContentFace));

            additUnhappy -= contentImprovements.Sum(i => i.Effects.GetValueOrDefault(Effects.ContentFace));
            // TODO: Adjustments to colosseum & cathedral for Tech
            // if (city.Improvements.Any(impr => impr.Type == ImprovementType.Temple)) additUnhappy -= 2;
            // if (city.Improvements.Any(impr => impr.Type == ImprovementType.Colosseum)) additUnhappy -= 3;
            // if (city.Improvements.Any(impr => impr.Type == ImprovementType.Cathedral)) additUnhappy -= 3;
            
            // Aristocrats
            int additArist = 0;
            switch (size + 1 - city.WorkedTiles.Count) // Populating aristocrats based on workers removed
            {
                case 1:
                    additArist += 1;
                    break;
                case 2:
                case 3:
                    additArist += 3;
                    break;
                case 4:
                case 5:
                case 6:
                    additArist += 4;
                    break;
                case 7:
                    additArist += 5;
                    break;
                case 8:
                case 9:
                    additArist += 6;
                    break;
                case 10:
                    additArist += 7;
                    break;
                case 11:
                    additArist += 8;
                    break;
                default: break;
            }

            // Elvis
            int additElvis = size + 1 - city.WorkedTiles.Count; // No of elvis = no of workers removed
            // Populate
            for (int i = 0; i < size; i++) people[i] = PeopleType.Worker;
            for (int i = 0; i < additUnhappy; i++) people[size - 1 - i] = PeopleType.Unhappy;
            for (int i = 0; i < additArist; i++) people[i] = PeopleType.Aristocrat;
            for (int i = 0; i < additElvis; i++) people[size - 1 - i] = PeopleType.Elvis;
            return people;
        }

        public static bool IsNextToOcean(this City city) =>
            city.Location.Neighbours().Any(t => t.Type == TerrainType.Ocean);

    }
}