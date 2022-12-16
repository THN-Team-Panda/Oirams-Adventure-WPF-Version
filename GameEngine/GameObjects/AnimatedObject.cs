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
        /// Contains a token from the current running session
        /// </summary>
        private long runningToken = -1;

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
        /// <param name="directionLeft">Sets the sprite rotation</param>
        /// <param name="lastIsDefault">Reset the sprite after running the sequence to the defualt sprite</param>
        /// <param name="interrupt">Interrupt the current running sequence</param>
        /// <param name="endSoundWithSequence">If set the sound ends with the sequence</param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public void PlaySequence(string name, bool directionLeft = false, bool lastIsDefault = false, bool interrupt = false, bool endSoundWithSequence = true)
        {
            if (!AnimationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();


            // If a session is running and interrupt is false, exit
            if (runningToken != -1 && !interrupt)
                return;

            long session = DateTime.Now.Ticks;

            // Make this session the current active session
            runningToken = session;

            PlayableSequence sequence = AnimationCollection[name];

            // If set, play the sound element
            if (sequence.SoundObject != null)
            {
                sequence.SoundObject.Position = TimeSpan.Zero;
                sequence.SoundObject.Play();
            }

            foreach (ImageSource image in sequence)
            {
                // Return is case that this is no longer the running session
                if (session != runningToken)
                {
                    if (endSoundWithSequence) sequence.SoundObject?.Stop();
                    return;
                }


                SetSprite(image, directionLeft);

                Thread.Sleep(sequence.Between);
            }

            // Return is case that this is no longer the running session
            if (session != runningToken)
            {
                if (endSoundWithSequence) sequence.SoundObject?.Stop();
                return;
            }

            if (lastIsDefault)
                SetSprite(defaultSprite, directionLeft);

            // If this is the active session reset the token
            if (session == runningToken)
                runningToken = -1;

            // If set end sound with sequence
            if (endSoundWithSequence)
                sequence.SoundObject?.Stop();


            // Call all events defined in the sequence
            sequence.CallEvents(this);
        }

        /// <summary>
        /// Play a sequence one time async in the background
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <param name="directionLeft">Sets the sprite rotation</param>
        /// <param name="lastIsDefault">Reset the sprite after running the sequence to the defualt sprite</param>
        /// <param name="interrupt">Interrupt the current running sequence</param>
        /// <param name="endSoundWithSequence">If set the sound ends with the sequence</param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public async void PlaySequenceAsync(string name, bool directionLeft = false, bool lastIsDefault = false, bool interrupt = false, bool endSoundWithSequence = true)
        {
            if (!AnimationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            // If a session is running and interrupt is false, exit
            if (runningToken != -1 && !interrupt)
                return;

            long session = DateTime.Now.Ticks;

            // Make this session the current active session
            runningToken = session;

            PlayableSequence sequence = AnimationCollection[name];

            // If set, play the sound element
            if (sequence.SoundObject != null)
            {
                sequence.SoundObject.Position = TimeSpan.Zero;
                sequence.SoundObject.Play();
            }

            foreach (ImageSource image in sequence)
            {
                // Return is case that this is no longer the running session
                if (session != runningToken)
                {
                    if (endSoundWithSequence) sequence.SoundObject?.Stop();
                    return;
                }

                SetSprite(image, directionLeft);

                await Task.Delay(sequence.Between);
            }

            // Return is case that this is no longer the running session
            if (session != runningToken)
            {
                if (endSoundWithSequence) sequence.SoundObject?.Stop();
                return;
            }

            if (lastIsDefault)
                SetSprite(defaultSprite, directionLeft);

            // If this is the active session reset the token
            if (session == runningToken)
                runningToken = -1;

            // If set end sound with sequence
            if (endSoundWithSequence) 
                sequence.SoundObject?.Stop();

            // Call all events defined in the sequence
            sequence.CallEvents(this);
        }

        /// <summary>
        /// Play a sequence endless in the background
        /// Note: This Method throws no sequence events
        /// </summary>
        /// <param name="name">Identifier of the sequence</param>
        /// <param name="directionLeft">Sets the sprite rotation</param>
        /// <exception cref="UnknownAnimationSequenceException">If the sequence name is unkown</exception>
        public async void EndlessLoopSequenceAsync(string name, bool directionLeft = false)
        {
            if (!AnimationCollection.ContainsKey(name))
                throw new UnknownAnimationSequenceException();

            if (runningToken != -1)
                return;

            long session = DateTime.Now.Ticks;

            // Make this session the current active session
            runningToken = session;

            PlayableSequence sequence = AnimationCollection[name];

            while (true)
            {
                // If set, play the sound element
                if (sequence.SoundObject != null)
                {
                    sequence.SoundObject.Position = TimeSpan.Zero;
                    sequence.SoundObject.Play();
                }

                foreach (ImageSource image in sequence)
                {
                    // Return is case that this is no longer the running session
                    if (session != runningToken)
                    {
                        sequence.SoundObject?.Stop();
                        return;
                    }

                    SetSprite(image, directionLeft);

                    await Task.Delay(sequence.Between);
                }

                if (session != runningToken)
                {
                    sequence.SoundObject?.Stop();
                    return;
                }
            }
        }

        /// <summary>
        /// Stop the current sequence from playing
        /// </summary>
        public void StopCurrentSequence() => runningToken = -1;
    }
}