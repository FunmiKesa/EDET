using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Logic
{
    public class TSPLogic : ProblemLogic
    {
        public TSPLogic(TSP _tsp)
        {
            this.Problem = _tsp;
        }
        public TSPLogic()
            : this(new TSP())
        {
        }

        public override Boolean LoadFile(String fileName)
        {
            Boolean successful = false;
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    Boolean readingHeader = true;
                    Boolean readingDisplayData = false;
                    Boolean readingEdgeWeights = false;
                    Boolean readingCoordinates = false;
                    double x, y;
                    int i = 0, j = 0;
                    String key;
                    String value;
                    String[] items;
                    String line = null;
                    while (!String.IsNullOrEmpty(line = reader.ReadLine()) && !line.Equals("EOF"))
                    {
                        line = line.Trim();
                        if (line.Equals("DISPLAY_DATA_SECTION"))
                        {
                            ((TSP)Problem).Cities = new List<Coordinate>();
                            readingDisplayData = true;
                            readingCoordinates = false;
                            readingHeader = false;
                            readingEdgeWeights = false;
                            continue;
                        }
                        if (readingHeader)
                        {
                            this.ProgressIndicator.RaiseProgress("Reading header...", 20);
                            items = line.Split(':');
                            key = items[0].Trim().ToUpper();
                            value = items.Length > 1 ? items[1].Trim() : "";
                            switch (key)
                            {
                                case "NAME":
                                    Problem.Name = value;
                                    break;
                                case "DIMENSION":
                                    Problem.Dimension = int.Parse(value);
                                    break;
                                case "COMMENT":
                                    Problem.Comment = value;
                                    break;
                                case "TYPE":
                                    Problem.Type = (ProblemType)Enum.Parse(typeof(ProblemType), value);
                                    break;
                                case "EDGE_WEIGHT_FORMAT":
                                    try
                                    {
                                        ((TSP)Problem).EdgeWeightFormat = (EdgeWeightFormat)Enum.Parse(typeof(EdgeWeightFormat), value);
                                    }
                                    catch (ArgumentException)
                                    {
                                        throw new EdgeWeightFormatNotSupported(value);
                                    }
                                    break;
                                case "DISPLAY_DATA_TYPE":
                                    ((TSP)Problem).DisplayDataType = (DisplayDataType)Enum.Parse(typeof(DisplayDataType), value);
                                    break;
                                case "EDGE_WEIGHT_TYPE":
                                    try
                                    {
                                        ((TSP)Problem).EdgeWeightType = (EdgeWeightType)Enum.Parse(typeof(EdgeWeightType), value);
                                    }
                                    catch (ArgumentException)
                                    {
                                        throw new EdgeWeightTypeNotSupported(value);
                                    }
                                    break;
                                case "NODE_COORD_SECTION":
                                    readingHeader = false;
                                    readingCoordinates = true;
                                    ((TSP)Problem).Cities = new List<Coordinate>();
                                    break;
                                case "EDGE_WEIGHT_SECTION":
                                    readingHeader = false;
                                    readingEdgeWeights = true;
                                    ((TSP)Problem).EdgeWeights = new int[Problem.Dimension, Problem.Dimension];
                                    break;
                                default:
                                    throw new InvalidDataException("Invalid tsp format. Cannot read file.");

                            }
                        }
                        else if (readingCoordinates)
                        {
                            int percent = (int)(((TSP)Problem).Cities.Count * 100 / Math.Pow(Problem.Dimension, 2));
                            this.ProgressIndicator.RaiseProgress(String.Format("Reading Coordinates...{0}%", percent), percent);
                            items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            x = double.Parse(items[1]);
                            y = double.Parse(items[2]);
                            ((TSP)Problem).Cities.Add(new Coordinate((float)x, (float)y));
                        }
                        else if (readingEdgeWeights)
                        {
                            this.ProgressIndicator.RaiseProgress("Reading Edge Weights...", 40);
                            items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            switch (((TSP)Problem).EdgeWeightFormat)
                            {
                                case EdgeWeightFormat.LOWER_DIAG_ROW:
                                    foreach (String s in items)
                                    {
                                        int w = int.Parse(s);
                                        ((TSP)Problem).EdgeWeights[i, j] = w;
                                        if (w == 0)
                                        {
                                            i++;
                                            j = 0;
                                        }
                                        else
                                        {
                                            j++;
                                        }
                                    }
                                    break;

                                case EdgeWeightFormat.UPPER_ROW:
                                    j = i + 1;
                                    foreach (var s in items)
                                    {
                                        int w = int.Parse(s);
                                        ((TSP)Problem).EdgeWeights[i, j] = w;
                                        if (j == Problem.Dimension - 1)
                                        {
                                            i++;
                                            j = i + 1;
                                        }
                                        else { j++; }
                                    }
                                    break;
                                case EdgeWeightFormat.UPPER_DIAG_ROW:
                                    foreach (var s in items)
                                    {
                                        int w = int.Parse(s);
                                        ((TSP)Problem).EdgeWeights[i, j] = w;
                                        if (j == Problem.Dimension - 1)
                                        {
                                            i++;
                                            j = i;
                                        }
                                        else { j++; }
                                    }
                                    break;
                                case EdgeWeightFormat.FULL_MATRIX:
                                    foreach (var s in items)
                                    {
                                        int w = int.Parse(s);
                                        ((TSP)Problem).EdgeWeights[i, j] = w;
                                        if (j == Problem.Dimension - 1)
                                        {
                                            i++;
                                            j = 0;
                                        }
                                        else { j++; }
                                    }
                                    break;
                            }
                        }
                        else if (readingDisplayData)
                        {
                            this.ProgressIndicator.RaiseProgress("Reading Display Data...", 50);
                            items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            switch ((Problem as TSP).DisplayDataType)
                            {
                                case DisplayDataType.TWOD_DISPLAY:
                                    int percent = (int)(((TSP)Problem).Cities.Count * 100 / Math.Pow(Problem.Dimension, 2));
                                    this.ProgressIndicator.RaiseProgress(String.Format("Reading Coordinates...{0}%", percent), percent);
                                    items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    x = double.Parse(items[1]);
                                    y = double.Parse(items[2]);
                                    ((TSP)Problem).Cities.Add(new Coordinate((float)x, (float)y));
                                    break;
                            }
                        }
                    }
                    ((TSP)Problem).Distances = calcDistances();
                    String bestTourFileName = fileName.Replace("." + Problem.Type.ToString().ToLower(), ".opt.tour");
                    if (File.Exists(bestTourFileName))
                    {
                        LoadOptimalTour(bestTourFileName);
                        var r = ObjectiveFunction(((TSP)Problem).OptimalTour.ToArray());
                        Problem.OptimalValue = r.ObjectiveArray[0];
                        Console.WriteLine("Optimal distance: " + Problem.OptimalValue);
                    }
                    successful = true;
                }
            }
            else
            {
                throw new FileNotFoundException("File does not exist.");
            }
            return successful;
        }

        private int calcDistance(int i, int j)
        {
            var percent = ((i * j) + ((Problem.Dimension - j) * (i - 1))) * 100 / (Problem.Dimension * Problem.Dimension);
            this.ProgressIndicator.RaiseProgress(String.Format("Calculating Distance...{0}%", percent), percent);
            i--;
            j--;
            if (percent == 80)
            {
            }
            // System.Console.WriteLine(i + " " + j);
            int distance;
            double xd, yd, d;
            Coordinate c1, c2;

            switch (((TSP)Problem).EdgeWeightType)
            {
                case EdgeWeightType.EUC_2D:
                    c1 = ((TSP)Problem).Cities[i];
                    c2 = ((TSP)Problem).Cities[j];
                    xd = c1.X - c2.X;
                    yd = c1.Y - c2.Y;
                    distance = nint(Math.Sqrt(xd * xd + yd * yd));
                    break;
                case EdgeWeightType.ATT:
                    c1 = ((TSP)Problem).Cities[i];
                    c2 = ((TSP)Problem).Cities[j];
                    xd = c1.X - c2.X;
                    yd = c1.Y - c2.Y;
                    d = Math.Sqrt((xd * xd + yd * yd) / 10);
                    distance = nint(d);
                    if (distance < d)
                    {
                        distance += 1;
                    }
                    break;
                case EdgeWeightType.GEO:
                    c1 = ((TSP)Problem).Cities[i];
                    c2 = ((TSP)Problem).Cities[j];
                    double q1 = Math.Cos(degreeToRadian(c1.Y) - degreeToRadian(c2.Y));
                    double q2 = Math.Cos(degreeToRadian(c1.X) - degreeToRadian(c2.X));
                    double q3 = Math.Cos(degreeToRadian(c1.X) + degreeToRadian(c2.X));
                    distance = (int)(TSP.RRR * Math.Acos(0.5 * ((1.0 + q1) * q2 - (1.0 - q1) * q3)) + 1.0);
                    break;

                case EdgeWeightType.EXPLICIT:
                    switch (((TSP)Problem).EdgeWeightFormat)
                    {
                        case EdgeWeightFormat.LOWER_DIAG_ROW:
                            distance = ((TSP)Problem).EdgeWeights[Math.Max(i, j), Math.Min(i, j)];
                            break;
                        case EdgeWeightFormat.UPPER_ROW:
                        case EdgeWeightFormat.UPPER_DIAG_ROW:
                            distance = ((TSP)Problem).EdgeWeights[Math.Min(i, j), Math.Max(i, j)];
                            break;
                        case EdgeWeightFormat.FULL_MATRIX:
                            distance = ((TSP)Problem).EdgeWeights[i, j];
                            break;
                        default:
                            distance = -1;
                            break;
                    }
                    break;
                default: distance = -1;
                    break;
            }
            return distance;
        }
        private void LoadOptimalTour(String filename)
        {
            ((TSP)Problem).OptimalTour = new List<int>();
            StreamReader reader = new StreamReader(File.OpenRead(filename));
            Boolean readingHeader = true;
            String key, value;
            String[] items;
            String line = null;
            while (!String.IsNullOrEmpty(line = reader.ReadLine().Trim()) && !line.Equals("EOF") && !line.Equals("-1"))
            {
                if (readingHeader)
                {
                    items = line.Split(':');
                    key = items[0].Trim().ToUpper();
                    value = items.Length > 1 ? items[1].Trim() : "";
                    if (key.Equals("TOUR_SECTION"))
                    {
                        readingHeader = false;
                    }
                }
                else
                {
                    items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in items)
                    {
                        int v = int.Parse(s);
                        if (v != -1)
                        {
                            ((TSP)Problem).OptimalTour.Add(v);
                        }
                    }
                }

            }
        }
        private int[,] calcDistances()
        {
            int[,] distances = new int[Problem.Dimension, Problem.Dimension];
            int d;
            for (int i = 1; i <= Problem.Dimension && !this.ProgressIndicator.Cancel; ++i)
            {
                for (int j = 1; j <= Problem.Dimension && !this.ProgressIndicator.Cancel; ++j)
                {

                    d = i == j ? 0 : calcDistance(i, j);
                    distances[i - 1, j - 1] = d;
                }

            }
            return distances;
        }
        private double degreeToRadian(double d)
        {
            int deg = (int)d;
            double min = d - deg;
            double rad = (Math.PI * (deg + (5 * min) / 3)) / 180;
            return rad;
        }

        private int nint(double p)
        {
            return p >= 0 ? (int)(p + 0.5) : (int)(p - 0.5);
        }
        public override ObjectiveFunctionOutput ObjectiveFunction(int[] permutation)
        {
            if (permutation == null) return null;
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();
            String s = "\n[";
            permutation.ToList().ForEach(x => s += " " + x);
            s += "]";
            int sum = ((TSP)Problem).Distances[permutation[permutation.Length - 1] - 1, permutation[0] - 1];

            for (int i = 0; i < permutation.Length - 1 && !this.ProgressIndicator.Cancel; i++)
            {
                int dist = ((TSP)Problem).Distances[permutation[i] - 1, permutation[i + 1] - 1];
                sum += dist;
                s += " + " + dist;
            }
            s += "\nSum is " + sum;
            //File.AppendAllLines("C:\\Users\\Oluwafunmilola\\Documents\\output.txt", new String[] { s });


            //  Console.WriteLine("\nMean flow time:" + (flowtime / array.GetLength(1)));
            output.ObjectiveArray = new long[1] { sum };
            return output;
        }

        public int GetDistance(int i, int j) { return ((TSP)Problem).Distances[i - 1, j - 1]; }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }


}

