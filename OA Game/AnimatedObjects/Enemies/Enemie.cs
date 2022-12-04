using System.Windows.Media;
using GameEngine.GameObjects;
using GameEngine;

namespace OA_Game.Enemies
{
    /// <summary>
    /// Class Enemies is the parent class for FliegeVieh, KonkeyDong, Skeleton. 
    /// </summary>
    public abstract class Enemie : AnimatedObject,IDirectable
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public abstract int Damage { get; }

        public abstract bool DirectionLeft { get; set; }

        public abstract void Move(Map map);

        public abstract void Attack();

        public bool Is_Attacking = false;

        public Enemie(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }
    }
}
