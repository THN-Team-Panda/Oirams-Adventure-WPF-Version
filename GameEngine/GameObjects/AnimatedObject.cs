using System.Windows.Shapes;
using System.Windows.Media;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// A drawable object with different sprites
    /// </summary>
    public class AnimatedObject : DrawableObject
    {
        /// <summary>
        /// List of selectable sprites
        /// </summary>
        protected ImageSource[] sprites;

        /// <summary>
        /// Helper method for the sprite array length
        /// </summary>
        public int SpriteCount
        {
            get { return sprites.Length; }
        }

        /// <summary>
        /// Get the current active sprite number
        /// </summary>
        public int CurrentSprite
        {
            get; private set;
        }

        /// <summary>
        /// Construct an animated drawable game object
        /// </summary>
        /// <param name="Rectangel">Object on map</param>
        /// <param name="images">list of sprite sources</param>
        /// <param name="initSprite">init sprite number (default: 0).</param>
        /// <exception cref="ArgumentOutOfRangeException">initSprite out of range</exception>
        public AnimatedObject(Rectangle Rectangel, ImageSource[] images, int initSprite = 0) : base(Rectangel)
        {
            if(initSprite >= images.Length)
                throw new ArgumentOutOfRangeException("Initial number out of range!");

            // Copy Image Source array into the image brush
            sprites = images;

            // Set the initSprite
            SetSprite(initSprite);  
        }

        /// <summary>
        /// Change the visible sprite of the object
        /// </summary>
        /// <param name="number">Sprite to select</param>
        /// <exception cref="ArgumentOutOfRangeException">number out of range</exception>
        public void SetSprite(int number)
        {
            if (number >= sprites.Length)
                throw new ArgumentOutOfRangeException("Number out of range!");

            CurrentSprite = number;

            // To create a clean instance, create a new image brush
            Rectangel.Fill = new ImageBrush(sprites[CurrentSprite]);
        }
    }
}
