using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FuncAnimationTest
{

    public class CircleCollection<T> : Collection<T> where T : DependencyObject, new()
    {
        public CircleCollection(int count)
        {
            while (count-- > 0)
                Add(new T());
        }
        
        public CircleCollection<T> WithProperty<U>(DependencyProperty property, Func<int, U> generator)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].SetValue(property, generator(i));
            }

            return this;
        }

        public CircleCollection<T> WithXY<U>(Func<int, U> xGen, Func<int, U> yGen)
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].SetValue(Canvas.LeftProperty, xGen(i));
                this[i].SetValue(Canvas.TopProperty, yGen(i));
            }

            return this;
        }
    }

    public class CircleDoubleAnimation : DoubleAnimation
    {
        public Func<double, double> ValueGenerator { get; set; }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
            => ValueGenerator(base.GetCurrentValueCore(defaultOriginValue, defaultDestinationValue, animationClock));

    }

    public class CircleDoubleAnimationCollection : Collection<CircleDoubleAnimation>
    {
        public CircleDoubleAnimationCollection(int count, Func<int, double> from, Func<int, double> to, Func<int, Duration> duration,
            Func<int, Func<double, double>> valueGenerator)
        {
            for (int i = 0; i < count; i++)
            {
                var cda = new CircleDoubleAnimation
                {
                    From = from(i),
                    To = to(i),
                    Duration = duration(i),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever,
                    ValueGenerator = valueGenerator(i)
                };

                Add(cda);
            }
        }

        public void BeginApplpyAnumation(UIElement[] targets, DependencyProperty property)
        {
            for (int i = 0; i < Count; i++)
                targets[i].BeginAnimation(property, Items[i]);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CircleCollection<Ellipse> _circles;

        public MainWindow()
        {
            InitializeComponent();

            const int count = 500;
            _circles = new CircleCollection<Ellipse>(count)
                .WithProperty(WidthProperty, i => 20.0)
                .WithProperty(HeightProperty, i => 20.0)
                .WithProperty(Shape.FillProperty,
                    i => new SolidColorBrush(Color.FromArgb(255, 
                        (byte) (255 - 2.5 * i),
                        (byte) (255 - 22.5 * i),
                        (byte) (255 - 12.5 * i))))
                .WithXY(x => 800 + Math.Pow(x, 1.0) * Math.Sin(x / 50.0 * Math.PI),
                    y => 500 + Math.Pow(y, 1.0) * Math.Cos(y / 50.0 * Math.PI));



            foreach (var ellipse in _circles)
            {
                DrawCanvas.Children.Add(ellipse);
            }
        }

        private void DrawCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var c = new CircleDoubleAnimationCollection(_circles.Count,
                x => 800 + Math.Pow(x, 1.0) * Math.Sin(x / 50.0 * Math.PI),
                x => 5 * x,
                x => new Duration(TimeSpan.FromSeconds(2)),
                x => j => 50.0 / j);

            c.BeginApplpyAnumation(_circles.Cast<UIElement>().ToArray(), Canvas.LeftProperty);
        }
    }
}
