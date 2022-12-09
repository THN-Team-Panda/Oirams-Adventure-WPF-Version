using GameEngine;
using GameEngine.GameObjects;
using OA_Game.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OA_Game.Bullets
{
    internal class Tone : Bullet, IInteractable
    {
        public Tone(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
            PlayableSequence toneAnimation = new PlayableSequence(new ImageSource[]
         {
                new BitmapImage(Assets.GetUri("Images/Note/Note_1.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_2.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_3.png")),
         });
            toneAnimation.Between = TimeSpan.FromMilliseconds(50);
            AddSequence("animation_tone", toneAnimation);

            PlayableSequence toneCollide = new PlayableSequence(new ImageSource[]
            {
                new BitmapImage(Assets.GetUri("Images/Note/Note_1.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_2.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_3.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_4.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_5.png")),
                new BitmapImage(Assets.GetUri("Images/Note/Note_6.png"))
            });

            toneCollide.SequenceFinished += (object sender) => { ObjectIsTrash = true; };
            toneCollide.Between = TimeSpan.FromMilliseconds(50);
            AddSequence("collide_tone", toneCollide);

            EndlessLoopSequenceAsync("animation_tone", true);
        }

        public bool DirectionLeft { get; set; }
        public bool IsDying { get; set; } = false;

        public void Attack(AnimatedObject obj)
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            IsDying = true;
            PlaySequenceAsync("collide_tone", DirectionLeft, false, true);
        }

        public void GetDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public void Move(Map map)
        {
            if (IsDying)
                return;

            if (DirectionLeft)
                Velocity = Velocity with { X = -3 };
            else
                Velocity = Velocity with { X = 3 };

            this.PlaySequenceAsync("animation_tone", DirectionLeft, true);
            TileTypes[] collidedWithWhat = Physics.IsCollidingWithMap(map, this);
            if (collidedWithWhat[1] == TileTypes.Ground || collidedWithWhat[3] == TileTypes.Ground)  // fliegt von rechts gegen wand || fliegt von links gegen Wand 
            {
                Die();
            }
            foreach (DrawableObject obj in map.SpawnedObjects)
            {
                if (obj is IInteractable && obj is Enemy)
                {
                    if (Physics.CheckCollisionBetweenGameObjects(this, obj))
                    {
                        ((IInteractable)obj).GetDamage(1);
                        Die();
                    }
                }
            }
            Position += Velocity;
        }
    }
}
