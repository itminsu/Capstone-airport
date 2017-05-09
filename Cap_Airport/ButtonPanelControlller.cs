using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Cap_Airport
{
    public class ButtonPanelControlller : PanelController
    {
        private bool isArrival;
        private Button btnQ;
        private RadioButton rbtnDep;
        private RadioButton rbtnGate1;
        private RadioButton rbtnGate2;
        private RadioButton rbtnGate3;
        private RadioButton rbtnGate4;
        private RadioButton rbtnGate5;
        private RadioButton rbtnGate6;
        private RadioButton rbtnGate7;
        private RadioButton rbtnGate8;
        private RadioButton rbtnGate9;
        private RadioButton rbtnGate10;

        private bool isLocked = true;

        private string planeId;
        MainArea mainForm;

        public ButtonPanelControlller(Panel panel, Point originPoint, int delay, int length, bool isMovingPositiveAxis, bool isHorizontal, bool isArrival,
            FlagS semaphoreThis, FlagS semaphoreNext, FlagS semaphoreWait, Buffer bufferThis, Buffer bufferNext, Button btnQ,
            RadioButton rbtnDep, RadioButton rbtnGate1, RadioButton rbtnGate2, RadioButton rbtnGate3, RadioButton rbtnGate4, RadioButton rbtnGate5,
            RadioButton rbtnGate6, RadioButton rbtnGate7, RadioButton rbtnGate8, RadioButton rbtnGate9, RadioButton rbtnGate10) :
            base(panel, originPoint, delay, length, isMovingPositiveAxis, isHorizontal, semaphoreThis, semaphoreNext, semaphoreWait, bufferThis, bufferNext)
        {

            // Assign the value of instance variables from constructor args
            this.isArrival = isArrival;
            this.btnQ = btnQ;
            this.rbtnDep = rbtnDep;
            this.rbtnGate1 = rbtnGate1;
            this.rbtnGate2 = rbtnGate2;
            this.rbtnGate3 = rbtnGate3;
            this.rbtnGate4 = rbtnGate4;
            this.rbtnGate5 = rbtnGate5;
            this.rbtnGate6 = rbtnGate6;
            this.rbtnGate7 = rbtnGate7;
            this.rbtnGate8 = rbtnGate8;
            this.rbtnGate9 = rbtnGate9;
            this.rbtnGate10 = rbtnGate10;

            // If panel is assigned to arrivals, create a new instance of plane obj
            if (this.isArrival)
            {
                if (mainForm != null && mainForm.arrQ != null && mainForm.arrQ.Count > 0)
                {
                    planeId = mainForm.arrQ.Dequeue();
                }
                else planeId = "";
                this.plane = new Plane(new Point(0, 0), 0, this.getRandomColor(), planeId);
            }
            // Assign a method to mainBtn click action and panel paint method
            this.btnQ.Click += new System.EventHandler(this.btnQ_Click);
            this.panel.Paint += new PaintEventHandler(this.panelPaint);
        }

        public override void Start()
        {
            Thread.Sleep(delay);
            for (int k = 1; k <= 200; k++)
            {
                semaphoreThis.signal();

                // Check the availability of next panel
                if (this.plane != null)
                    semaphoreThis.wait();

                // Move plane on terminal (to park)
                if (this.plane == null && !isArrival)
                {
                    bufferThis.read(ref this.plane);    // Read the plane in terminal's buffer and set reference to this.plane

                    this.plane.setPosition(this.panel.Size.Width-15, this.panel.Size.Height-5); // Move plane to bottom of the terminal
                    panel.Invalidate();

                    for (int i = 1; i < length; i++)
                    {
                        this.plane.movePlane(this.xDelta, -this.yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }

                    // After parking, set plane's next gate to take off
                    this.plane.setGate(0);

                    this.isLocked = true; // Lock the gate
                    this.btnQ.BackColor = this.isLocked ? Color.Pink : Color.Green;
                    lock (this)
                    {
                        if (!this.isLocked)
                            Monitor.Pulse(this);
                    }
                }

                this.plane.setPosition(this.originPoint.X, this.originPoint.Y); // Set plane to initial position of panel
                panel.Invalidate();

                lock (this)
                {
                    while (this.isLocked)
                    {
                        Monitor.Wait(this);
                    }
                }

                // Move the plane from the terminal once the terminal lock has been released
                for (int i = 1; i < length; i++)
                {
                    this.plane.movePlane(this.xDelta, this.yDelta);
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }


                // When a new plane is generated and it's not a direct take off
                // Check the availability of taxiway 1 (to avoid blocking the runway)
                if (plane.getGate() != 0 && isArrival)
                    this.semaphoreWait.wait();

                this.semaphoreNext.wait();
                bufferNext.write(this.plane);
                this.plane = null;
                panel.Invalidate();

                // Genereating new plane
                if (this.isArrival)
                {
                    this.isLocked = true;
                    this.btnQ.BackColor = this.isLocked ? Color.Pink : Color.Green;
                    this.plane = new Plane(this.originPoint, 0, this.getRandomColor(), planeId);
                    panel.Invalidate();
                }
            }
        }

        private void btnQ_Click(object sender, System.EventArgs e)
        {
            this.isLocked = !this.isLocked;   // Unlock the resource only when the program wants to use it
            if (this.plane != null) // Toggle the button's color only If there's a plane occupying the panel
            {
                this.btnQ.BackColor = this.isLocked ? Color.Pink : Color.Green;

                if (mainForm != null && mainForm.arrQ != null && mainForm.arrQ.Count > 0)
                {
                    string s = mainForm.arrQ.Dequeue();
                    System.Console.WriteLine("in dequeue == " + s);
                    planeId = mainForm.arrQ.Dequeue();


                }
                else planeId = "";
            }
            // Check if button has been clicked from arrivals section 
            if (this.isArrival && this.plane.getPositionX() == this.originPoint.X)
            {
                // Read gate from radiobutton and set to plan
                this.plane.setGate(0);
                if (rbtnGate1.Checked) this.plane.setGate(1);
                if (rbtnGate2.Checked) this.plane.setGate(2);
                if (rbtnGate3.Checked) this.plane.setGate(3);
                if (rbtnGate4.Checked) this.plane.setGate(4);
                if (rbtnGate5.Checked) this.plane.setGate(5);
                if (rbtnGate6.Checked) this.plane.setGate(6);
                if (rbtnGate7.Checked) this.plane.setGate(7);
                if (rbtnGate8.Checked) this.plane.setGate(8);
                if (rbtnGate9.Checked) this.plane.setGate(9);
                if (rbtnGate10.Checked) this.plane.setGate(10);
            }

            // Lock the resource while using
            lock (this)
                if (!isLocked) Monitor.Pulse(this);
        }

        protected override void panelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (plane != null)
            {
                SolidBrush brush = new SolidBrush(plane.getColor());
                g.FillRectangle(brush, plane.getPositionX(), plane.getPositionY(), 10, 10);
                SolidBrush blackBrush = new SolidBrush(Color.White);
                try
                {
                    g.DrawString(plane.getGate().ToString(), new Font("Helvetica", 7), blackBrush, new PointF((float)plane.getPositionX(), (float)plane.getPositionY()));
                    //g.DrawString(plane.getPlaneID().ToString(), new Font("Helvetica", 7), blackBrush, new PointF((float)plane.getPositionX()+5, (float)plane.getPositionY()+5));
                }
                catch (Exception ae) { }
                brush.Dispose();
            }
            g.Dispose();
        }

        private Color getRandomColor()
        {
            Random random = new Random();
            return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }
        
    }
}
