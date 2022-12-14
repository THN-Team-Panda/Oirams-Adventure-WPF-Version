using GameEngine;
using GameEngine.GameObjects;
using System.Windows.Media;
using System;
using System.Windows;
using GameEngine.GameObjects;
using System.Windows.Media.Imaging;
using GameEngine.Exceptions;
using TiledCS;
using System.Windows.Controls;
using System.Linq;
using System.Threading.Tasks;

namespace OA_Game.AnimatedObjects.Enemies
{
    /// <summary>
    /// FliegeVieh is an enemy that fly around and drops eggs.
    /// </summary>
    public class FliegeVieh : Enemy, IInteractable
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public bool DirectionLeft { get; set; }

        public bool IsDying { get; set; } = false;

        public double Anfangsposition;

        public int Eggs { get; set; } = 20;

        public readonly TimeSpan CooldownTime = TimeSpan.FromMilliseconds(3000);

        public bool CanLayEgg
        {
            get
            {
                return canLayEggState;
            }
            set
            {
                if (value)
                {
                    canLayEggState = true;
                    return;
                }

                canLayEggState = false;
                
            }
        }
        /// <summary>
        /// Private canShootState
        /// do not chage this value
        /// </summary>
        private bool canLayEggState = true;

        public FliegeVieh(int height, int width, ImageSource defaultSprite, Map map, Vector anfangsposition) : base(height, width, defaultSprite)
        {
            DirectionLeft = true;
            PlayableSequence fliegeviehMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/FliegeVieh_1.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/FliegeVieh_2.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/FliegeVieh_3.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/FliegeVieh_4.png"))
            });
            fliegeviehMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_fliegevieh", fliegeviehMove);

            PlayableSequence fliegeviehDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_1.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_2.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_3.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_4.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_5.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_6.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_7.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_8.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Dying/FliegeVieh_Dying_9.png")),

            });
            fliegeviehDying.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            fliegeviehDying.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying_fliegevieh", fliegeviehDying);
            Anfangsposition = anfangsposition.Y;
            Egg_Attack(map);
        }

        public void Attack(AnimatedObject obj)
        {
            if (IsDying)
                return;

            if (obj is Player player)
                player.GetDamage(Damage);

            PlaySequenceAsync("attack_fliegevieh", DirectionLeft, true, true);
            
        }

        public void Egg_Attack(Map map)
        {
            if (IsDying || !CanLayEgg || Eggs < 1)
                return;

            CanLayEgg = false;
            map.AddNotSpawnedObject(new NotSpawnedObject("Egg", "Enemy", Position));
            Eggs--;
            Cooledown(map);
        }

        private async void Cooledown(Map map)
        {
            if (CanLayEgg)
                return;

            await Task.Delay(CooldownTime);

            CanLayEgg = true;
            Egg_Attack(map);
        }

        public void Move(Map map)
        {
            if (IsDying)
                return;
            
            Velocity = Velocity with { X = -1.4 };            
            Position += Velocity;
            Position = Position with { Y = ((Math.Sin(Position.X/40)* 30) + Anfangsposition) };
            PlaySequenceAsync("move_fliegevieh", false, true);
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
            PlaySequenceAsync("dying_fliegevieh", DirectionLeft, false, true);
        }
    }
}
