using GameEngine;
using GameEngine.GameObjects;
using System.Windows.Media;

namespace OA_Game.Enemies
{
    /// <summary>
    /// FliegeVieh is an enemy that fly around and drops eggs.
    /// </summary>
    public class FliegeVieh : Enemy, IInteractable
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public bool DirectionLeft { get; set; }

        public bool IsDying { get; set; } = false;

        public FliegeVieh(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            //TODO
        }

        public void Attack(AnimatedObject obj)
        {
            //TODO
        }

        public void Move(Map map)
        {
            // TODO
        }

        public void GetDamage(int damage)
        {
            // TODO
        }

        public void Die()
        {
            //TODO
        }
    }
}
