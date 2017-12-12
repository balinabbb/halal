using System.Collections.Generic;
using System.Drawing;

namespace TravelingSalesMan.SimulatedAnnealing
{
    public interface ITsp
    {
        void Init(List<Point> points, int iterations);
        IEnumerable<Individual> Solve();
    }
}
