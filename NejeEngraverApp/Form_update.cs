using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using NejeEngraverApp.Properties;

namespace NejeEngraverApp
{
    public class Form_update : Form
    {
        private string new_version;

        private string old_version;

        private string update_link;

        private IContainer components;

        private Button button1;

        private CheckBox checkBox1;

        private Label label1;

        private Label label_old_version;

        private Label label_new_version;

        private Label label_link;

        private LinkLabel linkLabel1;

        public Form_update()
        {
            this.InitializeComponent();
        }

        public Form_update(string old_version, string new_version, string update_link)
        {
            this.InitializeComponent();
            this.old_version = old_version;
            this.new_version = new_version;
            this.update_link = update_link;
            this.label_old_version.Text = this.label_old_version.Text + old_version;
            this.label_new_version.Text = this.label_new_version.Text + new_version;
            this.linkLabel1.Text = update_link;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start(this.update_link);
            base.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                Settings.Default.Upgrade_remind = false;
                Settings.Default.Save();
                return;
            }
            Settings.Default.Upgrade_remind = true;
            Settings.Default.Save();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.button1_Click(null, null);
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
            this.button1 = new Button();
            this.checkBox1 = new CheckBox();
            this.label1 = new Label();
            this.label_old_version = new Label();
            this.label_new_version = new Label();
            this.label_link = new Label();
            this.linkLabel1 = new LinkLabel();
            base.SuspendLayout();
            this.button1.Location = new Point(254, 197);
            this.button1.Margin = new Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new Size(168, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "Veiw Website";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(16, 210);
            this.checkBox1.Margin = new Padding(3, 4, 3, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(142, 19);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Don't show me again";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.label1.Font = new Font("Arial", 14f);
            this.label1.Location = new Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new Size(433, 68);
            this.label1.TabIndex = 2;
            this.label1.Text = "There is a new version of the software released, do you want to upgrade the software?";
            this.label_old_version.AutoSize = true;
            this.label_old_version.Location = new Point(13, 89);
            this.label_old_version.Name = "label_old_version";
            this.label_old_version.Size = new Size(144, 15);
            this.label_old_version.TabIndex = 3;
            this.label_old_version.Text = "current software version: ";
            this.label_new_version.AutoSize = true;
            this.label_new_version.Location = new Point(13, 115);
            this.label_new_version.Name = "label_new_version";
            this.label_new_version.Size = new Size(129, 15);
            this.label_new_version.TabIndex = 4;
            this.label_new_version.Text = "new software version: ";
            this.label_link.AutoSize = true;
            this.label_link.Location = new Point(12, 141);
            this.label_link.Name = "label_link";
            this.label_link.Size = new Size(108, 15);
            this.label_link.TabIndex = 5;
            this.label_link.Text = "new software link: ";
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new Point(13, 156);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(64, 15);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "linkLabel1";
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(457, 262);
            base.Controls.Add(this.linkLabel1);
            base.Controls.Add(this.label_link);
            base.Controls.Add(this.label_new_version);
            base.Controls.Add(this.label_old_version);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.button1);
            this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Margin = new Padding(3, 4, 3, 4);
            base.Name = "Form_update";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Update";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
