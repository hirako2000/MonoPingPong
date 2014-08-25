using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pong {
    class CpuController
    {

        private readonly Bar cpuBar;
        private readonly Ball ball;
        private readonly int worldHeight;
        private const int MovementLength = 5;

        private const int BarHeight = 80;

        private const int ProximityTolerance = 3;

        public CpuController(Bar cpuBar, Ball ball, int worldHeight)
        {
            this.cpuBar = cpuBar;
            this.ball = ball;
            this.worldHeight = worldHeight;
        }

        public void UpdatePosition()
        {
            MoveCloseToBall();
        }

        public void MoveCloseToBall()
        {
            if (Math.Abs(ball.GetYposition() - cpuBar.YCenterOfBar()) > ProximityTolerance) ;
            {
                MoveCloserTo(ball.GetYposition());
            }
     
        }

        private void AdjustToCenter()
        {
            if (!ball.IsGoingRight())
            {
                MoveCloserTo(worldHeight / 2);
            }
        }

        private void MoveCloserTo(int point)
        {
            var middleOfBar = cpuBar.GetYposition() + BarHeight/2;
            if (middleOfBar < point)
            {
                cpuBar.MoveDown();
            }
            if (middleOfBar > point)
            {
                cpuBar.MoveUp(); ;
            }
        }


    }
}
