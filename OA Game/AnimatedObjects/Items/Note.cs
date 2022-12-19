using GameEngine;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.AnimatedObjects.Items
{
    /// <summary>
    /// Class for item Note. Is the munition to shoot for the player.
    /// </summary>
    public class Note : Item
    {

        public override bool IsCollected { get; set; } = false;

        public Note(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            
            PlayableSequence noteAnimation = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Note/Note_1.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_2.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_3.png")),
            });
            noteAnimation.Between = TimeSpan.FromMilliseconds(150);
            AddSequence("animation_note", noteAnimation);

            MediaPlayer soundCollectAmmo = new MediaPlayer();
            soundCollectAmmo.Open(Assets.GetUri("Sounds/Player/CollectAmmo.wav"));
            soundCollectAmmo.Volume = 0.3;

            PlayableSequence noteCollect = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Note/Note_1.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_2.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_3.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_4.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_5.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_6.png"))
            }, soundCollectAmmo);

            noteCollect.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            noteCollect.Between = TimeSpan.FromMilliseconds(150);
            AddSequence("collect_note", noteCollect);

            EndlessLoopSequenceAsync("animation_note", true);

        }

        public override void Collect()
        {
            IsCollected = true;
            PlaySequenceAsync("collect_note", true, false, true);
        }
    }
}
