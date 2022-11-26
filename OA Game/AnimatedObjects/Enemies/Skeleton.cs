using System.Windows.Media;

namespace OA_Game.Enemies
{
    /// <summary>
    /// Skeleton is an enemy that walk around and try to hit the player if the player comes close to it.
    /// </summary>
    public class Skeleton : Enemies
    {
        public Skeleton(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }
    }
}
