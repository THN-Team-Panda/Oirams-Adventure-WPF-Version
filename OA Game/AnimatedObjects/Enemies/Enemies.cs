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
        public Enemies(Rectangle Rectangel, ImageSource[] images, int initSprite = 0) : base(Rectangel, images, initSprite)
        {
        }
    }
}
