using System.Windows.Media;
using GameEngine.GameObjects;
using GameEngine;

namespace OA_Game.AnimatedObjects.Enemies
{
    /// <summary>
    /// Class Enemies is the parent class for FliegeVieh, KonkeyDong, Skeleton. 
    /// </summary>
    public abstract class Enemy : AnimatedObject
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public abstract int Damage { get; }

        public Enemy(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        { }
    }
}
