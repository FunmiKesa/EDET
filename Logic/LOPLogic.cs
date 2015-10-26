using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class LOPLogic : ProblemLogic
    {
        public LOPLogic(LOP lop)
        {
            this.Problem = lop;
        }

        public LOPLogic(int[,] cost)
        {
            (Problem as LOP).Cost = cost;
        }

        public LOPLogic()
            : this(new LOP())
        {

        }

        public override ObjectiveFunctionOutput ObjectiveFunction(int[] order)
        {
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();

            int totalcost = 0;

            for (int i = 0; i < order.Length - 1; i++)
            {
                for (int j = i + 1; j < order.Length; j++)
                {
                    int c = (Problem as LOP).Cost[order[i] - 1, order[j] - 1];
                    totalcost += c;
                }
            }
            output.ObjectiveArray = new long[1] { totalcost }; //its a maximizer
            return output;
        }

        public override Boolean LoadFile(string filename)
        {
            Boolean successful = false;

            if (File.Exists(filename))
            {
                this.ProgressIndicator.RaiseProgress("Loading file...", 0);
                using (StreamReader reader = new StreamReader(filename))
                {

                    this.ProgressIndicator.RaiseProgress("Reading file...", 10);
                    Boolean readingDimension = true;
                    Boolean readingCosts = false;
                    int i = 0, j = 0;
                    String[] items;
                    String line = null;
                    try
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            line = line.Trim();
                            if (!String.IsNullOrEmpty(line))
                            {
                                Console.WriteLine(line);
                                if (readingDimension)
                                {
                                    try
                                    {
                                        Problem.Dimension = int.Parse(line);
                                        (Problem as LOP).Cost = new int[Problem.Dimension, Problem.Dimension];
                                        readingCosts = true;
                                        readingDimension = false;
                                    }
                                    catch (FormatException)
                                    {
                                        Problem.Comment = line;
                                    }
                                }
                                else if (readingCosts)
                                {
                                    items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var s in items)
                                    {
                                        int c = int.Parse(s);
                                        ((LOP)Problem).Cost[i, j] = c;
                                        if (j == Problem.Dimension - 1)
                                        {
                                            i++;
                                            j = 0;
                                        }
                                        else
                                        {
                                            j++;
                                        }
                                        if (i == Problem.Dimension)
                                        {
                                            readingCosts = false;
                                            successful = true;

                                        }
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw new InvalidDataException("Invalid lop format. Cannot read file.");
                    }
                }
            }
            return successful;
        }

        public override string ToString()
        {
            var cost = (Problem as LOP).Cost;
            string s = "Costs: \n";

            for (int i = 0; i < cost.GetLength(0); i++)
            {
                for (int j = 0; j < cost.GetLength(1); j++)
                {
                    s += " " + cost[i, j];
                }
                s += "\n";
            }
            return s;
        }
    }
}
