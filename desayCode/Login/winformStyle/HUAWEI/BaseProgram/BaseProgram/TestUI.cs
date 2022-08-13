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
            //double Yiled;
            C1.PNumber = Convert.ToInt32(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "PASS", "0"));
            C1.SNumber =Convert.ToInt32(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "SUM", "0"));
            string XML_FilePath = FilePath;
            //T1.PassLab = lblOKCount1;
            //T1.lblTime = lblTestTime1;
            T1.DataGrid = dgvTest;
            T1.dgvTest = dgvTest;
            T1.lblRes = lab_Channel1;
            T1.Timer = label5;
            //T1.lblOKCount = lblOKCount1;
            //T1.control = txtBarcode1;
            //T1.SumLab = labelSUM1;
            //T1.TestLab = lblFPY1;
            //T1.ClearButton = lblTestTime1;
            //if (T1.Connnect != false)
            //{
            //    T1.ChangeXML(FilePath);
            //}
            //else     
            //{
            //T1.CreateChannel(FilePath, IP, out Yiled, 1, c34661, BaudRate, br, Win32API.INIGetStringValue(iniFilePath, "Channel1", "COMadressFix", ""));
            T1.CreateChannel("COM1",38400, IP, RelayIP, 1, XML_FilePath,C1);           //}
            //string TestDeviceNumber = "";
            //for (int i = 0; i < 10; i++)
            //{
            //    TestDeviceNumber = T1.TestFunction.GetString("C5-BB", 50).Replace(" ", "");
            //    if (TestDeviceNumber != "")
            //    {
            //        break;
            //    }
            //}
            //Win32API.INIWriteValue(ScanerFuuPath, "1", "DeviceNumber", TestDeviceNumber);
            //if (TestDeviceNumber != null)
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        if (DB.GetSqlserverNumber(ScanerFuuPath, TestDeviceNumber.Trim(), 1))
            //        {
            //            break;
            //        }
            //        TestDeviceNumber = T1.TestFunction.GetString("C5-BB", 50).Replace(" ", "");
            //        if (i == 2)
            //        {
            //            MessageBox.Show("通道1未成功");
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("通道1设备未曾使用过，请先加载参数档案获取DeviceKey");
            //}
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
    }
}
