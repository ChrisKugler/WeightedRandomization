using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedRandomization
{
    public class WeightedRandomizer<T>
    {
        private bool adjusted;
        private List<WeightedChance<T>> weights;
        public IRandomizationProvider Provider { get; set; }

        public WeightedRandomizer()
        {
            this.weights = new List<WeightedChance<T>>();
        }

        public void AddOrUpdateWeight(T value, float weight)
        {
            if (weight < 0)
                throw new ArgumentException("weighted value cannot have a negative.");

            WeightedChance<T> existing = this.weights.FirstOrDefault(x => Object.Equals(x.Value, value));
            if (existing == null)
                this.weights.Add(new WeightedChance<T> { Value = value, Weight = weight });
            else
                existing.Weight = weight;

            this.adjusted = false;
        }

        public void RemoveWeight(T value)
        {
            WeightedChance<T> existing = this.weights.FirstOrDefault(x => Object.Equals(x.Value, value));
            if (existing != null)
            {
                this.weights.Remove(existing);
                this.adjusted = false;
            }
        }

        public void ClearWeights()
        {
            this.weights.Clear();
            this.adjusted = false;
        }

        /// <summary>
        /// Determines the adjusted weights for all items in the collection. This will be called automatically if GetNext is called after there are changes to the weights collection. 
        /// </summary>
        public void CalculateAdjustedWeights()
        {
            var sorted = this.weights.OrderBy(x => x.Weight).ToList();
            decimal weightSum = 0;
            for (int i = 0; i < sorted.Count(); i++)
            {
                weightSum += (decimal)sorted[i].Weight;
                if (i == 0)
                    sorted[i].AdjustedWeight = sorted[i].Weight;
                else
                    sorted[i].AdjustedWeight = sorted[i].Weight + sorted[i - 1].AdjustedWeight;
            }

            if (weightSum != 1.0m)
                throw new InvalidOperationException("The weights of all items must add up to 1.0 ");

            this.weights = this.weights.OrderBy(x => x.AdjustedWeight).ToList();

            this.adjusted = true;
        }
    
        /// <summary>
        /// Return a value based on the weights provided
        /// </summary>
        /// <returns></returns>
        public T GetNext()
        {
            if (this.Provider == null)
                this.Provider = SystemRandomizationProvider.Default;

            if (!adjusted)
                this.CalculateAdjustedWeights();

            double d = this.Provider.NextRandomValue();
            var item = this.weights.FirstOrDefault(x => d <= x.AdjustedWeight);
            return item.Value;
        }
    }
}
