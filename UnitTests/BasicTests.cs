using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeightedRandomization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq; 

namespace UnitTests
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void RandomFloat()
        {
            WeightedRandomizer<float> randFloat = new WeightedRandomizer<float>();
            randFloat.AddOrUpdateWeight(1, .25f);
            randFloat.AddOrUpdateWeight(2, .25f);
            randFloat.AddOrUpdateWeight(3, .50f);             
            float val = 0;
            int oneCount = 0;
            int twoCount = 0;
            int threeCount = 0; 
            for (int i = 0; i < 200; i++)
            {
                val = randFloat.GetNext();
                if(val == 1)
                    oneCount++;
                if(val == 2)
                    twoCount++;                        
                if(val == 3)
                    threeCount++;                                        
            }

            Trace.WriteLine(string.Format("one: {0}, two: {1}, three: {2}", oneCount, twoCount, threeCount));
            Assert.AreNotEqual(0, oneCount);
            Assert.AreNotEqual(0, twoCount);
            Assert.AreNotEqual(0, threeCount);

            Assert.IsTrue(threeCount > oneCount);
            Assert.IsTrue(threeCount > twoCount);
            double diff = Math.Abs( (float)(oneCount - twoCount) / (oneCount + twoCount)); 
            Assert.IsTrue( diff < .25f); 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "The weights of all items must add up to 1.0 ")]
        public void LowWeight()
        {
            WeightedRandomizer<int> randInt = new WeightedRandomizer<int>();
            randInt.AddOrUpdateWeight(20, .30f); 
            int val = randInt.GetNext(); 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "The weights of all items must add up to 1.0 ")]
        public void NoWeight()
        {
            WeightedRandomizer<int> randInt = new WeightedRandomizer<int>();            
            int val = randInt.GetNext(); 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "The weights of all items must add up to 1.0 ")]
        public void HighWeight()
        {
            WeightedRandomizer<int> randInt = new WeightedRandomizer<int>();
            randInt.AddOrUpdateWeight(1, .3f);
            randInt.AddOrUpdateWeight(2, .3f);
            randInt.AddOrUpdateWeight(3, .5f);             
            int val = randInt.GetNext();
        }

        [TestMethod]
        public void CustomProvider()
        {
            WeightedRandomizer<int> randInt = new WeightedRandomizer<int>();
            randInt.Provider = new BadRandomization();

            randInt.AddOrUpdateWeight(1, .1f);
            randInt.AddOrUpdateWeight(2, .4f);
            randInt.AddOrUpdateWeight(3, .5f);             

            for (int i = 0; i < 20; i++)
            {
                int val = randInt.GetNext();
                if (val == 1)
                    Assert.Fail();
                if (val == 3)
                    Assert.Fail(); 
            }            
        }

        [TestMethod]
        public void ManyWeights()
        {            
            WeightedRandomizer<int> randInt = new WeightedRandomizer<int>();
            for (int i = 0; i < 100; i++)
            {
                randInt.AddOrUpdateWeight(i, .01f);                 
            }

            Dictionary<int, int> counts = new Dictionary<int, int>(); 
            for (int i = 0; i < 100; i++)
            {
                int val = randInt.GetNext();
                if (counts.ContainsKey(val))
                    counts[val] += 1;
                else
                    counts.Add(val, 1); 
            }

            foreach (var item in counts.OrderBy(x => x.Key))
                Debug.WriteLine(string.Format("{0}: {1}", item.Key, item.Value)); 
        }        
    }
}
