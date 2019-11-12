using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace VlcDotNetTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load( object sender, EventArgs e )
        {
            Width = 1080;
            Height = 1920;

            VlcControl = VlcHelper.CreateVlcControl(this );
        }

        public VlcControl VlcControl { get; set; }
    }
}
