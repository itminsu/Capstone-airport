using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cap_Airport
{
    public class WaitPanelController: PanelController
    {
        private Buffer bufferGate;
        private int gateId;

        public WaitPanelController(Panel panel, Point originPoint, int delay, int length, bool isMovingPositiveAxis, bool isHorizontal, FlagS semaphoreThis,
            FlagS semaphoreNext, FlagS semaphoreWait, Buffer bufferThis, Buffer bufferNext, Buffer bufferGate, int gateId) :
            base(panel, originPoint, delay, length, isMovingPositiveAxis, isHorizontal, semaphoreThis, semaphoreNext, semaphoreWait, bufferThis, bufferNext)
        {
            // Assign the value of instance variables from constructor args
            this.bufferGate = bufferGate;
            this.gateId = gateId;

            // Panel paint function
            this.panel.Paint += new PaintEventHandler(this.panelPaint);
        }

        public override void Start()
        {
            Thread.Sleep(delay);
            for (int k = 1; k <= 200; k++)
            {
                semaphoreThis.signal(); // Lock the current panel
                bufferThis.read(ref this.plane);

                // Set plane position to initial point of the panel
                if (this.plane != null)
                    this.plane.setPosition(this.originPoint.X, this.originPoint.Y);

                // Plane reaches the taxiway to which the destination terminal is attached to
                if (this.plane != null && new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Contains(this.gateId) && this.gateId == this.plane.getGate())
                {
                    this.plane.movePlane(xDelta, yDelta);
                    panel.Invalidate();
                    Thread.Sleep(delay);
                    bufferGate.write(this.plane);
                    this.plane = null;
                    panel.Invalidate();
                }
                else
                {
                    try
                    {
                        // Move plane on runway
                        if (this.plane.getGate() == 0 && gateId == -1)
                        {
                            for (int i = 1; i <= this.length; i++)
                            {
                                this.plane.movePlane(xDelta, yDelta);
                                panel.Invalidate();
                                Thread.Sleep(delay);
                            }
                            this.plane = null;
                            panel.Invalidate();
                        }
                        else
                        {
                            if (gateId == -1)
                            {
                                // Move panel on runway (on the way to taxiway)
                                int turningPoint = this.length - 12; // calculate turning point
                                for (int i = 1; i <= turningPoint; i++)
                                {
                                    this.plane.movePlane(xDelta, yDelta);
                                    panel.Invalidate();
                                    Thread.Sleep(delay);
                                }
                            }
                            else
                            {
                                // Move plane on wait panels
                                for (int i = 1; i < length; i++)
                                {
                                    panel.Invalidate();
                                    this.plane.movePlane(xDelta, yDelta);
                                    Thread.Sleep(delay);
                                }
                            }

                            // Write plane to terminal
                            if (this.plane.getGate() != this.gateId)
                            {
                                if (this.gateId == 0 && this.plane.getGate() == 1)
                                    semaphoreWait.wait();

                                if (this.gateId == 1 && this.plane.getGate() == 2)
                                    semaphoreWait.wait();

                                if (this.gateId == 2 && this.plane.getGate() == 3)
                                    semaphoreWait.wait();

                                if (this.gateId == 3 && this.plane.getGate() == 4)
                                    semaphoreWait.wait();

                                if (this.gateId == 4 && this.plane.getGate() == 5)
                                    semaphoreWait.wait();

                                if (this.gateId == 5 && this.plane.getGate() == 6)
                                    semaphoreWait.wait();

                                if (this.gateId == 6 && this.plane.getGate() == 7)
                                    semaphoreWait.wait();

                                if (this.gateId == 7 && this.plane.getGate() == 8)
                                    semaphoreWait.wait();

                                if (this.gateId == 8 && this.plane.getGate() == 9)
                                    semaphoreWait.wait();

                                if (this.gateId == 9 && this.plane.getGate() == 10)
                                    semaphoreWait.wait();
                                
                                if (this.gateId != -1)
                                    semaphoreNext.wait();

                                bufferNext.write(this.plane);
                                this.plane = null;
                                panel.Invalidate();
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Caught exception: " + e.Message);
                    }
                }
            }
        }

        protected override void panelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (plane != null)
            {
                SolidBrush brush = new SolidBrush(plane.getColor());
                g.FillRectangle(brush, plane.getPositionX(), plane.getPositionY(), 10, 10);
                SolidBrush blackBrush = new SolidBrush(Color.White);
                g.DrawString(plane.getGate().ToString(), new Font("Arial", 7), blackBrush, new PointF((float)plane.getPositionX(), (float)plane.getPositionY()));
                brush.Dispose();
            }
            g.Dispose();
        }
    }
}
