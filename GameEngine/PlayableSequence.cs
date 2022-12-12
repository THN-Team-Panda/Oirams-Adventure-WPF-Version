using System.Collections;
using System.Diagnostics;
using System.Windows.Media;

namespace GameEngine
{
    /// <summary>
    /// A information set for a playable sequence
    /// </summary>
    public class PlayableSequence : IEnumerable
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
        /// Sound Object
        /// </summary>
        public readonly MediaPlayer? SoundObject;

        /// <summary>
        /// Event collection
        /// </summary>
        public event SequenceEndEvent? SequenceFinished;

        /// <summary>
        /// Event template
        /// </summary>
        /// <param name="sender">Event sender objekt</param>
        public delegate void SequenceEndEvent(object sender);

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
        /// Instance a playable sequence
        /// Note: Must be played by an animated object
        /// Note: The sprites are sorted and the array starts with zero
        /// </summary>
        /// <param name="spriteOrder">A list of sprite numbers; at least two</param>
        /// <param name="soundFile">Uri to the sound file</param>
        /// <exception cref="ArgumentException">If the sequence is to small</exception>
        public PlayableSequence(ImageSource[] spriteOrder, MediaPlayer soundObj) : this(spriteOrder)
        {
            SoundObject = soundObj;

            if (SoundObject.NaturalDuration > Between * spriteOrder.Length)
                Debug.WriteLine("Sound Media is longer than the animation sequence.");
        }

        /// <summary>
        /// Get all sprites as a image source list
        /// </summary>
        /// <returns>list of image source</returns>
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < sprites.Length; i++)
                yield return sprites[i];
        }

        /// <summary>
        /// Calls all events. Helper function to make the events accessable from the outside.
        /// </summary>
        /// <param name="sender">Sender objekt</param>
        public void CallEvents(object sender) => SequenceFinished?.Invoke(sender);
    }
}