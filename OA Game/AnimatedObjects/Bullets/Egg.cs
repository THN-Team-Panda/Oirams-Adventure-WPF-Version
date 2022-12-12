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
        public Egg(int height, int width, ImageSource defaultSprite) : base(height, width, defaultSprite)
        {
        }

        public bool DirectionLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsDying { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Attack(AnimatedObject obj)
        {
            throw new NotImplementedException();
        }

        public void Die()
        {
            throw new NotImplementedException();
        }

        public void GetDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public void Move(Map map)
        {
            throw new NotImplementedException();
        }
    }
}
