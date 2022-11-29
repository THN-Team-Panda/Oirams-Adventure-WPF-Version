using GameEngine;
using GameEngine.GameObjects;
using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace OA_Game.Items
{
    /// <summary>
    /// Class for item Note. Is the munition to shoot for the player.
    /// </summary>
    public class Note : Items
    {
        public Note(int height, int width, ImageSource deafultSprite) : base(height, width)
        {
            PlayableSequence animation = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri(""))
            });
            
        }
        

    }
}
