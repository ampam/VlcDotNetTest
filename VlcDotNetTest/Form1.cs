using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace VlcDotNetTest
{
    public partial class Form1 : Form
    {
        public VlcControl VlcControl { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Top = 0;
            Left = 0;
            Width = 1080;
            Height = 1920;

            VlcControl = VlcHelper.CreateVlcControl(this);


        }


        private void Form1_Shown( object sender, EventArgs e )
        {
            var form2 = new Form2 {Width = 1080, Height = 1920, Left = 1200};
            form2.Show();
        }
    }
}
