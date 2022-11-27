using System.Windows.Media;

namespace OA_Game.Enemies
{
    /// <summary>
    /// FliegeVieh is an enemy that fly around and drops eggs.
    /// </summary>
    public class FliegeVieh : Enemies
    {
        public FliegeVieh(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }
    }
}
