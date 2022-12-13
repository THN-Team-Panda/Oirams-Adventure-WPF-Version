using GameEngine;
using GameEngine.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OA_Game.AnimatedObjects.Bullets
{
    public class Egg : Bullet, IInteractable
    {
        public bool DirectionLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDying { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Egg(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            DirectionLeft = true;
            PlayableSequence eggMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_1.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_2.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_3.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_4.png"))
            });
            eggMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_egg", eggMove);

            PlayableSequence EggDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_8.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_11.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_9.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_10.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_12.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_13.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_14.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_15.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_16.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_17.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_18.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Egg/Egg_19.png"))

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
            PlaySequenceAsync("dying_konkeydong", DirectionLeft, false, true);
        }

        public void GetDamage(int damage)
        {
            if (IsDying)
                return;

            if (damage > 0)
                Die();
        }

        public void Move(Map map)
        {
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);
            Velocity += Physics.Gravity;
            Position += Velocity;
        }
    }
}
