using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TravelingSalesMan.SimulatedAnnealing
{
    public class TspSa : ITsp
    {
        static Random Random { get; } = new Random();
        List<Point> _points;
        int _iterations;

        const double Epsilon = 1000;

        public void Init(List<Point> points, int iterations)
        {
            if (!points.Any())
                throw new ArgumentException(nameof(points));
            _points = points.ToList();
            _iterations = iterations;
        }

        public IEnumerable<Individual> Solve()
        {

            var p = new Individual { Genes = _points.Shuffle(Random).ToList() };
            var best = p;
            var t = 1.0;
            const double boltzmann = 5.6704E-8;
            const double defaultTmp = Program.Iterations / 100_000_000_000_000.0 * 5;
            double TemperatureFunction()
            {
                return defaultTmp / t++;
            }

            Individual GetQ()
            {
                //return new Individual { Genes = p.Genes.Shuffle().ToList() };

                var rnd = Random.Next(1, p.Genes.Count-1);
                return new Individual {Genes = p.Genes.Take(rnd).Concat(p.Genes.Skip(rnd).Shuffle()).ToList() };
            }

            for (var generation = 0; generation < _iterations; generation++)
            {
                var q = default(Individual); //should be a random individual  : q ←−− { x ∈ S | ds(x, p) = eps}[rnd]
                while (q == null)
                {
                    q = GetQ();
                    if (Math.Abs(q.Fitness() - p.Fitness()) > Epsilon)
                        q = null;
                }
                var distance = q.Fitness() - p.Fitness();
                if (distance < 0)
                {
                    p = q;
                    if (p.Fitness() < best.Fitness())
                        best = p;
                }
                else
                {
                    var temperature = TemperatureFunction();

                    var asdf = distance / (boltzmann * temperature);
                    var propability = 1-Math.Pow(Math.E, -asdf);
                    if (Random.NextDouble() < propability)
                    {
                        p = q;
                        if (p.Fitness() < best.Fitness())
                            best = p;
                    }
                }
                yield return best;
            }
        }
    }
}
