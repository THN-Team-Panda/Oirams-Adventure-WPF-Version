using GameEngine;
using GameEngine.GameObjects;
using System.Windows.Media;

namespace OA_Game.Enemies
{
    /// <summary>
    /// KonkeyDong is an enemy that jumps around his jukebox and try to hit the player if he comes close.
    /// </summary>
    public class KonkeyDong : Enemy, IInteractable
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 2;

        public bool DirectionLeft { get; set; }

        public bool IsDying { get; set; } = false;

        public KonkeyDong(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            // TODO
        }

        public void Attack(AnimatedObject obj)
        {
            // TODO
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
