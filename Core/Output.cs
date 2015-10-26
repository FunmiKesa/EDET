using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class OutputStructure
    {
        public String Log { get; set; }
        public int[][] Individuals { get; set; }
        public int[] BestIndividual { get; set; }
        public double BestValue { get; set; }
        
    }

    public class ObjectiveFunctionOutput
    {
        public long[] ObjectiveArray { get; set; }
    }

}
