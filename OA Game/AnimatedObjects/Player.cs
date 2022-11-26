using System.Windows.Media;
using GameEngine.GameObjects;

namespace OA_Game
{
    /// <summary>
    /// Represent the main character O'iram.
    /// </summary>
    public class Player : AnimatedObject
    {
        /// <summary>
        /// If the player gets damage he is for a short time invincible (can't get damage).
        /// </summary>
        public bool Invincible { get; }

        /// <summary>
        /// Is the amount of munition the player has to shoot.
        /// </summary>
        public int Munition { get; set; } = 0;

        /// <summary>
        /// Represent the extra live.
        /// </summary>
        public bool HasHat { get; set; }

        /// <summary>
        /// Max amount of munition the player can carry.
        /// </summary>
        private const int MaxMunition = 10;

        /// <summary>
        /// Bool to Indicates if the Player can jump
        /// </summary>
        public bool CanJump { get; set; }

        public Player(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width, images, initSprite)
        {
        }

        /// <summary>
        /// If player die this method will end the game.  DELETE
        /// </summary>
        public void die()
        {
            //TODO
        }

        /// <summary>
        /// Checks if player can lose the hat if not he dies
        /// </summary>
        public void GetDamage()
        {
            //TODO code
            if (HasHat) HasHat = false;
            else die();
        }

        /// <summary>
        /// TODO Explain DELETE
        /// </summary>
        public void Move()
        {
            //TODO use class Keyboard
        }

        /// <summary>
        /// Check if its possible to collect game items DELETE
        /// </summary>
        /// <returns>return true if the item was successfully collected</returns>
        public bool CollectItem(int itemId)
        {
            switch (itemId)
            {
                case 0: // Hat
                    if (HasHat) return false;
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
