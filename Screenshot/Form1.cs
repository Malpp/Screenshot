using System;
using System.Drawing;
using System.Windows.Forms;

namespace Screenshot
{
    public partial class Form1 : Form
    {

        #region Public Constructors

        public Form1()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Form1_Load(object sender, EventArgs e)
        {

            this.TopMost = true;
            //this.Focus();
            this.BringToFront();
            this.TopMost = false;

            //this.Size = new System.Drawing.Size(screenWidth, screenHeight);
            //this.Location = new System.Drawing.Point(screenLeft, screenTop);
        }

        #endregion Private Methods

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("REEEEEEEEEEEEEEEEEE");
        }
    }
}