using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class PFSPLogic : ProblemLogic
    {
        public PFSPLogic(int[,] jobtimes)
        {
            Problem = new PFSP();
            (Problem as PFSP).ProcessingTime = jobtimes;
            (Problem as PFSP).Dimension = jobtimes.GetLength(1);
            (Problem as PFSP).NumberOfMachines = jobtimes.GetLength(0);
            (Problem as PFSP).NumberOfJobs = jobtimes.GetLength(1);
        }

        public PFSPLogic(PFSP pfsp)
        {
            Problem = pfsp;
        }

        public PFSPLogic()
            : this(new PFSP())
        {

        }
        public override ObjectiveFunctionOutput ObjectiveFunction(int[] permutation)
        {
            if (permutation == null) return null;
            ObjectiveFunctionOutput output = new ObjectiveFunctionOutput();

            int[,] c = new int[(Problem as PFSP).NumberOfMachines, (Problem as PFSP).NumberOfJobs];
            int flowtime = 0;
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    int prevjob = j == 0 ? 0 : c[i, permutation[j - 1] - 1];
                    int prevmach = i == 0 ? 0 : c[i - 1, permutation[j] - 1];
                    int max = Math.Max(prevmach, prevjob);
                    c[i, permutation[j] - 1] = max + (Problem as PFSP).ProcessingTime[i, permutation[j] - 1];
                    //   Console.WriteLine("Max : {4} \n C({0},{1}) = {2} P({0},{1}) = {3}", i + 1, schedule[j], c[i, schedule[j]-1], array[i, schedule[j]-1], max);
                }

            }
            for (int j = 0; j < c.GetLength(1); j++)
            {
                flowtime += c[(Problem as PFSP).NumberOfMachines - 1, permutation[j] - 1];

            }
            //  Console.WriteLine("\nMean flow time:" + (flowtime / array.GetLength(1)));
            var makespan = c[(Problem as PFSP).NumberOfMachines - 1, permutation[permutation.Length - 1] - 1];
            output.ObjectiveArray = new Int64[2] { makespan, flowtime };
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
                    Boolean readingParameters = true;
                    Boolean readingProcessingTimes = false;
                    int i = 0, j = 0;
                    String[] items;
                    String line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (!String.IsNullOrEmpty(line))
                        {
                            Console.WriteLine(line);
                            if (line.Contains("number"))
                            {
                                continue;
                            }
                            if (line.Contains("processing times"))
                            {
                                continue;
                            }
                            if (readingParameters)
                            {
                                items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                Problem.Dimension = (Problem as PFSP).NumberOfJobs = int.Parse(items[0]);
                                (Problem as PFSP).NumberOfMachines = int.Parse(items[1]);
                                (Problem as PFSP).InitialSeed = items.Length > 2 ? int.Parse(items[2]) : 0;
                                (Problem as PFSP).UpperBound = items.Length > 3 ? int.Parse(items[3]) : 0;
                                (Problem as PFSP).LowerBound = items.Length > 4 ? int.Parse(items[4]) : 0;
                                (Problem as PFSP).ProcessingTime = new int[(Problem as PFSP).NumberOfMachines, (Problem as PFSP).NumberOfJobs];
                                readingProcessingTimes = true;
                                readingParameters = false;
                            }
                            else if (readingProcessingTimes)
                            {
                                items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var s in items)
                                {
                                    int p = int.Parse(s);
                                    ((PFSP)Problem).ProcessingTime[i, j] = p;
                                    if (j == ((PFSP)Problem).NumberOfJobs - 1)
                                    {
                                        i++;
                                        j = 0;
                                    }
                                    else
                                    {
                                        j++;
                                    }
                                    if (i == ((PFSP)Problem).NumberOfMachines)
                                    {
                                        readingProcessingTimes = false;
                                        successful = true;
                                    }
                                }
                            }

                        }
                    }
                }
            }

            return successful;

        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
