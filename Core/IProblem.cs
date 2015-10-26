using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public interface IProblem
    {
        String Name { get; set; }
        ProblemType Type { get; set; }
        String Comment { get; set; }
        int Dimension { get; set; }
        long OptimalValue { get; set; }


    }
    public enum ProblemType
    {
        TSP, ATSP, QAP, LOP, PFSP,
        UNKNOWN

    }
}
