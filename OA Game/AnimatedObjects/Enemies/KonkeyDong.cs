using GameEngine;
using GameEngine.GameObjects;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.Enemies
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

        public KonkeyDong(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            DirectionLeft = true;
            PlayableSequence donkeykongMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_1.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_2.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_3.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_4.png"))
            });
            donkeykongMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_donkeykong", donkeykongMove);

            PlayableSequence konkeydongAttack = new PlayableSequence(new ImageSource[]
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
            konkeydongAttack.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("attack_konkeydong", konkeydongAttack);

            PlayableSequence konkeydongDying = new PlayableSequence(new ImageSource[]
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

            });
            konkeydongDying.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            konkeydongDying.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying_konkeydong", konkeydongDying);
            Spawn_Boombox();
        }
        /// <summary>
        /// spawn boombox (enemy)
        /// </summary>
        public void Spawn_Boombox()
        {

        }

        public void Attack(AnimatedObject obj)
        {
            if (IsDying)
                return;

            if (obj is Player player)
                player.GetDamage(Damage);

            PlaySequenceAsync("attack_konkeydong", DirectionLeft, true, true);
        }

        public void Move(Map map)
        {
            // TODO
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
            PlaySequenceAsync("dying_konkeydong", DirectionLeft, false, true);
        }
    }
}
