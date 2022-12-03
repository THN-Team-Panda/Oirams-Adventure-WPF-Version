using GameEngine.GameObjects;
using System.Windows.Media;

namespace OA_Game.Items
{
    /// <summary>
    /// Parent class for items(hat and note).
    /// </summary>
    public class Item : AnimatedObject
    {
        public Item(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }
    }
}
