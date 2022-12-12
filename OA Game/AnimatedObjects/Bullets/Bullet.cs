using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEngine;
using GameEngine.GameObjects;

namespace OA_Game.AnimatedObjects.Bullets
{
    public abstract class Bullet: AnimatedObject
    {
        public Bullet(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {      
        }
    }
}
