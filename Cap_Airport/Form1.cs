// The c# airport simulator
// by Yan Liu
// Capstore Project 
// basic idea is come from 
// Multithread controller: https://github.com/aliharis/concurrent-airport-simulator


using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finisar.SQLite;


namespace Cap_Airport
{
    public partial class MainArea : Form
    {
        // Timer related 
        private System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
        public int currentTicks = 0;
        Random rand = new Random();

        // Queue
        public List<string> _items = new List<string>();
        public Queue<string> arrQ = new Queue<string>();
        public int MaxQ = 3;
        //public ArrivalQueue aQ = new ArrivalQueue();

        private PanelController buttonArrival;
        private PanelController buttonPanelGate1, buttonPanelGate2, buttonPanelGate3, buttonPanelGate4, buttonPanelGate5;
        private PanelController buttonPanelGate6, buttonPanelGate7, buttonPanelGate8, buttonPanelGate9,buttonPanelGate10;
        private FlagS FSArrival;
        private FlagS FSRunway;
        private FlagS FSTaxiway1, FSTaxiway2, FSTaxiway3, FSTaxiway4, FSTaxiway5;
        private FlagS FSTaxiway6, FSTaxiway7, FSTaxiway8, FSTaxiway9, FSTaxiway10;
        private FlagS FSTaxiwayMain;
        private FlagS FSTaxiwayMain2;
        private FlagS FSGate1,FSGate2,FSGate3,FSGate4,FSGate5;
        private FlagS FSGate6, FSGate7, FSGate8, FSGate9, FSGate10;

        private Buffer bufferArrival;
        private Buffer bufferRunway;
        private Buffer bufferTaxiway1, bufferTaxiway2, bufferTaxiway3, bufferTaxiway4, bufferTaxiway5;
        private Buffer bufferTaxiway6, bufferTaxiway7, bufferTaxiway8, bufferTaxiway9, bufferTaxiway10;
        private Buffer bufferTaxiwayMain;
        private Buffer bufferTaxiwayMain2;
        private Buffer bufferGate1, bufferGate2, bufferGate3, bufferGate4, bufferGate5;
        private Buffer bufferGate6, bufferGate7, bufferGate8, bufferGate9, bufferGate10;
        private PanelController waitRunway;
        private PanelController waitTaxiway1, waitTaxiway2, waitTaxiway3, waitTaxiway4, waitTaxiway5;
        private PanelController waitTaxiway6, waitTaxiway7, waitTaxiway8, waitTaxiway9, waitTaxiway10;
        private PanelController waitTaxiwayMain;
        private PanelController waitTaxiwayMain2;

