using GameEngine;
using GameEngine.GameObjects;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;


namespace OA_Game.AnimatedObjects.Enemies
{
    public class Egg : Enemy, IInteractable
    {
        public bool DirectionLeft { get; set; } = false;

        public bool IsDying { get; set; } = false;

        public override int Damage { get; } = 1;

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
            IsDying = true;
            ObjectIsTrash = true;
        }

        public void GetDamage(int damage)
        {
            if (IsDying)
                return;
            //5 sec warten
            if (damage > 0)
                Die();
                
        }
        public void Move(Map map)
        {
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);


            if (!(collidedWithWhat[1] == TileTypes.Ground || collidedWithWhat[2] == TileTypes.Ground || collidedWithWhat[3] == TileTypes.Ground || collidedWithWhat[0] == TileTypes.Ground) )
            {
                Velocity += Physics.Gravity/2;
                Position += Velocity;
            }
            else
            {
                Vector Friedegg_Position = new Vector(Position.X - 10, Position.Y - (20 - Height));
                map.AddNotSpawnedObject(new NotSpawnedObject("FriedEgg", "Enemy", Friedegg_Position));
                GetDamage(1);
            }
        }
    }
}
