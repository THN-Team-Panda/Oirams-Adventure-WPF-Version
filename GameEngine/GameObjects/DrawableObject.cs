using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// Drawable object represents a visible element
    /// </summary>
    public class DrawableObject 
    {
        public bool ObjectIsTrash = false;

        /// <summary>
        /// Element on map
        /// </summary>
        public Rectangle Rectangle
        {
            get; protected set;
        }

        /// <summary>
        /// Each object has a position vector!
        /// </summary>
        private Vector _position;

        public Vector Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                Canvas.SetTop(Rectangle,_position.Y);
                Canvas.SetLeft(Rectangle,_position.X);
            }
        }

        /// <summary>
        /// Each object has a velocity vector in which the Object is moving
        /// </summary>
        public Vector Velocity { get; set; }

        /// <summary>
        /// Helper method to set/get the height
        /// </summary>
        public double Height
        {
            get { return Rectangle.Height; }
            protected set { Rectangle.Height = value; }
        }

        /// <summary>
        /// Helper method to set/get the width
        /// </summary>

        public double Width
        {
            get { return Rectangle.Width; }
            protected set { Rectangle.Width = value; }
        }

        /// <summary>
        /// Check if the rectangel is in a canvas
        /// </summary>
        public bool InCanvas
        {
            get { return Rectangle.Parent is Canvas; }
        }

        /// <summary>
        /// Construct the element
        /// </summary>
        /// <param name="mapObject">Object reference on the map</param>
        public DrawableObject(int height, int width)
        {
            Rectangle = new Rectangle();
            Rectangle.Height = height;
            Rectangle.Width = width;

            // Set default background
            Rectangle.Fill = new SolidColorBrush(Color.FromRgb(255, 192, 203));
        }
    }
}
