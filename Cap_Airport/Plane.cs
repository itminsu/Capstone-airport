using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cap_Airport
{
    public class Plane : MovingObj
    {
        //private Position position;
        private Point position;
        private Color color;
        public string planeID;
        private int gate;
        private string departureTime;
        public int arrivalTime, waitingTime;
        
        private bool isInService = false;
        private bool isWaiting = false;
        private bool isTakeoff = true;

        public Random randomGenerator;
        //public Plane (Position position, Direction d, int altitude, int speed, string planeID)
        //{
        //    this.planeID = planeID;
        //    if (altitude == 0)
        //    {
        //        isTakeoff = false;
        //    }
        //}
        public Plane(Point position, int gate, Color color, string ss)
        {
            this.position = position;
            this.gate = gate;
            this.color = color;
            this.planeID = ss;
        }
        public string getPlaneID()
        {
            return planeID;
        }
        //public string getRandomID()
        //{
        //    string[] possibleID = new string[]
        //    {
        //        "AC010"
                
        //    };
        //    return possibleID[randomGenerator.Next(possibleID.Length)];
        //}
        public Color getColor()
        {
            return this.color;
        }

        public int getGate()
        {
            return gate;
        }

        public int getPositionX()
        {
            return this.position.X;
        }

        public int getPositionY()
        {
            return this.position.Y;
        }

        public void setPosition(int x, int y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        public void setGate(int gate)
        {
            this.gate = gate;
        }

        public void movePlane(int xDelta, int yDelta)
        {
            this.position.X += xDelta;
            this.position.Y += yDelta;
        }
    }
}
