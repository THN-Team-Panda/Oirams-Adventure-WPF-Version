using GameEngine;
using GameEngine.GameObjects;
using System.Windows.Media;
using System;
using System.Windows;

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

        public FliegeVieh(int height, int width, ImageSource defaultSprite, Map map, Vector anfangsposition) : base(height, width, defaultSprite)
        {
            DirectionLeft = true;
            PlayableSequence fliegeviehMove = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/FliegeVieh_1.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/FliegeVieh_2.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/FliegeVieh_3.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/FliegeVieh_4.png"))
            });
            fliegeviehMove.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("move_fliegevieh", fliegeviehMove);

            PlayableSequence fliegeviehDying = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_1.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_2.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_3.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_4.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_5.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_6.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_7.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_8.png")),
                new BitmapImage(Assets.GetUri("Images/Enemies/FliegeVieh/Dying/FliegeVieh_Dying_9.png")),

            });
            fliegeviehDying.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            fliegeviehDying.Between = TimeSpan.FromMilliseconds(150);
            this.AddSequence("dying_fliegevieh", fliegeviehDying);
            Anfangsposition = anfangsposition - 3;
        }

        public void Attack(AnimatedObject obj)
        {
            
            map.AddNotSpawnedObject(new NotSpawnedObject("Egg", "Bullet", Position));
        }

        public void Move(Map map)
        {
            if (IsDying)
                return;
            Velocity = Velocity with { X = +1.4 };
            Velocity = Velocity with { Y = 400 - ((Math.Cos((Position.X - Anfangsposition - 3) * (Position.X - Anfangsposition - 3)) - Math.Sin((Position.X - Anfangsposition - 3)))* 80 + 200) };
            Position += Velocity;
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
