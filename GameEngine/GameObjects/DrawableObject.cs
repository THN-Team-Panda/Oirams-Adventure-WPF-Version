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
        /// <summary>
        /// Determs if the garbage collector can remove this object
        /// </summary>
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
        private Vector position;

        public Vector Position
        {
            get => position;
            set
            {
                position = value;
                Canvas.SetTop(Rectangle, position.Y);
                Canvas.SetLeft(Rectangle, position.X);
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
            get => Rectangle.Height;
            protected set => Rectangle.Height = value;
        }

        /// <summary>
        /// Helper method to set/get the width
        /// </summary>

        public double Width
        {
            get => Rectangle.Width;
            protected set => Rectangle.Width = value;
        }

        /// <summary>
        /// Check if the rectangel is in a canvas
        /// </summary>
        public bool InCanvas => Rectangle.Parent is Canvas;

        /// <summary>
        /// Construct the element
        /// </summary>
        /// <param name="height">height of hitbox</param>
        /// <param name="width">width of hitbox</param>
        public DrawableObject(int height, int width)
        {
            Rectangle = new Rectangle
            {
                Height = height,
                Width = width,
                // Set default background
                Fill = new SolidColorBrush(Color.FromRgb(255, 192, 203))
            };
        }

        public DrawableObject(int height, int width, ImageSource image)
        {
            Rectangle = new Rectangle
            {
                Height = height,
                Width = width,
                Fill = new ImageBrush(image)
            };


        }

        /// <summary>
        /// Set a sprite as Image brush
        /// NOTE: Default is the direction right
        /// </summary>
        /// <param name="image">image source</param>
        /// <param name="directionLeft">direction</param>
        public void SetSprite(ImageSource image, bool directionLeft = false)
        {
            // To create a clean instance, create a new image brush
            ImageBrush brush = new ImageBrush(image);

            if (directionLeft)
            {
                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new ScaleTransform(-1, 1, 0.5, 0));
                transformGroup.Children. Add(new SkewTransform(-1, 0, 0.5, 0));
                brush.RelativeTransform = transformGroup;
            }

            Rectangle.Fill = brush;
        }
    }
}
