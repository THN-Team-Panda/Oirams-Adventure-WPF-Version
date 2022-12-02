using GameEngine;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.Items
{
    /// <summary>
    /// Class for item hat. Is the extra live for player.
    /// </summary>
    public class Hat : Item
    {
        public Hat(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            PlayableSequence Hat_Animation = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_5.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_6.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_7.png"))
            });
            Hat_Animation.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("animation_hat", Hat_Animation);

            PlayableSequence Hat_Collect = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_4.png"))
            });
            Hat_Collect.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("collect_hat", Hat_Collect);
        }
    }
}
