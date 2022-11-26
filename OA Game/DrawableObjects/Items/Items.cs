using GameEngine.GameObjects;

namespace OA_Game.Items
{
    /// <summary>
    /// Parent class for items(hat and note).
    /// </summary>
    public class Items : DrawableObject
    {
        /// <summary>
        /// Shows if item is collected. If the item is collected it's going to be deleted.
        /// </summary>
        public bool Collected { get; set; }

        public Items(int height, int width) : base(height, width)
        {
        }
    }
}
