﻿using System.Collections.Generic;

namespace Civ2engine;

public class TUnitKilled : ITrigger
{
    /// <summary>
    /// 0xFE = ANYUNIT
    /// </summary>
    public int UnitKilledId { get; set; }

    /// <summary>
    /// 0xFE = ANYBODY
    /// </summary>
    public int AttackerCivId { get; set; }

    /// <summary>
    /// 0xFE = ANYBODY
    /// </summary>
    public int DefenderCivId { get; set; }
    public bool DefenderOnly { get; set; }
    public int MapId { get; set; }
    public List<string> Strings { get; set; }
}
