using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Optimizer
    {

        public delegate ObjectiveFunctionOutput Function(int[] array);
        public Function objectiveFunction;
        public ProgressIndicator ProgressIndicator { get; set; }
        public Boolean Minimize { get; set; }
        private HashSet<int> tabu ;
        private List<long> targetVectorValues;
        //private string outT;
        //private string outI;

        public Optimizer(Function objectiveFunction, Boolean minimize)
        {
            this.ProgressIndicator = new ProgressIndicator();
            this.objectiveFunction = objectiveFunction;
            this.Minimize = minimize;
        }

        public OutputStructure Optimize(InputStructure input)
        {
            #region/* Variables */
            OutputStructure output = new OutputStructure();

            int problemSize = input.ProblemSize;
            int populationSize = input.PopulationSize;
            double crossoverRate = input.CrossoverRate;
            int[] minbound = input.MinimumBound;
            int[] maxbound = input.MaximumBound;
            int numberOfGenerations = input.NumberOfGenerations;
            double weightingFactor = input.WeightingFactor;
            Random rand = new Random();
            int generation = 0;
            double[][] noisyRandomVector = new double[populationSize][];
            double[][] trialVector = new double[populationSize][];
            double[][] perturbedVector = new double[populationSize][];

            int[][] discretePopulation = new int[populationSize][];
            int[][] discretePopulationBT = new int[populationSize][]; // for backward transformation
            double[][] continuousPopulation = new double[populationSize][];
            double[] bestvalues = new double[numberOfGenerations + 1];
            int[] bestval_indexes = new int[numberOfGenerations + 1]; // stores the index of the best individual for each generation
            int[] bestVectorG = new int[problemSize];
            int[] targetVector;
            List<int> missingValues;
            Dictionary<int, List<int>> duplicateValues;
            List<int> duplicateIndices;
            List<int> positionalindexes;

            tabu= new HashSet<int>();
            const int scalingfactor = 100;
            int strategy = input.Strategy;
            double controlVariable = input.ControlVariable;
            int lastStagIndex = 0;
            //int[] insertionCount;
            int heuristic;
            String[] insertedIndexes = new String[numberOfGenerations + 1];
            int r1, r2, r3, r4 = 0, r5 = 0; // used for pertubation

            #endregion

            #region /*INITIALIZATION*/
            heuristic = input.Heuristic;
            minbound = new int[problemSize];
            maxbound = new int[problemSize];

            for (int j = 0; j < problemSize; j++)
            {
                minbound[j] = 1;
                maxbound[j] = problemSize;
            }

            if (crossoverRate < 0 || crossoverRate > 1)
            {
                crossoverRate = 0.5;
            }

            for (int i = 0; i < populationSize/* && !this.ProgressIndicator.Cancel*/; i++)
            {
                discretePopulation[i] = new int[problemSize];
                continuousPopulation[i] = new double[problemSize];
                List<int> list = new List<int>();
                for (int j = 0; j < problemSize /*&& !this.ProgressIndicator.Cancel*/; j++)
                {
                    int jth;
                    do
                    {
                        jth = (int)(minbound[j] + rand.NextDouble() * (maxbound[j] + 1 - minbound[j]));
                    } while (list.Contains(jth) /*&& !this.ProgressIndicator.Cancel*/);
                    list.Add(jth);
                    discretePopulation[i][j] = jth;
                    var percent = (int)Math.Truncate(((i * j) + ((problemSize - j) * i)) * 100.0 / (populationSize * problemSize));
                    //this.ProgressIndicator.RaiseProgress(String.Format("Generating Initial Population...{0}%", percent), percent);
                }
            }

            //if (this.ProgressIndicator.Cancel)
            //{
            //    return null;
            //}
            long bestValue;
            int bestIndex;

            // find the best vector in the initial population
            var bestVector = FindBestVector(discretePopulation, out bestValue, out bestIndex,ref targetVectorValues);
            bestVectorG = bestVector; // sets the best vector for the initial population
            bestval_indexes[generation] = bestIndex;
            bestvalues[generation] = bestValue;
            //this.ProgressIndicator.RaiseProgress("\nPlease wait...", 50, new { individuals = discretePopulation, best = bestval_indexes[generation] });
            duplicateIndices = new List<int>();
            missingValues = new List<int>();
            positionalindexes = new List<int>();

            #endregion
            // this.ProgressIndicator.RaiseProgress("Mutation and Recombination phase...", 0);
            while (generation < numberOfGenerations && bestValue > input.ThresholdMin && !this.ProgressIndicator.Cancel)
            {
                insertedIndexes[generation + 1] = "";
              
                for (int i = 0; i < populationSize; i++)
                {
                    var percent = (int)Math.Truncate((i) * 100.0 / populationSize);
                    //this.ProgressIndicator.RaiseProgress("Generation " + generation + String.Format(" Best Value: {3} at index {4} Perturbing individual {0} out of {1}...{2}%", i, populationSize, percent, bestValue, bestIndex), percent);
                    //this.ProgressIndicator.RaiseProgress(String.Format("\nGeneration {0}: {1}", generation, bestvalues[generation]), 50, new { individuals = discretePopulation, best = bestval_indexes[generation] });

                    Boolean stopcrossover = false; // used for exponential crossover

                    noisyRandomVector[i] = new double[problemSize];
                    trialVector[i] = new double[problemSize];
                    discretePopulationBT[i] = new int[problemSize];

                    targetVector = discretePopulation[i];

                    // int targetVectorValue = objectiveFunction(targetVector).ObjectiveArray[0];

                    //outT += String.Format(
                    //       "{0,-5} | {1,-10}|", i, targetVectorValue);

                    #region /* MUTATION */
                    // this.ProgressIndicator.RaiseProgress("Mutation...", percent);
                    // generate noisy random vector
                    do { r1 = (int)Math.Truncate(rand.NextDouble() * populationSize); } while (r1 == i);
                    do { r2 = (int)Math.Truncate(rand.NextDouble() * populationSize); } while (r2 == i || r2 == r1);
                    do { r3 = (int)Math.Truncate(rand.NextDouble() * populationSize); } while (r3 == i || r3 == r2 || r3 == r1);
                    if (populationSize > 4 && new int[] { 4, 9, 5, 10 }.Contains(strategy))
                    {
                        do { r4 = (int)Math.Truncate(rand.NextDouble() * populationSize); } while (r4 == i || r4 == r2 || r4 == r1 || r4 == r3);
                    }
                    else if (populationSize > 5 && new int[] { 5, 10 }.Contains(strategy))
                    {
                        do { r5 = (int)Math.Truncate(rand.NextDouble() * populationSize); } while (r5 == i || r5 == r2 || r5 == r1 || r5 == r3 || r5 == r4);
                    }

                    int Irand = (int)Math.Truncate(rand.NextDouble() * problemSize);

                    for (int j = 0; j < problemSize; j++)
                    {
                        //outT += String.Format(
                        // "{0,7}", discretePopulation[i][j]);
                        // forward transformation
                        continuousPopulation[i][j] = ((discretePopulation[i][j] * 5 * scalingfactor) / (Math.Pow(10, 3) - 1)) - 1;
                        continuousPopulation[r1][j] = ((discretePopulation[r1][j] * 5 * scalingfactor) / (Math.Pow(10, 3) - 1)) - 1;
                        continuousPopulation[r2][j] = ((discretePopulation[r2][j] * 5 * scalingfactor) / (Math.Pow(10, 3) - 1)) - 1;
                        continuousPopulation[r3][j] = ((discretePopulation[r3][j] * 5 * scalingfactor) / (Math.Pow(10, 3) - 1)) - 1;
                        if (populationSize > 4 && new int[] { 4, 9, 5, 10 }.Contains(strategy))
                        {
                            continuousPopulation[r4][j] = ((discretePopulation[r4][j] * 5 * scalingfactor) / (Math.Pow(10, 3) - 1)) - 1;
                        }

                        // calculate the noisy random vector
                        switch (strategy)
                        {
                            case 1:// DE/best/1/exp
                            case 6: // DE/best/1/bin
                                noisyRandomVector[i][j] = bestVectorG[j] + weightingFactor * (continuousPopulation[r1][j] - continuousPopulation[r2][j]);
                                break;
                            case 2: // DE/rand/1/exp
                            case 7: // DE/rand/1/bin
                                noisyRandomVector[i][j] = continuousPopulation[r3][j] + weightingFactor * (continuousPopulation[r1][j] - continuousPopulation[r2][j]);
                                break;
                            case 3: // DE/rand-to-best/1/exp
                            case 8: // DE/rand-to-best/1/bin
                                noisyRandomVector[i][j] = targetVector[j] + controlVariable * (bestVectorG[j] - targetVector[j]) + weightingFactor * (continuousPopulation[r1][j] - continuousPopulation[r2][j]);
                                break;
                            case 4: // DE/best/2/exp
                            case 9: // DE/best/2/bin
                                noisyRandomVector[i][j] = bestVectorG[j] + weightingFactor * (continuousPopulation[r1][j] + continuousPopulation[r2][j] - continuousPopulation[r3][j] - continuousPopulation[r4][j]);
                                break;
                            case 5: // DE/rand/2/exp
                            case 10:// DE/rand/2/bin
                                continuousPopulation[r5][j] = ((discretePopulation[r5][j] * 5 * scalingfactor) / (Math.Pow(10, 3) - 1)) - 1;
                                noisyRandomVector[i][j] = continuousPopulation[r5][j] + weightingFactor * (continuousPopulation[r1][j] + continuousPopulation[r2][j] - continuousPopulation[r3][j] - continuousPopulation[r4][j]);
                                break;

                        }

                        #region /* RECOMBINATION */
                        // generate trial vector
                        switch (strategy)
                        {
                            //exp
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                if (rand.NextDouble() < crossoverRate && !stopcrossover)
                                {
                                    trialVector[i][j] = noisyRandomVector[i][j];
                                }
                                else
                                {
                                    trialVector[i][j] = continuousPopulation[i][j];
                                    stopcrossover = true;
                                }
                                break;
                            //bin
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                                if (rand.NextDouble() < crossoverRate || j == Irand)
                                {
                                    trialVector[i][j] = noisyRandomVector[i][j];
                                }
                                else
                                {
                                    trialVector[i][j] = continuousPopulation[i][j];
                                }
                                break;
                        }
                        #endregion
                        // Backward transformation
                        discretePopulationBT[i][j] = (int)Math.Round(((1 + trialVector[i][j]) * (1000 - 1)) / (5 * scalingfactor));

                        // bounded solution
                        if (discretePopulationBT[i][j] < minbound[j])
                        {
                            discretePopulationBT[i][j] = minbound[j];
                        }
                        else if (discretePopulationBT[i][j] > maxbound[j])
                        {
                            discretePopulationBT[i][j] = maxbound[j];
                        }
                    }
                    // outT += String.Format("\n");
                    #endregion

                    #region /* Random mutation */
                    duplicateValues = DuplicateValues(discretePopulationBT[i].ToList());

                    // set default
                    int r;
                    for (int k = 0; k < duplicateValues.Count(); k++)
                    {
                        r = (int)(rand.NextDouble() * duplicateValues.ElementAt(k).Value.Count);
                        duplicateValues.ElementAt(k).Value.RemoveAt(r);
                        List<int> v = duplicateValues.ElementAt(k).Value;
                        for (int l = 0; l < v.Count; l++)
                        {
                            duplicateIndices.Add(v.ElementAt(l));
                        }
                    }

                    //find missing values
                    missingValues = MissingValues((int[])discretePopulationBT[i].Clone());

                    // set missing values
                    if (missingValues.Count == 1 && duplicateValues.ElementAt(0).Value.Count == 1)
                    {
                        discretePopulationBT[i][duplicateValues.ElementAt(0).Value.First()] = missingValues[0];
                    }
                    else
                    {
                        duplicateIndices = duplicateIndices.OrderBy(x => x).ToList();
                        for (int k = 0; k < missingValues.Count; k++)
                        {
                            int ind;
                            do
                            {
                                ind = (int)(rand.NextDouble() * missingValues.Count);
                            }
                            while (positionalindexes.Count(p => p == ind) > 0);
                            positionalindexes.Add(ind);
                        }
                        for (int k = 0; k < positionalindexes.Count; k++)
                        {
                            discretePopulationBT[i][duplicateIndices.ElementAt(positionalindexes[k])] = missingValues[k];
                        }
                    }
                    duplicateIndices.Clear();
                    missingValues.Clear();
                    positionalindexes.Clear();
                    #endregion

                    #region /* Standard Mutation */
                    int index1, index2 = 0;
                    //var n = discretePopulationBT[i].ToList();
                    index1 = (int)(rand.NextDouble() * problemSize);
                    do { index2 = (int)(rand.NextDouble() * problemSize); } while (index2 == index1);
                    int[] tempIndividual = (int[])discretePopulationBT[i].Clone();
                    int temp = tempIndividual[index1];
                    tempIndividual[index1] = tempIndividual[index2];
                    tempIndividual[index2] = temp;
                    var tempV = objectiveFunction(tempIndividual).ObjectiveArray[0];
                    var disValue = objectiveFunction(discretePopulationBT[i]).ObjectiveArray[0];
                    if (IsBetter(tempV, disValue))
                    {
                        discretePopulationBT[i] = tempIndividual;
                        disValue = tempV;
                    }
                    #endregion


                    #region /* Insertion */
                    index1 = (int)(rand.NextDouble() * problemSize);
                    do { index2 = (int)(rand.NextDouble() * problemSize); } while (index2 == index1);

                    int lowerindex = Math.Min(index1, index2);
                    int upperindex = Math.Max(index1, index2);
                    tempIndividual = (int[])discretePopulationBT[i].Clone();

                    var value = tempIndividual[lowerindex];
                    for (int k = lowerindex; k < upperindex; k++)
                    {
                        tempIndividual[k] = tempIndividual[k + 1];
                    }
                    tempIndividual[upperindex] = value;
                    tempV = objectiveFunction(tempIndividual).ObjectiveArray[0];
                    disValue = objectiveFunction(discretePopulationBT[i]).ObjectiveArray[0];
                    if (IsBetter(tempV, disValue))
                    {
                        discretePopulationBT[i] = tempIndividual;
                        disValue = tempV;
                    }
                    #endregion
                }// end for loop
                //output.Log += String.Format("\n\nGeneration [{0}]\n", generation) + outT;

                #region/* SELECTION */
                // new population for next generation
                // this.ProgressIndicator.RaiseProgress("Selection phase...", percent);
                for (int i = 0; i < populationSize; i++)
                {
                    targetVector = discretePopulation[i];
                    targetVectorValues[i] = objectiveFunction(targetVector).ObjectiveArray[0];
                    long trialvalue = objectiveFunction(discretePopulationBT[i]).ObjectiveArray[0];
                    if (IsBetter(trialvalue, targetVectorValues[i]))
                    {
                        discretePopulation[i] = discretePopulationBT[i];
                        targetVectorValues[i] = trialvalue;
                        if (tabu.Contains(i))
                        {
                            tabu.Remove(i); // removes index i from the tabu since the vector has changed
                        }
                        if (IsBetter(trialvalue, bestValue))
                        {
                            bestVector = discretePopulationBT[i];
                            bestValue = trialvalue;
                            bestIndex = i;
                        }
                        insertedIndexes[generation + 1] += " " + i;

                    }
                }
                #endregion

                #region /*OUTPUT*/
                //var log = String.Format("\nBest Value: {0} at index {1}", bestvalues[generation], bestval_indexes[generation]);
                //log += "\nInserted individuals : " + insertedIndexes[generation];

                //if (this.ProgressIndicator.Cancel)
                //{
                //    return null;
                //}


                #endregion
                generation++;

                bestvalues[generation] = bestValue;
                bestval_indexes[generation] = bestIndex;
                bestVectorG = bestVector;

                #region// local search
                if (Stagnation(bestvalues, generation, lastStagIndex))
                {
                    var bestV = bestVector.ToList();
                    switch (heuristic)
                    {
                        case 1:
                            bestValue = TwoOpt(ref bestV);
                            break;
                        case 2:
                            var e = Equal(bestVectorG.ToList(), bestVector.ToList()); // checks if bestvector has changed
                            if (!tabu.Contains(bestIndex) || !e)
                            {
                                bestValue = TwoOpt(ref bestV);
                                if (bestvalues[generation] == bestValue)
                                {
                                    // stuck at best vector
                                    tabu.Add(bestIndex);
                                }
                                else
                                {
                                    tabu.Remove(bestIndex);
                                }
                                //log += String.Format("\nLocal Search value : {0} at {1} Best Individual: ", bestValue, bestIndex);
                                //bestV.ForEach(x => log += String.Format(" " + x));
                            }
                            else// if (e)
                            {
                                if (tabu.Count != populationSize) // checks if every individual in the population is forbidden
                                {
                                    var tempVectors = (int[][])discretePopulation.Clone();

                                    for (int k = 0; k < tabu.Count; k++)
                                    {
                                        // removes the vectors in the tabu from the search for the best vector
                                        tempVectors[tabu.ElementAt(k)] = null;

                                    }
                                    List<int> tempbestV;
                                    long tempbestValue;
                                    int tempbestIndex;
                                    List<long> tempValues = new List<long> ();

                                    tempbestV = FindBestVector(tempVectors, out tempbestValue, out tempbestIndex,ref tempValues).ToList();
                                    var temp = tempbestValue;
                                    tempbestValue = TwoOpt(ref tempbestV);

                                    discretePopulation[tempbestIndex] = tempbestV.ToArray();
                                    if (temp == tempbestValue)
                                    {
                                        tabu.Add(tempbestIndex); // the vector cannot be improved
                                    }
                                    else
                                    {
                                        if (IsBetter(tempbestValue, bestValue))
                                        {
                                            bestV = tempbestV;
                                            bestValue = tempbestValue;
                                            bestIndex = tempbestIndex;
                                        }
                                        tabu.Remove(tempbestIndex);
                                    }
                                    //log += String.Format("\nLevel 2 Local Search value : {0} at index {1} Best Individual: ", tempbestValue, tempbestIndex);
                                    //tempbestV.ForEach(x => log += String.Format(" " + x));
                                }
                            }
                            break;
                    }

                    bestvalues[generation] = bestValue;
                    bestVectorG = bestVector = bestV.ToArray(); ; // used in the next iteration by some strategies
                    discretePopulation[bestIndex] = bestVector; // updates the population
                    bestval_indexes[generation] = bestIndex;
                }
                if (bestValue != bestvalues[generation - 1])
                {
                    lastStagIndex = generation;
                }

                //if (generation % (numberOfGenerations / 10) == 0)
                //{
                //    //output.Log += log;
                //    //this.ProgressIndicator.RaiseProgress("", 50, new { log = output.Log });
                //}
                #endregion
                //Console.WriteLine(String.Format("Best value {0}: {1}",generation, bestValue));
                var b = String.Format("Best value {0}: {1}", generation, bestValue);
                this.ProgressIndicator.RaiseProgress(b, (int)(generation * 100.0 / numberOfGenerations));
            }//end while loop
            output.BestValue = bestValue;
            output.BestIndividual = bestVector;
            //this.ProgressIndicator.RaiseProgress("Optimization Completed!", 100);
            return output;
        }

        private bool Stagnation(double[] bestvalues, int generation, int lastStagIndex)
        {
            bool stag = false;
            if ((generation - lastStagIndex + 1) % 5 == 0)
            {
                stag = true;
                for (int i = generation - 1; i >= lastStagIndex; i--)
                {
                    if (bestvalues[generation] != bestvalues[i])
                    {
                        stag = false;
                    }
                }
            }
            return stag;
        }

        private Boolean Equal(List<int> a, List<int> b)
        {

            if (a.Count == b.Count)
            {
                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i] != b[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private Boolean Equal(int[] a, int[] b)
        {
            return Equal(a.ToList(), b.ToList());

        }


        private List<int> MissingValues(int[] p)
        {
            List<int> list = new List<int>();
            for (int i = 1; i <= p.Length; i++)
            {
                if (!p.Contains(i))
                {
                    list.Add(i);
                }
            }
            return list;
        }
        /// <summary>
        /// Checks if a is better than b
        /// </summary>
        /// <param name="a">first value</param>
        /// <param name="b">second value</param>
        /// <returns>true if a is less than b for a minizer or a is greater than b for a maximizer, false otherwise</returns>
        private Boolean IsBetter(double a, double b)
        {
            if (Minimize)
            {
                if (a < b)
                {
                    return true;
                }
            }
            else
            {
                if (a > b)
                {
                    return true;
                }
            }
            return false;
        }
        private int[] FindBestVector(int[] vector1, int[] vector2)
        {
            var vector1V = objectiveFunction(vector1).ObjectiveArray[0];
            var vector2V = objectiveFunction(vector2).ObjectiveArray[0];
            if ((vector1V > vector2V && Minimize) || (vector1V < vector2V && !Minimize))
            {
                return vector2;
            }
            else
            {
                return vector1;
            }
        }
        /// <summary>
        /// Uses the 2 Opt local search to check if the current value is the best possible value for the vector
        /// </summary>
        /// <param name="vector">vector to be checked</param>
        /// <param name="value">value of vector</param>
        /// <returns>True if the value is the best value of the vector otherwise false</returns>
        private Boolean IsBestValue(int[] vector, int value)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                for (int j = i + 1; j < vector.Length; j++)
                {
                    var newvector = TwoOptSwap(vector.ToArray(), i, j).ToList();
                    var cost = objectiveFunction(newvector.ToArray()).ObjectiveArray[0];
                    if (IsBetter(cost, value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private int[] FindBestVector(int[][] population, out long bestValue, out int bestIndex, ref List<long> values)
        {
            int i;
            int[] best = null;
            bestValue = Minimize ? int.MaxValue : int.MinValue;
            values = new List<long>();
            bestIndex = 0;
            for (i = 0; i < population.Length; i++)
            {
                var ithRow = population[i];
                if (ithRow == null) continue;
                var ithValue = objectiveFunction(ithRow).ObjectiveArray[0];
            
                    values.Add(ithValue);
               
                if (IsBetter(ithValue, bestValue))
                {
                    bestValue = ithValue;
                    best = ithRow;
                    bestIndex = i;
                }

            }
            return best;
        }


        public Dictionary<int, List<int>> DuplicateValues(List<int> arr)
        {
            Dictionary<int, List<int>> dict = new Dictionary<int, List<int>>();
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr.Count(x => x == arr[i]) > 1 && !dict.ContainsKey(arr[i]))
                {
                    List<int> d = arr.Skip(i).Select((x, y) => new { x, y })
                        .Where(x => x.x == arr[i]).Select(x => x.y + i).ToList();
                    dict.Add(arr[i], d);
                }
            }
            return dict;
        }
        public long TwoOpt(ref int[] path)
        {
            var pList = path.ToList();
            var v = TwoOpt(ref pList);
            path = pList.ToArray();
            return v;
        }
        public long TwoOpt(ref List<int> path)
        {
            List<int> newpath = new List<int>();
            var bestcost = objectiveFunction(path.ToArray()).ObjectiveArray[0];
            for (int i = 0; i < path.Count; i++)
            {
                for (int j = i + 1; j < path.Count; j++)
                {
                    newpath = TwoOptSwap(path.ToArray(), i, j).ToList();
                    var cost = objectiveFunction(newpath.ToArray()).ObjectiveArray[0];
                    if (IsBetter(cost, bestcost))
                    {
                        path = newpath;
                        //for (int m = 0; m < newpath.Length; m++) { Console.Write(newpath[m] + " "); }
                        //Console.WriteLine();
                        bestcost = cost;
                    }
                }
            }
            return bestcost;
        }
        public static int[] TwoOptSwap(int[] path, int i, int k)
        {
            int[] newpath = new int[path.Length];
            for (int j = 0; j < i; j++)
            {
                newpath[j] = path[j];
            }
            int l = k;
            for (int j = i; j <= k; j++)
            {
                newpath[j] = path[l];
                l--;
            }
            for (int j = k + 1; j < path.Length; j++)
            {
                newpath[j] = path[j];
            }
            return newpath;
        }


        public Boolean HasConverged(List<long> values, double epsilon)
        {
            if (values != null)
            {
                var best = values[0];
                double sum = best;
                for (int i = 1; i < values.Count; i++)
                {
                    sum += values[i];
                    if (IsBetter(values[i], best))
                    {
                        best = values[i];
                    }
                }
                var aver = sum / values.Count;
                if (Math.Abs(aver - best) < epsilon)
                    return true;

            } 
            return false;
        }
    }
}
