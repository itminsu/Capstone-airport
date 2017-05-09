using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;


namespace GroceryStoreSimulation
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Run (new Store ());
		}
	}

	class Store : Form
	{
		/* totalCheckoutLines (int)
		 * 
		 * - Determines how many checkout lines appear in the store (will not expand canvas if too many are added).
		 **/
		public const int totalCheckoutLines   = 5;

		/* totalTicks (int)
		 * 
		 * - How long the grocery store simulation should run for.
		 **/
		public const int totalTicks           = 60;

		/*
		 * simulationSpeed (string) [supersonic|minute|hour]
		 * 
		 * - The speed at which each tick occurs.
		 **/
		public const string simulationSpeed   = "minute";

		/*
		 * lineSelectionMode (string) [random,fastest]
		 * 
		 * - The method in which shoppers select a line. Random will select a completely random (open) line,
		 * - despite how many people are in it, while fastest will select the (open) line with the fewest people.
		 **/
		public const string lineSelectionMode = "random";

		public int currentTicks = 0;
		public bool simulationDataAdded;
		public bool simulationStarted;
		public bool simulationCompleteBool;

		public string title = "Grocery Store Simulation";

		public List<Person> People = new List<Person>();

		public List<CheckoutLine> CheckoutLines = new List<CheckoutLine>();

		public List<int> PeopleInStore = new List<int>();

		public Dictionary<int, List<int>> arrivalTimeToPerson = new Dictionary<int, List<int>>();
		public Dictionary<int, List<int>> enterLineTimeToPerson = new Dictionary<int, List<int>>();

		public System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer ();

		public Random randomGenerator = new Random(DateTime.Now.Millisecond);

		public int totalPeopleCheckedOut = 0;
		public double totalSales         = 0;
		public int avgWaitTime           = 0;
		public int peopleLeftInLine      = 0;

		public Store()
		{
			simulationDataAdded = false;

			int timerInterval = 1 * 1000;
			if (simulationSpeed == "hour")
				timerInterval = 1 * 60 * 1000;
			else if (simulationSpeed == "supersonic")
				timerInterval = 1 * 10;
				
			timer.Interval = timerInterval;
			timer.Tick += new EventHandler (TimerTick);

			MenuItem importSimulationData = new MenuItem ("Import Simulation Data");
			importSimulationData.Click += importSimulationDataClick;

			MenuItem startSimulator = new MenuItem ("Start Simulation");
			startSimulator.Click += StartSimulatorClick;

			MenuItem pauseSimulation = new MenuItem ("Pause Simulation");
			pauseSimulation.Click += PauseSimulation_Click;

			MenuItem file = new MenuItem ("File");
			file.MenuItems.Add (importSimulationData);
			file.MenuItems.Add (startSimulator);
			file.MenuItems.Add (pauseSimulation);

			MainMenu bar = new MainMenu ();
			bar.MenuItems.Add (file);

			Menu = bar;

			Width = 1400;
			Height = 1000;

			Click += Form_Click;
		}

		void PauseSimulation_Click (object sender, EventArgs e)
		{
			if (simulationStarted == true)
			{
				simulationStarted = false;
				timer.Stop ();
				Console.WriteLine ("Simulation Paused!");
			}
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			Font tickCounterFont = new Font (SystemFonts.DefaultFont.FontFamily, 45, FontStyle.Regular);
			g.DrawString((simulationCompleteBool == false ? (currentTicks.ToString() + " / " + totalTicks.ToString() + " Minutes") : "Simulation Statistics"), tickCounterFont, Brushes.Black, ((Width / 2) - 200), 30);

			if (simulationCompleteBool == false)
			{
				SolidBrush rectangleBrush = new SolidBrush (Color.White);
				g.FillRectangle (rectangleBrush, ((Width - 1175) / 2), ((Height - 795) / 2), 1175, 775);

				int personWidth   = 50;
				int xOffset       = Width - 188;
				int checkoutY     = 677;
				int checkoutWidth = 75;
				if (CheckoutLines.Count > 0)
				{
					for (int iteration = 1; iteration <= CheckoutLines.Count; iteration++)
					{
						int checkoutNumber = CheckoutLines.Count - iteration;
						CheckoutLine thisCheckout = CheckoutLines [checkoutNumber];
						thisCheckout.drawCheckout (e, xOffset, checkoutY);
						xOffset -= personWidth + 30 + checkoutWidth;
						foreach (int personId in thisCheckout.peopleInLine)
						{
							Person thisPerson = People [personId];
							if (thisPerson.numberInLine > 0)
								thisPerson.numberInLine = thisPerson.numberInLine - 1;
						}
					}
				}

				if (PeopleInStore.Count > 0)
				{
					foreach (int personId in PeopleInStore)
					{
						Person thisPerson = People [personId];
						if (thisPerson.state != "outside" && thisPerson.state != "finished")
							People [personId].drawPerson (e, CheckoutLines [People [personId].checkoutLine]);
					}
				}
			}
			else
			{
				//-- The wonderful world of statistics.
				Console.WriteLine ("-----------------Simulation Data-----------------");

				double totalSales = 0;
				int totalWaitTime = 0;

				double mostSales = 0;
				int mostPeopleCheckedOut = 0, longestAverageWaitTime = 0, mostPeopleLeftInLine = 0;
				int mostPeopleCheckedOutId = 0, mostSalesId = 0, longestAverageWaitTimeId = 0, mostPeopleLeftInLineId = 0;

				for (int iteration = 0; iteration < CheckoutLines.Count; ++iteration)
				{
					CheckoutLine thisCheckoutLine = CheckoutLines [iteration];
					totalSales += thisCheckoutLine.getTotalSpent ();
					totalWaitTime += thisCheckoutLine.getTotalWaitTime ();
					totalPeopleCheckedOut += thisCheckoutLine.getTotalPeopleCheckedOut ();
					peopleLeftInLine += thisCheckoutLine.getWaitingInLineCount ();

					if (thisCheckoutLine.totalPeopleCheckedOut > mostPeopleCheckedOut)
					{
						mostPeopleCheckedOut = thisCheckoutLine.totalPeopleCheckedOut;
						mostPeopleCheckedOutId = iteration;
					}
					if (thisCheckoutLine.totalSales > mostSales)
					{
						mostSales = thisCheckoutLine.totalSales;
						mostSalesId = iteration;
					}
					if (thisCheckoutLine.getAvgWaitTimeForLine () > longestAverageWaitTime)
					{
						longestAverageWaitTime = thisCheckoutLine.getAvgWaitTimeForLine ();
						longestAverageWaitTimeId = iteration;
					}
					if (thisCheckoutLine.numberOfPeopleInLine > mostPeopleLeftInLine)
					{
						mostPeopleLeftInLine = thisCheckoutLine.numberOfPeopleInLine;
						mostPeopleLeftInLineId = iteration;
					}
				}
				if (totalPeopleCheckedOut > 0) {
					avgWaitTime = totalWaitTime / totalPeopleCheckedOut;
				}

				Font mainStatsBoldFont = new Font (SystemFonts.DefaultFont.FontFamily, 20, FontStyle.Bold);
				Font mainStatsFont     = new Font (SystemFonts.DefaultFont.FontFamily, 20, FontStyle.Regular);

				Pen pen = new Pen(Color.Black);
				g.DrawLine (pen, 0, 105, Width, 105);

				g.DrawString("Total People Checked Out:", mainStatsBoldFont, Brushes.Black, 275, 125);
				g.DrawString (totalPeopleCheckedOut.ToString (), mainStatsFont, Brushes.Black, 825, 125);

				g.DrawString("Total Sales from All Checkout Lines:", mainStatsBoldFont, Brushes.Black, 275, 175);
				g.DrawString ("$" + totalSales.ToString("#,##0"), mainStatsFont, Brushes.Black, 825, 175);

				g.DrawString("Average Wait Time in Checkout Lines:", mainStatsBoldFont, Brushes.Black, 275, 225);
				g.DrawString (avgWaitTime.ToString() + " Tick" + (((int) avgWaitTime) != 1 ? "s" : ""), mainStatsFont, Brushes.Black, 825, 225);

				g.DrawString("People Left in Checkout Lines:", mainStatsBoldFont, Brushes.Black, 275, 275);
				g.DrawString (peopleLeftInLine.ToString(), mainStatsFont, Brushes.Black, 825, 275);

				g.DrawLine (pen, 0, 325, Width, 325);

				Font checkoutFont     = new Font (SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Regular);
				Font checkoutFontBold = new Font (SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Bold);

				for (int iteration = 0; iteration < CheckoutLines.Count; iteration++)
				{
					CheckoutLine thisCheckout = CheckoutLines [iteration];
					g.DrawString ("Checkout " + (iteration + 1) + ":", mainStatsBoldFont, Brushes.Black, (275 * (iteration + 1)), 375);
					g.DrawString ("Total People Checked Out: " + thisCheckout.totalPeopleCheckedOut, (mostPeopleCheckedOutId == iteration && mostPeopleCheckedOut > 0 ? checkoutFontBold : checkoutFont), Brushes.Black, (275 * (iteration + 1)), 425);
					g.DrawString ("Total Sales: $" + thisCheckout.totalSales.ToString("#,##0"), (mostSalesId == iteration && mostSales > 0 ? checkoutFontBold : checkoutFont), Brushes.Black, (275 * (iteration + 1)), 475);
					g.DrawString ("Average Wait Time: " + thisCheckout.getAvgWaitTimeForLine() + " Tick" + (((int) thisCheckout.getAvgWaitTimeForLine()) != 1 ? "s" : ""), (longestAverageWaitTimeId == iteration && longestAverageWaitTime > 0 ? checkoutFontBold : checkoutFont), Brushes.Black, (275 * (iteration + 1)), 525);
					g.DrawString ("People Left in this Line: " + thisCheckout.numberOfPeopleInLine, (mostPeopleLeftInLineId == iteration && mostPeopleLeftInLine > 0 ? checkoutFontBold : checkoutFont), Brushes.Black, (275 * (iteration + 1)), 575);
				}

				Console.WriteLine ("---------------------------- Statistics Page ----------------------------");
				Console.WriteLine ("Total People Checked Out: {0}", totalPeopleCheckedOut);
				Console.WriteLine ("Total Sales from All Checkout Lines: ${0}", totalSales.ToString("#,##0"));
				Console.WriteLine ("Average Wait Time in Checkout Lines: {0}", avgWaitTime);
				Console.WriteLine ("People Left in Checkout Lines: {0}", peopleLeftInLine);
			}
		}

		void Form_Click(object sender, EventArgs e)
		{
			if (CheckoutLines.Count > 0)
			{
				Point cursorPos = this.PointToClient(Cursor.Position);
				for (int iteration = 0; iteration < CheckoutLines.Count; iteration++)
				{
					CheckoutLine thisCheckout = CheckoutLines [iteration];
					if (thisCheckout.checkoutRectangle.Contains(cursorPos))
					{
						thisCheckout.isOpen = thisCheckout.isOpen == true ? false : true;
					}
				}
			}
		}

		void TimerTick(object sender, EventArgs e)
		{
			if (currentTicks == totalTicks)
			{
				simulationComplete ();
				return;
			}

			Console.WriteLine ("Tick: " + currentTicks);

			for (int count = 0; count < totalCheckoutLines; ++count)
			{
				CheckoutLine thisCheckoutLine = CheckoutLines [count];
				if (thisCheckoutLine.numberOfPeopleInLine > 0)
				{
					int lastFinishedPerson = thisCheckoutLine.completeCheckout (currentTicks);
					People[lastFinishedPerson].changeState ("finished");
					PeopleInStore.Remove (lastFinishedPerson);
				}
			}

			if (arrivalTimeToPerson.ContainsKey(currentTicks))
			{
				foreach (int personId in arrivalTimeToPerson [currentTicks])
				{
					Person thisPerson = People [personId];
					PeopleInStore.Add (personId);
					Console.WriteLine ("--{0} has entered the store.", thisPerson.getName ());
					thisPerson.changeState ("shopping");
				}
			}

			if (enterLineTimeToPerson.ContainsKey(currentTicks))
			{
				foreach (int personId in enterLineTimeToPerson [currentTicks])
				{
					if (lineSelectionMode == "random")
					{
						int tempRandomCheckoutLine;
						int loopCounter = 0;
						do
						{
							tempRandomCheckoutLine = randomGenerator.Next (Store.totalCheckoutLines);
							++loopCounter;
						} while (CheckoutLines [tempRandomCheckoutLine].isOpen == false && loopCounter < 100);

						if (CheckoutLines [tempRandomCheckoutLine].isOpen == true)
						{
							CheckoutLine thisCheckout = CheckoutLines [tempRandomCheckoutLine];						
							thisCheckout.addPersonToLine (personId);

							Person thisPerson = People [personId];
							thisPerson.changeState ("checking_out");
							thisPerson.checkoutLine = tempRandomCheckoutLine;
							thisPerson.numberInLine = thisCheckout.numberOfPeopleInLine;
						}
						else
						{
							int nextTick = currentTicks + 1;
							if (!enterLineTimeToPerson.ContainsKey (nextTick))
							{
								List<int> thisEnterLineTimeList = new List<int> ();
								enterLineTimeToPerson.Add (nextTick, thisEnterLineTimeList);
							}
							enterLineTimeToPerson [nextTick].Add (personId);
						}
					}
					else if (lineSelectionMode == "fastest")
					{
						int? fewestPeople = null;
						int? fewestPeopleId = null;
						for (int iteration = 0; iteration < CheckoutLines.Count; iteration++)
						{
							CheckoutLine thisCheckout = CheckoutLines [iteration];
							if (thisCheckout.isOpen == false)
								continue;

							if (fewestPeople == null || thisCheckout.numberOfPeopleInLine < fewestPeople)
							{
								fewestPeople = thisCheckout.numberOfPeopleInLine;
								fewestPeopleId = iteration;
							}
						}

						if (fewestPeopleId != null)
						{
							CheckoutLine thisCheckout = CheckoutLines [fewestPeopleId.GetValueOrDefault()];						
							thisCheckout.addPersonToLine (personId);

							Person thisPerson = People [personId];
							thisPerson.changeState ("checking_out");
							thisPerson.checkoutLine = fewestPeopleId.GetValueOrDefault();
							thisPerson.numberInLine = thisCheckout.numberOfPeopleInLine;
						}
						else
						{
							int nextTick = currentTicks + 1;
							if (!enterLineTimeToPerson.ContainsKey (nextTick))
							{
								List<int> thisEnterLineTimeList = new List<int> ();
								enterLineTimeToPerson.Add (nextTick, thisEnterLineTimeList);
							}
							enterLineTimeToPerson [nextTick].Add (personId);
						}
					}
				}
			}
			Invalidate ();
			++currentTicks;
		}

		void simulationComplete()
		{
			timer.Stop ();
			Console.WriteLine ("Simulation Complete!");
			simulationCompleteBool = true;

			/*MessageBox.Show (
				"Total People Checked Out: " + totalPeopleCheckedOut + "\n\n" +
				"Total Sales: $" + totalSales.ToString("#,##0") + "\n\n" +
				"Average Wait Time in Line: " + avgWaitTime + "\n\n" +
				"People Left in Checkout Lines: " + peopleLeftInLine,
				"Simulation Complete!",
				MessageBoxButtons.OK,
				MessageBoxIcon.Asterisk
			);*/
			Invalidate ();
		}

		void StartSimulatorClick(object sender, EventArgs e)
		{
			if (simulationDataAdded == false)
			{
				MessageBox.Show ("You must import data before starting the simulation.", title, 
					MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			Console.WriteLine ("Simulation Started");
			simulationStarted = true;
			timer.Start ();
		}

		void importSimulationDataClick(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog ();
			if (fileDialog.ShowDialog() == DialogResult.OK)
			{
				StreamReader reader = File.OpenText (fileDialog.FileName);
				string line;
				int row = 0;
				while ((line = reader.ReadLine()) != null)
				{
					++row;
					string[] customerData = line.Split('|');
					if (customerData.Length != 3)
					{
						MessageBox.Show (
							"Row " + row + " is not formatted properly. Row has been skipped.\n\nCorrect Format: ArriveMinute(Int)|ShoppingTime(Int)|AmountSpent(Double).",
							title,
							MessageBoxButtons.OK,
							MessageBoxIcon.Asterisk
						);
						continue;
					}

					int arriveMinute, shoppingTime;
					double amountSpent;

					bool arriveMinuteConverted, shoppingTimeConverted, amountSpentConverted;

					arriveMinuteConverted = int.TryParse(customerData [0], out arriveMinute);
					shoppingTimeConverted = int.TryParse(customerData [1], out shoppingTime);
					amountSpentConverted  = double.TryParse(customerData [2], out amountSpent);

					if (arriveMinuteConverted == false || shoppingTimeConverted == false || amountSpentConverted == false)
					{
						MessageBox.Show (
							"Row " + row + " does not have the proper data types.\n\nCorrect Format: ArriveMinute(Int)|ShoppingTime(Int)|AmountSpent(Double).",
							title,
							MessageBoxButtons.OK,
							MessageBoxIcon.Asterisk
						);
						continue;
					}
					Person newPerson = new Person (arriveMinute, shoppingTime, amountSpent, randomGenerator);
					People.Add (newPerson);
				}
				simulationDataAdded = true;
				Console.WriteLine ("Data Imported");
				for (int iteration = 0; iteration < People.Count; iteration++)
				{
					Person thisPerson = People [iteration];

					if (!arrivalTimeToPerson.ContainsKey(thisPerson.arriveMinute))
					{
						List<int> thisArrivalTimeList = new List<int>();
						arrivalTimeToPerson.Add (thisPerson.arriveMinute, thisArrivalTimeList);
					}
					arrivalTimeToPerson [thisPerson.arriveMinute].Add(iteration);

					int tempAdditiveTime = thisPerson.arriveMinute + thisPerson.shoppingTime;
					if (!enterLineTimeToPerson.ContainsKey(tempAdditiveTime))
					{
						List<int> thisEnterLineTimeList = new List<int> ();
						enterLineTimeToPerson.Add (tempAdditiveTime, thisEnterLineTimeList);
					}
					enterLineTimeToPerson [tempAdditiveTime].Add(iteration);

					/*Console.WriteLine ("Person:");
					Console.WriteLine ("Arrive Minute: " + thisPerson.arriveMinute);
					Console.WriteLine ("Shopping Time: " + thisPerson.shoppingTime);
					Console.WriteLine ("Amount Spent: " + thisPerson.amountSpent);
					Console.WriteLine ("Enter Line Time: " + tempAdditiveTime);
					Console.WriteLine ("------------------------");*/
				}

				//-- Create our checkout lines.
				for (int iteration = 0; iteration < Store.totalCheckoutLines; ++iteration)
				{
					CheckoutLines.Add (new CheckoutLine ((iteration + 1), People));
				}

				// TEST: arrivalTimeToPerson [12].ForEach (x => Console.WriteLine (People[x].amountSpent));
				// TEST: enterLineTimeToPerson[21].ForEach(x => Console.WriteLine(x));
			}
		}
	}
}