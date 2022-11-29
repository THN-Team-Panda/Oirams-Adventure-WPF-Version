using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using GameEngine.Exceptions;

namespace GameEngine.GameObjects
{
    /// <summary>
    /// Drawable object represents a visible element
    /// </summary>
    public class DrawableObject
    {
        public bool ObjectIsTrash = false;

        /// <summary>
        /// Element on map
        /// </summary>
        public Rectangle Rectangle
        {
            get; protected set;
        }

        /// <summary>
        /// Each object has a position vector!
        /// </summary>
        private Vector position;

        public Vector Position
        {
            get => position;
            set
            {
                position = value;
                Canvas.SetTop(Rectangle, position.Y);
                Canvas.SetLeft(Rectangle, position.X);
            }
        }

        /// <summary>
        /// Each object has a velocity vector in which the Object is moving
        /// </summary>
        public Vector Velocity { get; set; }

        /// <summary>
        /// Helper method to set/get the height
        /// </summary>
        public double Height
        {
            get => Rectangle.Height;
            protected set => Rectangle.Height = value;
        }

        /// <summary>
        /// Helper method to set/get the width
        /// </summary>

        public double Width
        {
            get => Rectangle.Width;
            protected set => Rectangle.Width = value;
        }

        /// <summary>
        /// Check if the rectangel is in a canvas
        /// </summary>
        public bool InCanvas => Rectangle.Parent is Canvas;

        /// <summary>
        /// Construct the element
        /// </summary>
        /// <param name="height">height of hitbox</param>
        /// <param name="width">width of hitbox</param>
        public DrawableObject(int height, int width)
        {
            Rectangle = new Rectangle
            {
                Height = height,
                Width = width,
                // Set default background
                Fill = new SolidColorBrush(Color.FromRgb(255, 192, 203))
            };
        }

        public DrawableObject(int height, int width, ImageSource image)
        {
            Rectangle = new Rectangle
            {
                Height = height,
                Width = width,
                Fill = new ImageBrush(image)
            };


        }

        /// <summary>
        /// A collection of playable sequences to animate drawable objects
        /// </summary>
        protected Dictionary<string, PlayableSequence> AnimationCollection = new();



        /// <summary>
        /// Default sprite
        /// </summary>
        protected ImageSource defaultSprite;

        /// <summary>
        /// Set a sprite as Image brush
        /// NOTE: Default is the direction right
        /// </summary>
        /// <param name="image">image source</param>
        /// <param name="directionLeft">direction</param>
        public void SetSprite(ImageSource image, bool directionLeft = false)
        {
            // To create a clean instance, create a new image brush
            ImageBrush brush = new ImageBrush(image);

            if (directionLeft)
            {
                TransformGroup transformGroup = new TransformGroup();
                transformGroup.Children.Add(new ScaleTransform(-1, 1, 0.5, 0));
                transformGroup.Children. Add(new SkewTransform(-1, 0, 0.5, 0));
                brush.RelativeTransform = transformGroup;
            }

            Rectangle.Fill = brush;
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
