using System.Windows.Media;
using GameEngine.Exceptions;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// A drawable object with different sprites
    /// </summary>
    public class AnimatedObject : DrawableObject
    {


        /// <summary>
        /// When true you are unable to start a new async sequence 
        /// </summary>
        private bool playToken = false;

        /// <summary>
        /// Instance the animated gameobject
        /// </summary>
        /// <param name="height">height of the drawable object</param>
        /// <param name="width">width of the drawable object</param>
        /// <param name="defaultImage">the animated object needs a default sprite</param>
        public AnimatedObject(int height, int width, ImageSource defaultImage) : base(height, width, defaultImage)
        {
            this.defaultSprite = defaultImage;
        }

        
    }
}
