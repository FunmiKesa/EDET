using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ProgressLogic
    {
        public delegate void ProgressDelegate(string message, int percent);
        public event ProgressDelegate Progress;

        public void RaiseProgress(string message, int percent)
        {
            if (Progress != null)
            {
                Progress(message, percent);
            }
        }
    }
    
}
