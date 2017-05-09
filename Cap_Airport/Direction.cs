using System;

namespace Cap_Airport
{
    
    public class Direction
    {
        private int x;
        private int y;

        public Direction(Direction d) { x = d.x; y = d.y;  Normalize(); }

        public Direction(int xdir, int ydir)
        {
            this.x = xdir;
            this.y = ydir;
            Normalize();
        }

        public Direction(string a)
        {
            switch (a.ToUpper())
            {
                case "W":
                    this.x = -1;
                    this.y = 0;
                    break;
                case "S":
                    this.x = 0;
                    this.y = -1;
                    break;
                case "E":
                    this.x = 1;
                    this.y = 0;
                    break;
                case "N":
                    this.x = 0;
                    this.y = 1;
                    break;
                case "SW":
                    this.x = -1;
                    this.y = -1;
                    break;
                case "SE":
                    this.x = 1;
                    this.y = -1;
                    break;
                case "NE":
                    this.x = 1;
                    this.y = 1;
                    break;
                case "NW":
                    this.x = -1;
                    this.y = 1;
                    break;
            }
        }

        public Direction(Position from, Position to)
        {
            int dx, dy;
            dx = to.x - from.x;
            dy = to.y - from.y;
            // 0.4 ~= tan(pi/8)
            if (0.4 * Math.Abs(dx) > Math.Abs(dy)) dy = 0;
            if (0.4 * Math.Abs(dy) > Math.Abs(dx)) dx = 0;

            x = dx; y = dy;
            Normalize();
        }
        protected void Normalize()
        {
            if (x != 0) x = x > 0 ? 1 : -1;
            if (y != 0) y = y > 0 ? 1 : -1;
        }

        public string GetDirectionName()
        {
            string name ="";
            if (y != 0)
            {
                if (x == 0) name += y > 0 ? "S" : "N";
                else
                {
                    if (x > 0) name += y > 0 ? "NE" : "SE";
                    else name += y > 0 ? "NW" : "SW";
                }
            }
            else
            {
                if (x != 0)
                    name += x > 0 ? "E" : "W";
            }
            return name;
        }
    }
}