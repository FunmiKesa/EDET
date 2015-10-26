using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDifferentialEvolution.Logic.Problems
{
    public class QAP : IProblem
    {
        int[,] distance, weight;
        public QAP(int[,] _weight, int[,] _distance)
        {
            distance = _distance;
            weight = _weight;
        }

        public ObjectiveFunctionOutput ObjectiveFunction(int[] locations)
        {
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();

            int totalcost = 0;

            for (int i = 0; i < locations.Length; i++)
            {
                for (int j = 0; j < locations.Length; j++)
                {
                    totalcost += weight[i, j] * distance[locations[i] - 1, locations[j] - 1];
                }

            }
            output.ObjectiveArray = new double[1] { totalcost };
            return output;
        }


    }
}
