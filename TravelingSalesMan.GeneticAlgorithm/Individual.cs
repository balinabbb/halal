using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TravelingSalesMan.GeneticAlgorithm
{
    public class Individual
    {
        public List<Point> Genes { get; set; }

        public double Fitness() =>
            Genes?.Zip(Genes.Skip(1), (x, y) => Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2))).Sum() ??
            throw new InvalidOperationException($"{nameof(Genes)} can not be null");

    }
}
