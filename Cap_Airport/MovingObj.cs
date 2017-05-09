using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cap_Airport
{
    public class MovingObj
    {
        private Position currentPosition;
        public Direction d = null;
        public int speed = 1;
        private int altitude;

        public MovingObj() {  }
        
        public MovingObj(Position p, Direction d, int altitude, int speed)
        {
            if (p != null)
                currentPosition = new Position(p);
            if (d != null)
                d = new Direction(d);
            this.altitude = altitude;
            this.speed = speed;
        }
    }
}
