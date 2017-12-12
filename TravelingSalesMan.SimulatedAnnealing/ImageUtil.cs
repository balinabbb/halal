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

        public static void SaveAsImage(IReadOnlyCollection<Point> solution)
        {
            var bitmap = new Bitmap(2000, 1000);
            var image = Image.FromHbitmap(bitmap.GetHbitmap());
            var graphics = Graphics.FromImage(image);
            for (var i = 0; i < solution.Count - 1; i++)
            {
                graphics.DrawEllipse(new Pen(Brushes.Black, 3), solution.ElementAt(i).X - 7, solution.ElementAt(i).Y - 7, 14, 14);
                graphics.DrawLine(new Pen(Brushes.Red, 3),
                    new Point(solution.ElementAt(i).X, solution.ElementAt(i).Y),
                    new Point(solution.ElementAt(i + 1).X, solution.ElementAt(i + 1).Y));
            }
            graphics.DrawEllipse(new Pen(Brushes.Black, 3), solution.ElementAt(solution.Count - 1).X - 7, solution.ElementAt(solution.Count - 1).Y - 7, 14, 14);

            image.Save(FileName, ImageFormat.Jpeg);
        }
    }
}
