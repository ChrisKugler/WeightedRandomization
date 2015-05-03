using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedRandomization
{
    internal class WeightedChance<T>
    {
        /// <summary>
        /// Target value of this randomization
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Weight from 0..1
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// Adjusted weight based on the weights of other items added to the randomizer
        /// </summary>
        public float AdjustedWeight { get; set; }        
    }
}
