using GameEngine;
using GameEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GameEngine;
using System.Windows;
using System.Windows.Media.Imaging;
using GameEngine.Exceptions;
using TiledCS;
using System.Windows.Controls;
using OA_Game.AnimatedObjects.Enemies;
using System.ComponentModel;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace OA_Game.AnimatedObjects.Enemies
{
    public class Egg : Enemy, IInteractable
    {
        public bool DirectionLeft { get; set; } = false;
        public bool IsDying { get; set; } = false;
        public override int Damage { get; } = 1;
        public readonly TimeSpan CooldownTime = TimeSpan.FromMilliseconds(5000);


        public Egg(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            DirectionLeft = true;
            PlayableSequence eggMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_1.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_2.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_3.png")),
                new BitmapImage(Assets.GetUri("Images/FliegeVieh/Egg/Egg_4.png"))
            });
            eggMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_egg", eggMove);

            PlayableSequence eggDamage = new PlayableSequence(new ImageSource[]
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
            eggDamage.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("damage_egg", eggDamage);

            PlayableSequence eggDying = new PlayableSequence(new ImageSource[]
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
            eggDying.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            eggDying.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying_egg", eggDying);
        }       

        public void Attack(AnimatedObject obj)
        {
            if (IsDying)
                return;

            if (obj is Player player)
                player.GetDamage(Damage);
        }

        public void Die()
        {
            //wait 5sec than die
            IsDying = true;
            PlaySequenceAsync("dying_egg", DirectionLeft, false, true);
        }

        public void GetDamage(int damage)
        {
            if (IsDying)
                return;
            //5 sec warten
            if (damage > 0)
                PlaySequenceAsync("damage_egg", DirectionLeft, false, false);
        }
        /// <summary>
        /// cooldown before egg disapears after hitting the ground
        /// </summary>
        /// <param name="map"></param>
        private async void Cooledown(Map map)
        {
            await Task.Delay(CooldownTime);
            Die();
        }
        public void Move(Map map)
        {
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);


            if ((collidedWithWhat[1] == TileTypes.Ground))
            {
                GetDamage(2);
            }
            else
            {
                Velocity += Physics.Gravity;
                Position += Velocity;
            }
        }
    }
}
