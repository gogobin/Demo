using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivi.Driver.Interop;
using Ivi.Visa.Interop;
using System.Threading;
using System.Diagnostics;

namespace ControlProgram
{
    public class CAgilent
    {
        #region "Constants"
        private const string STOP_CHARS = "\r\n";

        private const string CMD_ERROR = "System:Error?\r\n";
        private const string RSP_NO_ERROR = "+0,\"No error\"";

        private const String CMD_REMOTE = "system:remote\r\n";
        private const String CMD_LOCAL = "system:local\r\n";
        //private const string CMD_MEAS_VOLT = "meas:volt:dc?\r\n";
        private const string CMD_MEAS_VOLT = "MEAS:VOLT:DC?\r\n";
        private const string CMD_MEAS_CURR = "meas:curr:dc?\r\n";
        private const string CMD_MEAS_RES = "meas:res?\r\n";

        private const string CMD_CFG_VOLT_10 = "conf:volt:dc 10,  0.00001\r\n";
        private const string CMD_CFG_VOLT_20 = "conf:volt:dc 20,  0.00001\r\n";
        private const string CMD_CFG_CURR = "conf:curr:dc  0.0001\r\n";
        private const string CMD_CFG_RES = "CONF:RES 1000000,  DEF\r\n";
        private const string CMD_TRIG_IMM = "trigger:source immediate\r\n";
        private const string CMD_READ = "read?\r\n";
        #endregion

        Ivi.Visa.Interop.ResourceManager rm = new Ivi.Visa.Interop.ResourceManager(); //Open up a new resource manager
        Ivi.Visa.Interop.FormattedIO488 myDmm = new Ivi.Visa.Interop.FormattedIO488(); //Open a new Formatted IO 488 session 
        #region "Constructors"
        bool flag;
        System.Threading.Mutex mutex;
        public enum ControlMode
        {
            USBMode,
            TCPMode
        }
        public CAgilent(string Address,ControlMode mode)
        {
            mutex = new Mutex(true, "Test", out flag);
            mutex.ReleaseMutex();
            string DutAddr = Address; //CIniFile.ReadValue("Address", string.Format("USB"), ".\\USB Address.ini");// "USB0::0x2A8D::0x1301::MY57220321::0::INSTR"; //String for USB  
                                      //myDmm.IO = (IMessage)rm.Open("TCPIP0::192.168.1.60", AccessMode.NO_LOCK, 1000, ""); //Open up a handle to the DMM 
            if (mode == ControlMode.TCPMode)
            {
                myDmm.IO = (IMessage)rm.Open("TCPIP0::" + Address, AccessMode.NO_LOCK, 1000, "");
            }
            else if (mode == ControlMode.USBMode)
            {
                myDmm.IO = (IMessage)rm.Open(Address, AccessMode.NO_LOCK, 1000, "");
            }
            myDmm.IO.Timeout = 700;
            myDmm.IO.Clear(); //Send a device clear first to stop any measurements in process
            Thread.Sleep(100);
            myDmm.WriteString("*RST", true); //Reset the device
                                             //Thread.Sleep(100);
                                             //myDmm.WriteString("*IDN?", true); //Get the IDN string                
                                             ////string IDN = myDmm.ReadString();
                                             ////Console.WriteLine(IDN); //report the DMM's identity
                                             //Thread.Sleep(100);
        }
        #endregion

        #region IMultimeter Θ

        public bool Open()
        {
            //if (!m_deviceInterface.Open())
            //{
            //    return false;
            //}

            return true;// myDmm.WriteString(CMD_REMOTE, true);
        }


        public bool Close()
        {
            myDmm.WriteString("system:local", true);
            //myDmm .
            //    return false;
            //}
            return true;
            //return m_deviceInterface.Close();
        }


        public bool MeasureVoltage10V(out float dblVoltage)
        {
            dblVoltage = 0;
            mutex.WaitOne();
            string Readings1 = "1e1";
            try
            {
                myDmm.WriteString(CMD_CFG_VOLT_10, false);
                //CTime.Delay(50);
                Thread.Sleep(50);
                myDmm.WriteString(CMD_MEAS_VOLT, true);
                //CTime.Delay(100);
                Thread.Sleep(50);
                Readings1 = myDmm.ReadString();
            }
            catch
            {

            }
            mutex.ReleaseMutex();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblVoltage = Convert.ToSingle(b * Math.Pow(10, c) * 1000);
            }
            else
            {
                dblVoltage = float.Parse(Readings1) * 1000;
            }
            //myDmm.WriteString(CMD_CFG_CURR, true);
            return true;
        }



