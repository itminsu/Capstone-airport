using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cap_Airport
{
    public abstract class PanelController
    {
        protected Panel panel;
        protected Point originPoint;
        protected int delay;
        protected int length;
        protected bool isMovingPositiveAxis;
        protected FlagS semaphoreThis;
        protected FlagS semaphoreNext;
        protected FlagS semaphoreWait;
        protected Buffer bufferThis;
        protected Buffer bufferNext;
        protected int xDelta = 0;
        protected int yDelta = 0;
        protected Plane plane = null;

        public PanelController(Panel panel, Point originPoint, int delay, int length, bool isMovingPositiveAxis, bool isHorizontal,
            FlagS semaphoreThis, FlagS semaphoreNext, FlagS semaphoreWait, Buffer bufferThis, Buffer bufferNext)
        {
            // Set the instance variables from constructor args
            this.panel = panel;
            this.originPoint = originPoint;
            this.delay = delay;
            this.length = length;
            this.isMovingPositiveAxis = isMovingPositiveAxis;
            this.semaphoreThis = semaphoreThis;
            this.semaphoreNext = semaphoreNext;
            this.semaphoreWait = semaphoreWait;
            this.bufferThis = bufferThis;
            this.bufferNext = bufferNext;

            // Calculate delta based on panel's orientation and moving direction
            if (isHorizontal)
                this.xDelta = this.isMovingPositiveAxis ? +5 : -5;
            else
                this.yDelta = this.isMovingPositiveAxis ? +5 : -5;
        }

        public abstract void Start();
        protected abstract void panelPaint(object sender, PaintEventArgs e);
    }

}
