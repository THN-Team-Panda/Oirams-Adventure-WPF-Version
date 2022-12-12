using GameEngine;
using OA_Game.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using GameEngine.GameObjects;
using System.Windows;
using System.Windows.Media.Imaging;
using GameEngine.Exceptions;
using TiledCS;


namespace OA_Game.AnimatedObjects.Enemies
{
    internal class Boombox : Enemy, IInteractable
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public bool DirectionLeft { get; set; }

        public bool IsDying { get; set; } = false;

        public Boombox(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            PlayableSequence boombox = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Boombox/Boombox_1.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Boombox/Boombox_2.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Boombox/Boombox_3.png")),
                new BitmapImage(Assets.GetUri("Images/KonkeyDong/Boombox/Boombox_4.png"))               
            });
            boombox.Between = TimeSpan.FromMilliseconds(150);
            AddSequence("animation_boombox", boombox);

            EndlessLoopSequenceAsync("animation_boombox", true);
        }
        public void Attack(AnimatedObject obj)
        {

            if (obj is Player player)
                player.GetDamage(Damage);

        }

        public void Move(Map map)
        {
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);
            
            
            if (!(collidedWithWhat[1] == TileTypes.Ground))
            {
                Velocity += Physics.Gravity;
                Position += Velocity;
            }
            
        }

        public void GetDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            throw new NotImplementedException();
        }
    }
}
