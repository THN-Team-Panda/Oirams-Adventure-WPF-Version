using System.Windows.Media;
using GameEngine.GameObjects;

namespace OA_Game
{
    /// <summary>
    /// Represent the main character O'iram
    /// </summary>
    public class Player : AnimatedObject
    {
        /// <summary>
        /// TODO Explain
        /// </summary>
        public bool Invincible { get; }

        /// <summary>
        /// TODO Explain
        /// </summary>
        public int Munition { get; set; } = 0;

        /// <summary>
        /// TODO Explain
        /// </summary>
        public bool HasHat { get; set; }

        /// <summary>
        /// TODO Explain
        /// </summary>
        private const int MaxMunition = 10;

        public Player(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }

        /// <summary>
        /// If player die this method will end the game.
        /// </summary>
        public void die()
        {
            //TODO
        }

        /// <summary>
        /// checks if player can lose the hat if not he dies
        /// </summary>
        public void GetDamage()
        {
            if (HasHat) HasHat= false;
            else die();
        }

        /// <summary>
        /// TODO Explain
        /// </summary>
        public void Move()
        {
            //TODO use class Keyboard
        }

        /// <summary>
        /// Check if its possible to collect game items
        /// </summary>
        /// <returns>return true if the item was successfully collected</returns>
        public bool CollectItem(int itemId)
        {
            switch (itemId)
            {
                case 0: // Hat
                    if(HasHat) return false;
                    HasHat = true;
                    break;
                case 1: // Note
                    if (Munition >= MaxMunition) return false;
                     Munition++;
                    break;
            }
            return true;
        }

        /// <summary>
        /// TODO Explain
        /// </summary>
        public void Shoot()
        {
            if (Munition > 0) Munition--;
                //TODO führe schuss aus
            
        }

        /// <summary>
        /// TODO Explain
        /// </summary>
        public void Melee()
        {
            //TODO
        }
    }
}
