using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// Drawable object represents a visible element
    /// </summary>
    public class DrawableObject : GameObject
    {
        /// <summary>
        /// Element on map
        /// </summary>
        protected Rectangle Rectangel
        {
            get; private set;
        }

        /// <summary>
        /// Helper method to set/get the height
        /// </summary>
        public double Height
        {
            get { return Rectangel.Height; }
            protected set { Rectangel.Height = value; }
        }

        /// <summary>
        /// Helper method to set/get the width
        /// </summary>

        public double Width
        {
            get { return Rectangel.Width; }
            protected set { Rectangel.Width = value; }
        }

        /// <summary>
        /// Check if the rectangel is in a canvas
        /// </summary>
        public bool InCanvas
        {
            get { return Rectangel.Parent is Canvas; }
        }

        /// <summary>
        /// Construct the element
        /// </summary>
        /// <param name="mapObject">Object reference on the map</param>
        public DrawableObject(Rectangle mapObject)
        {
            Rectangel = mapObject;
        }
    }
}
