using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AnimationDrawing.Classes
{
    public class Randomizer
    {
        private readonly Random _rnd = new Random();

        public Brush Brush()
        {
            Type brushesType = typeof(Brushes);
            PropertyInfo[] properties = brushesType.GetProperties();

            int random = _rnd.Next(properties.Length);
            return (Brush)properties[random].GetValue(null, null);
        }

        public IEasingFunction Easing()
        {
            EasingMode[] properties = {EasingMode.EaseIn, EasingMode.EaseInOut, EasingMode.EaseOut};

            int rnd = _rnd.Next(properties.Length);
            double e = _rnd.Next(1, 10);
            return new ExponentialEase {EasingMode = properties[rnd], Exponent = e};
        }

        public int Integer(int value) => _rnd.Next(value);

        public Duration Duration() => TimeSpan.FromSeconds(_rnd.Next(1, 5));
    }
}
