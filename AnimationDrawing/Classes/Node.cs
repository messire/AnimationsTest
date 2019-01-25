using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace AnimationDrawing.Classes
{
    public class Node
    {
        public Guid Guid { get; set; }
        public Dot Dot { get; set; }
        public DoubleAnimation DaX { get; set; }
        public DoubleAnimation DaY { get; set; }
        public DoubleAnimation DaZ { get; set; }
        
        public Point P1 { get; set; }
        public Point P2 { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public Canvas Canvas { get; set; }

        public Node()
        {
            P1 = new Point();
            P2 = new Point();
        }

        public Node(Guid guid) : this() => Guid = guid;

        private readonly Random _rnd = new Random();




    }
}
