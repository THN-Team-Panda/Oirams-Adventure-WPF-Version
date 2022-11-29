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
                new BitmapImage(Assets.GetUri(""))
            });

            animation.Between = TimeSpan.FromMilliseconds(150);

            this.AddSequence("Animation", animation);
        }
    }
}
