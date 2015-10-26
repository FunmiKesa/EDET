using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class InputStructure
    {
        public long ThresholdMin { get; set; }
        public int PopulationSize { get; set; }
        public int ProblemSize { get; set; }
        public double WeightingFactor { get; set; }
        public double CrossoverRate { get; set; }
        public int NumberOfGenerations { get; set; }
        public int[] MinimumBound { get; set; }
        public int[] MaximumBound { get; set; }
        public int Strategy { get; set; }
        public int Heuristic { get; set; }
        public double ControlVariable { get; set; }
    }
}
