using System.Windows;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// The Game Object is upper element in the game
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// Each object has a position vector!
        /// </summary>
        public Vector Position { get; set; }
    }
}