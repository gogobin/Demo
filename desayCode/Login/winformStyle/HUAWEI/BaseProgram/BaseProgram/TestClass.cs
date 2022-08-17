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

namespace BaseProgram
{
    public class TestClass
    {
        public Stopwatch Watchtime;
        public TestFunction TFunction;
        public TestFunction TFunctionComm;
        System.ComponentModel.BackgroundWorker BgwRelay;
        System.ComponentModel.BackgroundWorker BgwComm;
        //public HZH_Controls.Controls.UCProcessWave lblRes;
        public Label lblRes;
        public DataGridView dgvTest;
        public Label BarCodeNumber;
        BarcodeScaner Barcode;
        CAgilent KeySight;
        List<string> Errorcode;
        List<string> TestDetail;
        int Channel_Number = 1;
        public DataGridView DataGrid;
        public Label BarcodeLabel;
        public Label lblTime;
        public Label Timer;
        List<string> RecordData = new List<string>();
        public bool Connnect = false;
        TestKey TestKY = new TestKey();
        Key ky;
        System.Timers.Timer timeClick = new System.Timers.Timer();


        //private async void WriteResult(string path, string Word)
        //{
        //    await Task.Run(new Action(() =>
        //    {
        //        if (!Directory.Exists("D:\\Data\\txt"))
        //        {
        //            Directory.CreateDirectory("D:\\Data\\txt");
        //        }
        //        FileStream fs = new FileStream("D:\\Data\\txt\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-"+ path + ".txt", FileMode.Append);
        //        StreamWriter sw = new StreamWriter(fs);
        //        //开始写入
        //        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd h:mm:ss:fff") + ":"+Word);
        //        //清空缓冲区
        //        sw.Flush();
        //        //关闭流
        //        sw.Close();
        //        fs.Close();
        //    }));
        //}




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

        ChannelNumber Cnumber;
        string XML_Path = "";
        public bool CreateChannel(string BarcodeComport, int BaudRate, string comm_IP, string Relay_IP, int Channel, string xml_File, ChannelNumber CN)
        {
            try
            {
                Cnumber = CN;
                XML_Path = xml_File;
                Channel_Number = Channel;
                Watchtime = new Stopwatch();
                Barcode = new BarcodeScaner();
                TFunction = new TestFunction();
                TFunctionComm = new TestFunction();
                //KeySight = Aglient;
                //if(!Barcode.OpenCom(BarcodeComport,BaudRate,false))
                //{
                //    return false;
                //}
                timeClick.Interval = 10;
                timeClick.AutoReset = true;
                timeClick.Elapsed += new System.Timers.ElapsedEventHandler(Mytimer_tick);
                if (!TFunction.OpenConnt(Relay_IP, 10008))
                {
                    return false;
                }
                if (!TFunctionComm.OpenConnt(comm_IP, 10008))
                {
                    return false;
                }
                ShowTestPara(xml_File, dgvTest);
                TFunctionComm.GetVersion();
                //TFunctionComm.SendString("11-22-33-44-55-66-77-88");
                //Thread.Sleep(5500);
                //Barcode._Batcode = "";
                //string[] Value = Convert.ToString(CParamter.BasicParameter[dgvTest.Rows[0].Cells[0].Value.ToString()]).Split(new char[] { ',' });
                //if (Value[2].ToString().Trim().Length > 0)
                //{
                //    Retry_Number = Convert.ToInt32(Value[2].ToString());
                //}
                //if (Value[3].ToString().Trim().Length > 0)
                //{
                //    Delay_Number = Convert.ToInt32(Value[3].ToString());
                //}
                //BgwComm = new BackgroundWorker();
                BgwRelay = new BackgroundWorker();
                BgwComm = new BackgroundWorker();
                BgwComm.DoWork += BgwComm_DoWork;
                BgwComm.RunWorkerCompleted += BgwComm_Complete;
                BgwComm.RunWorkerAsync();
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

        private void Mytimer_tick(object sender, ElapsedEventArgs e)
        {
            Timer.Invoke(new Action(() =>
            {
                TimeSpan temp = new TimeSpan(0, 0, 1);
                //lblTime.Text = string.Format("{0:00}:{1:00}:{2:00}", temp.Hours, temp.Minutes, temp.Seconds);
                Timer.Text = Watchtime.Elapsed.TotalSeconds.ToString("0.00");
            }
            ));
        }

        public void ShowTestTime(string data)
        {
            if (lblTime.InvokeRequired)
            {
                Action<string> action = x => { lblTime.Text = x.Trim(); };
                lblTime.Invoke(action, data);
            }
            lblTime.Text = data;
        }
        //public void ShowTestStatues(string data, Color wave, Color Background)
        //{
        //    lblRes.Invoke(new Action(() =>
        //    {
        //        lblRes.ShowString = data;
        //    }
        //    ));
        //    lblRes.Invoke(new Action(() =>
        //    {
        //        lblRes.ValueColor = wave;
        //    }
        //    ));
        //    lblRes.Invoke(new Action(() =>
        //    {
        //        lblRes.BackColor = Background;
        //    }
        //    ));
        //}

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
            Errorcode = new List<string>();
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
        List<int> CommThread = new List<int> { 31, 32, 33, 34, 35, 36, 37, 38, 39 };
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
                if (CommThread.Contains(iStep))
                {
                    CommRowList.Add(Row);
                    CommThreadDic.Add(Row, iStep);
                }
                else
                {
                    RelayRowList.Add(Row);
                    RelayThreadDic.Add(Row, iStep);
                }
                Row = Row + 1;
            }
            RelayStep = RelayRowList.ToArray();
            CommStep = CommRowList.ToArray();
            return DG;
        }

