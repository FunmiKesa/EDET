using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDifferentialEvolution.Logic.Problems
{
    public class PFSSP : IProblem
    {
        int[,] jobtimes;
        public PFSSP(int[,] _jobtimes)
        {

            jobtimes = _jobtimes;
        }

        public ObjectiveFunctionOutput ObjectiveFunction(int[] schedule)
        {
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();

            int[,] c = new int[jobtimes.GetLength(0), jobtimes.GetLength(1)];
            int flowtime = 0;
            for (int i = 0; i < jobtimes.GetLength(0); i++)
            {


                for (int j = 0; j < jobtimes.GetLength(1); j++)
                {
                    int prevjob = j == 0 ? 0 : c[i, schedule[j - 1] - 1];
                    int prevmach = i == 0 ? 0 : c[i - 1, schedule[j] - 1];
                    int max = Math.Max(prevmach, prevjob);
                    c[i, schedule[j] - 1] = max + jobtimes[i, schedule[j] - 1];
                    flowtime += c[i, j];
                    //   Console.WriteLine("Max : {4} \n C({0},{1}) = {2} P({0},{1}) = {3}", i + 1, schedule[j], c[i, schedule[j]-1], array[i, schedule[j]-1], max);
                }

            }
            //  Console.WriteLine("\nMean flow time:" + (flowtime / array.GetLength(1)));
            output.ObjectiveArray = new double[1] { c[jobtimes.GetLength(0) - 1, schedule[jobtimes.GetLength(1) - 1] - 1] };
            return output;
        }


    }
}
