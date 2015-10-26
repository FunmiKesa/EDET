using Core;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ProblemLogic logic = new QAPLogic();
            var r = logic.LoadFile("C:\\Users\\Oluwafunmilola\\Documents\\Trainingdata\\qap\\bur26b.dat");
            if (r)
            {
                InputStructure input = new InputStructure()
                {
                    ControlVariable = 0.5,
                    CrossoverRate = 0.9,
                    Heuristic = 2,
                    NumberOfGenerations = 1000,
                    PopulationSize = 100,
                    WeightingFactor = 0.1,
                    ProblemSize = logic.Problem.Dimension,

                };
                Optimizer opt = new Optimizer(logic.ObjectiveFunction, true);
                for (int i = 0; i < 10; i++)
                {
                    input.Strategy = i + 1;
                    var start = DateTime.Now.TimeOfDay;
                    var result = opt.Optimize(input);
                    var stop = DateTime.Now.TimeOfDay;
                    Console.Write("Strategy " + (i + 1) + " Best Value: " + result.BestValue + " Time Spent(millisecs): " + stop.Subtract(start).Milliseconds);
                    Console.WriteLine();
                }
            }
        }
    }
}
