using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingSalesMan.GeneticAlgorithm
{
    public class TspGenetic : ITsp
    {
        static Random Random { get; } = new Random();
        List<Point> _points;
        int _iterations;

        const int Population = 100;
        const int Parents = 20;
        const int KeepIndividuals = 3;

        public void Init(List<Point> points, int iterations)
        {
            if (!points.Any())
                throw new ArgumentException(nameof(points));
            _points = points.ToList();
            _iterations = iterations;
        }

        public IEnumerable<Individual> Solve()
        {
            var population = Enumerable.Range(0, Population)
                .Select(x => new Individual { Genes = _points.Shuffle(Random).ToList() })
                .OrderBy(x => x.Fitness())
                .ToList();

            var best = population.First();

            for (var generation = 0; generation < _iterations; generation++)
            {
                var parents = population.Take(Parents).ToList();
                var newPopulation = population.Take(KeepIndividuals).ToList();

                for (var i = 0; i < Population - KeepIndividuals; i++)
                {
                    var p1Index = Random.Next(0, Parents);
                    var p1 = parents[p1Index];
                    var p2 = parents.Where((x,j) => j != p1Index).ElementAt(Random.Next(0, Parents - 1));

                    var separator = Random.Next(1, _points.Count - 1);
                    //cross
                    var child = new Individual
                    {
                        Genes = p1.Genes.Take(separator).Concat(p2.Genes.Skip(separator)).ToList()
                    };
                    //mutate
                    for (var j = 0; j < Random.Next(0, 5); j++)
                    {
                        var a = Random.Next(0, child.Genes.Count);
                        var b = Random.Next(0, child.Genes.Count);
                        var tmp = child.Genes[a];
                        child.Genes[a] = child.Genes[b];
                        child.Genes[b] = tmp;
                    }
                    newPopulation.Add(child);
                }

                population = newPopulation.OrderBy(x => x.Fitness()).ToList();
                if (best.Fitness() > population[0].Fitness())
                {
                    best = population[0];
                    yield return best;
                }
            }
        }
    }
}
