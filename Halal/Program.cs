using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Halal
{
    internal class Program
    {
        const string Points1 = "points-1.txt";
        const string Points2 = "points-2.txt";
        const string Points3 = "points-3.txt";
        const int Polygon = 6;
        const int Iterations = 1000000;

        const int PointOffset = 200;

        static ISmallestBoundary _solver;

        static void Main()
        {
            var points = File.ReadLines(Points3)
                .Select(x => x.Split('\t'))
                .Select(x => new Point {X = int.Parse(x[0]) + PointOffset, Y = int.Parse(x[1]) + PointOffset})
                .ToList();
            _solver = new Solver();
            _solver.Init(points, Polygon, Iterations);
            var result = _solver.Solve();

            ImageUtil.SaveAsImage(result.points, result.solutionPoints, result.centroid, result.maxDistance);
            Process.Start(Path.Combine(Directory.GetCurrentDirectory(), ImageUtil.FileName));
        }
    }
}
