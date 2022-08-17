using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlProgram;

namespace WatchProgram
{
    public partial class TestControl : UserControl
    {
        public TestControl()
        {
            InitializeComponent();
            Source1.Add(channel1);
            labSUM_Channel1.DataBindings.Add("Text", Source1, "SNumber");
            labPass_Channel1.DataBindings.Add("Text", Source1, "PNumber");

        }
        public void Connect_aglient(string aglientAddress)
        {
            agilent = new CAgilent("192.168.1.61", CAgilent.ControlMode.TCPMode);
        }
        CAgilent agilent;
        #region//定义数据绑定
        ChannelNumber channel1 = new ChannelNumber();
        ChannelNumber channel2 = new ChannelNumber();
        ChannelNumber channel3 = new ChannelNumber();
        ChannelNumber channel4 = new ChannelNumber();
        BindingSource Source1 = new BindingSource();
        BindingSource Source2 = new BindingSource();
        BindingSource Source3 = new BindingSource();
        BindingSource Source4 = new BindingSource();
        #endregion
        public TestClass T1 = new TestClass();
        public bool Connect_Channel1(string IP, string FilePath, out string Msg, int channel, int BaudRate, string RelayIP)
        {
            //double Yiled;
            channel1.PNumber = Convert.ToInt32(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "PASS", "0"));
            channel1.SNumber = Convert.ToInt32(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "SUM", "0"));
            string XML_FilePath = FilePath;
            string[] FileLisr = XML_FilePath.Split(new char[] { '\\' });
            groupBox1.Text = FileLisr[FileLisr.Length - 1].Split(new char[] { '.' })[0];
            T1.dgvTest = dgvTest;
            T1.lblRes = lab_Channel1;
            T1.Timer = label5;
            T1.CreateChannel("COM1", 38400, IP, RelayIP, 1, XML_FilePath,channel1,agilent);
            Msg = "";
            return true;
        }
    }
}
