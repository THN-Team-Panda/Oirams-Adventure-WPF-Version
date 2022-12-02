using System.Windows.Media;

namespace OA_Game.Enemies
{
    /// <summary>
    /// FliegeVieh is an enemy that fly around and drops eggs.
    /// </summary>
    public class FliegeVieh : Enemies
    {
        private int damage = 1;
        public FliegeVieh(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }
    }
}
