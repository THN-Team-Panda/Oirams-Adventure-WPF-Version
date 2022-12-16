using GameEngine;
using GameEngine.GameObjects;
using OA_Game.AnimatedObjects.Enemies;
using System;
using System.Numerics;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.AnimatedObjects.Enemies
{
    /// <summary>
    /// Class for item hat. Is the extra live for player.
    /// </summary>
    public class Finish : Enemy, IInteractable
    {
        public bool DirectionLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDying { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int Damage { get; } = 1;

        public Finish(int height, int width, ImageSource defaultSprite, Map map) : base(height, width, defaultSprite)
        {
            PlayableSequence finishAnimation = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Finish/Finish.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_1.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_2.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_3.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_4.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_5.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_6.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_7.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_8.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_9.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_10.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_11.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_12.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_13.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_14.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_15.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_16.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_17.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_18.png")),
                new BitmapImage(Assets.GetUri("Images/Finish/Finish_19.png"))

            });
            finishAnimation.Between = TimeSpan.FromMilliseconds(150);
            AddSequence("animation_finish", finishAnimation);
        }

        public void Move(Map map)
        {
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);


            if (!(collidedWithWhat[1] == TileTypes.Ground || collidedWithWhat[2] == TileTypes.Ground || collidedWithWhat[3] == TileTypes.Ground || collidedWithWhat[0] == TileTypes.Ground))
            {
                Velocity += Physics.Gravity/2;
                Position += Velocity;
            }
        }

        public void Attack(AnimatedObject obj)
        {
            PlaySequenceAsync("animation_finish", false, false, true);
            if(obj is Player player)
                player.Is_Finish = true;
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
