using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AnimationTest.Classes
{
    public class Node
    {
        public Node() => Guid = new Guid();
        public Node(int radius, Brush color) : this()
        {
            Ellipse = new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = color,
                Stroke = color,
                StrokeThickness = 1
            };
        }

        public Guid Guid { get; set; }
        public Ellipse Ellipse { get; set; }
        public DoubleAnimation DoubleAnimation { get; set; }

        public void NewAnimation(double start, double end)
        {
            DoubleAnimation = new DoubleAnimation
            {
                From = start,
                To = end,
                Duration = TimeSpan.FromSeconds(new Random().Next(4, 10)),
                EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseInOut, Exponent = 1 }
            };
        }
    }
}
