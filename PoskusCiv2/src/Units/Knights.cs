﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoskusCiv2.Enums;

namespace PoskusCiv2.Units
{
    internal class Knights : BaseUnit
    {
        public Knights() : base(40, 4, 2, 1, 1, 2)
        {
            Type = UnitType.Knights;
            LSA = UnitLSA.Land;
            Name = "Knights";
        }
    }
}
