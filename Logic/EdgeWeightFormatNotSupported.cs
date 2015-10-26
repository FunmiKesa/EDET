using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
   public class EdgeWeightFormatNotSupported : Exception
    {
        private string value;

        public EdgeWeightFormatNotSupported(string value)
        {
            this.value = value;
            throw new Exception(value + " format is not supported.");
        }
    }
}
