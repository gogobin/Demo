using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace BaseProgram
{
    public partial class TestUI : UserControl
    {
        public TestUI()
        {
            InitializeComponent();
            bsA.Add(C1);
            //bsB.Add(C2);
            //bsC.Add(C3);
            //bsD.Add(C4);
            labSUM_Channel1.DataBindings.Add("Text", bsA, "SNumber");
            //labSUM_Channel2.DataBindings.Add("Text", bsB, "SNumber");
            //labSUM_Channel3.DataBindings.Add("Text", bsC, "SNumber");
            //labSUM_Channel4.DataBindings.Add("Text", bsD, "SNumber");
            labPass_Channel1.DataBindings.Add("Text", bsA, "PNumber");
            //labPass_Channel2.DataBindings.Add("Text", bsB, "PNumber");
            //labPass_Channel3.DataBindings.Add("Text", bsC, "PNumber");
            //labPass_Channel4.DataBindings.Add("Text", bsD, "PNumber");
        }
        ChannelNumber C1 = new ChannelNumber();
        ChannelNumber C2 = new ChannelNumber();
        ChannelNumber C3 = new ChannelNumber();
        ChannelNumber C4 = new ChannelNumber();
        BindingSource bsA = new BindingSource(); // Channel1
        BindingSource bsB = new BindingSource(); // Channel2
        BindingSource bsC = new BindingSource(); // Channel3
        BindingSource bsD = new BindingSource(); // Channel4
        public TestClass T1 = new TestClass();
        public bool Connect_Channel1(string IP, string FilePath, out string Msg, int channel, int BaudRate,string RelayIP)
        {
            C1.PNumber = Convert.ToInt32(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "PASS", "0"));
            C1.SNumber =Convert.ToInt32(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "SUM", "0"));
            string XML_FilePath = FilePath;
            string[] FileLisr = XML_FilePath.Split(new char[] { '\\' });
            groupBox1.Text=FileLisr[FileLisr.Length - 1].Split(new char[] { '.' })[0];
            T1.DataGrid = dgvTest;
            T1.dgvTest = dgvTest;
            T1.lblRes = lab_Channel1;
            T1.Timer = label5;
            T1.CreateChannel("COM1",38400, IP, RelayIP, 1, XML_FilePath,C1);
            Msg = "";
            return true;
        }

        private void 查看优率ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists("D:\\Data\\txt"))
                {
                    return;
                }
                System.Diagnostics.Process.Start("D:\\Data\\txt\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-" +"1"+ ".txt");
            }
            catch
            {

            }
        }

        private void labSUM_Channel1_TextChanged(object sender, EventArgs e)
        {
            double FPY = 0;
            if (C1.SNumber!=0)
            {
                FPY = double.Parse(C1.PNumber.ToString()) / double.Parse(C1.SNumber.ToString());
            }
            Channel1.Text = FPY.ToString("P");
            Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "SUM", C1.SNumber.ToString());
            Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "PASS", C1.PNumber.ToString());
        }

        private void 清空计数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "SUM","0");
            Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "PASS", "0");
            C1.PNumber = 0;
            C1.SNumber = 0;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MainClass.IsTest = true;
                MainClass.Barcode = textBox1.Text;
                textBox1.SelectAll();
            }
        }
    }
}
