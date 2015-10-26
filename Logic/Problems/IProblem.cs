using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDifferentialEvolution.Logic.Problems
{
    public interface IProblem
    {
        ObjectiveFunctionOutput ObjectiveFunction(int[] permutation);
    }
}
