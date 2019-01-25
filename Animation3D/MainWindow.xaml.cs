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
using Animation3D.Base;


namespace Animation3D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random _rnd = new Random();
        private Guid _activeNode { get; set; }
        private static List<LineSource> _pairList { get; set; }
        private List<Dot> _dotList { get; set; }



        public MainWindow()
        {
            InitializeComponent();
            _dotList = new List<Dot>();
            _pairList = new List<LineSource>();
        }

        #region WindowEvent

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Line line = CreateLine(e.GetPosition(this));
            //DrawLine(line);
            Dot dot = new Dot
            {
                Ellipse = CreateEllipse()
            };

            dot.A = e.GetPosition(this);

            DrawDot(dot);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            ShowMousePosition(e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowWindowInfo();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ShowWindowInfo();
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearCanvas();
            ShowDotCount();
        }

        #endregion
        
        private Ellipse CreateEllipse()
        {
            int radius = _rnd.Next(5, 10);
            return new Ellipse
            {
                Width = radius,
                Height = radius,
                Fill = Brushes.Black
            };
        }

        private void DrawDot(Dot dot)
        {
            DrawPanel.Children.Add(dot.Ellipse);
            double axisX = dot.A.X - dot.Ellipse.Width / 2;
            double axisY = dot.A.Y - dot.Ellipse.Height / 2;

            Canvas.SetLeft(dot.Ellipse, axisX);
            Canvas.SetTop(dot.Ellipse, axisY);

            ShowDotCount();
        }

        private Line CreateLine(Point bPoint)
        {
            Line line = new Line
            {
                X1 = bPoint.X,
                X2 = _rnd.Next(100),
                Y1 = bPoint.Y,
                Y2 = _rnd.Next(100),
                Stroke = Brushes.Black
            };

            return line;
        }

        private void DrawLine(Line line) => DrawPanel.Children.Add(line);
        

        private void ClearCanvas() => DrawPanel.Children.Clear();

        #region Animation

        private void AnimateMove(Dot dot)
        {
            AnimateX(dot);
            AnimateY(dot);
        }

        private void AnimateX(Dot dot)
        {
            Storyboard story = new Storyboard();

            dot.A = new Point(dot.B.X, dot.A.Y);
            dot.B = new Point(_rnd.Next(GeoData.Width), dot.B.Y);
            var point = new Point(dot.A.X, dot.B.X);
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

            dot.A = new Point(dot.A.X, dot.B.Y);
            dot.B = new Point(dot.B.X, _rnd.Next(GeoData.Height));
            var point = new Point(dot.A.Y, dot.B.Y);
            dot.GenerateDaY(point);

            dot.DaY.Completed += (sender, args) => AnimateY(dot);

            Storyboard.SetTarget(dot.DaY, dot.Ellipse);
            Storyboard.SetTargetProperty(dot.DaY, new PropertyPath(Canvas.TopProperty));

            story.Children.Add(dot.DaY);
            story.Begin();
        }

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

        #endregion


        #region ShowInfo

        private void ShowWindowInfo() => TbWindowInfo.Text = $" Width: {ActualWidth} | Height: {ActualHeight}";
        private void ShowMousePosition(MouseEventArgs e) => TbMouseInfo.Text = $" X: {e.GetPosition(this).X} | Y: {e.GetPosition(this).Y}";
        private void ShowDotCount() => TbDotInfo.Text = $" {DrawPanel.Children.Count}";

        #endregion
    }
}
