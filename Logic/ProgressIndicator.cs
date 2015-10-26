using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ProgressIndicator
    {
        public delegate void ProgressDelegate(string message, int percent, dynamic output);
        public event ProgressDelegate Progress;
        public Boolean Cancel { get; set; }

        public void RaiseProgress(string message, int percent, dynamic output = null)
        {
            if (Cancel)
            {
                message = "Cancelling operation....";
            }
            if (Progress != null)
            {
                Progress(message, percent, output);
            }

        }


    }
}
