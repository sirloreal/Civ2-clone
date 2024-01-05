﻿using System.Collections.Generic;

namespace Civ2engine;

public class TAlphaCentauriArrival : ITrigger
{
    /// <summary>
    /// 0xFE = ANYBODY
    /// </summary>
    public int RaceCivId { get; set; }

    /// <summary>
    /// 0xFE = ANYSIZE
    /// </summary>
    public int Size { get; set; }
    public List<string> Strings { get; set; }
}
