using System.Collections.Generic;
using System.Drawing;

namespace Halal
{
    public interface ISmallestBoundary
    {
        void Init(List<Point> points, int polygon, int iterations);

        (IReadOnlyCollection<Point> points, IReadOnlyCollection<Point> solutionPoints, Point centroid, int maxDistance)
            Solve();
    }
}
