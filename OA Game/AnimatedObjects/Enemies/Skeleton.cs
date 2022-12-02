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

        public Skeleton(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }
    }
}
