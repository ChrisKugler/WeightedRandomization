using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightedRandomization;

namespace UnitTests
{
    public class BadRandomization : IRandomizationProvider 
    {
        public double NextRandomValue()
        {
            return .2f; 
        }
    }
}
