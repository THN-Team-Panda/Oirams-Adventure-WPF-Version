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
        /// <summary>
        /// TODO Explain DELETE
        /// </summary>
        public bool Visible { get; set; } = true;

        public Items(int height, int width) : base(height, width)
        {
        }

        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="item"></param>
        public void GetCollected(Items item)
        {
            item.Collected = true;
            item.Visible = false;  //DELTE CODE LINE
        }
    }
}
