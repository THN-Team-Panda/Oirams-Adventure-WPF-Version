using GameEngine;
using OA_Game.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.AnimatedObjects.Enemies
{
    internal class Boombox : Enemy
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public Boombox(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            PlayableSequence boombox = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_5.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_6.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_7.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_6.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_5.png"))
            });
            boombox.Between = TimeSpan.FromMilliseconds(150);
            AddSequence("animation_boombox", boombox);

            EndlessLoopSequenceAsync("animation_boombox", true);
        }
    }
}
