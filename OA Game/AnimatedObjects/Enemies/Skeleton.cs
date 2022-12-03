using GameEngine;
using System.Numerics;
using System.Windows.Media;

namespace OA_Game.Enemies
{
    /// <summary>
    /// Skeleton is an enemy that walk around and try to hit the player if the player comes close to it.
    /// </summary>
    public class Skeleton : Enemie
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public override bool DirectionLeft { get ; set ; }

        public override void Move(Map map)
        {          
            Velocity = Velocity with { X = -1.4 };           
            Velocity += Physics.Gravity;
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);
            Position += Velocity;

        }

        public Skeleton(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }
    }
}
