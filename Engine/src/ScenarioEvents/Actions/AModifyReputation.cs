﻿using System.Collections.Generic;

namespace Civ2engine;

public class AModifyReputation : IAction
{
    /// <summary>
    /// 0xFC=TRIGGERDEFENDER/TRIGGERRECEIVER, 0xFD=TRIGGERATTACKER
    /// </summary>
    public int WhoId { get; set; }

    /// <summary>
    /// 0xFC=TRIGGERDEFENDER/TRIGGERRECEIVER, 0xFD=TRIGGERATTACKER, 0xFF=BETRAY USED
    /// </summary>
    public int WhomId { get; set; }
    public int Betray { get; set; }
    public int Modifier { get; set; }
    public List<string> Strings { get; set; }
}
