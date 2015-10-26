using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDifferentialEvolution.Logic.Problems
{
    public class TSP : IProblem
    {
        int[,] distance;
        public TSP(int[,] _distance)
        {
            distance = _distance;
        }
       
        public ObjectiveFunctionOutput ObjectiveFunction(int[] cities)
        {
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();

            int sum = distance[cities.LastOrDefault() - 1, cities.FirstOrDefault() - 1];

            for (int i = 0; i < cities.Length - 1; i++)
            {
                sum += distance[cities[i] - 1, cities[i + 1] - 1];
            }
            //  Console.WriteLine("\nMean flow time:" + (flowtime / array.GetLength(1)));
            output.ObjectiveArray = new double[1] { sum };
            return output;
        }
    }
}
