using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Pong {
    class Bar {


        private readonly SoundEffect soundEffect;
        private readonly Texture2D barTexture;

        private readonly int worldHeight;

        private readonly int xPosition;
        private int yPosition;
        private int previousYposition;

        public const int Height = 80;
        public const int Width = 10;

        private const int InitialMovementLength = 3;
        private const int MaxMovementLength = 10;
        private int movementLength = InitialMovementLength;

        public Bar(Texture2D texture, int xPosition, int worldHeight, SoundEffect soundEffect)
        {
            this.soundEffect = soundEffect;
            this.worldHeight = worldHeight;
            barTexture = texture;
            this.xPosition = xPosition - Width / 2;
            this.yPosition = worldHeight/2 - Height/2;

            this.previousYposition = this.yPosition;
        }

        public void IncreaseMovementSpeed()
        {
            if (movementLength > MaxMovementLength) {
                // Set some limit
                return;
            }
            movementLength = movementLength + InitialMovementLength;
        }

        public void ResetHorizontalMovementSpeed()
        {
            movementLength = InitialMovementLength;
        }

        public int GetXposition()
        {
            return xPosition;
        }

        public int GetYposition()
        {
            return yPosition;
        }

        public void MoveUp() {
            previousYposition = yPosition;
            if (yPosition > 0)
            {
                yPosition = yPosition - movementLength;
            }
            
        }

        public void MoveDown() {
            previousYposition = yPosition;
            if (yPosition < worldHeight - Height)
            {
                yPosition = yPosition + movementLength;
            }
        }

        public Texture2D GetTexture() {
            return barTexture;
        }

        public int GetPreviousYposition()
        {
            return previousYposition;
        }

        public int YCenterOfBar()
        {
            return yPosition + Height/2;
        }

        public void PlaySound() {
            soundEffect.Play();
        }
    }
}
