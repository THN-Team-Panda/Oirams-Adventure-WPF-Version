using GameEngine;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;

namespace OA_Game.Items
{
    /// <summary>
    /// Class for item hat. Is the extra live for player.
    /// </summary>
    public class Hat : Items
    {
        public Hat(int height, int width) : base(height, width)
        {
            PlayableSequence animation = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_5.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_6.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_7.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_6.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_5.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png"))
            });

            animation.Between = TimeSpan.FromMilliseconds(150);

            this.AddSequence("Animation", animation);
        }
    }
}
