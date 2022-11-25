using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OA_Game.AnimatedObjects
{
    public class KonkeyDong : Enemies
    {
        public KonkeyDong(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }
    }
}
