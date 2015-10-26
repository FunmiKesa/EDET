using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class QAP:IProblem
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public int Dimension { get; set; }
        public ProblemType Type { get; set; }
        public int[,] Distances { get; set; }
        public int[,] Weights { get; set; }
        public List<int> OptimalIndividual { get; set; }
        public long OptimalValue { get; set; }
    }
}
