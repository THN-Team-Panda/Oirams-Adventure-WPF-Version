using System.Collections;
using System.Windows.Media;

namespace GameEngine
{
    /// <summary>
    /// A information set for a playable sequence
    /// </summary>
    public struct PlayableSequence : IEnumerable
    {
        /// <summary>
        /// The sprites to play in the correct order
        /// </summary>
        private readonly ImageSource[] sprites;

        /// <summary>
        /// Time between each sprite
        /// </summary>
        public TimeSpan Between = TimeSpan.FromMilliseconds(10);

        /// <summary>
        /// Instance a playable sequence
        /// Note: Must be played by an animated object
        /// Note: The sprites are sorted and the array starts with zero
        /// </summary>
        /// <param name="spriteOrder">A list of sprite numbers; at least two</param>
        /// <exception cref="ArgumentException">If the sequence is to small</exception>
        public PlayableSequence(ImageSource[] spriteOrder)
        {
            if (spriteOrder.Length < 2)
                throw new ArgumentException("The sequence needs at least 2 playables.");

            sprites = spriteOrder;
        }

        /// <summary>
        /// Get all sprites as a image source list
        /// </summary>
        /// <returns>list of image source</returns>
        public IEnumerator GetEnumerator()
        {
            for(int i = 0; i < sprites.Length; i++)
                yield return sprites[i];
        }
    }
}