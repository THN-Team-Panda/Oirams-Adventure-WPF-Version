using GameEngine.GameObjects;

namespace OA_Game.Items
{
    /// <summary>
    /// TODO Explain
    /// </summary>
    public class Items : DrawableObject
    {
        /// <summary>
        /// shows if item is collected
        /// </summary>
        public bool Collected { get; set; }
        /// <summary>
        /// TODO Explain
        /// </summary>
        public bool Visible { get; set; } = true;

        public Items(int height, int width) : base(height, width)
        {
        }

        /// <summary>
        /// when the item is collected it is no longer visible on the map and cannot be collected a second time 
        /// </summary>
        /// <param name="item"></param>
        public void GetCollected(Items item)
        {
            item.Collected = true;
            item.Visible = false;
        }
    }
}
