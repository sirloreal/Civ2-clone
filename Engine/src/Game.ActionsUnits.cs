﻿using System;
using System.Collections.Generic;
using System.Linq;
using Civ2engine.Units;
using Civ2engine.Enums;
using Civ2engine.Events;
using Civ2engine.MapObjects;
using Civ2engine.Terrains;

namespace Civ2engine
{
    public partial class Game
    {
        public event EventHandler<MapEventArgs> OnMapEvent;
        public event EventHandler<UnitEventArgs> OnUnitEvent;
        internal event EventHandler<CivEventArgs> OnCivEvent;

        private readonly int[] _doNothingOrders = { (int)OrderType.Fortified, (int)OrderType.Sleep };

        // Choose next unit for orders. If all units ended turn, update cities.
        public void ChooseNextUnit()
        {
            var units = _activeCiv.Units.Where(u => !u.Dead).ToList();

            var player = Players[_activeCiv.Id];
            
            //Look for units on this square or neighbours of this square
            
            var nextUnit = NextUnit(player, units);

            ActiveUnit = nextUnit;

            // End turn if no units awaiting orders
            if (nextUnit == null)
            {
                var anyUnitsMoved = units.Any(u => u.MovePointsLost > 0);
                if ((!anyUnitsMoved || Options.AlwaysWaitAtEndOfTurn))
                {
                    Players[_activeCiv.Id].WaitingAtEndOfTurn();
                    OnPlayerEvent?.Invoke(null, new PlayerEventArgs(PlayerEventType.WaitingAtEndOfTurn, _activeCiv.Id));
                }
                else
                {
                    if (ProcessEndOfTurn())
                    {
                        ChoseNextCiv();
                        return;
                    }
                }
            }
        }

        private Unit NextUnit(IPlayer player, List<Unit> units)
        {
            Unit nextUnit;
            if (player.WaitingList is { Count: > 0 })
            {
                nextUnit =
                    ActiveTile.UnitsHere.FirstOrDefault(u => u.AwaitingOrders && !player.WaitingList.Contains(u)) ??
                    CurrentMap
                        .Neighbours(ActiveTile)
                        .SelectMany(
                            t => t.UnitsHere.Where(u => u.Owner == _activeCiv && u.AwaitingOrders && !player.WaitingList.Contains(u)))
                        .FirstOrDefault();

                nextUnit ??= units.FirstOrDefault(u => u.AwaitingOrders && !player.WaitingList.Contains(u));
                if (nextUnit == null && player.WaitingList.Count > 0)
                {
                    nextUnit = player.WaitingList[0];
                    player.WaitingList.Clear();
                }
            }
            else
            {
                nextUnit =
                    ActiveTile.UnitsHere.FirstOrDefault(u => u.AwaitingOrders) ??
                    CurrentMap
                        .Neighbours(ActiveTile)
                        .SelectMany(
                            t => t.UnitsHere.Where(u => u.Owner == _activeCiv && u.AwaitingOrders))
                        .FirstOrDefault();

                nextUnit ??= units.FirstOrDefault(u => u.AwaitingOrders);
            }

            return nextUnit;
        }

        public bool ProcessEndOfTurn()
        {
            foreach (var unit in _activeCiv.Units.Where(u =>
                         u.MovePoints > 0 && !_doNothingOrders.Contains(u.Order)))
            {
                switch ((OrderType)unit.Order)
                {
                    case OrderType.Fortify:
                        unit.Order = (int)OrderType.Fortified;
                        unit.MovePointsLost = unit.MovePoints;
                        break;
                    case OrderType.GoTo:
                        if (unit.CurrentLocation.Map.IsValidTileC2(unit.GoToX, unit.GoToY))
                        {
                            var tile = unit.CurrentLocation.Map.TileC2(unit.GoToX, unit.GoToY);
                            var path = Path.CalculatePathBetween(this, unit.CurrentLocation, tile, unit.Domain,
                                unit.MaxMovePoints, unit.Owner, unit.Alpine, unit.IgnoreZonesOfControl);
                            path?.Follow(this, unit);
                        }

                        if (unit.MovePoints >= 0)
                        {
                            ActiveUnit = unit;
                            return false;
                        }
                        break;
                    default:
                    {
                        unit.ProcessOrder();
                    
                        if (TerrainImprovements.ContainsKey(unit.Building))
                        {
                            ActiveUnit = this.CheckConstruction(unit.CurrentLocation, TerrainImprovements[unit.Building])
                                .FirstOrDefault(u => u.MovePoints > 0);
                            if (ActiveUnit != null)
                            {
                                return false;
                            }
                        }

                        break;
                    }
                }
            }

            return ActiveUnit == null;
        }
    }
}
