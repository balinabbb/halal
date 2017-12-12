using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Halal
{
    internal class Solver : ISmallestBoundary
    {
        static readonly Random Rnd = new Random();
        List<Point> _points;
        List<Point> _solutionPoints = new List<Point>();
        int _numOfPolygon;
        int _iterations;

        public void Init(List<Point> points, int polygon, int iterations)
        {
            _numOfPolygon = polygon;
            _iterations = iterations;
            _solutionPoints = new List<Point>();
            _points = points;
        }

        public (IReadOnlyCollection<Point> points, IReadOnlyCollection<Point> solutionPoints, Point centroid, int maxDistance) Solve()
        {
            var centroid = CalculateCentroid();
            var maxDistance = GetDistanceToFurthestPoint(centroid);
            GenerateSolutionPoints(centroid);

            for (var i = 0; i < _iterations; i++)
            {
                var newsolution = _solutionPoints.ToList();
                var pId = Rnd.Next(0, _numOfPolygon);
                var value = Rnd.Next(-10, 11);
                var p = newsolution[pId];
                p.X += value;
                newsolution[pId] = p;
                value = Rnd.Next(-10, 11);
                p = newsolution[pId];
                p.Y += value;
                newsolution[pId] = p;
                if (Objective(newsolution) < Objective(_solutionPoints)
                     && AllPointsIsInBoundary(newsolution))
                {
                    _solutionPoints = newsolution;
                }
            }

            return (_points, _solutionPoints, centroid, (int)maxDistance);
        }

        Point CalculateCentroid()
        {
            var x = 0.0;
            var y = 0.0;
            for (var i = 0; i < _points.Count; i++)
            {
                x += _points.ElementAt(i).X;
                y += _points.ElementAt(i).Y;
            }
            x /= _points.Count;
            y /= _points.Count;
            return new Point((int)x, (int)y);
        }

        static double DistanceOfTwoPoints(Point p1, Point p2) 
            => Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));

        double GetDistanceToFurthestPoint(Point centroid) 
            => _points.Select(pont => DistanceOfTwoPoints(centroid, pont)).Concat(new double[] { 0 }).Max() * 3;

        void GenerateSolutionPoints(Point centroid)
        {
            var angleDif = 360 / _numOfPolygon;
            var angle = 0.0;
            var distance = GetDistanceToFurthestPoint(centroid);

            for (var i = 0; i < _numOfPolygon; i++)
            {
                var x = Math.Cos(angle / 180 * Math.PI) * distance + centroid.X;
                var y = Math.Sin(angle / 180 * Math.PI) * distance + centroid.Y;
                _solutionPoints.Add(new Point((int)x, (int)y));
                angle += angleDif;
            }
        }

        static double DistanceFromLine(Point lp1, Point lp2, Point p) =>
            ((lp2.Y - lp1.Y) * p.X - (lp2.X - lp1.X) * p.Y + lp2.X * lp1.Y - lp2.Y * lp1.X)
                                                                         / Math.Sqrt(Math.Pow(lp2.Y - lp1.Y, 2) + Math.Pow(lp2.X - lp1.X, 2));

        static double LengthOfBoundary(IReadOnlyList<Point> solution)
        {
            double sumLength = 0;

            for (var li = 0; li < solution.Count - 1; li++)
            {
                var p1 = solution[li];
                var p2 = solution[(li + 1) % solution.Count];
                sumLength += Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            }
            return sumLength;
        }

        static double Objective(IReadOnlyList<Point> solution) 
            => LengthOfBoundary(solution);

        bool AllPointsIsInBoundary(IReadOnlyList<Point> solution) 
            => _points.All(t => !solution.Select((t1, li) => DistanceFromLine(t1, solution[(li + 1) % solution.Count], t)).Any(actDist => actDist > 0));
    }
}
