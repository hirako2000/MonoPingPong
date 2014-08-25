using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Pong {


    class Ball
    {

        private readonly SoundEffect soundEffect;

        private const int BarHeight = 80;
        private const int BarWidth = 10;
        private const int TopAndBottomPadding = 20;

        private readonly Texture2D texture;

        private readonly int worldHeight;
        private readonly int worldWidth;

        private int xPosition;
        private int yPosition;

        private int verticalMovement = 0;
        private int horizontalMovement = 5;

        private int previousXposition;

        public Ball(Texture2D texture, int xPosition, int worldWidth, int worldHeight, SoundEffect soundEffect)
        {
            this.soundEffect = soundEffect;
            this.texture = texture;
            this.xPosition = xPosition;
            this.yPosition = worldHeight / 2;
            this.worldHeight = worldHeight;
            this.worldWidth = worldWidth;

            this.previousXposition = xPosition;

        }

        public void IncreasetHorizontalMovement() {
            if (Math.Abs(horizontalMovement) > 10) {
                return;
            }

            if (horizontalMovement > 0) {
                horizontalMovement = horizontalMovement + 1;
            }
            else {
                horizontalMovement = horizontalMovement - 1;
            }
        }

        public void ResetHorizontalMovement()
        {
            horizontalMovement = 5;
        }

        public Texture2D GetTexture()
        {
            return texture;
        }

        public int GetXposition()
        {
            return xPosition;
        }

        public int GetYposition()
        {
            return yPosition;
        }

        public void Move()
        {
            previousXposition = xPosition;
            MoveVertically();
            MoveHorizontally();
        }

        public void MoveToCenter() {
            xPosition = this.yPosition = worldHeight / 2;
            BounceHorizontal();
            verticalMovement = 0;
        }

        private void MoveHorizontally()
        {
            if (xPosition > worldWidth || xPosition < 0)
            {
                BounceHorizontal();
            }

           xPosition = xPosition + horizontalMovement;
        }

        private void MoveVertically()
        {
            if (yPosition > worldHeight - TopAndBottomPadding || yPosition < 0 + TopAndBottomPadding)
            {
                BounceVertical();
                PlayEdgeSound();
            }
  

            yPosition = yPosition + verticalMovement;
        }

        public void CheckHit(Bar playerBar, Bar cpuBar)
        {
            if(IsCollision(playerBar))
            {
                Swing(playerBar);
                BounceHorizontal();
                playerBar.PlaySound();

                playerBar.IncreaseMovementSpeed();

            }

            if(IsCollision(cpuBar)) {
                Swing(cpuBar);
                BounceHorizontal();
                cpuBar.PlaySound();

                cpuBar.IncreaseMovementSpeed();
            }
        }

        private void Swing(Bar bar)
        {
            verticalMovement = OffCollision(bar);
            IncreasetHorizontalMovement();
        }

        private void PlayEdgeSound() {
            soundEffect.Play();
        }

        private Boolean IsCollision(Bar bar)
        {

            return IsXcollision(bar) && IsYcollision(bar);
        }

        private Boolean IsXcollision(Bar bar)
        {
            return xPosition <= bar.GetXposition() + BarWidth && xPosition >= bar.GetXposition();
        }

        private int OffCollision(Bar bar)
        {
            // That's some interesting bit, how should the ball bounce when colliding with the bar??
            // Let's make it simple: the further away from the middle of the bar, the least horizontally the ball will go 
            var diff = yPosition - bar.YCenterOfBar();
            var off = (diff) / 2;
            return off;
        }

        public Boolean IsYcollision(Bar bar) {
            return yPosition < bar.GetYposition() + BarHeight && yPosition > bar.GetYposition();
        }


        private void BounceVertical()
        {
            verticalMovement = verticalMovement * -1;
        }

        private void BounceHorizontal() {
            horizontalMovement = horizontalMovement * -1;
        }

        public Boolean IsGoingRight()
        {
            return previousXposition < xPosition;
        }
    }
}
