namespace MachineSlot
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.glControl1 = new OpenTK.GLControl();
            this.button_spin = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_win = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button_less = new System.Windows.Forms.Button();
            this.button_more = new System.Windows.Forms.Button();
            this.label_bet = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_credite = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_add_credits = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(13, 13);
            this.glControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(842, 423);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            // 
            // button_spin
            // 
            this.button_spin.Location = new System.Drawing.Point(862, 394);
            this.button_spin.Name = "button_spin";
            this.button_spin.Size = new System.Drawing.Size(262, 42);
            this.button_spin.TabIndex = 1;
            this.button_spin.Text = "SPIN";
            this.button_spin.UseVisualStyleBackColor = true;
            this.button_spin.Click += new System.EventHandler(this.button_spin_Click_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_win);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.button_less);
            this.panel1.Controls.Add(this.button_more);
            this.panel1.Controls.Add(this.label_bet);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label_credite);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(862, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(262, 327);
            this.panel1.TabIndex = 2;
            // 
            // label_win
            // 
            this.label_win.AutoSize = true;
            this.label_win.Location = new System.Drawing.Point(51, 106);
            this.label_win.Name = "label_win";
            this.label_win.Size = new System.Drawing.Size(36, 16);
            this.label_win.TabIndex = 7;
            this.label_win.Text = "{win}";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Win :";
            // 
            // button_less
            // 
            this.button_less.Location = new System.Drawing.Point(139, 69);
            this.button_less.Name = "button_less";
            this.button_less.Size = new System.Drawing.Size(106, 23);
            this.button_less.TabIndex = 5;
            this.button_less.Text = "LESS";
            this.button_less.UseVisualStyleBackColor = true;
            this.button_less.Click += new System.EventHandler(this.button_less_Click);
            // 
            // button_more
            // 
            this.button_more.Location = new System.Drawing.Point(15, 69);
            this.button_more.Name = "button_more";
            this.button_more.Size = new System.Drawing.Size(120, 23);
            this.button_more.TabIndex = 4;
            this.button_more.Text = "MORE";
            this.button_more.UseVisualStyleBackColor = true;
            this.button_more.Click += new System.EventHandler(this.button_more_Click);
            // 
            // label_bet
            // 
            this.label_bet.AutoSize = true;
            this.label_bet.Location = new System.Drawing.Point(51, 38);
            this.label_bet.Name = "label_bet";
            this.label_bet.Size = new System.Drawing.Size(36, 16);
            this.label_bet.TabIndex = 3;
            this.label_bet.Text = "{bet}";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Bet :";
            // 
            // label_credite
            // 
            this.label_credite.AutoSize = true;
            this.label_credite.Location = new System.Drawing.Point(77, 10);
            this.label_credite.Name = "label_credite";
            this.label_credite.Size = new System.Drawing.Size(58, 16);
            this.label_credite.TabIndex = 1;
            this.label_credite.Text = "{credite}";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Credite : ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button_add_credits
            // 
            this.button_add_credits.Location = new System.Drawing.Point(862, 346);
            this.button_add_credits.Name = "button_add_credits";
            this.button_add_credits.Size = new System.Drawing.Size(262, 42);
            this.button_add_credits.TabIndex = 3;
            this.button_add_credits.Text = "Adauga credite (+100)";
            this.button_add_credits.UseVisualStyleBackColor = true;
            this.button_add_credits.Click += new System.EventHandler(this.button_add_credits_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1136, 449);
            this.Controls.Add(this.button_add_credits);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_spin);
            this.Controls.Add(this.glControl1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.Button button_spin;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_less;
        private System.Windows.Forms.Button button_more;
        private System.Windows.Forms.Label label_bet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_credite;
        private System.Windows.Forms.Label label_win;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_add_credits;
    }
}

