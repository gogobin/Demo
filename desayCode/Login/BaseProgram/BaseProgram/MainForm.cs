using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ControlProgram;
using System.Runtime.InteropServices;
using SettingForm;
using System.Reflection;
using IC.NOGagueIC.SW;
using System.Threading;

namespace BaseProgram
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = Assembly.GetExecutingAssembly().GetName().Name;
        }

        private void 调试工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolForm TF = new ToolForm();
            TF.ShowDialog();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void skinMenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void 最大化窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void 最小化窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void 关闭窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否关闭程序", "Note",MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void 系统设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm.SettingForm ST = new SettingForm.SettingForm();
            ST.ShowDialog();
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void normalsizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void 参数设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmParaMter frm = new frmParaMter();
            frm.ShowDialog();
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
                string[] ChannelSection=Win32API.INIGetAllSectionNames("C:\\DataLog\\BaseSetting.ini");
                foreach (var section in ChannelSection)
                {
                    if (section.Contains("Channel1"))
                    {
                        testUI1.Connect_Channel1(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "ComIP", ""), xml_FilePath, out Msg, 1, 38400, Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "RelayIP", ""));
                    }
                }
            }
        }

        private void 开始测试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainClass.IsTest = true;
        }

        private void 开始测试ToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            MainClass.IsTest = true;
        }

        private void 查看参数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Parameter pr = new Parameter(xml_FilePath);
                pr.Show();
            }
            catch (Exception ex)
            {

            }
        }
    }
    public static class MainClass
    {
        public static bool IsTest = false;
        public static string Barcode = "";
    }
}
