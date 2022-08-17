using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ControlProgram;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.IO;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Data;

namespace WatchProgram
{
    public class TestClass
    {
        #region//测试控件对象
        public Stopwatch Watchtime;
        public TestFunction TFunction;
        System.ComponentModel.BackgroundWorker BgwRelay;
        public Label lblRes;
        public DataGridView dgvTest;
        public Label BarCodeNumber;
        CAgilent KeySight;
        List<string> ErrorCode;
        List<string> TestDetail;
        BarcodeScaner Barcode;
        public int Channel_Number;
        public Label BarCodeLabel;
        public Label lblTime;
        public Label Timer;
        List<string> RecordData = new List<string>();
        public bool Connnect = false;
        TestKey TestKY = new TestKey();
        Key ky;
        System.Threading.Mutex mutex = new Mutex();
        System.Timers.Timer timeClick = new System.Timers.Timer();
        #endregion
        private void WriteResult(string path, string Word)
        {
            if (!Directory.Exists("D:\\Data\\txt"))
            {
                Directory.CreateDirectory("D:\\Data\\txt");
            }
            FileStream fs = new FileStream("D:\\Data\\txt\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + path + ".txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd h:mm:ss:fff") + ":" + Word);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        ChannelNumber ChannelNum;
        string XML_Path = "";

        public bool CreateChannel(string BarcodeComport, int BaudRate, string comm_IP, string Relay_IP, int Channel, string xml_File, ChannelNumber Number, CAgilent cAgilent)
        {
            try
            {
                ChannelNum = Number;
                XML_Path = xml_File;
                Channel_Number = Channel;
                Watchtime = new Stopwatch();
                Barcode = new BarcodeScaner();
                TFunction = new TestFunction();
                KeySight = cAgilent;
                timeClick.Interval = 10;
                timeClick.AutoReset = true;
                timeClick.Elapsed += new System.Timers.ElapsedEventHandler(Mytimer_tick);
                if (!TFunction.OpenConnt(Relay_IP, 10008))
                {
                    return false;
                }
                ShowTestPara(xml_File, dgvTest);
                BgwRelay = new BackgroundWorker();
                BgwRelay.DoWork += BgwRelay_DoWork;
                BgwRelay.RunWorkerCompleted += BgwRelay_Complete;
                BgwRelay.RunWorkerAsync();
                Connnect = true;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        List<int> RelayRowList = new List<int>();
        int[] RelayStep;
        public DataGridView ShowTestPara(string FilePath, DataGridView DG)
        {
            LoadFileClass.LoadFile(FilePath);
            int Row = 0;
            int[] DieabelShow = { 46 };
            DG.RowCount = CParamter.TestStep.Count;
            foreach (string Var in CParamter.TestStep.Keys)
            {
                DG.Rows[Row].Cells[0].Value = Var.ToString();
                int iStep = Convert.ToInt16(CParamter.TestStep[Var]);
                RelayRowList.Add(Row);
                RelayThreadDic.Add(Row, iStep);
                Row = Row + 1;
            }
            RelayStep = RelayRowList.ToArray();
            return DG;
        }

        private void Mytimer_tick(object sender, ElapsedEventArgs e)
        {
            Timer.Invoke(new Action(() =>
            {
                TimeSpan temp = new TimeSpan(0, 0, 1);
                Timer.Text = Watchtime.Elapsed.TotalSeconds.ToString("0.00");
            }
            ));
        }
        #region//测试list的属性
        public int TestRelayNumber;
        private int m_enumTestItem = 0;
        public int TestRelayRow { get; set; }
        Dictionary<int, int> RelayThreadDic = new Dictionary<int, int>();
        double TemperatureEN = 0;
        string sBarcode = "F5D31452TER14M73N106";
        #endregion
        private void NextRelayStep()
        {
            Thread.Sleep(10);
            if (ErrorCode.Count() > 0)
            {
                m_enumTestItem = 100;
                return;
            }
            if (TestRelayNumber < RelayRowList.Count)
            {
                TestRelayNumber++;
            }
            if (TestRelayNumber == RelayRowList.Count)
            {
                m_enumTestItem = 100;
            }
            else
            {
                TestRelayRow = RelayStep[TestRelayNumber];
                RelayThreadDic.TryGetValue(TestRelayRow, out m_enumTestItem);
            }
        }

        private void BgwRelay_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BgwRelay.IsBusy == false)
            {
                BgwRelay.RunWorkerAsync();
            }
        }

        public void ShowTestRow(int Row, string data, Color Clr)
        {
            UpdateGV(Row, data, Clr);
        }
        private delegate void UpdateDataGridView(int Row, string data, Color Clr);
        private void UpdateGV(int Row, string data, Color Clr)
        {
            if (dgvTest.InvokeRequired)
            {
                dgvTest.BeginInvoke(new UpdateDataGridView(UpdateGV), new object[] { Row, data, Clr });
            }
            else
            {
                try
                {
                    dgvTest.Rows[Row].Cells[Channel_Number].Value = data;
                    dgvTest.Rows[Row].Cells[Channel_Number].Style.BackColor = Clr;
                    dgvTest.FirstDisplayedScrollingRowIndex = Row;
                    if (data.Trim().Length == 0)
                    { dgvTest.FirstDisplayedScrollingRowIndex = 0; }
                    if (Row == dgvTest.RowCount)
                    {
                        dgvTest.ClearSelection();
                    }
                }
                catch
                {

                }
            }
        }
        public void ClearUI()
        {
            //ShowTestTime("0.00S");
            ErrorCode = new List<string>();
            TestDetail = new List<string>();
            string[] keys = CParamter.LogName.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                CParamter.LogName[keys[i]] = "";
            }
            for (int i = 0; i < dgvTest.RowCount; i++)
            {
                ShowTestRow(i, "", Color.Wheat);
            }
            ShowTestStatues("Waiting", Color.Aqua, Color.Cyan);
        }

        public void ShowTestStatues(string data, Color wave, Color Background)
        {
            lblRes.Invoke(new Action(() =>
            {
                lblRes.Text = data;
            }
            ));
            lblRes.Invoke(new Action(() =>
            {
                lblRes.BackColor = Background;
            }
            ));
        }

        private void BgwRelay_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BgwRelay.CancellationPending)
            {
                Thread.Sleep(100);
                e.Cancel = true;
                return;
            }
            switch (m_enumTestItem)
            {
                case 0:
                    if (StartUp())
                    {
                        Watchtime.Restart();
                        timeClick.Start();
                        WriteResult(Channel_Number.ToString(), ":" + BarCodeStr + "\r\n");
                        TFunction.ResetEQ();
                        lblRes.Invoke(new Action(() =>
                        {
                            lblRes.Text = "Testing";
                            lblRes.BackColor = Color.Yellow;
                        }
                        ));
                        ShowTestRow(0, "PASS", Color.LawnGreen);
                        NextRelayStep();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                        m_enumTestItem = 300;
                        break;
                    }
                case 1:
                    if (!Pack_wakeUp())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 2:
                    if (!Verify_i2c_Comm())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 4:
                    if (!i2c_sda_voltage())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 5:
                    if (!i2c_scl_voltage())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 6:
                    if (!pack_ocv())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 7:
                    if (!pack_acir())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 8:
                    if (!pack_DCIR())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 9:
                    if (!pack_cocp())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 10:
                    if (!pack_cocp_delay())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 11:
                    if (!pack_cocp_release())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 12:
                    if (!pack_docp())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 13:
                    if (!pack_docp_delay())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 14:
                    if (!pack_docp_release())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 15:
                    if (!pack_no_Load())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 16:
                    if (!pack_current_acc(Status.charge, CurrentMode.mA10))
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 17:
                    if (!pack_current_acc(Status.dischrge, CurrentMode.mA10))
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 18:
                    if (!pack_current_acc(Status.charge, CurrentMode.mA100))
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 19:
                    if (!pack_current_acc(Status.dischrge, CurrentMode.mA100))
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 20:
                    if (!pack_gg_temp_acc())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 21:
                    if (!pack_gg_volt_acc())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 22:
                    if (!pack_gg_ntc_acc())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 23:
                    if (!pack_gg_rsoc())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 24:
                    if (!pack_gg_fcc())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 25:
                    if (!pack_gg_ncc())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 26:
                    if (!pack_gg_cycle_count())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 27:
                    if (!pack_gg_chem_id())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 28:
                    if (!pack_gg_chem_dfcs())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 29:
                    if (!pack_gg_prot_cs())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 30:
                    if (!pack_Enable_IT())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 31:
                    if (!pack_gg_verify_qen())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 32:
                    if (!pack_Verify_sn())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 33:
                    if (!pack_write_sn())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 34:
                    if (!pack_Verify_cell_date())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 35:
                    if (!pack_shutdown_mode())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 36:
                    if (!pack_verify_FS_OP())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 37:
                    if (!pack_unseal())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 38:
                    if (!pack_gg_static_dfcs())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 39:
                    if (!pack_OCV_Current())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 40:
                    if (!pack_FET_Control())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 41:
                    if (!pack_bmu_sn())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 42:
                    if (!pack_sip_sn())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 43:
                    if (!pack_sip_sn_info())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 44:
                    if (!pack_LT_Reset())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 100:
                    bool Result = true;
                    Watchtime.Stop();
                    timeClick.Stop();
                    string Error = "";
                    if (ErrorCode.Count > 0)
                    {
                        for (int i = 0; i < ErrorCode.Count; i++)
                        { Error += ErrorCode[i] + ","; }
                        Error = Error.Substring(0, Error.Length - 1);
                        ShowTestStatues(Error, Color.Red, Color.Red);
                        Result = false;
                    }
                    else
                    {
                        ShowTestStatues("PASS", Color.Aqua, Color.Lime);
                        Result = false;
                    }
                   // Record_OneTables(XML_Path, Result, Error);
                    RecordData.Clear();
                    string TotalTime = Watchtime.Elapsed.TotalSeconds.ToString();
                    m_enumTestItem = 200;
                    if (ErrorCode.Count() == 0)
                    {
                        Timer.Invoke((Action)(() =>
                        {
                            ChannelNum.PNumber += 1;
                            ChannelNum.SNumber += 1;

                        }));
                    }
                    else
                    {
                        Timer.Invoke((Action)(() =>
                        {
                            ChannelNum.SNumber += 1;
                        }));
                    }
                    ErrorCode.Clear();
                    m_enumTestItem = 0;
                    MainClass.IsTest = false;
                    TestRelayNumber = 0;
                    RecordData.Clear();
                    ErrorCode.Clear();
                    TimeSpanDic.Clear();
                    //TimeSpan.Clear();
                    break;

            }
        }

