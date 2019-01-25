using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AnimationDrawing.Classes
{
    public class Dot
    {
        public Ellipse Ellipse { get; set; }

        public DoubleAnimation DaX { get; set; }
        public DoubleAnimation DaY { get; set; }
        public DoubleAnimation DaZ { get; set; }

        public Point P1 { get; set; }
        public Point P2 { get; set; }

        private readonly Random _rnd = new Random();

        public Dot()
        {
            int radius = _rnd.Next(5, 10);
            Ellipse = new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = Brushes.Black
            };
        }

        public Dot(int radius) : this() =>
            Ellipse = new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = Brushes.Black
            };

        public Dot(Point position) : this()
        {
            P1 = position;
            P2 = position;
        }

        public Dot(Point position, int radius) : this(radius)
        {
            P1 = position;
            P2 = position;
        }

        public void GenerateDaX(Point point) => DaX = GenerateAxisMoving(point);

        public void GenerateDaY(Point point) => DaY = GenerateAxisMoving(point);

        private DoubleAnimation GenerateAxisMoving(Point endPoint) => new DoubleAnimation
        {
            From = endPoint.X,
            To = endPoint.Y,
            Duration = TimeSpan.FromSeconds(_rnd.Next(1, 5)),
            EasingFunction = Easing()
        };

        private IEasingFunction Easing()
        {
            EasingMode[] properties = { EasingMode.EaseIn, EasingMode.EaseInOut, EasingMode.EaseOut };

            int rnd = _rnd.Next(properties.Length);
            double e = _rnd.Next(1, 10);
            return new ExponentialEase { EasingMode = properties[rnd], Exponent = e };
        }
    }
}
