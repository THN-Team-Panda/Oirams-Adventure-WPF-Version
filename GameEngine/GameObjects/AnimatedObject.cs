using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GameObjects
{
    public class AnimatedObject : DrawableObject
    {
        private ImageSource[] sprites;

        public int SpriteCount
        {
            get { return sprites.Length; }
        }

        public int CurrentSprite
        {
            get; private set;
        }

        public AnimatedObject(Rectangle Rectangel, ImageSource[] images, int initSprite = 0) : base(Rectangel)
        {
            if(initSprite >= images.Length)
                throw new ArgumentOutOfRangeException("Initial number out of range!");

            // Copy Image Source array into the image brush
            sprites = images;

            // Set the initSprite
            Rectangel.Fill = new ImageBrush(sprites[initSprite]);
        }

        public void SetSprite(int number)
        {
            if (number >= sprites.Length)
                throw new ArgumentOutOfRangeException("Number out of range!");

            CurrentSprite = number;

            Rectangel.Fill = new ImageBrush(sprites[CurrentSprite]);
        }
    }
}
