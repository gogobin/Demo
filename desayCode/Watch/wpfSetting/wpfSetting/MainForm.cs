using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ControlProgram;

namespace wpfSetting
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            System.Windows.Forms.Application.EnableVisualStyles();
            Application.EnableVisualStyles();
        }
        Setting app;
        private void 参数设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Setting setting = new Setting();
            //setting.ShowDialog();
            Setting app = new Setting();
            app.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        ToolForm toolForm;
        private void 调试工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolForm = new ToolForm();
            toolForm.Show();
        }
    }
}
