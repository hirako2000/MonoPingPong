#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
#endregion


namespace Pong {
    class BallOutMananger
    {

        private readonly SoundEffect soundEffect;

        private readonly Ball ball;
        private readonly int worldWidth;

        private const int ResetBallLatencyInFrames = 60;
        private const int LeftAndRightPadding = 10;

        private int latchDelay = 0;

        private readonly Score score;

        public BallOutMananger(Ball ball, int worldWidth, SoundEffect soundEffect, Score score)
        {
            this.score = score;
            this.ball = ball;
            this.worldWidth = worldWidth;
            this.soundEffect = soundEffect;
        }

        public void Move()
        {
            ball.Move();   
        }

        public void ResetBallAfterLatency()
        {
            if (latchDelay == 1) {
                ball.MoveToCenter();
            }
            else 
            {
                PlayBallOutSound();
                initDelay();
            }

            latchDelay--;
        }

        private void initDelay()
        {
            if (latchDelay == 0)
            {
                latchDelay = ResetBallLatencyInFrames;
            }
            
        }

        public Boolean IsBallOut()
        {
            if (IsOutLeft())
            {
                IncrementCpuScore();
                return true;

            }
            if (IsOutRight())
            {
                IncrementPlayerSCore();
                return true;
            }
            return false;
        }

        private void IncrementCpuScore()
        {
            if (latchDelay == 0)
            {
                score.IncrementCpuScore();
            }
        }

        private void IncrementPlayerSCore()
        {
            if (latchDelay == 0)
            {
                score.IncrementPlayerScore();
            }
        }

        private Boolean IsOutRight()
        {
            return ball.GetXposition() > worldWidth - LeftAndRightPadding;
        }

        private Boolean IsOutLeft()
        {
            return ball.GetXposition() < LeftAndRightPadding;

        }

        private void PlayBallOutSound()
        {
            if (latchDelay == 60 -1)
            {
                soundEffect.Play();
            }
            
        }

        private void PlayBallBackInSound()
        {
            
        }

        public Ball GetBall()
        {
            return ball;
        }

    }
}
