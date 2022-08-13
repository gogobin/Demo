using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Threading;

namespace BaseProgram
{
    public class BarcodeScaner
    {
        SerialPort comscan1 = new SerialPort();
        BackgroundWorker Bgw;
        public bool OpenCom(string ComPort,int BaudRate,bool physicalButton)
        {
            try
            {

                comscan1.BaudRate = BaudRate;
                comscan1.PortName = ComPort;
                comscan1.DataBits = 8;
                //comscan1.ReadTimeout =100;
                comscan1.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
                comscan1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "1");
                comscan1.Open();
                comscan1.DtrEnable = true;
                if (physicalButton)
                {
                    comscan1.DataReceived += new SerialDataReceivedEventHandler(comscan1_DataReceived);
                    Bgw = new System.ComponentModel.BackgroundWorker();
                    Bgw.DoWork += Bgw_DoWork;
                    Bgw.RunWorkerCompleted += Bgw_Complete;
                    Bgw.RunWorkerAsync();
                }
                //BarcodeLenth = Lenth;
            }
            catch (Exception ex)
            {
                MessageBox.Show(" 打开失败 " + ex.Message);
                return false;
            }

            return true;
        }

        public void CloseScanCom()
        {
            if (comscan1.IsOpen)
            {
                comscan1.DiscardInBuffer();
                comscan1.DiscardOutBuffer();
                comscan1.Close();
                comscan1.Dispose();

            }
        }
        public string _Batcode = "";
        //public static string Barcode
        //{
        //    set { _Batcode = value; }
        //    get { return _Batcode; }
        //}
        public int BarcodeLenth = 21;
        public void CloseBarcode()
        {
            byte[] data = new byte[4];
            data[0] = 0x16;
            data[1] = 0x16;
            data[2] = 0x55;
            data[3] = 0x0d;
            comscan1.Write(data, 0, 4);
        }

        //public int BarcodeLenth = 0;
        public string ReadBarcode(int lenth)
        {
            //byte[] data = new byte[4];
            //data[0] = 0x16;
            //data[1] = 0x16;
            //data[2] = 0x54;
            //data[3] = 0x0d;
            //comscan1.Write(data, 0, 4);

            _Batcode = "";
            BarcodeLenth = lenth;
            byte[] dataw = new byte[3];
            dataw[0] = 0x16;
            dataw[1] = 0x54;
            dataw[2] = 0x0d;

            comscan1.Write(dataw, 0, dataw.Length);
            string SBarcode = "";
            Stopwatch stopwatch = new Stopwatch();
            Thread.Sleep(200);
            stopwatch.Start();
            do
            {
                if (comscan1.BytesToRead >= lenth)
                {
                    byte[] data = new byte[BarcodeLenth];
                    _Batcode = comscan1.ReadExisting().Replace("\r", "");// (data, 0, BarcodeLenth);

                    //_Batcode = System.Text.Encoding.Default.GetString(data).Replace("\0", "").Replace("\r", "");
                    if (_Batcode != null && _Batcode.Trim().Length > 0)
                    {
                        break;
                    }
                }
                Thread.Sleep(100);
            } while (stopwatch.ElapsedMilliseconds < 2500);

            return _Batcode;
        }

        //System.Threading.Mutex MUtexx = new System.Threading.Mutex(true, "TestCode");
        public bool ReadBarcodedd()
        {
            //MUtexx.WaitOne();
            _Batcode = "";

            //     comscan1.WriteLine("123");
            //     Thread.sleep(250);
            //string sResult = "";
            for (int i = 0; i < 3; i++)
            {
                comscan1.WriteLine("123");
                Thread.Sleep(250);
                _Batcode = comscan1.ReadExisting().Replace("\r", "");// (data, 0, BarcodeLenth);
                if (_Batcode.Trim().Length > 00)
                {
                    //MUtexx.ReleaseMutex();
                    return true;
                }
                Thread.Sleep(100);
            }

            //Stopwatch stopwatch = new Stopwatch();
            //Thread.sleep(200);
            //stopwatch.Start();
            //do
            //{
            //    if (comscan1.BytesToRead >= lenth)
            //    {
            //        byte[] data = new byte[BarcodeLenth];
            //        _Batcode = comscan1.ReadExisting().Replace("\r", "");// (data, 0, BarcodeLenth);

            //        //_Batcode = System.Text.Encoding.Default.GetString(data).Replace("\0", "").Replace("\r", "");
            //        if (_Batcode != null && _Batcode.Trim().Length > 0)
            //        {
            //            break;
            //        }
            //    }
            //    Thread.sleep(100);
            //} while (stopwatch.ElapsedMilliseconds < 2500);
            //MUtexx.ReleaseMutex();
            return false;
        }

        public string ReadScannerConfigure()
        {
            byte[] data = new byte[11];
            data[0] = 0x16;
            data[1] = 0x4d;
            data[2] = 0x0d;
            data[3] = 0x52;
            data[4] = 0x45;
            data[5] = 0x56;
            data[6] = 0x49;
            data[7] = 0x4E;
            data[8] = 0x46;
            data[9] = 0x2E;
            data[10] = 0x00;
            comscan1.Write(data, 0, 11);
            data = new byte[1024];
            System.Threading.Thread.Sleep(150);
            try
            {
                comscan1.Read(data, 0, 1024);
                string ReturnValue = "";
                ReturnValue = System.Text.Encoding.Default.GetString(data);
                string[] ALLData = Regex.Split(ReturnValue, "\r\n", RegexOptions.None);
                string[] SerialNumber = ALLData[4].Split(new char[] { ':' });
                return SerialNumber[1].Replace(" ", "");
            }
            catch
            {
                return "";
            }
        }

        private void comscan1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _Batcode = "";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                if (comscan1.BytesToRead == BarcodeLenth)
                {
                    break;
                }
                Thread.Sleep(1);
            } while (stopwatch.ElapsedMilliseconds < 1000);
            if (comscan1.BytesToRead != 0)
            {
                //int Len = comscan1.BytesToRead;
                byte[] data = new byte[BarcodeLenth];
                //byte[] data = new byte[Len];
                comscan1.Read(data, 0, BarcodeLenth);
                Thread.Sleep(200);
                _Batcode = System.Text.Encoding.Default.GetString(data);
            }

        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Bgw.CancellationPending)
            {
                System.Threading.Thread.Sleep(100);
                e.Cancel = true;
                return;
            }
            comscan1.WriteLine("1");
            Thread.Sleep(100);
        }

        private void Bgw_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Bgw.IsBusy == false)
            {
                Bgw.RunWorkerAsync();
            }
        }
    }
}
