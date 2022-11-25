using System.Windows.Shapes;
using System.Windows.Media;
using GameEngine;
using GameEngine.Exceptions;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// A drawable object with different sprites
    /// </summary>
    public class AnimatedObject : DrawableObject
    {
        /// <summary>
        /// A collection of playable sequences to animate drawable objects
        /// </summary>
        protected Dictionary<string, PlayableSequence> animationCollection = new Dictionary<string, PlayableSequence>();

        /// <summary>
        /// List of selectable sprites
        /// </summary>
        protected ImageSource[] sprites;

        /// <summary>
        /// Helper method for the sprite array length
        /// </summary>
        public int SpriteCount
        {
            get { return sprites.Length; }
        }

        /// <summary>
        /// Get the current active sprite number
        /// </summary>
        public int CurrentSprite
        {
            get; private set;
        }

        /// <summary>
        /// Construct an animated drawable game object
        /// </summary>
        /// <param name="Rectangel">Object on map</param>
        /// <param name="images">list of sprite sources</param>
        /// <param name="initSprite">init sprite number (default: 0).</param>
        /// <exception cref="ArgumentOutOfRangeException">initSprite out of range</exception>
        public AnimatedObject(int height, int width, ImageSource[] images, int initSprite = 0) : base(height, width)
        {
            if(initSprite >= images.Length)
                throw new ArgumentOutOfRangeException("Initial number out of range!");

            // Copy Image Source array into the image brush
            sprites = images;

            // Set the initSprite
            SetSprite(initSprite);  
        }

        /// <summary>
        /// Change the visible sprite of the object
        /// </summary>
        /// <param name="number">Sprite to select</param>
        /// <exception cref="ArgumentOutOfRangeException">number out of range</exception>
        public void SetSprite(int number)
        {
            if (number >= sprites.Length || number < 0)
                throw new ArgumentOutOfRangeException("Number out of range!");

            CurrentSprite = number;

            // To create a clean instance, create a new image brush
            Rectangle.Fill = new ImageBrush(sprites[CurrentSprite]);
        }

        /// <summary>
        /// Add a playable sequence to the object
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <param name="sequence">Sequence object</param>
        public void AddSequence(string name, PlayableSequence sequence) => animationCollection.Add(name, sequence);

        /// <summary>
        /// Remove a playable sequence from the object
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public void RemoveSequence(string name)
        {
            if (!animationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            animationCollection.Remove(name);
        }

        /// <summary>
        /// Play a sequence one time
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public void PlaySequence(string name)
        {
            if (!animationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            PlayableSequence sequence = animationCollection[name];

            while(!sequence.EndOfSequence)
            {
                SetSprite(sequence.CurrentSpriteNumber);

                Thread.Sleep(sequence.Between);
            }
        }

        /// <summary>
        /// Play a sequence one time async in the background
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public async void PlaySequenceAsync(string name)
        {
            if (!animationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            PlayableSequence sequence = animationCollection[name];

            while (!sequence.EndOfSequence)
            {
                SetSprite(sequence.CurrentSpriteNumber);

                await Task.Delay(sequence.Between);
            }
        }
    }
}
