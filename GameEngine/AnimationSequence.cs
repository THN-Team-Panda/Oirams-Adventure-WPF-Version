namespace GameEngine
{
    public struct AnimationSequence
    {
        private readonly int[] spriteOrder;

        public bool Loop = false;

        public TimeSpan Between = TimeSpan.FromMilliseconds(0);

        private int currentSprite = 0;

        public int CurrentSpriteNumber
        {
            get
            {
                if (currentSprite == -1)
                    return -1;

                int current = currentSprite;

                // Count up
                if (!Loop && currentSprite + 1 == spriteOrder.Length)
                    currentSprite = -1;
                else
                    currentSprite = currentSprite + 1 % spriteOrder.Length - 1;

                return spriteOrder[current];
            }
        }

        public AnimationSequence(int[] spritesOrder)
        {
            spriteOrder = spritesOrder;

        }
    }
}