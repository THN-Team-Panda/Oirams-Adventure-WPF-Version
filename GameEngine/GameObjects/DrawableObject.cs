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
        public Rectangle MapObject
        {
            get; private set;
        }

        public double Height
        {
            get { return MapObject.Height; }
            set { MapObject.Height = value; }
        }

        public double Width
        {
            get { return MapObject.Width; }
            set { MapObject.Width = value; }
        }

        public bool InCanvas
        {
            get { return MapObject.Parent is Canvas; }
        }

        public DrawableObject(Rectangle mapObject)
        {
            MapObject = mapObject;
        }
    }
}
