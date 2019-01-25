using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AnimationTest.Classes;

namespace AnimationTest
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Random _rnd = new Random();
        private static Node Node;


        delegate DoubleAnimation CreateDoubleAnimate(Point point);

        //private class DotParam
        //{
        //    public Point Bpoint { get; set; }
        //    public Point Epoint { get; set; }
        //    public Point Size { get; set; }

        //    public DotParam(Ellipse dot, Point p)
        //    {
        //        Random rnd = new Random();
        //        Bpoint = p;
        //        Epoint = new Point(rnd.Next(0, 800), rnd.Next(0, 450));

        //        bool flag = true;

        //        int l = _rnd.Next(5, 30);

        //        while (flag)
        //        {
        //            l = _rnd.Next(5, 30);
        //            if (dot.Width - l > 10 || l - dot.Width > 10) flag = false;
        //        }

        //        Size = new Point(dot.Width, l);
        //    }

        //    public void GenerateNewEndPoint(string pathProperty)
        //    {
        //        Random rnd = new Random();
        //        Bpoint = Epoint;
        //        Point tmp = Epoint;

        //        switch (pathProperty)
        //        {
        //            case "Canvas.LeftProperty":
        //                tmp.X = rnd.Next(0, 800);
        //                break;
        //            case "Canvas.TopProperty":
        //                tmp.Y = rnd.Next(0, 450);
        //                break;
        //        }

        //        Epoint = tmp;
        //    }
        //}

        private class PathParam
        {
            public double Bvalue { get; set; }
            public double Evalue { get; set; }
            public Duration Time { get; set; }
        }

        //public class NodeDoubleAnimation : DoubleAnimation
        //{
        //    public Func<double, double> ValueGenerator { get; set; }

        //    protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        //        => ValueGenerator(base.GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock));
        //}

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            //for (int i = 0; i < 10; i++)
            //{
            //    int x = _rnd.Next(0, 800);
            //    int y = _rnd.Next(0, 450);
            //    CreateDot(x, y);
            //}

            InitNode();
            //Node.NewAnimation(e.GetPosition(null).X, e.GetPosition(null).Y);

            //DotParam dp = new DotParam(new_node, e.GetPosition(null));

            //CreateAnimation(new_node, dp);
            //CreateNodeLink();

        }

        private void InitNode()
        {
            CreateEllipse(_rnd.Next(5, 30), Brushes.Black);
            CheckLimit(4);
            DrawPanel.Children.Add(Node.Ellipse);
        }


        private void CreateEllipse(int radius, Brush color) => Node = new Node(radius, color);

        private void CheckLimit(int limit)
        {
            if (DrawPanel.Children.Count >= limit) DrawPanel.Children.RemoveAt(0);
        }

        //private void CreateAnimation(Ellipse node, DotParam dp)
        //{
        //    AddAnimation(node, dp, "Canvas.LeftProperty");
        //    AddAnimation(node, dp, "Canvas.TopProperty");
        //   // AnimateSize(node);
        //}

        //private void AddAnimation(DotParam dp, string pathProperty)
        private void AddAnimation(string pathProperty)
        {
            Storyboard story = new Storyboard();
            CreateDoubleAnimate cda = GetAnimX;
            //    PathParam param = GetDoubleAnimation(dp, pathProperty, t);

            DoubleAnimation da = cda(new Point(_rnd.Next(0, 400), _rnd.Next(0, 400)));

            //    da.Completed += (sender, args) =>
            //    {
            //        dp.GenerateNewEndPoint(pathProperty);
            //        AddAnimation(obj, dp, pathProperty);
            //    };

                Storyboard.SetTarget(da, Node.Ellipse);
                //Storyboard.SetTargetProperty(da, GetProperty(pathProperty));

                story.Children.Add(da);
                story.Begin();
        }

        private DoubleAnimation GetAnimX(Point point) => new DoubleAnimation
        {
            From = point.X,
            To = point.Y,
            Duration = TimeSpan.FromSeconds(_rnd.Next(4, 10)),
            EasingFunction = new ExponentialEase {EasingMode = EasingMode.EaseInOut, Exponent = 1}
        };

        //private PathParam GetDoubleAnimation(DotParam dp, string pathProperty, Duration t)
        //{
        //    var result = new PathParam { Time = t };

        //    switch (pathProperty)
        //    {
        //        case "Canvas.LeftProperty":
        //            result.Bvalue = dp.Bpoint.X;
        //            result.Evalue = dp.Epoint.X;
        //            break;
        //        case "Canvas.TopProperty":
        //            result.Bvalue = dp.Bpoint.Y;
        //            result.Evalue = dp.Epoint.Y;
        //            break;
        //    }

        //    return result;
        //}

        ////private void AnimateMoving()
        ////{
        ////    Duration t = TimeSpan.FromSeconds(_rnd.Next(2, 5));
        ////    CreateDoubleAnimate cda = GetAnimX;
        ////    PathParam param = GetDoubleAnimation(dp, pathProperty, t);

        ////    DoubleAnimation exisX = cda(param);
        ////    DoubleAnimation exisY = cda(param);

        ////    da.Completed += (sender, args) =>
        ////    {
        ////        dp.GenerateNewEndPoint(pathProperty);
        ////        AddAnimation(obj, dp, pathProperty);
        ////    };

        ////    Storyboard.SetTarget(da, obj);
        ////    Storyboard.SetTargetProperty(da, GetProperty(pathProperty));

        ////    Storyboard story = new Storyboard();

        ////    story.Children.Add(da);
        ////    story.Begin();
        ////}

        //private void AnimateSize(Ellipse obj)
        //{
        //    Storyboard story = new Storyboard();
        //    CreateDoubleAnimate cda = GetAnimX;

        //    double beginValue = obj.Width;
        //    double endValue = NewSize(beginValue);
        //    Duration duration = TimeSpan.FromSeconds(_rnd.Next(2, 5));

        //    PathParam param = new PathParam {Bvalue = beginValue, Evalue = endValue, Time = duration};
        //    DoubleAnimation widthAnimation = cda(param);
        //    DoubleAnimation heightAnimation = cda(param);

        //    widthAnimation.Completed += (sender, args) => AnimateSize(obj);

        //    Storyboard.SetTarget(widthAnimation, obj);
        //    Storyboard.SetTarget(heightAnimation, obj);
        //    Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(WidthProperty));
        //    Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));
        //    story.Children = new TimelineCollection {widthAnimation, heightAnimation};

        //    story.Begin();
        //}

        //private void CreateNodeLink()
        //{
        //    if (DrawPanel.Children.Count > 1)
        //    {

        //    }
        //}



        //private PropertyPath GetProperty(string value)
        //{
        //    DependencyProperty result = Canvas.LeftProperty;
        //    switch (value)
        //    {
        //        case "Canvas.LeftProperty":
        //            result = Canvas.LeftProperty;
        //            break;
        //        case "Canvas.TopProperty":
        //            result = Canvas.TopProperty;
        //            break;
        //    }

        //    return new PropertyPath(result);
        //}

        //private double NewSize(double oldValue)
        //{
        //    while (true)
        //    {
        //        double result = _rnd.Next(5, 30);
        //        if (Math.Abs(oldValue - result) > 10) return result;
        //    }
        //}


    }
}
