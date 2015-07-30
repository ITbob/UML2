using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication1
{
    class Liaison
    {
        Line line;

        public Line Line
        {
            get { return line; }
            set { line = value; }
        }

        public Liaison(Point p1, Point p2)
        {
            line = new Line
            {
                X1 = p1.X,
                Y1 = p1.Y,
                X2 = p2.X,
                Y2 = p2.Y,
                Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            };
        }
    }
}
