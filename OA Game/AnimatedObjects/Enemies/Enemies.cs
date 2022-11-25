using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using GameEngine.GameObjects;

namespace OA_Game
{
    public class Enemies : AnimatedObject
    {
        public Enemies(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }
    }
}
