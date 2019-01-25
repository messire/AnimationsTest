using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnimationDrawing.Classes;

namespace AnimationDrawing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random _rnd = new Random();
        private static GeometryData GeoData { get; set; } = new GeometryData();
        
        List<Ellipse> _dotList = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
            TbDotInfo.Text = $" 0";

        }

        private void InitializeGeometry()
        {
            GeoData = new GeometryData
            {
                Width = (int) ActualWidth - 17,
                Height = (int) ActualHeight - 63
            };
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckNodeLimit(2);
            Point mPos = e.GetPosition(null);
            Dot dot = CreateDot(mPos);

            DrawDot(dot);
            AnimateMove(dot);

            if (DrawPanel.Children.Count < 2) return;

            List<Ellipse> dotList = new List<Ellipse>();
            foreach (var child in DrawPanel.Children)
            {
                if (child is Ellipse ellipse) dotList.Add(ellipse);
            }

            Point bPoint = new Point(Canvas.GetLeft(dotList[0]), Canvas.GetTop(dotList[0]));
            Point ePoint = new Point(Canvas.GetLeft(dotList[1]), Canvas.GetTop(dotList[1]));

            Line line = CreateLine(bPoint, ePoint);
            DrawLine(line);
            
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            ShowMousePosition(e);
            //_coords = new Point(e.GetPosition(null).X, e.GetPosition(null).Y);
            //CreateCircle();
            //CreateAnimation(_node);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeGeometry();
            ShowWindowInfo();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GeoData.Width = (int) ActualWidth - 27;
            GeoData.Height = (int) ActualHeight - 53;
            ShowWindowInfo();
        }
        
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearCanvas();
            ShowDotCount();
        }

        private Dot CreateDot(Point mPos) => new Dot(mPos);

        private Line CreateLine(Point bPoint, Point ePoint)
        {
            Line line = new Line
            {
                X1 = bPoint.X,
                X2 = ePoint.X,
                Y1 = bPoint.Y,
                Y2 = ePoint.Y,
                Stroke = Brushes.Black
            };
            return line;
        }

        private void DrawDot(Dot dot)
        {
            DrawPanel.Children.Add(dot.Ellipse);
            double axisX = dot.P1.X - dot.Ellipse.Width / 2;
            double axisY = dot.P1.Y - dot.Ellipse.Height / 2;

            Canvas.SetLeft(dot.Ellipse, axisX);
            Canvas.SetTop(dot.Ellipse, axisY);

            _dotList.Add(dot.Ellipse);

            ShowDotCount();
        }

        private void DrawLine(Line line)
        {
            DrawPanel.Children.Add(line);
        }

        private void AnimateMove(Dot dot)
        {
            AnimateX(dot);
            AnimateY(dot);
        }

        private void AnimateX(Dot dot)
        {
            Storyboard story = new Storyboard();

            dot.P1 = new Point(dot.P2.X, dot.P1.Y);
            dot.P2 = new Point(_rnd.Next(GeoData.Width), dot.P2.Y);
            var point = new Point(dot.P1.X, dot.P2.X);
            dot.GenerateDaX(point);

            dot.DaX.Completed += (sender, args) => AnimateX(dot);

            Storyboard.SetTarget(dot.DaX, dot.Ellipse);
            Storyboard.SetTargetProperty(dot.DaX, new PropertyPath(Canvas.LeftProperty));

            story.Children.Add(dot.DaX);
            story.Begin();
        }

        private void AnimateY(Dot dot)
        {
            Storyboard story = new Storyboard();

            dot.P1 = new Point(dot.P1.X, dot.P2.Y);
            dot.P2 = new Point(dot.P2.X, _rnd.Next(GeoData.Height));
            var point = new Point(dot.P1.Y, dot.P2.Y);
            dot.GenerateDaY(point);

            dot.DaY.Completed += (sender, args) => AnimateY(dot);

            Storyboard.SetTarget(dot.DaY, dot.Ellipse);
            Storyboard.SetTargetProperty(dot.DaY, new PropertyPath(Canvas.TopProperty));

            story.Children.Add(dot.DaY);
            story.Begin();
        }

        private void CheckNodeLimit(int limit)
        {
            if (DrawPanel.Children.Count >= limit) DrawPanel.Children.RemoveAt(0);
        }

        private void ClearCanvas() => DrawPanel.Children.Clear();

        #region ShowInfo

        private void ShowWindowInfo() => TbWindowInfo.Text = $" Width: {ActualWidth} | Height: {ActualHeight}";
        private void ShowMousePosition(MouseEventArgs e) => TbMouseInfo.Text = $" X: {e.GetPosition(this).X} | Y: {e.GetPosition(this).Y}";
        private void ShowDotCount() => TbDotInfo.Text = $" {DrawPanel.Children.Count}";

        #endregion
    }
}
