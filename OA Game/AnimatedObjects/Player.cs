using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using GameEngine.GameObjects;

namespace OA_Game
{
    /// <summary>
    /// Represent the maincaracter O'iram
    /// </summary>
    public class Player : AnimatedObject
    {
        public bool invincible = false;
        public int munition = 0;
        public bool hat = false;
        private const int MAX_MUNITION = 10;
        public Player(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }

        /// <summary>
        /// If player die this Methode will end the game.
        /// </summary>
        public void die()
        {
            //TODO
        }

        /// <summary>
        /// checks if player can lose the hat if not he dies
        /// </summary>
        public void getDamage()
        {
            if (this.hat) this.hat = false;
            else die();
        }

        public void move()
        {
            //TODO use class Keyboard
        }

        /// <summary>
        /// Check if its possible to collect game items
        /// </summary>
        /// <returns>return true if the item was successfully collected</returns>
        public bool collectItem(int itemID)
        {
            switch (itemID)
            {
                case 0: // Hat
                    if(this.hat) return false;
                    else this.hat = true;
                    break;
                case 1: // Note
                    if (this.munition >= MAX_MUNITION) return false;
                    else this.munition++;
                    break;
            }
            return true;
        }

        public void shoot()
        {
            if (this.munition > 0)
            {
                this.munition--;
                //TODO führe schuss aus
            }
        }

        public void melee()
        {
            //TODO
        }
    }
}
