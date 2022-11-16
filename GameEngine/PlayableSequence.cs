namespace GameEngine
{
    /// <summary>
    /// A informationset for a playable sequence
    /// </summary>
    public struct PlayableSequence
    {
        /// <summary>
        /// The sprites to play in the correct order
        /// </summary>
        private readonly int[] spriteOrder;

        /// <summary>
        /// Will be true if the sequence is fully played
        /// </summary>
        public bool EndOfSequence { get; private set; }

        /// <summary>
        /// Time between each sprite
        /// </summary>
        public TimeSpan Between = TimeSpan.FromMilliseconds(0);

        /// <summary>
        /// Stores the current sprite to play
        /// </summary>
        private int currentSprite = 0;

        /// <summary>
        /// Gets the current sprite to play and increment the counter
        /// </summary>
        public int CurrentSpriteNumber
        {
            get
            {
                // Reset the end of sequence 
                EndOfSequence = false;

                int current = currentSprite;

                // if the next sprite is the last, set end of sequence
                if (current + 1 == spriteOrder.Length)
                    EndOfSequence = true;

                    currentSprite %= spriteOrder.Length;

                // Return the current sprite
                return spriteOrder[current];
            }
        }

        /// <summary>
        /// Instance a playable sequence
        /// Note: Must be played by an animated object
        /// Note: The sprites are sorted and the array starts with zero
        /// </summary>
        /// <param name="spriteOrder">A list of sprite numbers; at least two</param>
        /// <exception cref="ArgumentException">If the sequence is to small</exception>
        public PlayableSequence(int[] spriteOrder)
        {
            if (spriteOrder.Length < 2)
                throw new ArgumentException("The sequence needs at least 2 playables.");

            EndOfSequence = false;

            this.spriteOrder = spriteOrder;
        }
    }
}