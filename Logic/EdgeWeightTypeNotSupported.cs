using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class EdgeWeightTypeNotSupported : Exception
    {
        private string value;

        public EdgeWeightTypeNotSupported(string value)
        {
            this.value = value;
            throw new Exception(value + " type is not supported.");
        }
    }
}
