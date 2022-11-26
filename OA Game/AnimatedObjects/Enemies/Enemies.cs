using System.Windows.Media;
using GameEngine.GameObjects;

namespace OA_Game.Enemies
{
    /// <summary>
    /// Class Enemies is the parent class for FliegeVieh, KonkeyDong, Skeleton. 
    /// </summary>
    public class Enemies : AnimatedObject
    {
        public Enemies(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }
    }
}
