using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cap_Airport
{
    public partial class RequestArrivalForm : Form
    {
        MainArea mainForm;

        public RequestArrivalForm()
        {
            InitializeComponent();
        }

        public RequestArrivalForm(MainArea mainForm, string s)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            if (mainForm.arrQ.Count >= mainForm.MaxQ)
            {
                btnAddToAQ.BackColor = Color.Red;
                btnAddToAQ.Enabled = false;
                btnWaitAQ.BackColor = Color.Green;
                btnWaitAQ.Enabled = true;
            }
            else
            {
                btnAddToAQ.BackColor = Color.Green;
                btnAddToAQ.Enabled = true;
                btnWaitAQ.BackColor = Color.Red;
                btnWaitAQ.Enabled = false;
            }
            txtArP.Text = s;
        }

        private void btnAddToAQ_Click(object sender, EventArgs e)
        {
            if (this.txtArP.Text != string.Empty)
            {
                mainForm.arrQ.Enqueue(txtArP.Text);

                mainForm.listBoxArrival.DataSource = null;
                mainForm._items = new List<string>();
                foreach (string qitem in mainForm.arrQ)
                {
                    mainForm._items.Add(qitem);
                }
                mainForm.listBoxArrival.DataSource = mainForm._items;

            }
            this.Close();
        }
    }
}
