using GameEngine;
using GameEngine.GameObjects;
using System;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace OA_Game.AnimatedObjects.Enemies
{
    /// <summary>
    /// Skeleton is an enemy that walk around and try to hit the player if the player comes close to it.
    /// </summary>
    public class Skeleton : Enemy, IInteractable
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public bool DirectionLeft { get ; set ; }

        public bool IsDying { get; set; } = false;

        public Skeleton(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
          
            DirectionLeft =true;
            PlayableSequence skeletonMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_1.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_2.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_3.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_4.png"))
            });
            skeletonMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_skeleton", skeletonMove);

            PlayableSequence skeletonAttack = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_1.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_2.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_3.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_4.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_5.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_6.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_7.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Attack/Skeleton_Attacke_8.png"))
            });
            skeletonAttack.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("attack_skeleton", skeletonAttack);

            MediaPlayer soundSkeletondying = new MediaPlayer();
            soundSkeletondying.Open(Assets.GetUri("Sounds/Skeleton/Dying.wav"));
            soundSkeletondying.Volume = 0.2;

            PlayableSequence skeletonDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_1.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_2.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_3.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_4.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_5.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_6.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_7.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_8.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_9.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_10.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_11.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_12.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_13.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Dying/Skeleton_Dying_14.png"))

            }, soundSkeletondying);
            skeletonDying.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            skeletonDying.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying_skeleton", skeletonDying);
        }

        public void Attack(AnimatedObject obj)
        {
            if (IsDying)
                return;

            if (obj is Player player)
                player.GetDamage(Damage);

            PlaySequenceAsync("attack_skeleton", DirectionLeft, true, true);
        }

        public void Move(Map map)
        {
            if (IsDying)
                return;

            if (DirectionLeft)
            {
                Velocity = Velocity with { X = -1.4 };
                this.PlaySequenceAsync("move_skeleton", true, true);
            }
            else if (!DirectionLeft)
            {
                Velocity = Velocity with { X = +1.4 };
                this.PlaySequenceAsync("move_skeleton", false, true);
            }
            Velocity += Physics.Gravity;
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);
            if (collidedWithWhat[1] is TileTypes.Ground or TileTypes.Obstacle)
            {
                DirectionLeft = false;
                StopCurrentSequence();
            }
            else if (collidedWithWhat[3] is TileTypes.Ground or TileTypes.Obstacle)
            {
                DirectionLeft = true;
                StopCurrentSequence();
            }
            Position += Velocity;
        }

        public void GetDamage(int damage)
        {
            if (IsDying)
                return;

            if (damage > 0)
                Die();
        }
        public void Die()
        {
            IsDying = true;
            PlaySequenceAsync("dying_skeleton", DirectionLeft, false, true);
        }
    }
}
