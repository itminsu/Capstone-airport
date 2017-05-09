using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Cap_Airport
{
    public class ArrivalQueue
    {
        public int id;
        public Queue<int> planeInLine = new Queue<int>();
        public int maxPlaneInQueue = 8;
        public int totalPlaneArrival = 0;

        public bool isOpen = true;
        public int numberOfPlaneInQueue = 0;
        public int totalWaitTime = 0;
        public List<Plane> Planes = new List<Plane>();
        //public int arrivalWidth = 75;
        //public int arrivalHeight = 200;
        //public int arrivalX = 0;
        //public Rectangle arrivalRectangle;
        public ArrivalQueue() { }

        public ArrivalQueue(int tempId, List<Plane> tempPlanes)
        {
            id = tempId;
            Planes = tempPlanes;
        }

        //public Rectangle DrawArrival(PaintEventArgs e, int xOffset, int arrivalY)
        //{
        //    Graphics g = e.Graphics;

        //    arrivalX = xOffset;

        //    SolidBrush aBrush = new SolidBrush(Color.Navy);
        //    arrivalRectangle = new Rectangle(arrivalX, arrivalY, arrivalWidth, arrivalHeight);
        //    g.FillRectangle(aBrush, arrivalRectangle);

        //    Font aFont = new Font(SystemFonts.DefaultFont.FontFamily, 16, FontStyle.Regular);
        //    g.DrawString(id.ToString(), aFont, Brushes.White, xOffset + (arrivalWidth / 2) - 8, arrivalY + 89);

        //    SolidBrush laneStatusBrush = new SolidBrush(isOpen == true ? Color.Green : Color.Red);
        //    g.FillEllipse(laneStatusBrush, xOffset + (arrivalWidth / 2) - 8, arrivalY + 150, 20, 20);

        //    return arrivalRectangle;
        //}

        public void AddPlaneToQueue(int newPlaneId)
        {
            planeInLine.Enqueue(newPlaneId);
            ++numberOfPlaneInQueue;
            Console.WriteLine("--" + Planes[newPlaneId].getPlaneID() + " has entered arrival line " + id);
        }

        public int OutOfArrivalQueue(int currentTick)
        {
            int PlaneId = planeInLine.Peek();
            Plane tempPlane = Planes[PlaneId];
            planeInLine.Dequeue();

            ++totalPlaneArrival;
            totalWaitTime += currentTick - (tempPlane.arrivalTime + tempPlane.waitingTime);
            --numberOfPlaneInQueue;
            Console.WriteLine("--" + tempPlane.getPlaneID() + " finished Arrival waiting");
            return PlaneId;
        }

        public void QClose()
        {
            isOpen = false;
        }

        public void QOpen()
        {
            isOpen = true;
        }

        public int GetTotalWaitTime()
        {
            return totalWaitTime;
        }

        public int GetTotalPlaneArrival()
        {
            return totalPlaneArrival;
        }

        public int GetAvgWaitTime()
        {
            return totalPlaneArrival > 0 ? (totalWaitTime / totalPlaneArrival) : 0;
        }

        public int getWaitingCount()
        {
            return numberOfPlaneInQueue;
        }

    }
}
