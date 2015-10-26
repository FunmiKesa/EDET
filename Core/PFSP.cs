using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class PFSP : IProblem
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Dimension { get; set; }
        public ProblemType Type { get; set; }
        public int[,] ProcessingTime { get; set; }
        public int NumberOfJobs { get; set; }
        public int NumberOfMachines { get; set; }
        public int InitialSeed { get; set; }
        public int UpperBound { get; set; }
        public int LowerBound { get; set; }
        public long OptimalValue { get; set; }
    }
}
