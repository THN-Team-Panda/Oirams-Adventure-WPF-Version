using GameEngine.GameObjects;
using System.Windows.Media;

namespace OA_Game.Items
{
    /// <summary>
    /// Parent class for items(hat and note).
    /// </summary>
    public abstract class Item : AnimatedObject
    {
        public abstract bool IsCollected { get; set; }

        public Item(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }

        public abstract void Collect();
    }
}
