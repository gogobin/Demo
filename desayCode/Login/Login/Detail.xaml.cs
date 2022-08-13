using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Login
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class DetailForm : Window
    {
        DataTable dt;
        Dictionary<string, string> HeaderList = new Dictionary<string, string>();
        string[] ListView;
        ObservableCollection<DataItem> users;
        public DetailForm(string StepName,List<string> TestList,DataItem DItem)
        {
            InitializeComponent();
            dt = new DataTable();
            grd.CanUserAddRows = false;
            #region AddTestList
            HeaderList.Add("StartUp", "");
            HeaderList.Add("OCV_Left", "Voltage_Min,Voltage_Max,unit");
            HeaderList.Add("ACIR_Left", "Resistance_Min,Resistance_Max,unit");
            HeaderList.Add("ISolate_Resistance_Left", "Resistance_Min,Resistance_Max,unit");
            HeaderList.Add("ID_Resistance_Left", "Resistance_Min,Reistacne_Max,unit");
            HeaderList.Add("NTC_Left", "Resistance_Min,Reistacne_Max,Retry,unit");
            HeaderList.Add("ChargeCurrent_Left", "ChargeCurrent(p1),Current_Min,Current_Max,unit");
            HeaderList.Add("Charge_deltaVoltage_Left", "Voltage_Min,Voltage_Max,unit");
            HeaderList.Add("Docp_Left", "Current_Min,Current_Max,unit");
            HeaderList.Add("Docp_Delay", "Delay_Min,Delay_Max,unit");
            HeaderList.Add("ShortCirCuit_Protection", "Voltage_Min,Voltage_Max,unit");
            HeaderList.Add("OCV_Right", "Voltage_Min,Voltage_Max,unit");
            HeaderList.Add("ACIR_Right", "Resistance_Min,Resistacne_Max,unit");
            HeaderList.Add("ID_Resistance_Right", "Resistance_Min,Resistance_Max,unit");
            HeaderList.Add("ISolate_Resistance_Right", "Resistance_Min,Resistance_Max,unit");
            HeaderList.Add("NTC_Right", "Resistance_Min,Resistance_Max,unit");
            HeaderList.Add("DischargeCurrent_Right", "DisChargeCurrent(p1),Current_Min,Current_Max,unit");
            HeaderList.Add("Discharge_deltaVoltage_Right", "DeltaVoltage_Min,DeltaVoltage_Max,unit");
            HeaderList.Add("Vendor_ID", "Vender_ID(p1),unit");
            HeaderList.Add("Product_ID", "Product_ID(p1),unit");
            HeaderList.Add("ECC_Verify", "ECC_Verify(p1),unit");
            HeaderList.Add("Life_Span_Counter", "LifeSpanCounter,unit");
            HeaderList.Add("NVM_BarCode", "NVM_Number(p1),unit");
            HeaderList.Add("IC_Version", "IC_Version(p1),unit");
            HeaderList.Add("Lock_Status", "Lock_Status(p1),unit");
            HeaderList.Add("BarCode_Compare", "Status(p1),unit");
            HeaderList.Add("Chargeing_Leakage_Current_Right", "Current(p1),Current_min,Current_max,unit");
            HeaderList.Add("Chargeing_Leakage_Current_Left", "Current(p1),Current_min,Current_max,unit");
            HeaderList.Add("DisChargeing_Leakage_Current_Left", "Current(p1),Current_min,Current_max,unit");
            HeaderList.Add("TempSample", "Temperature_Min,Tempature_Max,unit");
            HeaderList.Add("TempDelta2", "DeltaTemperature_Min,DeltaTemperature,unit");
            HeaderList.Add("TempDelta1", "DeltaTemperature_Min,DeltaTemperature,unit");
            #endregion
            if (StepName =="")
            {
                return;
            }
            string[] ColList = Regex.Split(HeaderList[StepName], ",");
            foreach (var Col in ColList)
            {
                string ColName= Col.Replace("(p1)", "").Replace("(p2)", "").Replace("(p3)", "");
                dt.Columns.Add(new DataColumn(ColName));
            }
            txtFunction.Text = DItem.Name;
            txtRetry.Text = DItem.Retry.ToString();
            txtLogName.Text = DItem.LogName;
            txtErrorCode.Text = DItem.ErrorCode;
            CheckBoxSkip.IsChecked = DItem.skip;
            DataRow row = dt.NewRow();
            foreach (var col in ColList)
            {
                if (col.Contains("Min"))
                {
                    row[col] = DItem.MinValue;
                }
                else if (col.Contains("Max"))
                {
                    row[col] = DItem.MaxValue;
                }
                else if (col.Contains("unit"))
                {
                    row[col] = DItem.Unit;
                }
                else if (col.Contains("(p1)"))
                {
                    row[col.Replace("(p1)", "").Replace("(p2)", "").Replace("(p3)", "")] = DItem.ParaMeter1;
                }
            }
            dt.Rows.Add(row);
            grd.ItemsSource = dt.DefaultView;
            users = new ObservableCollection<DataItem>();
            users.Add(DItem);
            ListView = HeaderList.Keys.ToArray();
            this.DemoItemListBox.DataContext = ListView;
            dt = new DataTable();
            ICollectionView view = CollectionViewSource.GetDefaultView(ListView);
            new TextSearchFilter(view, this.DemoItemsSearchBox);
        }

        private void PlaceholdersListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            if (item != null)
            {
                dt = new DataTable();
                string hel = item.Content.ToString();
                grd.ItemsSource = null;
                string[] ColList = Regex.Split(HeaderList[hel], ",");
                foreach (var Col in ColList)
                {
                    dt.Columns.Add(new DataColumn(Col));
                }
                DataRow row = dt.NewRow();
                dt.Rows.Add(row); // 将此行添加到table中 
                grd.ItemsSource = dt.DefaultView;
            }
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            { }
        }
        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Drawhost.IsLeftDrawerOpen == false)
            {
                Drawhost.IsLeftDrawerOpen = true;
            }
        }

        private void btnup_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopupBox_OnOpened(object sender, RoutedEventArgs e)
        {

        }

        private void PopupBox_OnClosed(object sender, RoutedEventArgs e)
        {

        }
    }
}
