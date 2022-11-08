using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;

namespace GameEngine.GameObjects
{
    public class DrawableObject : GameObject
    {
        public Rectangle Rectangel
        {
            get; private set;
        }

        public double Height
        {
            get { return Rectangel.Height; }
            set { Rectangel.Height = value; }
        }

        public double Width
        {
            get { return Rectangel.Width; }
            set { Rectangel.Width = value; }
        }

        public bool InCanvas
        {
            get { return Rectangel.Parent is Canvas; }
        }

        public DrawableObject(Rectangle mapObject)
        {
            Rectangel = mapObject;
        }
    }
}
