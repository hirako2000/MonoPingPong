using System;
using Microsoft.Xna.Framework.Graphics;

namespace Pong {

    class Score {

        private int playerScore = 0;
        private int cpuScore = 0;
        private readonly SpriteFont font;

        private const int ScoreToWin = 9;

        public Score(SpriteFont font)
        {
            this.font = font;
        }

        public SpriteFont GetFont()
        {
            return font;
        }

        public void IncrementPlayerScore()
        {
            this.playerScore++;
        }

        public void IncrementCpuScore() {
            this.cpuScore++;
        }

        public int GetPlayerScore()
        {
            return this.playerScore;
        }

        public int GetCpuScore()
        {
            return this.cpuScore;
        }

        public Boolean IsGameFinished()
        {
            return this.playerScore >= ScoreToWin || this.cpuScore >= ScoreToWin;
        }

        public void Reset()
        {
            this.cpuScore = 0;
            this.playerScore = 0;
        }

        public Boolean PlayerWin()
        {
            return this.playerScore >= ScoreToWin;
        }

        public Boolean CpuWin() {
            return this.cpuScore >= ScoreToWin;
        }
    }
}
