using System.Windows.Media;
using GameEngine.GameObjects;
using GameEngine;
using System;
using System.Windows.Media.Imaging;
using OA_Game.Enemies;
using System.Threading.Tasks;

namespace OA_Game
{
    /// <summary>
    /// Represent the main character O'iram.
    /// </summary>
    public class Player : AnimatedObject, IDirectable
    {
        /// <summary>
        /// Represent the extra Live.
        /// </summary>
        private bool hat = false;

        /// <summary>
        /// Max amount of munition the player can carry.
        /// </summary>
        private const int MaxMunition = 10;

        /// <summary>
        /// TimeSpan Object how long the player invincible state should least
        /// </summary>
        public readonly TimeSpan InvincibleTime = TimeSpan.FromMilliseconds(2000);

        /// <summary>
        /// Private invincibleState
        /// Note: Do not change this value
        /// </summary>
        private bool invincibleState = false;

        /// <summary>
        /// If the player gets damage he is for a short time invincible (can't get damage).
        /// </summary>
        public bool Invincible
        {
            get
            {
                return invincibleState;
            }

            set
            {
                if (!value)
                    return;

                invincibleState = true;
                ResetInvincible();
            }
        }

        /// <summary>
        /// Is the amount of munition the player has to shoot.
        /// </summary>
        public int Munition { get; set; } = 0;

        /// <summary>
        /// Set the default image of player
        /// </summary>
        public bool HasHat
        {
            get { return hat; }
            set
            {
                if (value)
                {
                    this.defaultSprite = new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Standing.png"));
                }
                else
                {
                    this.defaultSprite = new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Standing.png"));
                }
                hat = value;
            }
        }



        /// <summary>
        /// Bool to Indicates if the Player can jump
        /// </summary>
        public bool CanJump { get; set; }

        /// <summary>
        /// Default should be false, means direction: right
        /// </summary>
        public bool DirectionLeft { get; set; }

        public Player(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            this.HasHat = false;
            DirectionLeft = false;

            PlayableSequence playerMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player3.png"))
            });
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move", playerMove);

            PlayableSequence playerMoveCap = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap3.png"))
            });
            playerMoveCap.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("moveCap", playerMoveCap);

            PlayableSequence playerJump = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player2.png")),
            });
            playerJump.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("jump", playerJump);

            PlayableSequence playerCapJump = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap2.png")),
            });
            playerCapJump.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("jumpCap", playerCapJump);

            PlayableSequence playerAttack = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_5.png"))
            });
            playerAttack.Between = TimeSpan.FromMilliseconds(40);
            this.AddSequence("attack", playerAttack);

            PlayableSequence playerCapAttack = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_5.png"))
            });
            playerCapAttack.Between = TimeSpan.FromMilliseconds(40);
            this.AddSequence("attackCap", playerCapAttack);

            PlayableSequence playerDamage = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_5.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_6.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_7.png"))
            });
            playerDamage.Between = TimeSpan.FromMilliseconds(50);
            this.AddSequence("damage", playerDamage);

            PlayableSequence playerDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_5.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_6.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_7.png"))
            });
            playerDying.Between = TimeSpan.FromMilliseconds(50);
            this.AddSequence("dying", playerDying);

        }

        /// <summary>
        /// Helper methode to play the sprite and set the player looking direction
        /// </summary>
        /// <param name="sequnece"></param>
        public void PlayPlayerSpriteMovement(string sequnece)
        {
            if (this.Velocity.Y > 0.01)  // jumping up
            {
                if (this.Velocity.X > 0.01) // looking right 
                {
                    this.DirectionLeft = false;
                    this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
                }
                else if (this.Velocity.X < -0.01) // looking left
                {
                    this.DirectionLeft = true;
                    this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
                }
                else // without x movement
                {
                    this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
                }
            }
            else if (this.Velocity.Y < -0.01) // falling down
            {
                if (this.Velocity.X > 0.01) // looking right
                {
                    this.DirectionLeft = false;
                    this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
                }
                else if (this.Velocity.X < -0.01) // looking left
                {
                    this.DirectionLeft = true;
                    this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
                }
                else // without x movement
                {
                    this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
                }
            }
            else if (this.Velocity.X > 0.01) // walking right
            {
                this.DirectionLeft = false;
                this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
            }
            else if (this.Velocity.X < -0.01) // walking left
            {
                this.DirectionLeft = true;
                this.PlaySequenceAsync(sequnece, this.DirectionLeft, true);
            }
        }

        /// <summary>
        /// checks if item is collectable and adds ammo if not full
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool Collect(Items.Item obj)
        {
            if (obj is Items.Hat && HasHat == false)
            {
                HasHat = true;
                return true;
            }

            if (obj is Items.Note)
            {
                if (Munition < MaxMunition)
                {
                    Munition += 1;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if player dies, else player only gets damage and returns false
        /// Note: If the player gets damage and doesn't die, set the invincible state
        /// </summary>
        /// <param name="enemie"></param>
        /// <returns></returns>
        public bool GetDamage(Enemie? enemie = null)
        {
            if (Invincible)
                return false;
            
            // Player got one damage
            if (enemie is null || enemie.Damage == 1)
            {
                if (HasHat)
                {
                    HasHat = false;
                }
                else return true;
            }

            // Player got two damage and is dead
            else if (enemie.Damage >= 2)
            {
                HasHat = false;
                return true;
            }

            Invincible = true;
            return false;
        }

        /// <summary>
        /// Helper method to reset the invincible state
        /// </summary>
        private async void ResetInvincible()
        {
            if (!Invincible)
                return;

            await Task.Delay(InvincibleTime);

            invincibleState = false;
        }
    }
}
