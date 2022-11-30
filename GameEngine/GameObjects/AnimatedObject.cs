using System.Windows.Media;
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
        protected Dictionary<string, PlayableSequence> AnimationCollection = new();

        /// <summary>
        /// Default sprite
        /// </summary>
        protected ImageSource defaultSprite;

        /// <summary>
        /// When true you are unable to start a new async sequence 
        /// </summary>
        private bool playToken = false;

        /// <summary>
        /// Instance the animated gameobject
        /// </summary>
        /// <param name="height">height of the drawable object</param>
        /// <param name="width">width of the drawable object</param>
        /// <param name="defaultImage">the animated object needs a default sprite</param>
        public AnimatedObject(int height, int width, ImageSource defaultImage) : base(height, width, defaultImage)
        {
            this.defaultSprite = defaultImage;
        }

        /// <summary>
        /// Add a playable sequence to the object
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <param name="sequence">Sequence object</param>
        public void AddSequence(string name, PlayableSequence sequence) => AnimationCollection.Add(name, sequence);

        /// <summary>
        /// Remove a playable sequence from the object
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public void RemoveSequence(string name)
        {
            if (!AnimationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            AnimationCollection.Remove(name);
        }

        /// <summary>
        /// Play a sequence one time
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <param name="directionLeft"></param>
        /// <param name="lastIsDefault"></param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public void PlaySequence(string name, bool directionLeft = false, bool lastIsDefault = false)
        {
            if (!AnimationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            PlayableSequence sequence = AnimationCollection[name];

            foreach (ImageSource image in sequence)
            {
                SetSprite(image, directionLeft);

                Thread.Sleep(sequence.Between);
            }

            if (lastIsDefault)
                SetSprite(defaultSprite, directionLeft);

        }

        /// <summary>
        /// Play a sequence one time async in the background
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <param name="directionLeft"></param>
        /// <param name="lastIsDefault"></param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public async void PlaySequenceAsync(string name, bool directionLeft = false, bool lastIsDefault = false)
        {
            if (playToken)
                return;

            playToken = true;

            if (!AnimationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            PlayableSequence sequence = AnimationCollection[name];

            foreach (ImageSource image in sequence)
            {
                SetSprite(image, directionLeft);

                await Task.Delay(sequence.Between);
            }

            if (lastIsDefault)
                SetSprite(defaultSprite, directionLeft);

            playToken = false;
        }
    }
}
