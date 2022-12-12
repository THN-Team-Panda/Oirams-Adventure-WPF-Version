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

        public bool IsDying { get; set; }

        public void Move(Map map);

        public void Attack(AnimatedObject obj);

        public void GetDamage(int damage);

        public void Die();
    }
}
