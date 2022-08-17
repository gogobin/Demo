using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using wpfSetting;

namespace BaseProgram
{
    public partial class Form1 : Form
    {
        wpfWindows main;
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Application.EnableVisualStyles();
            main = new wpfWindows();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ElementHost.EnableModelessKeyboardInterop(main);
            main.Show();
        }
    }
}
