using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Halal;

namespace TravelingSalesMan.GeneticAlgorithm
{
    class Program
    {
        const string Points1 = "points-1.txt";
        const string Points2 = "points-2.txt";
        const string Points3 = "points-3.txt";
        const int Iterations = 100000;

        static ITsp _solver;

        static void Main()
        {
            var points = File.ReadLines(Points3)
                .Select(x => x.Split('\t'))
                .Select(x => new Point { X = int.Parse(x[0]), Y = int.Parse(x[1]) })
                .ToList();

            _solver = new TspGenetic();
            _solver.Init(points, Iterations);

            var best = default(Individual);
            foreach (var current in _solver.Solve())
            {
                best = current;
                Console.WriteLine(current.Fitness());
            }

            ImageUtil.SaveAsImage(best.Genes);
            Process.Start(Path.Combine(Directory.GetCurrentDirectory(), ImageUtil.FileName));
        }
    }
}
