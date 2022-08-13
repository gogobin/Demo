using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string filedialog;
        public Form1()
        {
            InitializeComponent();
        }


        public void MethodCreate(string Path)
        {
            string[] values=Path.Split('\\');
            //System.IO.DriveInfo[] driveInfos = System.IO.DriveInfo.GetDrives();
            //var scoreQuery = from vd in driveInfos
            //                 where vd.Name.Contains(values[0])
            //                 select vd;
            string value=Path.Replace(values[values.Count() - 1], "");
            if (!Directory.Exists(value))
            {
                Directory.CreateDirectory(Path.Replace(values[values.Count() - 1],""));
            }
            //List<string> vs1 = new List<string>();
            //foreach (var vs in scoreQuery)
            //{
            //    vs1.Add(vs.Name);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog.SelectedPath;
                filedialog = dialog.SelectedPath;
                //MethodCreate(filedialog);
            }
            MethodCreate("C:\\dirt\\etc\\acir.txt");



        }
    }
}
