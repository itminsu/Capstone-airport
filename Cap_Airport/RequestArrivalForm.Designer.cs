namespace Cap_Airport
{
    partial class RequestArrivalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddToAQ = new System.Windows.Forms.Button();
            this.btnWaitAQ = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtArP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAddToAQ
            // 
            this.btnAddToAQ.BackColor = System.Drawing.Color.OliveDrab;
            this.btnAddToAQ.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddToAQ.Location = new System.Drawing.Point(104, 66);
            this.btnAddToAQ.Name = "btnAddToAQ";
            this.btnAddToAQ.Size = new System.Drawing.Size(144, 23);
            this.btnAddToAQ.TabIndex = 0;
            this.btnAddToAQ.Text = "Add To Arrival Queue";
            this.btnAddToAQ.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAddToAQ.UseVisualStyleBackColor = false;
            this.btnAddToAQ.UseWaitCursor = true;
            this.btnAddToAQ.Click += new System.EventHandler(this.btnAddToAQ_Click);
            // 
            // btnWaitAQ
            // 
            this.btnWaitAQ.BackColor = System.Drawing.Color.OrangeRed;
            this.btnWaitAQ.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWaitAQ.Location = new System.Drawing.Point(333, 66);
            this.btnWaitAQ.Name = "btnWaitAQ";
            this.btnWaitAQ.Size = new System.Drawing.Size(131, 23);
            this.btnWaitAQ.TabIndex = 1;
            this.btnWaitAQ.Text = "Please Wait";
            this.btnWaitAQ.UseMnemonic = false;
            this.btnWaitAQ.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(285, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(202, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "send a request for landing !!";
            // 
            // txtArP
            // 
            this.txtArP.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtArP.Enabled = false;
            this.txtArP.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArP.Location = new System.Drawing.Point(36, 12);
            this.txtArP.Name = "txtArP";
            this.txtArP.Size = new System.Drawing.Size(243, 31);
            this.txtArP.TabIndex = 4;
            this.txtArP.Text = "plane";
            // 
            // RequestArrivalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 108);
            this.Controls.Add(this.txtArP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnWaitAQ);
            this.Controls.Add(this.btnAddToAQ);
            this.Location = new System.Drawing.Point(600, 200);
            this.Name = "RequestArrivalForm";
            this.Text = "RequestArrivalForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddToAQ;
        private System.Windows.Forms.Button btnWaitAQ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtArP;
    }
}