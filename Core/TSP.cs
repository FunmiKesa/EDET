using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TSP : IProblem
    {
        public string Name { get; set; }

        public string Comment { get; set; }
        public int Dimension { get; set; }

        public EdgeWeightFormat EdgeWeightFormat { get; set; }
        public EdgeWeightType EdgeWeightType { get; set; }
        public DisplayDataType DisplayDataType { get; set; }
        public int[,] EdgeWeights { get; set; }
        public int[,] Distances { get; set; }
        public const double RRR = 6378.388;
        public List<Coordinate> Cities { get; set; }
        public ProblemType Type { get; set; }
        public List<int> OptimalTour { get; set; }


        public long OptimalValue { get; set; }
    }
    public enum EdgeWeightFormat
    {
        LOWER_DIAG_ROW, UPPER_DIAG_ROW, UPPER_ROW, FULL_MATRIX
    }

    public enum EdgeWeightType
    {
        GEO, EUC_2D, EXPLICIT, ATT
    }
    public enum DisplayDataType
    {
        NO_DISPLAY, TWOD_DISPLAY, COORD_DISPLAY
    }
    public class Coordinate
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Coordinate(float x, float y)
        {
            // TODO: Complete member initialization
            this.X = x;
            this.Y = y;
        }
    }
}