        private int m_enumTestItem = 46;
        int[] RelayStep;
        int[] CommStep;
        List<int> RelayRowList = new List<int>();
        List<int> CommRowList = new List<int>();
        Dictionary<int, int> RelayThreadDic = new Dictionary<int, int>();
        Dictionary<int, int> CommThreadDic = new Dictionary<int, int>();
        public int TestRelayRow { get; set; }
        public int TestCommRow { get; set; }
        public int TestRelayNumber;
        public int TestCommNumber = -1;
        private void NextTestStep()
        {
            if (TestRelayRow < dgvTest.RowCount)
            {
                TestRelayRow++;
            }
            if (TestRelayRow == dgvTest.RowCount)
            {
                m_enumTestItem = 100;
            }
            else
            {
                m_enumTestItem = Convert.ToInt16(CParamter.TestStep[dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString()]);
            }
        }

        private void NextRelayStep()
        {
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
            Thread.Sleep(10);
        }

        private void NextCommStep()
        {
            if (TestCommNumber < CommRowList.Count)
            {
                TestCommNumber++;
            }
            if (TestCommNumber == CommRowList.Count)
            {
                m_CommItem = 100;
            }
            else
            {
                if (Errorcode.Count > 0)
                {
                    m_CommItem = 100;
                }
                else
                {
                    TestCommRow = CommStep[TestCommNumber];
                    CommThreadDic.TryGetValue(TestCommRow, out m_CommItem);
                }
            }
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
                case 46:
                    //string Msg = "";
                    if (StartUp())
                    {
                        Watchtime.Restart();
                        timeClick.Start();
                        WriteResult(Channel_Number.ToString(), ":" + BarCodeRead + "\r\n");
                        TFunction.ResetEQ();
                        TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-00");
                        lblRes.Invoke(new Action(() =>
                        {
                            lblRes.Text = "Testing";
                            lblRes.BackColor = Color.Yellow;
                        }
                        ));
                        ShowTestRow(0, "PASS", Color.LawnGreen);
                        //ShowTestStatues("Test..", Color.Yellow);
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
                    if (!Pack_OCV_Left())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 2:
                    if (!Pack_ACIR_Left())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    NextCommStep();
                    break;
                case 3:
                    if (!Pack_Resistance())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 4:
                    if (!Pack_IDResistance())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 5:
                    if (!Pack_NTCResistacne())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 6:
                    if (!Pack_ChargeTest())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 7:
                    if (!Pack_deltaVoltage())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 8:
                    if (!Pack_DOCP())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 9:
                    if (!Pack_DOCP_delay())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 10:
                    if (!ShortCircuit())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 11:
                    if (!Pack_OCV_Right())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 12:
                    if (!ACIR_Right())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 13:
                    if (!ID_Resistance_Right())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 14:
                    if (!Self_Isolate_Resistance())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 15:
                    if (!NTC_Resistance())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 16:
                    if (!DischargeCurcuit_Test())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 17:
                    if (!DischargeVoltage_Drop())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 18:
                    if (!Pack_ChargeLeakage_Right())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 19:
                    if (!Pack_ChargeLeakage_Left())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 20:
                    if (!Pack_DisChargeLeakage_Left())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 21:
                    if (!TemperatureEnvironment())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 22:
                    if (!TemperatureDelta2())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 23:
                    if (!TemperatureDelta1())
                    {
                        m_enumTestItem = 100;
                        break;
                    }
                    NextRelayStep();
                    break;
                case 100:
                    if (m_CommItem ==1)
                    {
                        Watchtime.Stop();
                        timeClick.Stop();
                    }
                    //ShowTestStatues("PASS", Color.Aqua, Color.Lime);
                    bool Result = true;
                    string Error = "";
                    if (Errorcode.Count > 0)
                    {
                        for (int i = 0; i < Errorcode.Count; i++)
                        { Error += Errorcode[i] + ","; }
                        Error = Error.Substring(0, Error.Length - 1);
                        ShowTestStatues(Error, Color.Red, Color.Red);
                        Result = false;
                    }
                    else
                    {
                        ShowTestStatues("PASS", Color.Aqua, Color.Lime);
                        Result = false;
                    }
                    Record_OneTables(XML_Path, Result, Error);
                    string TotalTime = Watchtime.Elapsed.TotalSeconds.ToString();
                    m_enumTestItem = 200;
                    if (Errorcode.Count() == 0)
                    {
                        Timer.Invoke((Action)(() =>
                        {
                            Cnumber.PNumber += 1;
                            Cnumber.SNumber += 1;

                        }));
                    }
                    else
                    {
                        Timer.Invoke((Action)(() =>
                        {
                            Cnumber.SNumber += 1;
                        }));
                    }
                    Errorcode.Clear();
                    TimeSpan.Clear();
                    break;
                case 200:
                    while (!MainClass.IsTest)
                    {
                        Thread.Sleep(1);
                    }
                    Thread.Sleep(10);
                    MainClass.IsTest = false;
                    TestRelayNumber = 0;
                    TestCommNumber = -1;
                    m_enumTestItem = 46;
                    break;
            }
        }

