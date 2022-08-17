using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IC.NOGagueIC.SW;
using ControlProgram;
using SettingForm;

namespace WatchProgram
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void 参数设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmParaMter frmPara = new frmParaMter();
            frmPara.ShowDialog();
        }

        private void 调试工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolForm toolForm = new ToolForm();
            toolForm.Show();
        }
        string xml_FilePath = "";
        private void 加载档案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();//一个打开文件的对话框
            openFileDialog1.Filter = "xml(*.xml)|*.xml";//设置允许打开的扩展名
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xml_FilePath = openFileDialog1.FileName;//记录用户选择的文件路径
                string Msg = "";
                string[] ChannelSection = Win32API.INIGetAllSectionNames("C:\\DataLog\\BaseSetting.ini");
                testUI1.Connect_aglient("");
                foreach (var section in ChannelSection)
                {
                    if (section.Contains("Channel1"))
                    {
                        testUI1.Connect_Channel1(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "ComIP", ""), xml_FilePath, out Msg, 1, 38400, Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "RelayIP", ""));
                    }
                }
            }
        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm.SettingForm ST = new SettingForm.SettingForm();
            ST.ShowDialog();
        }

        private void 开始测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainClass.IsTest = true;

        }
    }
    public static class MainClass
    {
        public static bool IsTest = false;
        public static string Barcode = "";
    }
}
