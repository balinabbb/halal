using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Halal;

namespace TravelingSalesMan.SimulatedAnnealing
{
    class Program
    {
        const string Points1 = "points-1.txt";
        const string Points2 = "points-2.txt";
        const string Points3 = "points-3.txt";
        public const int Iterations = 10_000_000;

        static ITsp _solver;

        static void Main()
        {
            var points = File.ReadLines(Points3)
                .Select(x => x.Split('\t'))
                .Select(x => new Point { X = int.Parse(x[0]), Y = int.Parse(x[1]) })
                .ToList();

            _solver = new TspSa();
            _solver.Init(points, Iterations);

            var best = default(Individual);
            var count = 0;
            foreach (var current in _solver.Solve())
            {
                best = current;
                Console.Write($"\r{count++}-{current.Fitness()}");
            }

            ImageUtil.SaveAsImage(best.Genes);
            Process.Start(Path.Combine(Directory.GetCurrentDirectory(), ImageUtil.FileName));
        }
    }
}
