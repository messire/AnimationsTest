using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Animation3D.Base
{
    public class Dot
    {
        public Guid Guid { get; set; }
        public Ellipse Ellipse { get; set; }

        public Point A { get; set; }
        public Point B { get; set; }

        public Dot()
        {
            Guid = Guid.NewGuid();
        }
    }
}
