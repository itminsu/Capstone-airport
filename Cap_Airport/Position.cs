using System;

namespace Cap_Airport
{
    public class Position
    {
        public int x;
        public int y;

        public Position(Position p)
        {
            this.x = p.x;
            this.y = p.y;
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsCollision(Position pos)
        {
            if (pos == null) return false;
            return Math.Abs(pos.x - x) <= 1 && Math.Abs(pos.y - y) <= 1;
        }
    }
}