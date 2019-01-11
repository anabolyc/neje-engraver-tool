using NejeEngraverApp.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NejeEngraverApp
{
    public class Form_Loading : Form
    {
        private IContainer components;

        private Label label1;

        private PictureBox pictureBox1;

        public Form_Loading()
        {
            this.InitializeComponent();
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
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form_Loading));
            this.label1 = new Label();
            this.pictureBox1 = new PictureBox();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(122, 199);
            this.label1.Name = "label1";
            this.label1.Size = new Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please Wait...";
            this.pictureBox1.Image = Resources.Sending;
            this.pictureBox1.Location = new Point(98, 49);
            this.pictureBox1.Margin = new Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(131, 119);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(331, 238);
            base.Controls.Add(this.pictureBox1);
            base.Controls.Add(this.label1);
            this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Icon = NejeEngraverApp.Properties.Form_Loading.Icon;
            base.Margin = new Padding(3, 4, 3, 4);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "Form_Loading";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Sending...";
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
