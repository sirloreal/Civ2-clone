﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoskusCiv2.Enums;

namespace PoskusCiv2.Units
{
    internal class Archers : BaseUnit
    {
        public Archers() : base(30, 3, 2, 1, 1, 1)
        {
            Type = UnitType.Archers;
            LSA = UnitLSA.Land;
            Name = "Archers";
        }
    }
}
