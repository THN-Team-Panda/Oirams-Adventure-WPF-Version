using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameEngine;
using GameEngine.GameObjects;

namespace OA_Game.AnimatedObjects.Enemies
{
    public class FriedEgg : Enemy, IInteractable
    {
        public bool DirectionLeft { get; set; } = false;

        public bool IsDying { get; set; } = false;

        public override int Damage { get; } = 1;

        public readonly TimeSpan CooldownTime = TimeSpan.FromMilliseconds(8000);

        public FriedEgg(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            PlayableSequence friedeggDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_8.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_11.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_9.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_10.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_12.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_13.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_14.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_15.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_16.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_17.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_18.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_19.png"))

            });
            friedeggDying.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying_friedegg", friedeggDying);
            Cooledown();
        }

        public void Move(Map map)
        {
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);
            if (!(collidedWithWhat[1] == TileTypes.Ground || collidedWithWhat[2] == TileTypes.Ground || collidedWithWhat[3] == TileTypes.Ground || collidedWithWhat[0] == TileTypes.Ground))
            {
                Velocity += Physics.Gravity / 2;
                Position += Velocity;
            }
        }

        public void Attack(AnimatedObject obj)
        {
            if (IsDying)
                return;

            if (obj is Player player)
                player.GetDamage(Damage);
        }

        public void GetDamage(int damage)
        {
            return;
        }

        /// <summary>
        /// cooldown before egg disapears after hitting the ground
        /// </summary>
        /// <param name="map"></param>
        private async void Cooledown()
        {
            PlaySequenceAsync("dying_friedegg", true, false, false);
            await Task.Delay(CooldownTime);
            Die();
        }

        public void Die()
        {
            IsDying = true;
            ObjectIsTrash = true;
        }
    }
}
