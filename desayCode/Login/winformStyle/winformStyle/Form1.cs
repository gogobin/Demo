using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseProgram;

namespace winformStyle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Customsize();
        }
        private void Customsize()
        {
            MenuPanel.Visible = false;
            panel_seismometer.Visible = false;
            panel_Manager.Visible = false;
        }
        FormManage fm = new FormManage();


        private Form activeForm = null;
        private void openChildFormInPanel(Form childForm)
        {
            //if (activeForm != null)
            //    activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            //childForm.Parent = panelChildForm;
            panelChildForm.Controls.Add(childForm);
            panelChildForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            //bunifuTransition1.ShowSync(childForm);
        }

        private void button_Administrator_Click(object sender, EventArgs e)
        {
            if (fm.IsDisposed)
            {
                fm = new FormManage();
            }
            openChildFormInPanel(fm);
        }
        MainForm MF = new MainForm();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button_Menu_Click_1(object sender, EventArgs e)
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

        private void btnHuawei_Click_1(object sender, EventArgs e)
        {
            if (MF.IsDisposed)
            {
                MF = new MainForm();
            }
            openChildFormInPanel(MF);
        }

        private void Button_seismometer_Click_1(object sender, EventArgs e)
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
    }
}
