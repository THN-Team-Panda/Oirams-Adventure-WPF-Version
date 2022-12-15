namespace GameEngine.GameObjects
{
    /// <summary>
    /// An interface for all directable game objects
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Default should be false, means direction: right
        /// </summary>
        public bool DirectionLeft { get; set; }

        /// <summary>
        /// State befor the object changes to ObjectIsTrash
        /// </summary>
        public bool IsDying { get; set; }

        /// <summary>
        /// Move the Object on the map object
        /// </summary>
        /// <param name="map">Map instance</param>
        public void Move(Map map);

        /// <summary>
        /// Attack another AnimatedObject
        /// </summary>
        /// <param name="obj">AnimatedObject instance</param>
        public void Attack(AnimatedObject obj);

        /// <summary>
        /// Receive demage points
        /// </summary>
        /// <param name="damage">damage value</param>
        public void GetDamage(int damage);

        /// <summary>
        /// Method to execute die animations and stuff
        /// </summary>
        public void Die();
    }
}
