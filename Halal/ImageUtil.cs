using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Halal
{
    public static class ImageUtil
    {
        public const string FileName = "result.jpg";

        static void ForEach(this IEnumerable<Point> source, Action<Point, int> action)
        {
            var index = 0;
            foreach (var item in source)
            {
                var current = index++;
                action(item, current);
            }
        }

        public static void SaveAsImage(IReadOnlyCollection<Point> points, IReadOnlyCollection<Point> solution, Point centroid, int circle)
        {
            var bitmap = new Bitmap(2000, 1000);
            var image = Image.FromHbitmap(bitmap.GetHbitmap());
            var graphics = Graphics.FromImage(image);

            solution.ForEach((x, i) =>
            {
                graphics.FillEllipse(Brushes.Green, x.X - 13, x.Y - 13, 26, 26);
                graphics.DrawLine(new Pen(Color.Green, 3), solution.Last(), solution.First());
                if(i < solution.Count - 1)
                    graphics.DrawLine(new Pen(Color.Green, 3), x, solution.ElementAt(i + 1));
            });

            graphics.FillEllipse(Brushes.Black, centroid.X - 7,
                centroid.Y - 7, 14, 14);
            graphics.DrawEllipse(Pens.Blue, centroid.X - circle,
                centroid.Y - circle, circle * 2, circle * 2);

            foreach(var point in points)
                graphics.FillEllipse(Brushes.Red, point.X - 7, point.Y - 7, 14, 14);

            image.Save(FileName, ImageFormat.Jpeg);
        }
    }
}
