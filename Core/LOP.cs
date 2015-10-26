using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class LOP : IProblem
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Dimension { get; set; }
        public ProblemType Type { get; set; }

        public int[,] Cost { get; set; }


        public long OptimalValue { get; set; }
    }
}