        public double MeasureVoltage()
        {
            double dblVoltage = 0;
            mutex.WaitOne();
            string Readings1 = "1e1";
            try
            {
                myDmm.WriteString(CMD_CFG_VOLT_10, false);
                //CTime.Delay(50);
                Thread.Sleep(50);
                myDmm.WriteString(CMD_MEAS_VOLT, true);
                //CTime.Delay(100);
                Thread.Sleep(50);
                Readings1 = myDmm.ReadString();
            }
            catch
            {

            }
            mutex.ReleaseMutex();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblVoltage = Convert.ToSingle(b * Math.Pow(10, c) * 1000);
            }
            else
            {
                dblVoltage = float.Parse(Readings1) * 1000;
            }
            return dblVoltage*10;
            //myDmm.WriteString(CMD_CFG_CURR, true);
        }



        public bool MeasureVoltage20V(out float dblVoltage)
        {
            dblVoltage = 0;
            mutex.WaitOne();
            myDmm.WriteString(CMD_CFG_VOLT_20, true);
            myDmm.WriteString(CMD_MEAS_VOLT, true);
            string Readings1 = myDmm.ReadString();
            mutex.ReleaseMutex();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblVoltage = Convert.ToSingle(b * Math.Pow(10, c) * 1000);
            }
            else
            {
                dblVoltage = float.Parse(Readings1) * 1000;
            }
            return true;
        }

        public bool MeasureCurrent(out float dblCurrent)
        {
            dblCurrent = 0;
            mutex.WaitOne();
            myDmm.WriteString(CMD_CFG_CURR, true);
            myDmm.WriteString(CMD_MEAS_CURR, true);
            string Readings1 = myDmm.ReadString();
            mutex.ReleaseMutex();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblCurrent = Convert.ToSingle(b * Math.Pow(10, c) * 1000);
            }
            else
            {
                dblCurrent = float.Parse(Readings1) * 1000;
            }


            return true;
        }
        public float MeasureCurrent()
        {
            Stopwatch ssrt = new Stopwatch();
            ssrt.Restart();
            float dblCurrent = 0;
            //myDmm.WriteString("CONF:CURR:DC 0.001,0.00001" + string.Format(";CURR:DC:NPLC 0.1;"), true);
            //myDmm.WriteString("CURR:DC:NPLC 1", true);
            myDmm.WriteString("CONF:CURR:DC 3,0.1", true);
            myDmm.WriteString("CURR:DC:NPLC 1", true);
            System.Threading.Thread.Sleep(10);
            myDmm.WriteString("READ?", true);
            //CTime.Delay(10);
            Thread.Sleep(10);
            string Readings1 = myDmm.ReadString();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblCurrent = Convert.ToSingle(b * Math.Pow(10, c) * 1000);
            }
            else
            {
                dblCurrent = float.Parse(Readings1) * 1000;
            }
            double Lodt = ssrt.Elapsed.TotalMilliseconds;

            return dblCurrent;
        }

        public float MeasureCurrent(string Mode)
        {
            Stopwatch ssrt = new Stopwatch();
            ssrt.Restart();
            float dblCurrent = 0;
            //myDmm.WriteString("CONF:CURR:DC 0.001,0.00001" + string.Format(";CURR:DC:NPLC 0.1;"), true);
            if (Mode == "100uA")
            {
                myDmm.WriteString("CONF:CURR:DC 3,0.0001", true);
            }
            else if (Mode == "1mA")
            {
                myDmm.WriteString("CONF:CURR:DC 3,0.001", true);
            }
            else if (Mode == "10mA")
            {
                myDmm.WriteString("CONF:CURR:DC 3,0.01", true);
            }
            else if (Mode == "100mA")
            {
                myDmm.WriteString("CONF:CURR:DC 0.1,default", true);
            }
            else
            {
                myDmm.WriteString("CONF:CURR:DC 3,0.0001", true);
            }
            //myDmm.WriteString(CMD_CFG_CURR, true);
            Thread.Sleep(10);
            myDmm.WriteString("READ?", true);
            Thread.Sleep(10);
            string Readings1 = myDmm.ReadString();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblCurrent = Convert.ToSingle(b * Math.Pow(10, c) * 1000);
            }
            else
            {
                dblCurrent = float.Parse(Readings1) * 1000;
            }
            double Lodt = ssrt.Elapsed.TotalMilliseconds;
            return dblCurrent;
        }

        public float Read()
        {
            float dblCurrent = 0;
            myDmm.WriteString("READ?", true);
            System.Threading.Thread.Sleep(10);
            string Readings1 = myDmm.ReadString();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblCurrent = Convert.ToSingle(b * Math.Pow(10, c) * 1000);
            }
            else
            {
                dblCurrent = float.Parse(Readings1) * 1000;
            }
            return dblCurrent;
        }

        public bool MeasureResistance(out float dblCurrent)
        {
            dblCurrent = 0;
            mutex.WaitOne();
            myDmm.WriteString(CMD_CFG_RES, true);
            myDmm.WriteString(CMD_MEAS_RES, true);
            string Readings1 = myDmm.ReadString();
            mutex.ReleaseMutex();
            if (Readings1.ToUpper().Contains("E"))
            {
                float b = float.Parse(Readings1.ToUpper().Split('E')[0].ToString());//整数部分
                float c = float.Parse(Readings1.ToUpper().Split('E')[1].ToString());//指数部分
                dblCurrent = Convert.ToSingle(b * Math.Pow(10, c) / 1000);
            }
            else
            {
                dblCurrent = float.Parse(Readings1) / 1000;
            }

            return true;
        }

        #endregion

        #region "Private Interface"
        //private bool CheckPackage(string sCommand, string sResponse)
        //{
        ////    if (!m_deviceInterface.Query(CMD_ERROR, ref sResponse, STOP_CHARS, CheckPackageIgnor))
        ////    {
        ////        return false;
        ////    }
        ////    else
        ////    {
        ////        return RSP_NO_ERROR == sResponse ? true : false;
        ////    }
        //}

        private bool CheckPackageIgnor(string sCommand, string sResponse)
        {
            return true;
        }
        #endregion

    }
}