        public MainArea()
        {
            InitializeComponent();
           
            //Flag Signals
            FSArrival = new FlagS();
            FSRunway = new FlagS();
            FSTaxiwayMain = new FlagS();
            FSTaxiwayMain2 = new FlagS();
            FSTaxiway1 = new FlagS();
            FSTaxiway2 = new FlagS();
            FSTaxiway3 = new FlagS();
            FSTaxiway4 = new FlagS();
            FSTaxiway5 = new FlagS();
            FSTaxiway6 = new FlagS();
            FSTaxiway7 = new FlagS();
            FSTaxiway8 = new FlagS();
            FSTaxiway9 = new FlagS();
            FSTaxiway10 = new FlagS();
            // Gate signal
            FSGate1 = new FlagS();
            FSGate2 = new FlagS();
            FSGate3 = new FlagS();
            FSGate4 = new FlagS();
            FSGate5 = new FlagS();
            FSGate6 = new FlagS();
            FSGate7 = new FlagS();
            FSGate8 = new FlagS();
            FSGate9 = new FlagS();
            FSGate10 = new FlagS();

            //Buffer Taxiway
            bufferArrival = new Buffer();
            bufferRunway = new Buffer();
            bufferTaxiwayMain = new Buffer();
            bufferTaxiwayMain2 = new Buffer();
            bufferTaxiway1 = new Buffer();
            bufferTaxiway2 = new Buffer();
            bufferTaxiway3 = new Buffer();
            bufferTaxiway4 = new Buffer();
            bufferTaxiway5 = new Buffer();
            bufferTaxiway6 = new Buffer();
            bufferTaxiway7 = new Buffer();
            bufferTaxiway8 = new Buffer();
            bufferTaxiway9 = new Buffer();
            bufferTaxiway10 = new Buffer();
            // Buffer gates
            bufferGate1 = new Buffer();
            bufferGate2 = new Buffer();
            bufferGate3 = new Buffer();
            bufferGate4 = new Buffer();
            bufferGate5 = new Buffer();
            bufferGate6 = new Buffer();
            bufferGate7 = new Buffer();
            bufferGate8 = new Buffer();
            bufferGate9 = new Buffer();
            bufferGate10 = new Buffer();

            //Set ButtonPanels
            buttonArrival = new ButtonPanelControlller(TaxiWay0, new Point(50, 5), 50, 10, false, true, true, FSArrival, FSRunway, FSTaxiwayMain, null, bufferRunway, btnQ, rbtnDep, rbtn1, rbtn2, rbtn3, rbtn4, rbtn5, rbtn6, rbtn7, rbtn8, rbtn9, rbtn10);
            buttonPanelGate1 = new ButtonPanelControlller(pnlGate1, new Point(3, 5), 50, 25, true, false, false, FSGate1, FSTaxiway1, null, bufferGate1, bufferTaxiway1, btnGate1, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate2 = new ButtonPanelControlller(pnlGate2, new Point(3, 5), 50, 25, true, false, false, FSGate2, FSTaxiway2, null, bufferGate2, bufferTaxiway2, btnGate2, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate3 = new ButtonPanelControlller(pnlGate3, new Point(3, 5), 50, 25, true, false, false, FSGate3, FSTaxiway3, null, bufferGate3, bufferTaxiway3, btnGate3, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate4 = new ButtonPanelControlller(pnlGate4, new Point(3, 5), 50, 25, true, false, false, FSGate4, FSTaxiway4, null, bufferGate4, bufferTaxiway4, btnGate4, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate5 = new ButtonPanelControlller(pnlGate5, new Point(3, 5), 50, 25, true, false, false, FSGate5, FSTaxiway5, null, bufferGate5, bufferTaxiway5, btnGate5, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate6 = new ButtonPanelControlller(pnlGate6, new Point(3, 5), 50, 25, true, false, false, FSGate6, FSTaxiway6, null, bufferGate6, bufferTaxiway6, btnGate6, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate7 = new ButtonPanelControlller(pnlGate7, new Point(3, 5), 50, 25, true, false, false, FSGate7, FSTaxiway7, null, bufferGate7, bufferTaxiway7, btnGate7, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate8 = new ButtonPanelControlller(pnlGate8, new Point(3, 5), 50, 25, true, false, false, FSGate8, FSTaxiway8, null, bufferGate8, bufferTaxiway8, btnGate8, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate9 = new ButtonPanelControlller(pnlGate9, new Point(3, 5), 50, 25, true, false, false, FSGate9, FSTaxiway9, null, bufferGate9, bufferTaxiway9, btnGate9, null, null, null, null, null, null, null, null, null, null, null);
            buttonPanelGate10 = new ButtonPanelControlller(pnlGate10, new Point(3, 5), 50, 25, true, false, false, FSGate10, FSTaxiway10, null, bufferGate10, bufferTaxiway10, btnGate10, null, null, null, null, null, null, null, null, null, null, null);

            // Initialize WaitPanel objects for all waitpanels
            // Runway and Taxiway Waiting controllers
            waitRunway = new WaitPanelController(runway, new Point(850, 5), 50, 180, false, true, FSRunway, FSTaxiwayMain, null, bufferRunway, bufferTaxiwayMain, null, -1);
            waitTaxiwayMain = new WaitPanelController(TaxiWayMain, new Point(5, 200), 50, 38, false, false, FSTaxiwayMain, FSTaxiway1, FSGate1, bufferTaxiwayMain, bufferTaxiway1, null, 0);
            waitTaxiway1 = new WaitPanelController(TaxiWay1, new Point(5, 3), 50, 13, true, true, FSTaxiway1, FSTaxiway2, FSGate2, bufferTaxiway1, bufferTaxiway2, bufferGate1, 1);
            waitTaxiway2 = new WaitPanelController(TaxiWay2, new Point(5, 3), 50, 13, true, true, FSTaxiway2, FSTaxiway3, FSGate3, bufferTaxiway2, bufferTaxiway3, bufferGate2, 2);
            waitTaxiway3 = new WaitPanelController(TaxiWay3, new Point(5, 3), 50, 13, true, true, FSTaxiway3, FSTaxiway4, FSGate4, bufferTaxiway3, bufferTaxiway4, bufferGate3, 3);
            waitTaxiway4 = new WaitPanelController(TaxiWay4, new Point(5, 3), 50, 13, true, true, FSTaxiway4, FSTaxiway5, FSGate5, bufferTaxiway4, bufferTaxiway5, bufferGate4, 4);
            waitTaxiway5 = new WaitPanelController(TaxiWay5, new Point(5, 3), 50, 13, true, true, FSTaxiway5, FSTaxiway6, FSGate6, bufferTaxiway5, bufferTaxiway6, bufferGate5, 5);
            waitTaxiway6 = new WaitPanelController(TaxiWay6, new Point(5, 3), 50, 13, true, true, FSTaxiway6, FSTaxiway7, FSGate7, bufferTaxiway6, bufferTaxiway7, bufferGate6, 6);
            waitTaxiway7 = new WaitPanelController(TaxiWay7, new Point(5, 3), 50, 13, true, true, FSTaxiway7, FSTaxiway8, FSGate8, bufferTaxiway7, bufferTaxiway8, bufferGate7, 7);
            waitTaxiway8 = new WaitPanelController(TaxiWay8, new Point(5, 3), 50, 13, true, true, FSTaxiway8, FSTaxiway9, FSGate9, bufferTaxiway8, bufferTaxiway9, bufferGate8, 8);
            waitTaxiway9 = new WaitPanelController(TaxiWay9, new Point(5, 3), 50, 20, true, true, FSTaxiway9, FSTaxiway10, FSGate10, bufferTaxiway9, bufferTaxiway10, bufferGate9, 9);
            waitTaxiway10 = new WaitPanelController(TaxiWay10, new Point(5, 3), 50, 20, true, true, FSTaxiway10, FSTaxiwayMain2, null, bufferTaxiway10, bufferTaxiwayMain2, bufferGate10, 10);
            waitTaxiwayMain2 = new WaitPanelController(TaxiWayMain2, new Point(5, 3), 50, 38, true, false, FSTaxiwayMain2, FSRunway, null, bufferTaxiwayMain2, bufferRunway, null, 11);

            // Threads for gates
            new Thread(buttonArrival.Start).Start();
            new Thread(buttonPanelGate1.Start).Start();
            new Thread(buttonPanelGate2.Start).Start();
            new Thread(buttonPanelGate3.Start).Start();
            new Thread(buttonPanelGate4.Start).Start();
            new Thread(buttonPanelGate5.Start).Start();
            new Thread(buttonPanelGate6.Start).Start();
            new Thread(buttonPanelGate7.Start).Start();
            new Thread(buttonPanelGate8.Start).Start();
            new Thread(buttonPanelGate9.Start).Start();
            new Thread(buttonPanelGate10.Start).Start();

            // Threads for Taxiway 
            new Thread(waitRunway.Start).Start();
            new Thread(waitTaxiwayMain.Start).Start();
            new Thread(waitTaxiway1.Start).Start();
            new Thread(waitTaxiway2.Start).Start();
            new Thread(waitTaxiway3.Start).Start();
            new Thread(waitTaxiway4.Start).Start();
            new Thread(waitTaxiway5.Start).Start();
            new Thread(waitTaxiway6.Start).Start();
            new Thread(waitTaxiway7.Start).Start();
            new Thread(waitTaxiway8.Start).Start();
            new Thread(waitTaxiway9.Start).Start();
            new Thread(waitTaxiway10.Start).Start();
            new Thread(waitTaxiwayMain2.Start).Start();

            // listbox
            ShowData();
        }
        
        private void ShowData()
        {
            listBoxArrival.DataSource = null;
            foreach (string qitem in arrQ)
            {
                _items.Add(qitem);
            }
            listBoxArrival.DataSource = _items;
        }

        private void MainArea_Load(object sender, EventArgs e)
        { 
            //this.WindowState = FormWindowState.Maximized;
            //https://github.com/Labradoodle-360/GroceryStoreSimulation/blob/master/Program.cs
            //https://github.com/wilberh/Simulation-of-waiting-queues/blob/master/Program.cs
            //Random time interval
            tm.Tick += new EventHandler(TimerTick);
            int fortimerinterval = rand.Next(10000, 20000);
            tm.Interval = fortimerinterval;
            tm.Enabled = true;
            tm.Start();  
        }

        void TimerTick(object sender, EventArgs e)
        {
            int fortimerinterval = rand.Next(10000, 20000);
            tm.Interval = fortimerinterval;
            
            //connect to sqlite
            SQLiteConnection sqlite_conn;
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;

            // create a new database connection-- airport.db:
            sqlite_conn = new SQLiteConnection("Data Source=airport.db;Version=3;New=False;Compress=True;");
            // open the connection:
            sqlite_conn.Open();
            // create a new SQL command:
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM plane";

            // Now the SQLiteCommand object can give us a DataReader-Object:
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            int totPlane = 0;
            //The SQLiteDataReader allows us to run through the result lines:
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                totPlane++;
            }
 
            int i = 1 + rand.Next(totPlane-1); // random pick one form db
            int count = 0;
            string newPlaneID = "";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                if (count == i) //System.Console.WriteLine(sqlite_datareader["planeID"]);
                    newPlaneID = (string)sqlite_datareader["planeID"];
                count++;
            }
            System.Console.WriteLine(newPlaneID);
            // check newPlaneIn in Queue?
            bool isExist = false;
            foreach(string ss in arrQ)
            {
                if (ss == newPlaneID)
                {
                    isExist = true;
                    break;
                }
            }
            if (newPlaneID != string.Empty && !isExist)
            {
                RequestArrivalForm rf = new RequestArrivalForm(this, newPlaneID);
                rf.Show();
            }

            //close database connection:
            sqlite_conn.Close();
            // add to arrivalQueue

            // Queue show in List

        }

    }
}
