﻿using System.Collections.Generic;

namespace Civ2engine;

/// <summary>
/// (Who + Technology + Flag)
/// or
/// (Mask + Threshold)
/// or
/// (Count + State)
/// </summary>
public class CheckFlag : ITrigger
{
    /// <summary>
    /// On, Off, Set or Clear
    /// </summary>
    public bool State { get; set; }
    public bool CountUsed { get; set; }
    public bool TechnologyUsed { get; set; }

    /// <summary>
    /// 0xFA=EVERYBODY, 0xFB=SOMEBODY
    /// </summary>
    public int WhoId { get; set; }
    public int Flag_Mask { get; set; }
    public int Count_Threshold { get; set; }
    public int TechnologyId { get; set; }
    public int Treshold { get; set; }
    public List<string> Strings { get; set; }
}
