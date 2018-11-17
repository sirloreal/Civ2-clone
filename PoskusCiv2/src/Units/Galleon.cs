﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoskusCiv2.Enums;

namespace PoskusCiv2.Units
{
    internal class Galleon : BaseUnit
    {
        public Galleon() : base(40, 0, 2, 2, 1, 4)
        {
            Type = UnitType.Galleon;
            LSA = UnitLSA.Sea;
            Name = "Galleon";
        }
    }
}
