using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using winformStyle;
using Panuon;
using Panuon.UI.Silver;

namespace Login
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Forms.Application.EnableVisualStyles();

        }
        SqlConnection sqlConnection;
        SqlDataAdapter myAdp;
        public void DataBaseClass(string databaseAdress, string dataBaseName, string user, string pwd)
        {
            string str = "server=" + databaseAdress + ";" + "database=" + dataBaseName + ";" + "uid=" + user + ";" + "pwd=" + pwd;
            sqlConnection = new SqlConnection(str);
            for (int i = 0; i < 10; i++)
            {
                sqlConnection.Open();
                if (sqlConnection.State == ConnectionState.Open)
                {
                    break;
                }
            }
        }
        public bool UserConfirm(string UserName, string Pwd)
        {
            string sql = "SELECT * FROM [dbo].[UserList] where Name='" + UserName + "'";
            myAdp = new SqlDataAdapter(sql, sqlConnection);
            DataSet ds = new System.Data.DataSet();
            System.Threading.Thread.Sleep(80);
            myAdp.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                //MessageBox.Show("未能查找到用户名");
                Notice.Show("未能查找到用户名称", "Notification",3,MessageBoxIcon.Error);
                return false;
            }
            else
            {
                string Password = Convert.ToString(ds.Tables[0].Rows[0]["Password"]);
                if (Password.Trim() == Pwd.Trim())
                {
                    return true;
                }
                else
                {
                    //MessageBox.Show("用户密码错误");
                    Notice.Show("用户密码错误","Notification",3,MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {
            }

        }

        private void ButtonPopLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        Setting ST;
        FormSet dialog;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataBaseClass("10.12.1.20", "BTS", "sa", "Desayte123");
            if (UserConfirm(txtUserName.Text, txtPassWord.Password))
            {
                dialog = new FormSet();
                this.Hide();
                dialog.ShowDialog();
                System.Diagnostics.Process.GetCurrentProcess().Close();
                Application.Current.Shutdown();
            }
        }

        private void DataPara_Click(object sender, RoutedEventArgs e)
        {
            UserControl1 user = new UserControl1();
            
            //ST = new Setting();
            //ST.Show();
            //this.Close();
        }
    }
}
