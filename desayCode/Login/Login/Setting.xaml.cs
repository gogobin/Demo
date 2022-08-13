using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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
using System.Windows.Shapes;
using System.Xml;

namespace Login
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        List<string> Box;
        public Setting()
        {
            InitializeComponent();
            Box = new List<string>();
            Box.Add("Apple");
            Box.Add("tree");
            Box.Add("Lemon");
            ListView = Box.ToArray();
            this.DemoItemsListBox.DataContext = ListView;
            grd.CanUserAddRows = false;
            grd.CanUserAddRows = false;
            contextmenu = new ContextMenu();
            grd.ContextMenu = contextmenu;
            var mi = new MenuItem();
            mi.Header = "Operate";
            var mia = new MenuItem();
            mia.Header = "New";
            mi.Items.Add(mia);
            mia.Click += Mia_Click;
            var mib = new MenuItem();
            mib.Header = "delete";
            mib.Click += Mib_Click;
            mi.Items.Add(mib);
            var mic = new MenuItem();
            mic.Header = "detail";
            mi.Items.Add(mic);
            mic.Click += Mic_Click;
            contextmenu.Items.Add(mi);
            ICollectionView view = CollectionViewSource.GetDefaultView(ListView);
            new TextSearchFilter(view, this.DemoItemsSearchBox);
        }

        private void Mic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataItem selData = grd.SelectedItem as DataItem;
                DetailForm DF = new DetailForm(selData.Name,Box,selData);
                DF.ShowDialog();
            }
            catch(Exception ex)
            {
                DetailForm DF = new DetailForm("", Box,null);
                DF.ShowDialog();
            }
        }
        private void Mib_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataItem selData = grd.SelectedItem as DataItem;
                int index = users.IndexOf(selData);
                users.Remove(selData);
            }
            catch
            {

            }
        }

        private void Mia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataItem selData = grd.SelectedItem as DataItem;
                int index = users.IndexOf(selData);
                users.Insert(index, new DataItem());
            }
            catch
            {
                grd.ItemsSource = null;
                users = new ObservableCollection<DataItem>();
                users.Add(new DataItem());
                grd.ItemsSource = users;
            }
        }

        string[] ListView;
        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            { }
        }

        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<DataItem> ls = users.ToList();
            this.Close();
        }

        private void Maxsize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void Normalsize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }
        ObservableCollection<DataItem> users;
        ContextMenu contextmenu;
        private bool LoadLocalFile()
        {
            string xml_FilePath = "";
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();//一个打开文件的对话框
            openFileDialog1.Filter = "xml(*.xml)|*.xml";//设置允许打开的扩展名
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)//判断是否选择了文件  
            {
                xml_FilePath = openFileDialog1.FileName;//记录用户选择的文件路径
                XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”
                xmlDocument.Load(xml_FilePath);//载入路径这个xml
                users = new ObservableCollection<DataItem>();
                try
                {
                    grd.ItemsSource = null;
                    XmlNodeList xmlSeal = xmlDocument.SelectSingleNode("/Test/TestStep").ChildNodes;//选择class为根结点并得到旗下所有子节点
                    foreach (XmlNode xmlNode in xmlSeal)//遍历class的所有节点
                    {
                        XmlElement xmlElement = (XmlElement)xmlNode;//对于任何一个元素，其实就是每一个<student>
                                                                    //旗下的子节点<name>和<number>分别放入dataGridView1
                                                                    //int index = dgvTestStep.Rows.Add();//在dataGridView1新加一行，并拿到改行的行标
                                                                    //grd.Items.Add(xmlElement);
                        //string aa=xmlElement.ChildNodes.Item(3).InnerText;
                        if (xmlElement.ChildNodes.Item(2).InnerText.Replace(" ", "").Trim() == "")
                        {
                            users.Add(new DataItem { Id = xmlElement.ChildNodes.Item(0).InnerText, Name = xmlElement.ChildNodes.Item(1).InnerText, skip = Convert.ToBoolean(xmlElement.ChildNodes.Item(6).InnerText), Retry = 1,ErrorCode=xmlElement.ChildNodes.Item(4).InnerText,LogName=xmlElement.ChildNodes.Item(5).InnerText,MinValue=xmlElement.ChildNodes.Item(7).InnerText,MaxValue=xmlElement.ChildNodes.Item(8).InnerText,ParaMeter1=xmlElement.ChildNodes.Item(9).InnerText,ParaMeter2=xmlElement.ChildNodes.Item(10).InnerText,ParaMeter3=xmlElement.ChildNodes.Item(11).InnerText});
                        }
                        else
                        {
                            users.Add(new DataItem { Id = xmlElement.ChildNodes.Item(0).InnerText, Name = xmlElement.ChildNodes.Item(1).InnerText, skip = Convert.ToBoolean(xmlElement.ChildNodes.Item(6).InnerText), Retry=Convert.ToInt32(xmlElement.ChildNodes.Item(2).InnerText),ErrorCode=xmlElement.ChildNodes.Item(4).InnerText,LogName=xmlElement.ChildNodes.Item(5).InnerText,MinValue=xmlElement.ChildNodes.Item(7).InnerText,MaxValue=xmlElement.ChildNodes.Item(8).InnerText,ParaMeter1=xmlElement.ChildNodes.Item(9).InnerText,ParaMeter2=xmlElement.ChildNodes.Item(10).InnerText,ParaMeter3=xmlElement.ChildNodes.Item(11).InnerText});
                        }
                    }
                    grd.ItemsSource = users;
                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("XML format is not correct.");
                }
            }
            else
            {
                //MessageBox.Show("请打开XML文件");
            }
            return true;
        }
        public static DataTable Query(string sql, string filePath)
        {
            using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath))
            {
                using (OleDbDataAdapter sda = new OleDbDataAdapter(sql, conn))
                {
                    try
                    {
                        sda.SelectCommand.CommandType = CommandType.Text;
                        //sda.SelectCommand.Parameters.AddRange(dbp);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        private void btnup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataItem selData = grd.SelectedItem as DataItem;
                int index = users.IndexOf(selData);
                users.Move(index, index -1);
            }
            catch
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            LoadLocalFile();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataItem selData = grd.SelectedItem as DataItem;
                int index = users.IndexOf(selData);
                users.Move(index, index +1);
            }
            catch
            {
            }

        }

        private void PlaceholdersListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                string hel = item.Content.ToString();
            }
        }
    }

    public class DataItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _Name;
        private string _Id;
        private int _retry;
        private string _error_Code;
        private bool _skip;
        private string _MinValue;
        private string _MaxValue;
        private string _ParaMeter1;
        private string _ParaMeter2;
        private string _ParaMeter3;
        private string _LogName;
        private string _unit;

        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public int Retry
        {
            get
            {
                return _retry;
            }
            set
            {
                _retry = value;
                NotifyPropertyChanged("retry");
            }
        }
        public string LogName
        {
            get
            {
                return _LogName;
            }
            set
            {
                _LogName = value;
                NotifyPropertyChanged("LogName");
            }
        }
        public string ErrorCode
        {
            get
            {
                return _error_Code;
            }
            set
            {
                _error_Code = value;
                NotifyPropertyChanged("ErrorCode");
            }
        }

        public bool skip
        {
            get
            {
                return _skip;
            }
            set
            {
                _skip = value;
                NotifyPropertyChanged("skip");
            }
        }


        public string MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                _MinValue = value;
                NotifyPropertyChanged("MinValue");
            }
        }

        public string MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                _MaxValue = value;
                NotifyPropertyChanged("MaxValue");
            }
        }

        public string ParaMeter1
        {
            get
            {
                return _ParaMeter1;
            }
            set
            {
                _ParaMeter1 = value;
                NotifyPropertyChanged("ParaMeter1");
            }
        }

        public string ParaMeter2
        {
            get
            {
                return _ParaMeter2;
            }
            set
            {
                _ParaMeter2 = value;
                NotifyPropertyChanged("ParaMeter2");
            }
        }
        public string ParaMeter3
        {
            get
            {
                return _ParaMeter3;
            }
            set
            {
                _ParaMeter3 = value;
                NotifyPropertyChanged("ParaMeter3");
            }
        }

        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
                NotifyPropertyChanged("unit");
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class TextSearchFilter
    {
        public TextSearchFilter(ICollectionView filteredView, TextBox textBox)
        {
            string filterText = "";
            filteredView.Filter = delegate (object obj)
              {
                  if (String.IsNullOrEmpty(filterText))
                      return true;
                  string str = obj as string;
                  if (String.IsNullOrEmpty(str))
                  {
                      return false;
                  }
                  int index = str.IndexOf(filterText, 0, StringComparison.InvariantCultureIgnoreCase);
                  return index > -1;
              };
            textBox.TextChanged += delegate
            {
                filterText = textBox.Text;
                filteredView.Refresh();
            };
        }

    }
}
