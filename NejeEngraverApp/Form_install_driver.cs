using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NejeEngraverApp
{
    public class Form_install_driver : Form
    {
        private IContainer components;

        private Label label1;

        private Button button_install;

        public Form_install_driver()
        {
            this.InitializeComponent();
        }

        private void button_install_Click(object sender, EventArgs e)
        {
            Process.Start("C:\\NEJE\\Driver\\driver.exe");
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.button_install = new Button();
            base.SuspendLayout();
            this.label1.Font = new Font("Arial", 12f);
            this.label1.Location = new Point(41, 25);
            this.label1.Name = "label1";
            this.label1.Size = new Size(226, 106);
            this.label1.TabIndex = 0;
            this.label1.Text = "Have you install the driver?\r\nyou need install the driver at the first time.\r\nclick the button below to install driver.";
            this.button_install.Location = new Point(66, 150);
            this.button_install.Name = "button_install";
            this.button_install.Size = new Size(174, 38);
            this.button_install.TabIndex = 1;
            this.button_install.Text = "Install Driver";
            this.button_install.UseVisualStyleBackColor = true;
            this.button_install.Click += new EventHandler(this.button_install_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(310, 216);
            base.Controls.Add(this.button_install);
            base.Controls.Add(this.label1);
            this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "Form_install_driver";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Information";
            base.ResumeLayout(false);
        }
    }
}
