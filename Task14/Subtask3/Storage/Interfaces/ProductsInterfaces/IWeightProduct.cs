﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Task14
{
    interface IWeightProduct : IProduct
    {
        public double Weight { get; }
    }
}
