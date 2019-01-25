using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Animation3D.Base
{
    public class Node
    {
        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public Line Line { get; set; }
        public DoubleAnimation PointADaX { get; set; }
        public DoubleAnimation PointADaY { get; set; }
        public DoubleAnimation PointBDaX { get; set; }
        public DoubleAnimation PointBDaY { get; set; }
    }
}
