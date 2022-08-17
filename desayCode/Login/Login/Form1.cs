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
using Login;

namespace winformStyle
{
    public partial class FormSet : Form
    {
        public FormSet()
        {
            InitializeComponent();
            System.Windows.Forms.Application.EnableVisualStyles();
            Customsize();
        }
        private void Customsize()
        {
            MenuPanel.Visible = false;
            panel_seismometer.Visible = false;
            panel_Manager.Visible = false;
        }
        FormManage fm = new FormManage();
        private void button_Menu_Click(object sender, EventArgs e)
        {
            if (MenuPanel.Visible == false)
            {
                MenuPanel.Visible = true;
            }
            else if (MenuPanel.Visible == true)
            {
                MenuPanel.Visible = false;
            }
        }

        private void Button_seismometer_Click(object sender, EventArgs e)
        {
            if (panel_seismometer.Visible == false)
            {
                panel_seismometer.Visible = true;
            }
            else if (panel_seismometer.Visible == true)
            {
                panel_seismometer.Visible = false;
            }
        }

        private void button_Manager_Click(object sender, EventArgs e)
        {
            if (panel_Manager.Visible == false)
            {
                panel_Manager.Visible = true;
            }
            else if (panel_Manager.Visible == true)
            {
                panel_Manager.Visible = false;
            }
        }
        private Form activeForm = null;
        private void openChildFormInPanel(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            //childForm.Parent = panelChildForm;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void button_Administrator_Click(object sender, EventArgs e)
        {
            if (fm.IsDisposed)
            {
                fm = new FormManage();
            }
            openChildFormInPanel(fm);
        }

        private void btnHuawei_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        ToolForm ToolForm=new ToolForm();
        private void button10_Click(object sender, EventArgs e)
        {
            if (ToolForm.IsDisposed)
            {
                ToolForm = new ToolForm();
            }
            ToolForm.Show();
        }
        Setting ST;
        private void button9_Click(object sender, EventArgs e)
        {
            ST = new Setting();
            ST.ShowDialog();
        }
    }
}
