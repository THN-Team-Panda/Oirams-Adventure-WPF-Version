using System.Windows.Media;
using GameEngine.GameObjects;
using GameEngine;
using System;
using System.Windows.Media.Imaging;

namespace OA_Game
{
    /// <summary>
    /// Represent the main character O'iram.
    /// </summary>
    public class Player : AnimatedObject
    {
        /// <summary>
        /// If the player gets damage he is for a short time invincible (can't get damage).
        /// </summary>
        public bool Invincible { get; }

        /// <summary>
        /// Is the amount of munition the player has to shoot.
        /// </summary>
        public int Munition { get; set; } = 0;

        /// <summary>
        /// Represent the extra live.
        /// </summary>
        public bool HasHat { get; set; }

        /// <summary>
        /// Max amount of munition the player can carry.
        /// </summary>
        private const int MaxMunition = 10;

        /// <summary>
        /// Bool to Indicates if the Player can jump
        /// </summary>
        public bool CanJump { get; set; }

        public Player(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {

            PlayableSequence playerMove = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player1.png")),
                                                                                   new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player2.png")),
                                                                                   new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player3.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move", playerMove);

            PlayableSequence playerMoveCap = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap1.png")),
                                                                                      new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap2.png")),
                                                                                      new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap3.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("moveCap", playerMoveCap);

            PlayableSequence playerJump = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Jumping.png")),
                                                                                   new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player1.png")),
                                                                                   new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player2.png")),
                                                                                   new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player3.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("jump", playerJump);

            PlayableSequence playerCapJump = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Jumping.png")),
                                                                                      new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap1.png")),
                                                                                      new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap2.png")),
                                                                                      new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap3.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("jumpCap", playerCapJump);

            PlayableSequence playerAttack = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_1.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_2.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_3.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_4.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_5.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("attack", playerAttack);

            PlayableSequence playerCapAttack = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_1.png")),
                                                                                        new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_2.png")),
                                                                                        new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_3.png")),
                                                                                        new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_4.png")),
                                                                                        new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_5.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("attackCap", playerCapAttack);

            PlayableSequence playerDamage = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_1.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_2.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_3.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_4.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_5.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_6.png")),
                                                                                     new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_7.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("damage", playerDamage);

            PlayableSequence playerDying = new PlayableSequence(new ImageSource[] { new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_1.png")),
                                                                                    new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_2.png")),
                                                                                    new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_3.png")),
                                                                                    new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_4.png")),
                                                                                    new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_5.png")),
                                                                                    new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_6.png")),
                                                                                    new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_7.png"))});
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying", playerDying);



        }

    }
}
