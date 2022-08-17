using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace SettingForm
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            DataGridSet.Rows.Add(4);
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

        private void 关闭界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("是否保存配置","Notification",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.OK)
            {

            }
            else
            {
                this.Close();
            }
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            DataGrid_Scanner.DataSource = SerialPort.GetPortNames();
            if (!Directory.Exists("C:\\DataLog"))
            {
                Directory.CreateDirectory("C:\\DataLog");
            }
            if (!File.Exists("C:\\DataLog\\BaseSetting.ini"))
            {
                File.Create("C:\\DataLog\\BaseSetting.ini").Dispose();
                return;
            }
            DataGridSet.Rows[0].Cells[0].Value=Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "RelayIP", "");
            DataGridSet.Rows[0].Cells[1].Value=Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel1", "ComIP", "");
            DataGridSet.Rows[1].Cells[0].Value=Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel2", "RelayIP","");
            DataGridSet.Rows[1].Cells[1].Value = Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel2", "ComIP", "");
            DataGridSet.Rows[2].Cells[0].Value = Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel3", "RelayIP", "");
            DataGridSet.Rows[2].Cells[1].Value = Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel3", "ComIP", "");
            DataGridSet.Rows[3].Cells[0].Value = Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel4", "RelayIP", "");
            DataGridSet.Rows[3].Cells[1].Value = Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel4", "ComIP", "");
            for (int i = 0; i < DataGridSet.Rows.Count; i++)
            {
                try
                {
                    DataGridSet.Rows[i].Cells[2].Value = Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel" + (i + 1).ToString(), "ComPort", "");
                    DataGridSet.Rows[i].Cells[3].Value =Convert.ToBoolean(Win32API.INIGetStringValue("C:\\DataLog\\BaseSetting.ini", "Channel" + (i + 1).ToString(), "StartUp", ""));
                }
                catch
                {
                    continue;
                }
            }
        }

        private void 保存设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < DataGridSet.Rows.Count; i++)
                {
                    Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel" + (i + 1).ToString(), "RelayIP", DataGridSet.Rows[i].Cells[0].EditedFormattedValue.ToString());
                    Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel" + (i + 1).ToString(), "ComIP", DataGridSet.Rows[i].Cells[1].EditedFormattedValue.ToString());
                    Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel" + (i + 1).ToString(), "ComPort", DataGridSet.Rows[i].Cells[2].EditedFormattedValue.ToString());
                    Win32API.INIWriteValue("C:\\DataLog\\BaseSetting.ini", "Channel" + (i + 1).ToString(), "StartUp", DataGridSet.Rows[i].Cells[3].EditedFormattedValue.ToString());
                }
                MessageBox.Show("保存配置成功");
            }
            catch
            {
                MessageBox.Show("保存配置失败", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }
        }
    }
}