        private bool Pack_ChargeLeakage_Right()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bool bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                double THR2 = 0;
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                TFunction.SendString("2A-11-22-12");
                TFunction.SendString("2A-11-22-16");
                TFunction.ChargeRelayON();
                for (int i = 0; i <= ky.Retry; i++)
                {
                    Thread.Sleep(10);
                    //BeforeVoltage = TFunction.MeasureVoltageInside();
                    TFunction.ClearCurrentRegister();
                    TFunction.ClearVoltageRegister();
                    TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[7]));
                    TFunction.SetVoltage(4800);
                    Thread.Sleep(Convert.ToInt32(Value[3]));
                    TFunction.SendString("2A-11-22-06");
                    TFunction.SendString("ED-BB-DD");
                    Thread.Sleep(50);
                    Current = TFunction.MeasureCurrentInside500uA();
                    Current = Math.Abs(Current);
                    if (Current < Convert.ToInt32(Value[9]) && Current > Convert.ToInt32(Value[8]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                TFunction.ResetEQ();
                TFunction.ResetRelay();
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Current.ToString() + "," + "" + "," + "" + "," + (IsPAss ? 1 : 0).ToString() + "," + "V",Value[5]);
                ShowTestRow(TestRelayRow, Current.ToString(), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_DisChargeLeakage_Left()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bool bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    Stopwatch st = new Stopwatch();
                    st.Start();
                    TFunction.SendString("2A-11-22-01");
                    TFunction.SendString("2A-11-22-10");
                    Thread.Sleep(10);
                    string br1 = st.Elapsed.TotalSeconds.ToString();
                    //TFunction.ClearCurrentRegister();
                    //TFunction.ClearVoltageRegister();
                    TFunction.DisChargeRelayON();
                   // TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[7]));
                    string br2 = st.Elapsed.TotalSeconds.ToString();
                    //TFunction.SetVoltage(4800);
                    Thread.Sleep(Convert.ToInt32(Value[3]));
                    //MuTE.Mutex.WaitOne();
                    TFunction.SendString("2A-11-22-06");
                    TFunction.SendString("ED-BB-DD");
                    //TFunction.SendString("11-22-33-44-55-66-FF-00");
                    //TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-00");
                    Thread.Sleep(100);
                    string br3 = st.Elapsed.TotalSeconds.ToString();
                    Current = Math.Abs(TFunction.MeasureCurrentInside10uA());
                    TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-11");
                    TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-11");
                    //MuTE.Mutex.ReleaseMutex();
                    TFunction.ResetEQ();
                    TFunction.ResetRelay();
                    string br4 = st.Elapsed.TotalSeconds.ToString();
                    if (Current < Convert.ToInt32(Value[9]) && Current > Convert.ToInt32(Value[8]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Current.ToString() + "," + "" + "," + "" + "," + (IsPAss ? 1 : 0).ToString() + "," + "V",Value[5]);
                ShowTestRow(TestRelayRow, Current.ToString(), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_ChargeLeakage_Left()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bool bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-02");
                    TFunction.SendString("2A-11-22-08");
                    Thread.Sleep(10);
                    TFunction.ClearCurrentRegister();
                    TFunction.ClearVoltageRegister();
                    TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[7]));
                    TFunction.ChargeRelayON();
                    TFunction.SetVoltage(4800);
                    Thread.Sleep(Convert.ToInt32(Value[3]));
                    MuTE.Mutex.WaitOne();
                    TFunction.SendString("2A-11-22-06");
                    TFunction.SendString("ED-BB-DD");
                    TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-00");
                    TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-00");
                    Thread.Sleep(100);
                    Charge_Leakage_Current = TFunction.MeasureCurrentInside10uA(); 
                    Charge_Leakage_Current = Math.Abs(Charge_Leakage_Current);
                    //TFunction.SendString("11-22-33-44-55-66-FF-11");
                    //TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-11");
                    MuTE.Mutex.ReleaseMutex();
                    TFunction.ResetEQ();
                    TFunction.ResetRelay();
                    if (Charge_Leakage_Current < Convert.ToInt32(Value[9]) && Charge_Leakage_Current > Convert.ToInt32(Value[8]))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Charge_Leakage_Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Charge_Leakage_Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Charge_Leakage_Current.ToString() + "," + "" + "," + "" + "," + (IsPAss ? 1 : 0).ToString() + "," + "V",Value[5]);
                ShowTestRow(TestRelayRow, Charge_Leakage_Current.ToString(), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private void Record_OneTables(string xml_FilePath, bool Result, string sError)
        {
            string LOG_PATH = LoadFileClass.LogFilePath();
            string ScanerFuuPath = @"C:\ConfigFile\" + "ScanerConfig.ini";
            string[] FileLisr = xml_FilePath.Split(new char[] { '\\' }); ;
            List<string> LogKeyList = new List<string>();
            if (Directory.Exists(LOG_PATH + "\\mdb\\") == false)
            {
                Directory.CreateDirectory(LOG_PATH + "\\mdb\\");
            }
            //foreach (var Logkey in CParamter.LogName.Keys)
            //{
            //    if (LogKeyList.Contains(Logkey))
            //    {
            //        continue;
            //    }
            //    LogKeyList.Add(Logkey);
            //}
            foreach (string keys in RecordData)
            {
                string[] SplitKey = keys.Split(new char[] { ',' });
                if (LogKeyList.Contains(SplitKey[0]) || SplitKey[0].Trim() == "")
                {
                    continue;
                }
                LogKeyList.Add(SplitKey[0]);
            }
            OleDbCommand cmd = new OleDbCommand();
            AccessDbHelper.CreateAccessDb_OneTable(LoadFileClass.LogFilePath() + "\\mdb\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + FileLisr[FileLisr.Length - 1].Split(new char[] { '.' })[0] + ".mdb", LogKeyList);
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + LoadFileClass.LogFilePath() + "\\mdb\\" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + FileLisr[FileLisr.Length - 1].Split(new char[] { '.' })[0] + ".mdb");
            conn.Open();
            cmd.Connection = conn;
            cmd.Transaction = conn.BeginTransaction();
            try
            {
                string AccessTitle = "Insert into TestData(BMU_Serial_Nnumber,BMU_Barcode,Test_Config_Name,FDate,Test_User,Test_Channel_No,StationId,Line_No,Host_Name,WorkPosition,Result,Error_Code,Flag,";
                string AccessValue = string.Format("Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',", BarCodeRead, sBarcode, FileLisr[FileLisr.Length - 1].Split(new char[] { '.' })[0], DateTime.Now, Win32API.INIGetStringValue(ScanerFuuPath, "TestUser", "User", ""), Channel_Number, 0, null, System.Environment.MachineName, "BQ+ATE", Result ? 1 : 0, Convert.ToString(sError), 0);
                LogKeyList = new List<string>();
                foreach (string keys in RecordData)
                {
                    string[] SplitKey = keys.Split(new char[] { ',' });
                    if (LogKeyList.Contains(SplitKey[0]) || SplitKey[0].Trim() == "")
                    {
                        continue;
                    }
                    LogKeyList.Add(SplitKey[0]);
                    AccessTitle += SplitKey[0] + ",";
                    AccessValue += "'" + SplitKey[1] + "'" + ",";
                }
                AccessTitle = AccessTitle.Substring(0, AccessTitle.Length - 1) + ")";
                AccessValue = AccessValue.Substring(0, AccessValue.Length - 1) + ")";
                string values = AccessTitle + AccessValue;
                cmd.CommandText = AccessTitle + AccessValue;
                cmd.ExecuteNonQuery();
                cmd.Transaction.Commit();  //提交事务
                conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据生成失败，请联系TE工程师");
                cmd.Transaction.Rollback();
            }
        }

        private bool DischargeVoltage_Drop()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (DisChargeDeltaVoltage > ky.maxValue || DisChargeDeltaVoltage < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (DisChargeDeltaVoltage < ky.maxValue && DisChargeDeltaVoltage > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + DisChargeDeltaVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + DisChargeDeltaVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(DisChargeDeltaVoltage.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, DisChargeDeltaVoltage.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool DischargeCurcuit_Test()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bool bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                double THR2 = 0;
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                TFunction.ResetEQ();
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-02");
                    TFunction.SendString("2A-11-22-08");
                    Thread.Sleep(20);
                    BeforeVoltage = TFunction.MeasureVoltageInside();
                    TFunction.ClearCurrentRegister();
                    TFunction.ClearVoltageRegister();
                    TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[7]));
                    TFunction.DisChargeRelayON();
                    Thread.Sleep(Convert.ToInt32(Value[3]));
                    Current = TFunction.MeasureCurrentInside();
                    string break1 = sW.Elapsed.TotalSeconds.ToString();
                    TFunction.ResetEQ();
                    string break2 = sW.Elapsed.TotalSeconds.ToString();
                    Thread.Sleep(20);
                    AfterVoltage = TFunction.MeasureVoltageInside();
                    DisChargeDeltaVoltage = AfterVoltage - BeforeVoltage;
                    TFunction.ResetRelay();
                    if (Math.Abs(Current) < Convert.ToInt32(Value[9]) && Math.Abs(Current) > Convert.ToInt32(Value[8]))
                    {
                        IsPAss = true;
                        WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Math.Abs(Current).ToString() + "," + (IsPAss ? "OK" : "NG"));
                        break;
                    }
                    //if (THR2 > ky.maxValue || THR2 < ky.minValue)
                    //{
                    //    IsPAss = false;
                    //    continue;
                    //}
                    //if (THR2 < ky.maxValue && THR2 > ky.minValue)
                    //{
                    //    IsPAss = true;
                    //    break;
                    //}
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Math.Abs(Current).ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(IsPAss ? "Pass" : "NG" + "," + "" + "," + "" + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, IsPAss ? "Pass" : "NG", IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        double THR1 = 0;
        private bool NTC_Resistance()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                THR1 = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-19");
                    Thread.Sleep(150);
                    THR1 = TFunction.ReadResistance10k();
                    TFunction.ResetRelay();
                    if (THR1 > ky.maxValue || THR1 < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (THR1 < ky.maxValue && THR1 > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + THR2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + THR2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(THR1.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, THR1.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Self_Isolate_Resistance()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double RES1 = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-14");
                    Thread.Sleep(150);
                    RES1 = TFunction.ReadResistance();
                    TFunction.ResetRelay();
                    if (RES1 > ky.maxValue || RES1 < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (RES1 < ky.maxValue && RES1 > ky.minValue)
                    {
                        IsPAss = true;

                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + RES1.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + RES1.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(RES1.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, RES1.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool ID_Resistance_Right()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double ID2 = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-18");
                    Thread.Sleep(150);
                    ID2 = TFunction.ReadResistance();
                    TFunction.ResetRelay();
                    if (ID2 > ky.maxValue || ID2 < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (ID2 < ky.maxValue && ID2 > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ID2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ID2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ID2.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, ID2.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool ACIR_Right()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double ACIR_Res = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态呢
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-15");
                    TFunction.SendString("2A-11-22-17");
                    Thread.Sleep(150);
                    ACIR_Res = TFunction.ACIR();
                    TFunction.ResetRelay();
                    if (ACIR_Res > ky.maxValue || ACIR_Res < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (ACIR_Res < ky.maxValue && ACIR_Res > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ACIR_Res.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ACIR_Res.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ACIR_Res.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, ACIR_Res.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_OCV_Right()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
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
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-13");
                    Thread.Sleep(50);
                    Voltage = TFunction.MeasureVoltageInside();
                    TFunction.ResetRelay();
                    //Voltage = Voltage / 1000;
                    if (Voltage > ky.maxValue || Voltage < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (Voltage < ky.maxValue && Voltage > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Voltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Voltage.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, Voltage.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool ShortCircuit()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            Stopwatch sW = new Stopwatch();
            double ProTime = 0;
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                double Vol = 0;
                //TFunction.SendString("2A-11-22-01");
                //TFunction.SendString("2A-11-22-10");
                //TFunction.ClearCurrentRegister();
                //TFunction.ClearVoltageRegister();
                //TFunction.SendString("E1-BB-DD");//大电流继电器
                //TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, 500);
                //Thread.Sleep(150);
                //TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, 600);
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.DisChargeRelayON();
                    Thread.Sleep(50);
                    TFunction.SetCurrnet(TestFunction.CurrentMode.lowAccuracy, 10800);
                    //double current=TFunction.MeasureCurrentInside();
                    Thread.Sleep(30);
                    if (TFunction.QueryProtection())
                    {
                        ProTime = TFunction.protectionTime()/10;
                        TFunction.ResetEQ();
                        TFunction.ResetRelay();
                        TFunction.SendString("2A-11-22-00");
                        Vol = TFunction.MeasureVoltageInside();

                        if (Vol > ky.maxValue || Vol < ky.minValue)
                        {
                            IsPAss = false;
                            continue;
                        }
                        if (Vol < ky.maxValue && Vol > ky.minValue)
                        {
                            IsPAss = true;
                            break;
                        }
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Vol.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Vol.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ProTime.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, ProTime.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_DOCP_delay()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (Docpdelay > ky.maxValue || Docpdelay < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (Docpdelay < ky.maxValue && Docpdelay > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Docpdelay.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Docpdelay.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Docpdelay.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, Docpdelay.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        double Docpdelay;
        double Discharge_Leakage_Current = 0;
        private bool Pack_DOCP()
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
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                TFunction.SendString("2A-11-22-01");
                TFunction.SendString("2A-11-22-10");
                TFunction.ClearCurrentRegister();
                TFunction.ClearVoltageRegister();
                TFunction.SendString("E1-BB-DD");//大电流继电器
                //TFunction.SendString("01-06-10-09-00-01-9C-C8");
                TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, 900);
                Thread.Sleep(20);
                TFunction.DisChargeRelayON();
                //TFunction.SendString("2A-11-22-06");
                //TFunction.SendString("11-22-33-44-55-66-FF-00");
                //Discharge_Leakage_Current = TFunction.MeasureCurrentInside();
                //TFunction.SendString("11-22-33-44-55-66-FF-11");
                //TFunction.ResetRelay();
                Thread.Sleep(50);
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.DisChargeRelayON();
                    Thread.Sleep(100);
                    TFunction.ClearCurrentRegister();
                    TFunction.SetCurrnet(TestFunction.CurrentMode.lowAccuracy, 10700);
                    //double current=TFunction.MeasureCurrentInside();
                    Thread.Sleep(100);
                    if (TFunction.QueryProtection())
                    {
                        Docpdelay = TFunction.protectionTime();
                        Docpdelay = Docpdelay / 10;
                        if (10900 > ky.maxValue || 10900 < ky.minValue)
                        {
                            IsPAss = false;
                            continue;
                        }
                        if (10900 < ky.maxValue && 10900 > ky.minValue)
                        {
                            IsPAss = true;
                            break;
                        }
                    }
                }
                TFunction.ClearCurrentRegister();
                //TFunction.ResetEQ();
                //TFunction.ResetRelay();
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + 10900.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + 10900.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(IsPAss ? "PASS" : "NG" + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                SaveLog("10700" + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", "OverDisRealCurrent");
                ShowTestRow(TestRelayRow, IsPAss ? "PASS" : "NG", IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ChargeDeltaVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_deltaVoltage()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (ChargeDeltaVoltage > ky.maxValue || ChargeDeltaVoltage < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (ChargeDeltaVoltage < ky.maxValue && ChargeDeltaVoltage > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ChargeDeltaVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ChargeDeltaVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ChargeDeltaVoltage.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, ChargeDeltaVoltage.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        double BeforeVoltage = 0;
        double AfterVoltage = 0;
        double ChargeDeltaVoltage = 0;
        double DisChargeDeltaVoltage = 0;
        double Charge_Leakage_Current = 0;
        private bool Pack_ChargeTest()
        {

            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bool bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                double Current = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    Stopwatch ST = new Stopwatch();
                    ST.Start();
                    TFunction.SendString("2A-11-22-02");
                    TFunction.SendString("2A-11-22-08");
                    string break1 = ST.Elapsed.TotalSeconds.ToString();
                    Thread.Sleep(10);
                    //BeforeVoltage = TFunction.MeasureVoltageInside();
                    TFunction.ClearCurrentRegister();
                    TFunction.ClearVoltageRegister();
                    TFunction.ChargeRelayON();
                    TFunction.SetCurrnet(TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(Value[7]));
                    TFunction.SetVoltage(4800);
                    Thread.Sleep(Convert.ToInt32(Value[3]));
                    Current = TFunction.MeasureCurrentInside();
                    //MuTE.Mutex.WaitOne();
                    //TFunction.SendString("2A-11-22-06");
                    //TFunction.SendString("11-22-33-44-55-66-FF-00");
                    //Charge_Leakage_Current = TFunction.MeasureCurrentInside();
                    //TFunction.SendString("11-22-33-44-55-66-FF-11");
                    //MuTE.Mutex.ReleaseMutex();
                    TFunction.ResetEQ();
                    Thread.Sleep(20);
                    AfterVoltage = TFunction.MeasureVoltageInside();
                    ChargeDeltaVoltage = AfterVoltage - BeforeVoltage;
                    TFunction.ResetRelay();
                    if (Current < Convert.ToInt32(Value[9]) && Current > Convert.ToInt32(Value[8]))
                    {
                        IsPAss = true;
                        break;
                    }
                    //if (THR2 > ky.maxValue || THR2 < ky.minValue)
                    //{
                    //    IsPAss = false;
                    //    continue;
                    //}
                    //if (THR2 < ky.maxValue && THR2 > ky.minValue)
                    //{
                    //    IsPAss = true;
                    //    break;
                    //}
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + Current.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(IsPAss ? "Pass" : "NG" + "," + "" + "," + "" + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, IsPAss ? "Pass" : "NG", IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        double THR2 = 0;
        private bool Pack_NTCResistacne()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];

                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-07");
                    Thread.Sleep(150);
                    THR2 = TFunction.ReadResistance10k();
                    TFunction.ResetRelay();
                    if (THR2 > ky.maxValue || THR2 < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (THR2 < ky.maxValue && THR2 > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + THR2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(THR2.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, THR2.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_IDResistance()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double ID2 = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-05");
                    Thread.Sleep(150);
                    ID2 = TFunction.ReadResistance();
                    TFunction.ResetRelay();
                    if (ID2 > ky.maxValue || ID2 < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (ID2 < ky.maxValue && ID2 > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ID2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ID2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ID2.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, ID2.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_Resistance()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double RES2 = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-04");
                    Thread.Sleep(150);
                    RES2 = TFunction.ReadResistance();
                    TFunction.ResetRelay();
                    if (RES2 > ky.maxValue || RES2 < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (RES2 < ky.maxValue && RES2 > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + RES2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + RES2.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(RES2.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, RES2.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_ACIR_Left()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                double ACIR_Res = 0;
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-03");
                    TFunction.SendString("2A-11-22-09");
                    Thread.Sleep(50);
                    ACIR_Res = TFunction.ACIR();
                    TFunction.ResetRelay();
                    if (ACIR_Res > ky.maxValue || ACIR_Res < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (ACIR_Res < ky.maxValue && ACIR_Res > ky.minValue)
                    {
                        IsPAss = true;

                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ACIR_Res.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + ACIR_Res.ToString() + "," + (IsPAss ? "OK" : "NG"));
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ACIR_Res.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, ACIR_Res.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        Dictionary<string, string> TimeSpan = new Dictionary<string, string>();
        private bool Pack_OCV_Left()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
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
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TFunction.SendString("2A-11-22-00");
                    Thread.Sleep(50);
                    Voltage = TFunction.MeasureVoltageInside();
                    BeforeVoltage = Voltage;
                    TFunction.ResetRelay();
                    //Voltage = Voltage / 1000;
                    if (Voltage > ky.maxValue || Voltage < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (Voltage < ky.maxValue && Voltage > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(Voltage.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, Voltage.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        double TemperatureEN = 0;
        private bool TemperatureEnvironment()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    TemperatureEN = TFunction.ReceiveDouble("F6-BB-DD");
                    if (TemperatureEN > ky.maxValue || TemperatureEN < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (TemperatureEN < ky.maxValue && TemperatureEN > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(TemperatureEN.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                ShowTestRow(TestRelayRow, TemperatureEN.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
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
                double Temp = 1 / ((Math.Log(resistance / 10000, Math.E) / 3435) + (1 / 298.15));
                return Temp - 273.15;
            }
            catch
            {

            }
            return -1;
        }

        private bool TemperatureDelta2()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            double DeltaTemp = 0;
            double NTCTemperature = 0;
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    NTCTemperature=NTC_TT(THR2);
                    DeltaTemp = NTCTemperature - TemperatureEN;
                    if (DeltaTemp > ky.maxValue || DeltaTemp < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (DeltaTemp < ky.maxValue && DeltaTemp > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(DeltaTemp.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                SaveLog(NTCTemperature.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", "NTC2");
                ShowTestRow(TestRelayRow, DeltaTemp.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool TemperatureDelta1()
        {
            bool IsPAss = false;
            string TestStepName = dgvTest.Rows[TestRelayRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            double DeltaTemp = 0;
            double NTCTemperature = 0;
            try
            {
                if (TestKY.GetValue(TestStepName) == true)
                {
                    return true;
                }
                ky = TestKY.dc[TestStepName];
                ShowTestRow(TestRelayRow, "Testing..", Color.Yellow);//开始测试的状态
                WriteResult(Channel_Number.ToString(), TestStepName + "开始测试");
                for (int i = 0; i <= ky.Retry; i++)
                {
                    NTCTemperature = NTC_TT(THR1);
                    DeltaTemp = NTCTemperature - TemperatureEN;
                    if (DeltaTemp > ky.maxValue || DeltaTemp < ky.minValue)
                    {
                        IsPAss = false;
                        continue;
                    }
                    if (DeltaTemp < ky.maxValue && DeltaTemp > ky.minValue)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    WriteResult(Channel_Number.ToString(), TestStepName + "测试电压结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                    Errorcode.Add(ky.ErrorCode);
                }
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结果为:" + BeforeVoltage.ToString() + "," + (IsPAss ? "OK" : "NG"));
                sW.Stop();
                WriteResult(Channel_Number.ToString(), TestStepName + "测试结束，测试时间为" + sW.Elapsed.TotalMilliseconds.ToString("0.00") + "\r\n");
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(DeltaTemp.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", ky.LogName);
                SaveLog(NTCTemperature.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "V", "NTC");
                ShowTestRow(TestRelayRow, DeltaTemp.ToString("0.00"), IsPAss == true ? Color.LawnGreen : Color.Red);
                //ShowTestData1(TestStepName + " =" + (IsPAss == true ? "PASS" : "Fail"));
                //ShowTestData1(TestStepName + " Test Time=" + sW.ElapsedMilliseconds);
                //ShowTestData1("//////////////////////////////////////////////////////////////////////"); sW.Stop();
            }
            catch (Exception ex)
            {
                //ShowErrCode(ex.ToString());
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }


        private void BgwRelay_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BgwRelay.IsBusy == false)
            {
                BgwRelay.RunWorkerAsync();
            }
        }
        int m_CommItem = 1;
        private void BgwComm_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BgwComm.CancellationPending)
            {
                Thread.Sleep(100);
                e.Cancel = true;
                return;
            }
            switch (m_CommItem)
            {
                case 1:
                    Thread.Sleep(10);
                    break;
                case 31:
                    if (!Pack_VenderID())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 32:
                    if (!Pack_ProductID())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 33:
                    if (!Pack_ECC())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 34:
                    if (!Pack_LifeSpanCounter())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 35:
                    if (!Barcode_writing())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 36:
                    if (!pack_ICVersion())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 37:
                    if (!pack_LockStatus())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 38:
                    if (!Pack_BarcodeCompare())
                    {
                        m_CommItem = 100;
                        break;
                    }
                    if (Errorcode.Count() > 0)
                    {
                        m_CommItem = 100;
                        break;
                    }
                    NextCommStep();
                    break;
                case 100:
                    if (Errorcode.Count != 0)
                    {
                        Watchtime.Stop();
                        timeClick.Stop();
                    }
                    m_CommItem = 1;
                    break;

            }
        }
        string BarCodeRead = "9LXUAYKC04X00092";
        public bool Barcode_writing()
        {
            bool IsPAss = false;
            bool bGon;
            string TestStepName = dgvTest.Rows[TestCommRow].Cells[0].Value.ToString();
            Dictionary<string, string> DIC = new Dictionary<string, string>();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                ShowTestRow(TestCommRow, "Testing..", Color.Yellow);//开始测试的状态
                for (int i = 0; i <= ky.Retry; i++)
                {
                    DIC=TFunctionComm.WriteBarcode(BarCodeRead);
                    if (DIC.Count==3)
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(IsPAss ? BarCodeRead : "NG" + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", Value[5]);
                SaveLog(DIC["Page7"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page7_Writing");
                SaveLog(DIC["Page12"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x","Page12_Writing");
                SaveLog(DIC["Page13"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x","Page13_Writing");
                ShowTestRow(TestCommRow, IsPAss ? BarCodeRead : "NG", IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_BarcodeCompare()
        {
            throw new NotImplementedException();
        }

        private bool pack_LockStatus()
        {
            bool IsPAss = false;
            bool bGon;
            string TestStepName = dgvTest.Rows[TestCommRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                ShowTestRow(TestCommRow, "Testing..", Color.Yellow);//开始测试的状态
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (ListView["NVM"].ToUpper().Trim().Replace("(100)", "").Trim() == Value[7].ToUpper().Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ListView["NVM"].ToUpper().Trim().Replace("(100)", "").Trim() + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", Value[5]);
                ShowTestRow(TestCommRow, ListView["NVM"].ToUpper().Trim().Replace("(100)", "").Trim(), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool pack_ICVersion()
        {
            bool IsPAss = false;
            bool bGon;
            string TestStepName = dgvTest.Rows[TestCommRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                ShowTestRow(TestCommRow, "Testing..", Color.Yellow);//开始测试的状态
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (ListView["SWI"].ToUpper().Trim().Replace("(100)", "").Trim() == Value[7].ToUpper().Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ListView["SWI"].ToUpper().Trim().Replace("(100)", "").Trim() + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", Value[5]);
                ShowTestRow(TestCommRow, ListView["SWI"].ToUpper().Trim().Replace("(100)", "").Trim(), IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_LifeSpanCounter()
        {
            bool IsPAss = false;
            bool bGon;
            string TestStepName = dgvTest.Rows[TestCommRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            string LifeF = "";
            string LifeX = "";
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                ShowTestRow(TestCommRow, "Testing..", Color.Yellow);//开始测试的状态
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (TFunctionComm.LifeSpanF("11-22-33-44-55-66-77-FF",out LifeF) && TFunctionComm.LifeSpanX("11-22-33-44-55-66-77-A0",out LifeX))
                    {
                        IsPAss = true;
                        break;
                    }
                }
                TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-00");
                TFunctionComm.SendStringNoReturn("11-22-33-44-55-66-FF-00");
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                string[] LIF=Regex.Split(LifeF, "\r\n", RegexOptions.IgnoreCase)[0].Split(new char[] { '=' });
                string[] LIX=Regex.Split(LifeX, "\r\n", RegexOptions.IgnoreCase)[0].Split(new char[] { '=' });
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(IsPAss ? "Pass" : "NG" + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", Value[5]);
                SaveLog(LIF[1].Substring(1,5) + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x","Life_Span_Counter_1");
                SaveLog(LIX[1].Substring(1,5) + "," + ky.minValue.ToString("0.00") + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x","Life_Span_Counter_2");
                ShowTestRow(TestCommRow, IsPAss ? "Pass" : "NG", IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_ECC()
        {
            bool IsPAss = false;
            bool bGon;
            string TestStepName = dgvTest.Rows[TestCommRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                ShowTestRow(TestCommRow, "Testing..", Color.Yellow);//开始测试的状态
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (ListView["ECCE"].ToUpper().Trim() == Value[7].ToUpper().Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ListView["ECCE"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", Value[5]);
                ShowTestRow(TestCommRow, ListView["ECCE"], IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private bool Pack_ProductID()
        {
            bool IsPAss = false;
            bool bGon;
            string TestStepName = dgvTest.Rows[TestCommRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                ShowTestRow(TestCommRow, "Testing..", Color.Yellow);//开始测试的状态
                for (int i = 0; i <= ky.Retry; i++)
                {
                    if (ListView["ProductID"].ToUpper().Trim() == Value[7].ToUpper().Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(ky.ErrorCode);
                }
                sW.Stop();
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ListView["ProductID"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", Value[5]);
                ShowTestRow(TestCommRow, ListView["ProductID"], IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }
        Dictionary<string, string> ListView;
        private bool Pack_VenderID()
        {
            ListView = new Dictionary<string, string>();
            bool IsPAss = false;
            bool bGon;
            string BackValue = "";
            string TestStepName = dgvTest.Rows[TestCommRow].Cells[0].Value.ToString();
            Stopwatch sW = new Stopwatch();
            sW.Restart();
            try
            {
                string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
                bool ITest = Convert.ToBoolean(Value[6]);
                bGon = Convert.ToBoolean(Value[29]);
                if (ITest == true)
                {
                    return true;
                }
                ShowTestRow(TestCommRow, "Testing..", Color.Yellow);//开始测试的状态
                for (int i = 0; i <= 1; i++)
                {
                    //MuTE.Mutex.WaitOne();
                    //
                    //Thread.Sleep(50);
                    ListView = TFunctionComm.GetVersion();
                    //MuTE.Mutex.ReleaseMutex();
                    if (ListView.Count() == 0)
                    {
                        IsPAss = false;
                        break;
                    }
                    BackValue = ListView["VenderID"];
                    if (ListView["VenderID"].ToUpper().Trim() == Value[7].ToUpper().Trim())
                    {
                        IsPAss = true;
                        break;
                    }
                }
                if (IsPAss == false)
                {
                    Errorcode.Add(Value[4]);
                }
                sW.Stop();
                TimeSpan.Add(TestStepName, sW.Elapsed.TotalSeconds.ToString("0.00"));
                SaveLog(ListView["Page0"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page0");
                SaveLog(ListView["Page1"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page1");
                SaveLog(ListView["Page2"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page2");
                SaveLog(ListView["Page3"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page3");
                SaveLog(ListView["Page4"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page4");
                SaveLog(ListView["Page5"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page5");
                SaveLog(ListView["Page6"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page6");
                SaveLog(ListView["Page7"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page7");
                SaveLog(ListView["Page11"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x","Page11");
                SaveLog(ListView["Page12"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x","Page12");
                SaveLog(ListView["Page13"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Page13");
                SaveLog(BackValue + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", Value[5]);
                SaveLog(ListView["UID"] + "," + ky.minValue.ToString("0.00") + "," + ky.maxValue.ToString("0.00") + "," + (IsPAss ? 1 : 0).ToString() + "," + "0x", "Unique_ID");
                ShowTestRow(TestCommRow, BackValue, IsPAss == true ? Color.LawnGreen : Color.Red);
            }
            catch (Exception ex)
            {
                Errorcode.Add(ex.ToString());
                IsPAss = false;
            }
            if (ky.bGon == true)
            {
                return true;
            }
            return IsPAss;
        }

        private void BgwComm_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (BgwComm.IsBusy == false)
            {
                BgwComm.RunWorkerAsync();
            }
        }
        public void ShowBarcode(string data)
        {
            if (BarCodeNumber.InvokeRequired)
            {
                Action<string> action = x => { BarCodeNumber.Text = x.Trim(); };
                BarCodeNumber.Invoke(action, data);
            }
            BarCodeNumber.Text = data.Trim();
        }
        private static bool isIntergerOrLetter(string content)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9+]+$");
            return reg1.IsMatch(content);
        }

        private bool Judement(string value, string barcode)//Value是参数档里面的条码，barcode是实际的条码
        {
            char[] list = value.Trim().ToArray();
            char[] list1 = barcode.ToArray();
            if (!(list.Length == list1.Length))
            {
                return false;
            }
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].ToString() == "*")
                {
                    continue;
                }
                if (!(list1[i].ToString() == list[i].ToString()))
                {
                    return false;
                }
            }
            if (!isIntergerOrLetter(barcode))
            {
                return false;
            }
            return true;
        }
        private void SaveLog(string Value, string LogName)
        {
            RecordData.Add(LogName + "," + Value);
        }

        string sBarcode;
        public bool StartUp()
        {
            TFunction.ResetEQ();
            ClearUI();
            return true;
            string[] Value = Convert.ToString(CParamter.BasicParameter[dgvTest.Rows[0].Cells[0].Value.ToString()]).Split(new char[] { ',' });
            Barcode.ReadBarcode(CParamter.BarcodeSpec.Trim().Length);
            Stopwatch stt = new Stopwatch();
            while (true)
            {
                while (Barcode._Batcode.Replace("\r", "").Replace("\n", "").Replace("\0", "").Trim().Length > 0)
                {
                    sBarcode = Barcode._Batcode.Replace("\0", "").Trim();
                    ClearUI();
                    ShowBarcode(sBarcode);
                    if (Convert.ToInt32(Value[8]) != sBarcode.Trim().Length)
                    {
                        Barcode._Batcode = "";
                        ShowTestStatues("条码长度错误", Color.Red, Color.Red);
                        //CTime.Delay(2000);
                        Thread.Sleep(2000);
                        ShowBarcode("");
                        return false;
                    }
                    if (!Judement(CParamter.BarcodeSpec, sBarcode))
                    {
                        Barcode._Batcode = "";
                        ShowTestStatues("条码格式错误", Color.Red, Color.Red);
                        //CTime.Delay(2000);
                        Thread.Sleep(2000);
                        ShowBarcode("");
                        return false;
                    }
                    ShowTestRow(0, "PASS", Color.Lime);
                    return true;
                }
                if (Convert.ToDouble(stt.Elapsed.TotalSeconds) > 5)
                {
                    ShowTestStatues("扫码未能启动", Color.Red, Color.Red);
                    Barcode.CloseBarcode();
                    Thread.Sleep(50);
                    return false;
                }
                Thread.Sleep(1000);
            }
        }
    }
    public class MuTE
    {
        public static System.Threading.Mutex Mutex = new Mutex(false, "Te");
    }
}
