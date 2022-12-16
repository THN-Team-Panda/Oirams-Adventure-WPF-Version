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
        /// <summary>
        /// Default should be false, means direction: right
        /// </summary>
        public bool DirectionLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// State befor the object changes to ObjectIsTrash
        /// </summary>
        public bool IsDying { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Move the Object on the map object
        /// </summary>
        /// <param name="map">Map instance</param>

        /// <summary>
        /// property to check the damage output
        /// </summary>
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

        /// <summary>
        /// move finish so it is always right above the ground
        /// </summary>
        /// <param name="map"></param>
        public void Move(Map map)
        {
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);


            if (!(collidedWithWhat[1] == TileTypes.Ground || collidedWithWhat[2] == TileTypes.Ground || collidedWithWhat[3] == TileTypes.Ground || collidedWithWhat[0] == TileTypes.Ground))
            {
                Velocity += Physics.Gravity/2;
                Position += Velocity;
            }
        }

        /// <summary>
        /// if player touches finishline, game is finished
        /// </summary>
        /// <param name="obj"></param>
        public void Attack(AnimatedObject obj)
        {
            PlaySequenceAsync("animation_finish", false, false, true);
            if(obj is Player player)
                player.IsFinish = true;
        }

        /// <summary>
        /// Receive demage points
        /// </summary>
        /// <param name="damage">damage value</param>
        public void GetDamage(int damage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to execute die animations and stuff
        /// </summary>
        public void Die()
        {
            throw new NotImplementedException();
        }
    }
}
