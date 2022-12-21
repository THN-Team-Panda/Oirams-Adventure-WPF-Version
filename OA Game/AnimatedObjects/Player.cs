using System.Windows.Media;
using GameEngine.GameObjects;
using GameEngine;
using System;
using System.Windows.Media.Imaging;
using OA_Game.AnimatedObjects.Enemies;
using OA_Game.AnimatedObjects.Items;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using OA_Game.AnimatedObjects.Bullets;
using System.Media;

namespace OA_Game.AnimatedObjects
{
    /// <summary>
    /// Represent the main character O'iram.
    /// </summary>
    public class Player : AnimatedObject, IInteractable
    {
        /// <summary>
        /// Represent the extra Live.
        /// </summary>
        private bool hat = false;

        /// <summary>
        /// Represents if the player is in the fishishline
        /// </summary>
        public bool IsFinish = false;

        /// <summary>
        /// Max amount of munition the player can carry.
        /// </summary>
        public const int MaxMunition = 10;

        /// <summary>
        /// Is the amount of munition the player has to shoot.
        /// </summary>
        public int Munition { get; set; } = 0;

        /// <summary>
        /// min TimeSpan between two shots 
        /// </summary>
        public readonly TimeSpan CooldownTime = TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// show if the player is able to shoot (no munition check)
        /// </summary>
        public bool CanShoot
        {
            get
            {
                return canShootState;
            }
            set
            {
                if (value)
                {
                    canShootState = true;
                    return;
                }

                canShootState = false;
                Cooledown();
            }
        }
        /// <summary>
        /// Private canShootState
        /// do not chage this value
        /// </summary>
        private bool canShootState = true;

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
        /// Interrup every other action.
        /// </summary>
        public bool IsDying { get; set; } = false;

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
            HasHat = true;
            DirectionLeft = false;

            MediaPlayer playerWalk = new MediaPlayer();
            playerWalk.Open(Assets.GetUri("Sounds/Player/PlayerWalking.wav"));
            playerWalk.Volume = 0.5;

            PlayableSequence playerMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player3.png"))
            }, playerWalk);
            playerMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move", playerMove);

            PlayableSequence playerMoveCap = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap3.png"))
            }, playerWalk);
            playerMoveCap.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("moveCap", playerMoveCap);

            MediaPlayer soundJump = new MediaPlayer();
            soundJump.Open(Assets.GetUri("Sounds/Player/PlayerJump1.wav"));
            soundJump.Volume = 0.7;

            PlayableSequence playerJump = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Normal/Player2.png")),
            }, soundJump);
            playerJump.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("jump", playerJump);          

            PlayableSequence playerCapJump = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap_Jumping.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Movement/Cap/Player_Cap2.png")),
            }, soundJump);
            playerCapJump.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("jumpCap", playerCapJump);

            MediaPlayer soundAttack = new MediaPlayer();
            soundAttack.Open(Assets.GetUri("Sounds/Player/LongRangeAttack.wav"));

            PlayableSequence playerAttack = new PlayableSequence(new ImageSource[]
           {
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Normal/Player_Attack_Normal_5.png"))
           }, soundAttack);
            playerAttack.Between = TimeSpan.FromMilliseconds(40);
            this.AddSequence("attack", playerAttack);

            PlayableSequence playerCapAttack = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Attack/Cap/Player_Attack_Cap_5.png"))
            }, soundAttack);
            playerCapAttack.Between = TimeSpan.FromMilliseconds(40);
            this.AddSequence("attackCap", playerCapAttack);

            MediaPlayer soundPlayerDamage = new MediaPlayer();
            soundPlayerDamage.Open(Assets.GetUri("Sounds/Player/PlayerDamage.wav"));
            soundPlayerDamage.Volume = 1;

            PlayableSequence playerDamage = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_5.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_6.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Damage/Player_Damage_Cap_7.png"))
            }, soundPlayerDamage);
            playerDamage.Between = TimeSpan.FromMilliseconds(50);
            this.AddSequence("damage", playerDamage);

            MediaPlayer soundPlayerDead = new MediaPlayer();
            soundPlayerDead.Open(Assets.GetUri("Sounds/Player/PlayerDead.wav"));
            soundPlayerDead.Volume = 1;

            PlayableSequence playerDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_1.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_2.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_3.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_4.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_5.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_6.png")),
                new BitmapImage(Assets.GetUri("Images/Player/Dying/Normal/Player_Dying_Normal_7.png"))
            }, soundPlayerDead);

            playerDying.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            playerDying.Between = TimeSpan.FromMilliseconds(50);
            this.AddSequence("dying", playerDying);

        }

        /// <summary>
        /// Helper methode to play the sprite and set the player looking direction
        /// </summary>
        /// <param name="sequnece"></param>
        private void PlayPlayerSpriteMovement(string sequnece)
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
        public void Collect(Item obj)
        {
            if (obj.IsCollected)
                return;

            if (obj is Hat && HasHat == false)
            {
                HasHat = true;
                obj.Collect();
            }

            if (obj is Note && Munition < MaxMunition)
            {
                Munition += 1;
                obj.Collect();
            }
        }

        /// <summary>
        /// move player on map
        /// </summary>
        /// <param name="map"></param>
        public void Move(Map map)
        {
            if (IsDying)
                return;

            Velocity = Velocity with { X = Velocity.X * .9 };
            Velocity += Physics.Gravity;
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);

            if (collidedWithWhat.Contains(TileTypes.Obstacle))
                GetDamage(1);

            // Check if player can jump
            if (collidedWithWhat[0] == TileTypes.Ground) CanJump = true;
            else CanJump = false;

            // Apply velocity
            Position += Velocity;

            if (HasHat)
            {
                if (!CanJump)
                {
                    PlayPlayerSpriteMovement("jumpCap");
                }
                PlayPlayerSpriteMovement("moveCap");
            }
            else
            {
                if (!CanJump)
                {
                    PlayPlayerSpriteMovement("jump");
                }
                PlayPlayerSpriteMovement("move");
            }
        }

        public void Attack(AnimatedObject obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Shoot bullet
        /// </summary>
        public void Shoot(Map map)
        {
            if (IsDying || !CanShoot || Munition < 1)
                return;

            CanShoot = false;
            map.AddNotSpawnedObject(new NotSpawnedObject("Tone", "Bullet", Position));
            Munition--;
            if(HasHat)
            {
                PlaySequenceAsync("attack", DirectionLeft, true, true);
            }
            else
            {
                PlaySequenceAsync("attackCap", DirectionLeft, true, true);
            }
            
        }

        /// <summary>
        /// Kills player gameover
        /// </summary>
        public void Die()
        {
            IsDying = true;
            PlaySequenceAsync("dying", DirectionLeft, true, true);
        }

        /// <summary>
        /// decrese players live
        /// </summary>
        /// <param name="damage"></param>
        public void GetDamage(int damage)
        {
            if (Invincible)
                return;

            if (damage == 1 && !HasHat)
            {
                Die();
            }
            else if (damage == 1 && HasHat)
            {
                HasHat = false;

                PlaySequenceAsync("damage", DirectionLeft, true, true);
            }

            else if (damage >= 2)
            {
                Die();
            }

            Invincible = true;
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

        /// <summary>
        /// Helper method to reset the shoot cooldown
        /// </summary>
        private async void Cooledown()
        {
            if (CanShoot)
                return;

            await Task.Delay(CooldownTime);

            CanShoot = true;
        }
    }
}
