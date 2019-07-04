using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChaosGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class Dot
        {
            public Point point { get; set; }
            public int count { get; set; }

            public Dot(Point point, int count)
            {
                this.point = point;
                this.count = count;
            }
        }

        private double wHeight { get; set; }
        private double wWidth { get; set; }
        private Point pCenter { get; set; }
        private int vertexes { get; set; }
        private int divider { get; set; }

        private List<Dot> _pointList { get; set; }
        private readonly int _dotSize = 5;
        private readonly Random _rnd;

        #region chaos variables

        private int _minVertex = 8;
        private int _maxVertex = 16;
        private int _iterationCount = 10000;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            pCenter = new Point();
            _pointList = new List<Dot>();
            _rnd = new Random();
            ChaosInfo.Text = "Поехали!";
        }

        #region Control handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadVariables();
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearDeck();
            InitPolygon();
            Point pick = CalibratePick(e.GetPosition(this));
            DrawClickPoint(pick);
            DoChaos(pick);
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearDeck();
        }

        #endregion

        private void DoChaos(Point pick)
        {
            for (int i = 0; i < _iterationCount; i++)
                pick = Iterate(pick);

            string message = $"Vertex count : {vertexes} ";
            //_pointList.ForEach(p => { message += $"|  {_pointList.IndexOf(p) + 1} : {p.count} "; });
            ChaosInfo.Text = message;
        }

        private void InitPolygon()
        {
            FillingPointList();
            _pointList.ForEach(p => DrawVertex(p.point));
        }

        private Point Iterate(Point pos)
        {
            int index = _rnd.Next(0, vertexes);
            _pointList[index].count++;
            Point z = _pointList[index].point;

            pos = new Point(GetCoordValue(z.X,pos.X), GetCoordValue(z.Y, pos.Y));
            DrawChaosDot(pos);

            return pos;
        }

        #region Drawing

        private void DrawChaosDot(Point coord) => PutDotToCanvas(CreateDot(Brushes.Black), coord);

        private void DrawClickPoint(Point coord) => PutDotToCanvas(CreateDot(Brushes.Green), coord);

        private void DrawVertex(Point coord) => PutDotToCanvas(CreateDot(Brushes.Red), coord);

        private void PutDotToCanvas(Ellipse dot, Point coord)
        {
            DrawPanel.Children.Add(dot);
            Canvas.SetLeft(dot, coord.X);
            Canvas.SetTop(dot, coord.Y);
        }

        #endregion

        #region Helpers

        private Point CalibratePick(Point pick) => new Point(pick.X - _dotSize / 2, pick.Y - _dotSize / 2);

        private void ClearDeck()
        {
            DrawPanel.Children.Clear();
            _pointList = new List<Dot>();
            ReloadVariables();
        }

        private Ellipse CreateDot(Brush dotColor) => new Ellipse {Height = _dotSize, Width = _dotSize, Fill = dotColor};

        private void FillingPointList()
        {
            for (int i = 0; i < vertexes; i++)
            {
                double x = pCenter.X + (pCenter.Y - 30) * Math.Sin(i * 2 * Math.PI / vertexes);
                double y = pCenter.Y + (pCenter.Y - 30) * Math.Cos(i * 2 * Math.PI / vertexes);

                _pointList.Add(new Dot(new Point(x, y), 0));
            }
        }

        private double GetCoordValue(double vertex, double dot) => vertex > dot ? vertex - (vertex - dot) / divider : vertex + (dot - vertex) / divider;

        private void ReloadVariables()
        {
            wHeight = ActualHeight - 41;
            wWidth = ActualWidth - 41;
            pCenter = new Point(wWidth / 2, wHeight / 2);
            vertexes = _rnd.Next(_minVertex, _maxVertex);
            divider = vertexes - 1;
        }

        #endregion
    }
}
