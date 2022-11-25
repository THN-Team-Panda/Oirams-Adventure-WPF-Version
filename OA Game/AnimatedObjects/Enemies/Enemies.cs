using System.Windows.Media;
using GameEngine.GameObjects;

namespace OA_Game.Enemies
{
    /// <summary>
    /// TODO Explain
    /// </summary>
    public class Enemies : AnimatedObject
    {
        public Enemies(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }
    }
}
