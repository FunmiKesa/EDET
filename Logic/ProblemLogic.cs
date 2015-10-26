using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public abstract class ProblemLogic
    {
        public ProblemLogic()
        {
            this.ProgressIndicator = new ProgressIndicator();
        }
        public IProblem Problem { get; set; }
        public abstract Boolean LoadFile(String filename);
        public ProgressIndicator ProgressIndicator { get; set; }
        public abstract ObjectiveFunctionOutput ObjectiveFunction(int[] schedule);
        public override abstract String ToString();
    }
}