        private void SaveLog(string Value, string LogName)
        {
            RecordData.Add(LogName + "," + Value);
        }
        Dictionary<string, string> TimeSpanDic = new Dictionary<string, string>();
        private bool Pack_wakeUp()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                TFunction.ClearCurrentRegister();
                TFunction.ClearVoltageRegister();
                TFunction.ChargeRelayON();
                TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[9]));
                TFunction.SetVoltage(4800);
                TFunction.ResetEQ();
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa,0x00,new byte[] { 0x0f,0x00});
                    Thread.Sleep(Convert.ToInt32(Value[3]));
                    Voltage = TFunction.MeasureVoltageInside();
                    if (Voltage < Convert.ToInt32(Value[8]) && Voltage > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                TFunction.ResetEQ();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Voltage.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Voltage.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        byte[] HW_Command = { 0x03, 0x00 };
        byte[] chem_id = { 0x08, 0x00 };
        byte[] chem_dfcs = { 0x1b, 0x00 };
        byte[] prot_cs = { 0x06, 0x00 };
        byte[] Enable_IT = { 0x21, 0x00 };
        byte[] static_dfcs = { 0x1c, 0x00 };
        byte[] LT_Reset = { 0x0E, 0x00 };
        private bool Verify_i2c_Comm()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, HW_Command);
                    byte[] HW_Verison = TFunction.I2C_Read(0xaa, 0x00, 4);
                    HW = HW_Verison[2].ToString("X2") + HW_Verison[1].ToString("X2");
                    if (HW == Value[7].Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                //SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            //if (ky.bGon == true)
            //{
            //    return true;
            //}
            return IsPAss;
        }

        private bool pack_gg_chem_id()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, chem_id);
                    byte[] HW_Verison = TFunction.I2C_Read(0xaa, 0x00, 4);
                    HW = HW_Verison[2].ToString("X2") + HW_Verison[1].ToString("X2");
                    if (HW == Value[7].Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private bool pack_gg_static_dfcs()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, static_dfcs);
                    Thread.Sleep(50);
                    byte[] HW_Verison = TFunction.I2C_Read(0xaa, 0x00, 4);
                    if (HW_Command == null)
                    {
                        continue;
                    }
                    HW = HW_Verison[2].ToString("X2") + HW_Verison[1].ToString("X2");

                    if (HW == Value[7].Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_gg_chem_dfcs()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, chem_dfcs);
                    byte[] HW_Verison = TFunction.I2C_Read(0xaa, 0x00, 4);
                    if (HW_Command == null)
                    {
                        continue;
                    }
                    HW = HW_Verison[2].ToString("X2") + HW_Verison[1].ToString("X2");
                    
                    if (HW == Value[7].Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_gg_prot_cs()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, prot_cs);
                    byte[] HW_Verison = TFunction.I2C_Read(0xaa, 0x00, 4);
                    HW = HW_Verison[2].ToString("X2") + HW_Verison[1].ToString("X2");
                    if (HW == Value[7].Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_Enable_IT()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, Enable_IT);
                    Thread.Sleep(50);
                    TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x00, 0x00 });
                    Thread.Sleep(50);
                    byte[] status = TFunction.I2C_Read_cycle(0xaa, 0x00, 4);
                    HW = status[2].ToString("X2") + status[1].ToString("X2");
                    IsPAss = true;
                    break;
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_LT_Reset()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, LT_Reset);
                    Thread.Sleep(50);
                    TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x00, 0x00 });
                    Thread.Sleep(50);
                    byte[] status = TFunction.I2C_Read_cycle(0xaa, 0x00, 4);
                    HW = status[2].ToString("X2") + status[1].ToString("X2");
                    IsPAss = true;
                    break;
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }



        private bool pack_unseal()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            //TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x41, 0x00 });
            TFunction.SendString("E6-BB-DD");
            string HW = "Fail";
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                if (TestStepName.Contains("1"))
                {
                    Thread.Sleep(3000);
                }
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, pss);
                    TFunction.I2C_Write(0xaa, 0x00, ps);
                    Thread.Sleep(200);
                    TFunction.I2C_Write(0xaa, 0x00, full_unseal);
                    TFunction.I2C_Write(0xaa, 0x00, full_unseal);
                    TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x00, 0x00 });
                    byte[] status = TFunction.I2C_Read_cycle(0xaa, 0x00, 4);
                    HW = status[2].ToString("X2")+status[1].ToString("X2");
                    if (status[2] == 0x00)
                    {
                        HW = "Pass";
                        IsPAss = true;
                        break;
                    }
                    Thread.Sleep(2500);
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private int EECheckSUM(byte[] data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return 255 - sum % 256;
        }

        private bool pack_write_sn()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            string OriginalBlock;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x61, new byte[] { 0x00});
                    Thread.Sleep(500);
                    TFunction.I2C_Write(0xaa, 0x3e, new byte[] { 0x3a });
                    Thread.Sleep(500);
                    TFunction.I2C_Write(0xaa, 0x3f, new byte[] { 0x01 });
                    byte[] ReadBlockB = TFunction.I2C_Read_cycle(0xaa, 0x40, 32).Skip(1).ToArray();
                    OriginalBlock = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB);
                    byte[] BarCode = System.Text.ASCIIEncoding.ASCII.GetBytes(sBarcode);
                    for (int j = 0; j < BarCode.Length; j++)
                    {
                        ReadBlockB[j] = BarCode[j];
                    }
                    //string vs = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB.Take(8).ToArray());
                    //string vs1= System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB.Skip(8).Take(8).ToArray());
                    //string vs2 = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB.Skip(16).Take(8).ToArray());
                    //string vs3 = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB.Skip(24).Take(8).ToArray());
                    //TFunction.I2C_Write(0xaa, 0x40, ReadBlockB.Take(8).ToArray());
                    //Thread.Sleep(50);
                    //TFunction.I2C_Write(0xaa, 0x48, ReadBlockB.Skip(8).Take(8).ToArray());
                    //Thread.Sleep(50);
                    //TFunction.I2C_Write(0xaa, 0x50, ReadBlockB.Skip(16).Take(8).ToArray());
                    //Thread.Sleep(50);
                    //TFunction.I2C_Write(0xaa, 0x58, ReadBlockB.Skip(24).Take(8).ToArray());

                    TFunction.I2C_Write(0xaa, 0x40, ReadBlockB);
                    int CheckSum = EECheckSUM(ReadBlockB);
                    Thread.Sleep(200);
                    TFunction.I2C_Write(0xaa, 0x60, new byte[] { Convert.ToByte(CheckSum),0x00 });
                    HW = sBarcode;
                    IsPAss = true;
                    break;
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        byte[] ReadBlockB;
        byte[] ReadBlockA;
        private bool pack_Verify_sn()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = "";
            string OriginalBlock;
            TFunction.ResetEQ();
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x61, new byte[] { 0x00 });
                    Thread.Sleep(200);
                    TFunction.I2C_Write(0xaa, 0x3e, new byte[] { 0x3a });
                    Thread.Sleep(200);
                    TFunction.I2C_Write(0xaa, 0x3f, new byte[] { 0x01 });
                    Thread.Sleep(500);
                    ReadBlockB = TFunction.I2C_Read_cycle(0xaa, 0x40, 32).Skip(1).ToArray();
                    TFunction.I2C_Write(0xaa,0x3f, new byte[] { 0x00 });
                    Thread.Sleep(500);
                    ReadBlockA = TFunction.I2C_Read_cycle(0xaa, 0x40, 32).Skip(1).ToArray();
                    OriginalBlock = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB);
                    HW = OriginalBlock;
                    if (OriginalBlock.Substring(0, 17) == sBarcode.Substring(0, 17))
                    {
                        HW = sBarcode;
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_bmu_sn()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = "";
            string OriginalBlock;
            TFunction.ResetEQ();
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    OriginalBlock = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockA);
                    HW = OriginalBlock.Substring(0,17);
                    if (Regex.IsMatch(HW, @"[0-9A-Za-z]+"))
                    {
                        IsPAss = true;
                        break;
                    }
                    //if (OriginalBlock.Substring(0, 17) == sBarcode.Substring(0, 17))
                    //if()
                    //{
                        //HW = sBarcode;

                    //}
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private bool pack_sip_sn()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = "";
            string OriginalBlock;
            string OriginalBlockB;
            TFunction.ResetEQ();
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    OriginalBlock = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockA);
                    OriginalBlockB = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB);
                    HW = OriginalBlock.Substring(17,15)+OriginalBlockB.Substring(20,2);
                    //if (OriginalBlock.Substring(0, 17) == sBarcode.Substring(0, 17))
                    //if()
                    //{
                    //HW = sBarcode;
                    IsPAss = true;
                    break;
                    //}
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private bool pack_sip_sn_info()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = "";
            string OriginalBlock;
            string OriginalBlockB;
            TFunction.ResetEQ();
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    OriginalBlock = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockA);
                    OriginalBlockB = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB);
                    HW = OriginalBlockB.Substring(22, 9);
                    //if (OriginalBlock.Substring(0, 17) == sBarcode.Substring(0, 17))
                    //if()
                    //{
                    //HW = sBarcode;
                    IsPAss = true;
                    break;
                    //}
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }



        private bool pack_Verify_cell_date()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = "";
            string OriginalBlock;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    //TFunction.I2C_Write(0xaa, 0x61, new byte[] { 0x00 });
                    //TFunction.I2C_Write(0xaa, 0x3e, new byte[] { 0x3a });
                    //TFunction.I2C_Write(0xaa, 0x3f, new byte[] { 0x01, 0x00 });
                    //byte[] ReadBlockB = TFunction.I2C_Read_cycle(0xaa, 0x40, 32).Skip(1).ToArray();
                    OriginalBlock = System.Text.ASCIIEncoding.ASCII.GetString(ReadBlockB);
                    if (OriginalBlock.Substring(17, 3) == sBarcode.Substring(17, 3))
                    {
                        HW = sBarcode.Substring(17, 3);
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_shutdown_mode()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = "";
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i < 5; i++)
                {
                    //TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x41, 0x00 });
                    TFunction.I2C_Write(0xaa, 0x00, pss);
                    TFunction.I2C_Write(0xaa, 0x00, ps);
                    Thread.Sleep(50);
                    TFunction.I2C_Write(0xaa, 0x00, full_unseal);
                    TFunction.I2C_Write(0xaa, 0x00, full_unseal);
                    Thread.Sleep(1000);
                    TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x00, 0x00 });
                    byte[] vs = TFunction.I2C_Read(0xaa, 0x00, 4);
                    if (vs[2] == 0x00)
                    {
                        break;
                    }
                }
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa,0x00,new byte[] { 0x10,0x00});
                    Thread.Sleep(50);
                    TFunction.I2C_Write(0xaa, 0x00, new byte[]{ 0x17, 0x00 });
                    Thread.Sleep(50);
                    TFunction.I2C_Write(0xaa, 0x00, new byte[]{ 0x20, 0x00 });
                    //Thread.Sleep(4000);
                    byte[] ControlStatus = null;
                    for (int j = 0; j < 10; j++)
                    {
                        byte[] Curr=TFunction.I2C_Read(0xaa, 0x14, 4);
                        TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x00, 0x00 });
                        ControlStatus = TFunction.I2C_Read(0xaa, 0x00, 4);
                        if (ControlStatus != null && ControlStatus[1] != 0)
                        {
                            break;
                        }
                    }
                    HW = ControlStatus[2].ToString("X2")+ControlStatus[1].ToString("X2");
                    if ((ControlStatus[2] & 0x60) == 96)
                    {
                        //HW = sBarcode;
                        IsPAss = true;
                        break;
                    }

                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private bool pack_gg_verify_qen()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = null;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x00, 0x00 });
                    byte[] status = TFunction.I2C_Read_cycle(0xaa, 0x00, 4);
                    HW = status[2].ToString("X2") + status[1].ToString("X2");
                    if ((status[1] & 0x01) == Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool i2c_sda_voltage()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Voltage_sda = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.SendString("E6-BB-DD");
                    TFunction.SendString("ED-BB-DD");
                    TFunction.SendString("EE-BB-DD");
                    Voltage_sda = TFunction.MeasureVoltageInside();
                    TFunction.ResetEQ();
                    if (Voltage_sda < Convert.ToInt32(Value[8]) && Voltage_sda > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage_sda.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage_sda.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Voltage_sda.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Voltage_sda.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool i2c_scl_voltage()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Voltage_scl = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.SendString("E6-BB-DD");
                    TFunction.SendString("ED-BB-DD");
                    TFunction.SendString("EF-BB-DD");
                    Voltage_scl = TFunction.MeasureVoltageInside();
                    TFunction.ResetEQ();
                    if (Voltage_scl < Convert.ToInt32(Value[8]) && Voltage_scl > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage_scl.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage_scl.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Voltage_scl.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Voltage_scl.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_ocv()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    Voltage = TFunction.MeasureVoltageInside();
                    if (Voltage < Convert.ToInt32(Value[8]) && Voltage > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Voltage.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Voltage.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private bool pack_verify_FS_OP()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    Voltage = TFunction.MeasureVoltageInside();
                    if (Voltage < Convert.ToInt32(Value[8]) && Voltage > Convert.ToInt32(Value[7]))
                    {
                        TFunction.I2C_Read_cycle(0xaa, 0x08, 4);
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Voltage.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Voltage.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_acir()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double acir = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    acir = TFunction.ReceiveDouble("D4-BB-CC");
                    if (acir < Convert.ToInt32(Value[8]) && acir > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + acir.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + acir.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(acir.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, acir.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_DCIR()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double DCIR = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    double Voltage1 = TFunction.MeasureVoltageInside();
                    TFunction.ClearCurrentRegister();
                    TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[9]));
                    TFunction.DisChargeRelayON();
                    Thread.Sleep(Convert.ToInt32(Value[3]));
                    double Voltage2 = TFunction.MeasureVoltageInside();
                    DCIR = ((Voltage1 - Voltage2) / Convert.ToInt32(Value[9])) * 1000;
                    TFunction.ResetEQ();
                    if (DCIR < Convert.ToInt32(Value[8]) && DCIR > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + DCIR.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + DCIR.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(DCIR.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, DCIR.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        public double Cocpdelay;
        private bool pack_cocp()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                TFunction.Cocp_setRange_Min(1000);
                TFunction.Cocp_setRange_Max(3000);
                TFunction.Cocp_Range(4000);
                double Current = Convert.ToInt32(Value[9]);
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                TFunction.ClearCurrentRegister();
                TFunction.ClearVoltageRegister();
                //TFunction.SendString("E1-BB-DD");//大电流继电器
                TFunction.ChargeRelayON();
                TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, 500);
                TFunction.SetVoltage(4800);
                Thread.Sleep(20);
                TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, 600);
                //if (TFunction.QueryProtection())
                //{
                //    return false;
                //}
                for (int i = 0; i <= 1; i++)
                {
                    TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[9]) + 5);
                    Thread.Sleep(3000);
                    if (TFunction.QueryProtection())
                    {
                        Cocpdelay = TFunction.protectionTime() / 10;
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                TFunction.ResetEQ();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Current.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Current.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_cocp_delay()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double delaytime = Cocpdelay;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    if (Cocpdelay < Convert.ToInt32(Value[8]) && Cocpdelay > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + delaytime.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + delaytime.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(delaytime.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, delaytime.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_cocp_release()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string HW = "True";
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ls = { 0x41, 0x00 };
                    TFunction.I2C_Write(0xaa, 0x00, ls);
                    TFunction.I2C_Write(0xaa, 0x00, ls);
                    Thread.Sleep(1500);
                    TFunction.ResetEQ();
                    IsPAss = true;
                    break;
                    //TFunction.I2C_Write(0xaa, 0x00, HW_Command);
                    //byte[] HW_Verison = TFunction.I2C_Read(0xaa, 0x00, 4);
                    //HW = HW_Verison[2].ToString("X2") + HW_Verison[1].ToString("X2");
                    //if (HW == Value[7].Trim())
                    //{
                    //    IsPAss = true;
                    //    break;
                    //}
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, HW, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        public double DocpDelay;
        private bool pack_docp()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                TFunction.Docp_setRange_Min(1000);
                TFunction.Docp_setRange_Max(3000);
                TFunction.Docp_Range(4000);
                double Current = Convert.ToInt32(Value[9]);
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                TFunction.ClearCurrentRegister();
                TFunction.ClearVoltageRegister();
                TFunction.DisChargeRelayON(); ;
                TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, 500);
                Thread.Sleep(20);
                TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, 600);
                Thread.Sleep(50);
                //if (TFunction.QueryProtection())
                //{
                //    return false;
                //}
                for (int i = 0; i <= 1; i++)
                {
                    TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[9]) + 5);
                    Thread.Sleep(3000);
                    if (TFunction.QueryProtection())
                    {
                        DocpDelay = TFunction.protectionTime() / 10;
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                TFunction.ResetEQ();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Current.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Current.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_docp_delay()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double delaytime = DocpDelay;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    if (DocpDelay < Convert.ToInt32(Value[8]) && DocpDelay > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + delaytime.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + delaytime.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(delaytime.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, delaytime.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        byte[] ps = { 0x18, 0x35 };
        byte[] pss = { 0x01, 0x56 };
        byte[] full_unseal = { 0xFF, 0xFF };
        private bool pack_docp_release()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            TFunction.I2C_Write(0xaa, 0x00, pss);
            TFunction.I2C_Write(0xaa, 0x00, ps);
            Thread.Sleep(500);
            TFunction.I2C_Write(0xaa, 0x00, full_unseal);
            TFunction.I2C_Write(0xaa, 0x00, full_unseal);
            byte[] vs1 = { 0x00 };
            TFunction.I2C_Write(0xaa, 0x00, vs1);
            byte[] vs = TFunction.I2C_Read_cycle(0xaa, 0x00, 4);
            string HW = "True";
            double Current;
            try
            {
                double Voltage = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ls = { 0x41, 0x00 };
                    TFunction.I2C_Write(0xaa, 0x00, ls);
                    TFunction.I2C_Write(0xaa, 0x00, ls);
                    //Thread.Sleep(1500);
                    TFunction.ResetEQ();
                    //byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x26, 4);
                    //if (ReadValue[2] == 0xff)//当电流为负数的时候为0xff
                    //{
                    //    ReadValue[1] = Convert.ToByte(255 - Convert.ToInt32(ReadValue[1]));
                    //    Current = -Convert.ToDouble(ReadValue[1]);
                    //}
                    //else
                    //{
                    //    Current = Convert.ToDouble(ReadValue[1]);
                    //}
                    //TFunction.SendString("E6-BB-DD");
                    //HW = Current.ToString("0.00");
                    IsPAss = true;
                    break;
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(HW + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, IsPAss?"Pass":"NG", IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_no_Load()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x14, 4);
                    if (ReadValue[2] == 0xff)//当电流为负数的时候为0xff
                    {
                        ReadValue[1] = Convert.ToByte(255 - Convert.ToInt32(ReadValue[1]));
                        Current = -Convert.ToDouble(ReadValue[1]);
                    }
                    else
                    {
                        Current = Convert.ToDouble(ReadValue[1]);
                    }
                    if (Current <= Convert.ToInt32(Value[8]) && Current >= Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Current.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Current.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }




        private bool pack_FET_Control()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            string FET_Control="";
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x62, 4);
                    FET_Control=ReadValue[2].ToString("X2") + ReadValue[1].ToString("X2");
                    if (FET_Control == Value[7].Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(FET_Control + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, FET_Control, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_OCV_Current()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            TFunction.SendString("E6-BB-DD");
            //TFunction.I2C_Write(0xaa, 0x00, new byte[] { 0x41, 0x00 });
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x26, 4);
                    if (ReadValue[2] == 0xff)//当电流为负数的时候为0xff
                    {
                        ReadValue[1] = Convert.ToByte(255 - Convert.ToInt32(ReadValue[1]));
                        Current = -Convert.ToDouble(ReadValue[1]);
                    }
                    else
                    {
                        Current = Convert.ToDouble(ReadValue[1]);
                    }
                    if (Current <= Convert.ToInt32(Value[8]) && Current >= Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Current.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Current.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private bool pack_gg_rsoc()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                int Battery_Rsoc = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x2c, 4);
                    int Rosc = Convert.ToInt32(ReadValue[1].ToString("X2"), 16);
                    Battery_Rsoc = Rosc;
                    if (Battery_Rsoc < Convert.ToInt32(Value[8]) && Battery_Rsoc > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Battery_Rsoc.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Battery_Rsoc.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Battery_Rsoc.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Battery_Rsoc.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_gg_fcc()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                int FCC = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x12, 4);
                    if(ReadValue==null)
                    {
                        Thread.Sleep(50);
                        continue;
                    }
                    int Full = Convert.ToInt32(ReadValue[2].ToString("X2") + ReadValue[1].ToString("X2"), 16);
                    FCC = Full;
                    if (FCC < Convert.ToInt32(Value[8]) && FCC > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + FCC.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + FCC.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(FCC.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, FCC.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_gg_ncc()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                int ncc = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x72, 4);
                    int Full = Convert.ToInt32(ReadValue[2].ToString("X2") + ReadValue[1].ToString("X2"), 16);
                    ncc = Full;
                    if (ncc < Convert.ToInt32(Value[8]) && ncc > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                    Thread.Sleep(1000);
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + ncc.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ncc.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ncc.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, ncc.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_gg_cycle_count()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                int Count = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x2A, 4);
                    int Full = Convert.ToInt32(ReadValue[2].ToString("X2") + ReadValue[1].ToString("X2"), 16);
                    Count = Full;
                    if (Count <= Convert.ToInt32(Value[8]) && Count >= Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                    Thread.Sleep(100);
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + Count.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Count.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Count.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, Count.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        public enum Status
        {
            charge,
            dischrge
        }
        public enum CurrentMode
        {
            mA100,
            mA10
        }
        private bool pack_current_acc(Status status, CurrentMode mode)
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double Current = 0;
                double deltaCurrent = 0;
                byte[] ReadValue = null;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                TFunction.ClearCurrentRegister();
                for (int i = 0; i < 10; i++)
                {
                    if (TFunction.SendString("E7-BB-DD"))
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }

                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    if (status == Status.charge)
                    {
                        TFunction.ChargeRelayON();
                        TFunction.SetVoltage(4800);
                    }
                    else if (status == Status.dischrge)
                    {
                        TFunction.DisChargeRelayON();
                    }
                    if (mode == CurrentMode.mA100)
                    {
                        TFunction.SendString("01-06-10-0C-00-01-8C-C9");
                    }
                    else if (mode == CurrentMode.mA10)
                    {
                        TFunction.SendString("01-06-10-0B-00-01-3D-08");
                    }
                    Thread.Sleep(ky.Delay);
                    Current = KeySight.MeasureVoltage();
                    if (Current < Convert.ToInt32(Value[9]) && Current > Convert.ToInt32(Value[8]))
                    {
                        ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x14, 4);
                        if (ReadValue[2] == 0xff)//当电流为负数的时候为0xff
                        {
                            ReadValue[1] = Convert.ToByte(255 - Convert.ToInt32(ReadValue[1]));
                            deltaCurrent = -Convert.ToDouble(ReadValue[1]) - Current;
                        }
                        else
                        {
                            deltaCurrent = Convert.ToDouble(ReadValue[1]) - Current;
                        }

                        if (deltaCurrent < Convert.ToInt32(Value[11]) && deltaCurrent > Convert.ToInt32(Value[10]))
                        {
                            IsPAss = true;
                            break;
                        }
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + deltaCurrent.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                TFunction.ResetEQ();
                Thread.Sleep(100);
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + deltaCurrent.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(deltaCurrent.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, deltaCurrent.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_gg_temp_acc()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double temperature = 0;
                double EnvirmentTemp = 0;
                double deltatemp = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x06, 4);
                    int AbTemp = Convert.ToInt32(ReadValue[2].ToString("X2") + ReadValue[1].ToString("X2"), 16);
                    double temp = double.Parse(AbTemp.ToString()) / 10 - 273.15;
                    temperature = temp;
                    EnvirmentTemp = TFunction.ReceiveDouble("F6-BB-DD");
                    deltatemp = temperature - EnvirmentTemp;
                    if (deltatemp < Convert.ToInt32(Value[8]) && deltatemp > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + deltatemp.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + deltatemp.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(deltatemp.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, deltatemp.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private bool pack_gg_volt_acc()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double gg_volt = 0;
                double volt = 0;
                double delt_volt = 0;
                TFunction.SendString("E6-BB-DD");
                //Thread.Sleep(1000);
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x08, 4);
                    TFunction.SendString("F0-BB-DD");
                    int AbTemp = Convert.ToInt32(ReadValue[2].ToString("X2") + ReadValue[1].ToString("X2"), 16);
                    double temp = double.Parse(AbTemp.ToString());
                    gg_volt = temp;
                    float tempvolt = 0;
                    KeySight.MeasureVoltage10V(out tempvolt);
                    volt = double.Parse(tempvolt.ToString());
                    delt_volt = gg_volt - volt;
                    if (delt_volt < Convert.ToInt32(Value[8]) && delt_volt > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + delt_volt.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + delt_volt.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(delt_volt.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, delt_volt.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        public double NTC_TT(double resistance)
        {
            try
            {
                double Temp = 1 / ((Math.Log(resistance / 10000, Math.E) / 3455) + (1 / 298.15));
                return Temp - 273.15;
            }
            catch
            {

            }
            return -1;
        }
        private bool pack_gg_ntc_acc()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double temperature = 0;
                double EnvirmentTemp = 0;
                double deltatemp = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= Convert.ToInt32(Value[2]); i++)
                {
                    TFunction.SendString("EC-BB-DD");
                    byte[] ReadValue = TFunction.I2C_Read_cycle(0xaa, 0x06, 4);
                    int AbTemp = Convert.ToInt32(ReadValue[2].ToString("X2") + ReadValue[1].ToString("X2"), 16);
                    double temp = double.Parse(AbTemp.ToString()) / 10 - 273.15;
                    temperature = temp;
                    EnvirmentTemp = NTC_TT(TFunction.ReceiveDouble("DE-BB-CC"));
                    deltatemp = temperature - EnvirmentTemp;
                    if (deltatemp < Convert.ToInt32(Value[8]) && deltatemp > Convert.ToInt32(Value[7]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + deltatemp.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    ErrorCode.Add(ky.ErrorCode);
                }
                TFunction.ResetEQ();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + deltatemp.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpanDic.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(deltatemp.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", Value[5]);
                ShowTestRow(TestRelayRow, deltatemp.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                ErrorCode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        string BarCodeStr;
        public bool StartUp()
        {
            TFunction.ResetEQ();
            while (!MainClass.IsTest)
            {
                TemperatureEN = TFunction.ReceiveDouble("F6-BB-DD");
                Thread.Sleep(10);
            }
            TFunction.SendString("F1-BB-DD-11");
            TFunction.SendString("E6-BB-DD");
            ClearUI();
            string RegexValue;
            return true;
            //BarCodeStr=Barcode.ReadBarcode(CParamter.BarcodeSpec.Trim().Length);
            if (BarCodeStr != "")
            {
                return Regex.IsMatch(BarCodeStr, String.Format(@"{0}", RegexValue.Replace("*", "[0-9A-Za-z]"))) & (BarCodeStr.Length == RegexValue.Length);
            }
            return false;
        }
    }

}
