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
        public override bool IsCollected { get; set; } = false;


        public Hat(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            PlayableSequence hatAnimation = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Cap/Cap_2.png"))
            });
            hatAnimation.Between = TimeSpan.FromMilliseconds(150);
            AddSequence("animation_hat", hatAnimation);

            PlayableSequence hatCollect = new PlayableSequence(new ImageSource[]
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
            hatCollect.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            hatCollect.Between = TimeSpan.FromMilliseconds(150);
            AddSequence("collect_hat", hatCollect);

            EndlessLoopSequenceAsync("animation_hat", true);
        }

        public override void Collect()
        {
            IsCollected = true;
            PlaySequenceAsync("collect_hat", true, false, true);
        }

    }
}
