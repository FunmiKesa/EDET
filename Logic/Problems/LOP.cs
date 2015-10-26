using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDifferentialEvolution.Logic.Problems
{
    public class LOP : IProblem
    {
        double[,] cost;
        public LOP(double[,] _cost)
        {
            cost = _cost;
        }

        public ObjectiveFunctionOutput ObjectiveFunction(int[] order)
        {
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();

            double totalcost = 0;

            for (int i = 0; i < order.Length - 1; i++)
            {
                for (int j = i + 1; j < order.Length; j++)
                {
                    double c = cost[order[i] - 1, order[j] - 1];
                    totalcost += c;
             //       Console.Write(String.Format(@"{0},{1}:{2} ", i, j, c));
                }
             //   Console.WriteLine(totalcost);
            }
            output.ObjectiveArray = new double[1] { totalcost };
            return output;
        }
    }
}
