using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class QAPLogic : ProblemLogic
    {
        public QAPLogic(QAP qap)
        {
            this.Problem = qap;
        }
        public QAPLogic()
            : this(new QAP())
        {

        }
        public QAPLogic(int[,] _weights, int[,] _distances)
        {
            (Problem as QAP).Distances = _distances;
            (Problem as QAP).Weights = _weights;
        }

        public override ObjectiveFunctionOutput ObjectiveFunction(int[] permutation)
        {
            if (permutation == null) return null;
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();

            int totalcost = 0;

            for (int i = 0; i < permutation.Length; i++)
            {
                for (int j = 0; j < permutation.Length; j++)
                {

                    totalcost = totalcost + (Problem as QAP).Weights[i, j] * (Problem as QAP).Distances[permutation[i] - 1, permutation[j] - 1];
                    //  Console.WriteLine("{0} += {1} * {2}", totalcost, (Problem as QAP).Weights[i, j], (Problem as QAP).Distances[permutation[i] - 1, permutation[j] - 1]);
                }

            }
            output.ObjectiveArray = new long[1] { totalcost };
            return output;
        }

        public override Boolean LoadFile(string filename)
        {
            Boolean successful = false;
            if (File.Exists(filename))
            {

                StreamReader reader = new StreamReader(filename);
                Boolean readingDimension = true;
                Boolean readingWeights = false;
                Boolean readingDistances = false;
                int i = 0, j = 0, k = 0, l = 0;
                String[] items;
                String line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (!String.IsNullOrEmpty(line))
                    {
                        //Console.WriteLine(line);
                        if (readingDimension)
                        {
                            Problem.Dimension = int.Parse(line);
                            ((QAP)Problem).Weights = new int[Problem.Dimension, Problem.Dimension];
                            ((QAP)Problem).Distances = new int[Problem.Dimension, Problem.Dimension];
                            readingDimension = false;
                            readingWeights = true;
                        }
                        else if (readingWeights)
                        {

                            items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var s in items)
                            {
                                int w = int.Parse(s);
                                ((QAP)Problem).Weights[i, j] = w;
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
                                    readingWeights = false;
                                    readingDistances = true;
                                }
                            }
                        }
                        else if (readingDistances)
                        {
                            items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var s in items)
                            {
                                int w = int.Parse(s);
                                ((QAP)Problem).Distances[k, l] = w;
                                if (l == Problem.Dimension - 1)
                                {
                                    k++;
                                    l = 0;
                                }
                                else
                                {
                                    l++;
                                }
                                if (k == Problem.Dimension)
                                {
                                    readingDistances = false;
                                }
                            }
                        }
                    }
                }
                items = filename.Split('.');
                String bestsolFileName = items.Length >= 0 ? items[0] + ".sln" : "";
                if (File.Exists(bestsolFileName))
                {
                    LoadOptimalSolution(bestsolFileName);
                    var r = ObjectiveFunction(((QAP)Problem).OptimalIndividual.ToArray());
                    Problem.OptimalValue = r.ObjectiveArray[0];
                    Console.WriteLine("Optimal distance: " + Problem.OptimalValue);
                }
                successful = true;
            }
            return successful;
        }

        private void LoadOptimalSolution(String filename)
        {
            ((QAP)Problem).OptimalIndividual = new List<int>();
            using (StreamReader reader = new StreamReader(filename))
            {

                Boolean readingDimension = true;
                Boolean readingOptimalSolution = false;
                String[] items;
                String line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (!String.IsNullOrEmpty(line))
                    {
                        if (readingDimension)
                        {
                            items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            ((QAP)Problem).OptimalValue = int.Parse(items[1]);
                            readingDimension = false;
                            readingOptimalSolution = true;
                        }

                        else if (readingOptimalSolution)
                        {
                            items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var s in items)
                            {
                                int v = int.Parse(s);
                                ((QAP)Problem).OptimalIndividual.Add(v);
                            }
                        }
                    }
                }
            }
        }

        public override String ToString()
        {
            var weights = (Problem as QAP).Weights;
            var distances = (Problem as QAP).Distances;
            string s = "Weights: \n";
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    s += " " + weights[i, j];
                }
                s += "\n";
            }
            s += "Distances: \n";
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    s += " " + distances[i, j];
                }
                s += "\n";
            }
            return s;
        }
    }
}
