using GameEngine;
using System.Windows;
using GameEngine.GameObjects;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEngine.Exceptions;
using TiledCS;
using System.Windows.Controls;
using System.Linq;


namespace OA_Game.AnimatedObjects.Enemies
{
    /// <summary>
    /// KonkeyDong is an enemy that jumps around his jukebox and try to hit the player if he comes close.
    /// </summary>
    public class KonkeyDong : Enemy, IInteractable
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 2;

        public bool DirectionLeft { get; set; }

        public bool IsDying { get; set; } = false;

        /// <summary>
        /// x-position of the boombox
        /// </summary>
        public double BoomboxArea;

        /// <summary>
        /// Bool to Indicates if the Player can jump
        /// </summary>
        public bool CanJump { get; set; }

        public KonkeyDong(int height, int width, ImageSource defaultSprite, Map map, Vector position) : base(height, width, defaultSprite)
        {           
            DirectionLeft = true;

            MediaPlayer soundWalk = new MediaPlayer();
            soundWalk.Open(Assets.GetUri("Sounds/KonkeyDong/KonkeyDongWalk.wav"));

            PlayableSequence donkeykongMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Movement/KonkeyDong.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Movement/KonkeyDong_Movement_1.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Movement/KonkeyDong_Movement_2.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Movement/KonkeyDong_Movement_2.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Movement/KonkeyDong_Movement_2.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Movement/KonkeyDong_Movement_2.png"))
            }, soundWalk);
            donkeykongMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_konkeydong", donkeykongMove);

            PlayableSequence konkeydongAttack = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_1.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_2.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_3.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_4.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_5.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_6.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_7.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_8.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Attack/KonkeyDong_Attack_9.png"))
            });
            konkeydongAttack.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("attack_konkeydong", konkeydongAttack);

            MediaPlayer soundGettingHit = new MediaPlayer();
            soundGettingHit.Open(Assets.GetUri("Sounds/KonkeyDong/KonkeyDongHit.wav"));

            PlayableSequence konkeydongDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_1.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_2.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_3.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_4.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_5.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_6.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_7.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_8.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_9.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Dying/KonkeyDong_Dying_10.png"))

            }, soundGettingHit);
            konkeydongDying.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            konkeydongDying.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying_konkeydong", konkeydongDying);
            Spawn_Boombox(map, position);
            
        }
        /// <summary>
        /// spawn boombox (enemy)
        /// </summary>
        public void Spawn_Boombox(Map map, Vector position)
        {                
            BoomboxArea = position.X;
            map.AddNotSpawnedObject(new NotSpawnedObject("Boombox", "Enemy", position));
        }

        /// <summary>
        /// attack player
        /// </summary>
        public void Attack(AnimatedObject obj)
        {
            if (IsDying)
                return;

            if (obj is Player player)
                player.GetDamage(Damage);

            PlaySequenceAsync("attack_konkeydong", DirectionLeft, true, true);
        }

        /// <summary>
        /// konkeydong jumps in the area of the boombox
        /// if konkeydong isnt in the area of his boombox, the next time he touches the ground, he changes his direction to the boombox
        /// </summary>
        public void Move(Map map)
        {
            if (IsDying)
                return;
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);
            Random rnd = new Random();
            if (DirectionLeft)
            {
                Velocity = Velocity with { X = -1.4 };
                if (collidedWithWhat[0] == TileTypes.Ground) 
                {                   
                    CanJump = false;
                    Velocity = Velocity with { Y = rnd.Next(-5, -2) };
                }               
                PlaySequenceAsync("move_konkeydong", true, true);
            }
            else if (!DirectionLeft)
            {
                Velocity = Velocity with { X = +1.4 };
                if (collidedWithWhat[0] == TileTypes.Ground) 
                {                   
                    CanJump = false;
                    Velocity = Velocity with { Y = rnd.Next(-5, -2) };
                }  
                PlaySequenceAsync("move_konkeydong", false, true);
            }
            Velocity += Physics.Gravity;
            
            if (collidedWithWhat[1] == TileTypes.Ground || Position.X < BoomboxArea - 8 * 16 && collidedWithWhat[0] == TileTypes.Ground)
            {
                DirectionLeft = false;
            }
            else if (collidedWithWhat[3] == TileTypes.Ground || Position.X > BoomboxArea + 8 * 16 && collidedWithWhat[0] == TileTypes.Ground)
            {
                DirectionLeft = true;
            }
            Position += Velocity;
        }

        /// <summary>
        /// animation for get damage and call die
        /// </summary>
        public void GetDamage(int damage)
        {
            if (IsDying)
                return;

            if (damage > 0)
                Die();
        }

        /// <summary>
        /// animation for dying
        /// </summary>
        public void Die()
        {
            IsDying = true;
            PlaySequenceAsync("dying_konkeydong", DirectionLeft, false, true);
        }
    }
}
