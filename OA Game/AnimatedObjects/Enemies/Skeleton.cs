using GameEngine;
using GameEngine.GameObjects;
using System;
using System.Numerics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.Enemies
{
    /// <summary>
    /// Skeleton is an enemy that walk around and try to hit the player if the player comes close to it.
    /// </summary>
    public class Skeleton : Enemie
    {
        /// <summary>
        /// property to check the damage output
        /// </summary>
        public override int Damage { get; } = 1;

        public override bool DirectionLeft { get ; set ; }

        public override void Move(Map map)
        {

            
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
            if (collidedWithWhat[1] == TileTypes.Ground)
            {
                DirectionLeft = false;
            }
            else if (collidedWithWhat[3] == TileTypes.Ground)
            {
                DirectionLeft = true;
            }
            Position += Velocity;
            

        }

        public Skeleton(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            DirectionLeft=true;
            PlayableSequence skeletonMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_1.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_2.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_3.png")),
                new BitmapImage(Assets.GetUri("Images/Skeleton/Movement/Skeleton_Movement_4.png"))
            });
            skeletonMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_skeleton", skeletonMove);
        }
    }
}
