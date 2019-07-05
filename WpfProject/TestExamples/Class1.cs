using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestExamples {
    [TestFixture]
    public class TestFixture1 {
        [Test]
        public void Test1() {
            
            Assert.AreEqual(18,Helper.ToCenterArrayPath(5,5));
            
        }
    }

    public static class Helper {
        public static int Sum(int x, int y) {
            return x + y;
        }

        public static int ToCenterArrayPath(uint columCount, uint rowCount ) {
            uint arraySize = columCount * rowCount;
            int[,] array = new int[columCount,rowCount];
            int[] pathArray = new int[arraySize];
            int step = 1;
            for(int i=0; i<rowCount;i++) {
                for(int j = 0; j < columCount; j++) {
                    array[i, j] = step;
                    step++;
                }
            }
            
          
            return array[3,2];
        }
    }
}
