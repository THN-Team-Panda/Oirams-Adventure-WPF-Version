using GameEngine;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.Items
{
    /// <summary>
    /// Class for item Note. Is the munition to shoot for the player.
    /// </summary>
    public class Note : Item
    {
        public Note(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            PlayableSequence Note_Animation = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Note/Note_1.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_2.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_3.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_4.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_5.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_6.png"))
            });
            Note_Animation.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("animation_note", Note_Animation);

            PlayableSequence Note_Collect = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Note/Note_1.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_2.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_3.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_4.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_5.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_6.png"))
            });
            Note_Collect.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("collect_note", Note_Collect);
        }
    }
}
