using GameEngine;
using System.Windows.Media;

namespace OA_Game.Enemies
{
    /// <summary>
    /// FliegeVieh is an enemy that fly around and drops eggs.
    /// </summary>
    public class FliegeVieh : Enemie
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public override bool DirectionLeft { get; set; }

        public override void Move(Enemie obj, Map map)
        {
            throw new System.NotImplementedException();
        }

        public FliegeVieh(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }
    }
}
