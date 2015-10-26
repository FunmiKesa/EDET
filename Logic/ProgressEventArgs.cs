using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ProgressEventArgs : EventArgs
    {
        public ProgressLogic CurrentProgress { get; private set; }
        public ProgressEventArgs(ProgressLogic prog)
        {
            CurrentProgress = prog;
        }
    }
}