using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace BaseProgram
{
    public static class LoadFileClass
    {
        public static string ParamterFilePath()
        {
            string FilePath = "";
            string LocalFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Process processes = Process.GetCurrentProcess();
            string[] name = processes.ProcessName.Split(new char[] { '.' });
            for (int i = 0; i < Directory.GetFiles(LocalFile).ToList().Count; i++)
            {
                string FileName = Directory.GetFiles(LocalFile)[i].ToString();
                if (FileName.EndsWith(".xml") && !FileName.EndsWith("MyDll.xml"))
                {
                    string[] filePath = FileName.Split(new char[] { '.' })[0].Split(new char[] { '\\' });
                    if (name[0].Trim() == filePath[filePath.Length - 1])
                    {
                        continue;
                    }
                    FilePath = Directory.GetFiles(LocalFile)[i].ToString();
                }
            }
            return FilePath;
        }
        public static bool LoadFile(string FilePth)
        {
            XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”
            xmlDocument.Load(FilePth);//载入路径这个xml
            try
            {
                #region  TestStep
                XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("/Test/TestStep").ChildNodes;//选择class为根结点并得到旗下所有子节点

                //dgvTestStep.Rows.Clear();//清空dataGridView1，防止和上次处理的数据混乱
                //Paramter.TestStep = new List< string>();
                CParamter.TestStep = new Dictionary<string, string>();
                CParamter.BasicParameter = new Dictionary<string, string>();
                CParamter.LogName = new Dictionary<string, string>();
                foreach (XmlNode xmlNode in xmlNodeList)//遍历class的所有节点
                {
                    //int Repeat = 0;
                    XmlElement xmlElement = (XmlElement)xmlNode;
                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim().Length == 0)
                    {
                        continue;
                    }
                    CParamter.TestStep.Add(xmlElement.ChildNodes.Item(1).InnerText, xmlElement.ChildNodes.Item(0).InnerText);
                }

                foreach (XmlNode xmlNode in xmlNodeList)//遍历class的所有节点
                {
                    //int Repeat = 0;
                    XmlElement xmlElement = (XmlElement)xmlNode;

                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim().Length == 0)
                    {
                        continue;
                    }
                    string Value = "";
                    for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
                    {
                        Value += xmlElement.ChildNodes.Item(i).InnerText + ",";
                    }
                    CParamter.BasicParameter.Add(xmlElement.ChildNodes.Item(1).InnerText, Value);

                }
                #endregion
                List<string> Loglist = new List<string>();
                foreach (XmlNode xmlNode in xmlNodeList)//遍历class的所有节点
                {
                    XmlElement xmlElement = (XmlElement)xmlNode;

                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim().Length == 0)
                    {
                        continue;
                    }

                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim() == "46")
                    {

                        CParamter.ProductNo = xmlElement.ChildNodes.Item(7).InnerText.Trim();

                        //if (xmlElement.ChildNodes.Item(9).InnerText.Trim().Length > 0)
                        //{
                        //    CParamter.CellCount = Convert.ToInt16(xmlElement.ChildNodes.Item(9).InnerText.Trim());
                        //}
                        //CParamter.CellType = xmlElement.ChildNodes.Item(10).InnerText.Trim();

                        //if (xmlElement.ChildNodes.Item(11).InnerText.Trim().Length > 0)
                        //{
                        //    CParamter.BarcodeLen = Convert.ToInt16(xmlElement.ChildNodes.Item(11).InnerText.Trim());
                        //}

                        //if (xmlElement.ChildNodes.Item(12).InnerText.Trim().Length > 0)
                        //{
                        //    CParamter.BarcodeSpec = Convert.ToString(xmlElement.ChildNodes.Item(12).InnerText.Trim());
                        //    CParamter.BarcodeSpecStartIndex = GetBarcodeSpecIndex(Convert.ToString(xmlElement.ChildNodes.Item(12).InnerText.Trim()));
                        //    CParamter.BarcodeSpecLen = GetBarcodeSpecLength(Convert.ToString(xmlElement.ChildNodes.Item(12).InnerText.Trim()));
                        //}


                        CParamter.Station = xmlElement.ChildNodes.Item(16).InnerText.Trim();
                        //CParamter.BQType = xmlElement.ChildNodes.Item(17).InnerText.Trim();
                        //CParamter.sTraffic = xmlElement.ChildNodes.Item(18).InnerText.Trim();
                        //CParamter.sDevice = xmlElement.ChildNodes.Item(19).InnerText.Trim();
                        CParamter.FileName = xmlElement.ChildNodes.Item(20).InnerText.Trim();
                        if (xmlElement.ChildNodes.Item(15).InnerText.Trim().Trim().Length > 0)
                        {
                            CParamter.CellVoltage = Convert.ToInt16(xmlElement.ChildNodes.Item(15).InnerText.Trim());
                        }
                        if (xmlElement.ChildNodes.Item(13).InnerText.Trim().Trim().Length > 0)
                        {
                            CParamter.WakeUpR = Convert.ToBoolean(xmlElement.ChildNodes.Item(13).InnerText.Trim());
                        }
                        //if (xmlElement.ChildNodes.Item(14).InnerText.Trim().Trim().Length > 0)
                        //{
                        //    CParamter.WakeUpN = Convert.ToBoolean(xmlElement.ChildNodes.Item(14).InnerText.Trim());
                        //}
                        if (xmlElement.ChildNodes.Item(21).InnerText.Trim().Trim().Length > 0)
                        {
                            CParamter.ClearCellVoltage = Convert.ToBoolean(xmlElement.ChildNodes.Item(21).InnerText.Trim());
                        }

                        //if (xmlElement.ChildNodes.Item(22).InnerText.Trim().Trim().Length > 0)
                        //{
                        //    CParamter.StartScanBarcode = Convert.ToBoolean(xmlElement.ChildNodes.Item(22).InnerText.Trim());
                        //}
                        //if (xmlElement.ChildNodes.Item(23).InnerText.Trim().Trim().Length > 0)
                        //{
                        //    CParamter.StartManual = Convert.ToBoolean(xmlElement.ChildNodes.Item(23).InnerText.Trim());
                        //}
                        ////public static bool ClearCellVoltage = false;
                        //
                        //break;
                    }
                    if (!Loglist.Contains(xmlElement.ChildNodes.Item(5).InnerText.Replace(" ", "")))
                    {
                        Loglist.Add(xmlElement.ChildNodes.Item(5).InnerText.Replace(" ", ""));
                        if (xmlElement.ChildNodes.Item(5).InnerText.Replace(" ", "") == "")
                        {
                            continue;
                        }
                        CParamter.LogName.Add(xmlElement.ChildNodes.Item(5).InnerText.Replace(" ", ""), "");
                    }
                }
                //CParamter.LogName.Add("oqc_BlockB", "");
                //CParamter.LogName.Add("Temp_environment", "");
                //CParamter.LogName.Add("ntc_temp", "");
                //CParamter.LogName.Add("tempIC", "");
                //CParamter.LogName.Add("VoltageMeter", "");
                //CParamter.LogName.Add("VoltageIC", "");
                //CParamter.LogName.Add("current_reference_Chg", "");
                //CParamter.LogName.Add("current_IC_Chg", "");
                //CParamter.LogName.Add("current_reference_dsg", "");
                //CParamter.LogName.Add("current_IC_dsg", "");
                //CParamter.LogName.Add("current_reference_Chg_10mA", "");
                //CParamter.LogName.Add("current_IC_Chg_10mA", "");
                //CParamter.LogName.Add("current_reference_dsg_10mA", "");
                //CParamter.LogName.Add("current_IC_dsg_10mA", "");
            }
            catch
            {
                MessageBox.Show("Load FileFail");
                return false;
            }

            return true;
        }
        private static int GetBarcodeSpecIndex(string BarcodeSpec)
        {
            int Index = 1;
            string[] File = BarcodeSpec.Trim().Split(new char[] { '*' });
            for (int i = 0; i < File.Length; i++)
            {
                if (File[i].Trim().Length != 0)
                {
                    Index = i;
                    break;
                }
            }
            return Index;
        }
        private static int GetBarcodeSpecLength(string BarcodeSpec)
        {
            int Index = 1;
            string[] File = BarcodeSpec.Trim().Split(new char[] { '*' });
            for (int i = 0; i < File.Length; i++)
            {
                if (File[i].Trim().Length != 0)
                {
                    Index = File[i].Trim().Length;
                    break;
                }
            }
            return Index;
        }
        public static string LogFilePath()
        {
            System.IO.DriveInfo[] allDrives = System.IO.DriveInfo.GetDrives();
            string downLoadPath = @"C:\Data";
            if (allDrives.Length > 1)
            {
                foreach (DriveInfo driver in allDrives)
                {

                    string name = "(" + driver.Name.Substring(0, driver.Name.IndexOf(":") + 1) + ")";//读驱动器的名字

                    ListViewItem node = new ListViewItem();

                    if (driver.DriveType == DriveType.Fixed && !driver.Name.Contains("C"))
                    {
                        downLoadPath = driver.Name + @"Data";
                        break;
                    }

                }

                if (Directory.Exists(downLoadPath) == false)
                {
                    Directory.CreateDirectory(downLoadPath);
                }
            }
            return downLoadPath;
        }
    }

    public static class CParamter
    {
        public static string BQType = "";
        public static string Station = "";
        public static string sDevice = "";
        public static string sTraffic;
        public static string ProductName = "";
        public static string ProductNo = "";
        public static string FileName = "";
        public static string CellType = "";
        public static int CellCount = 0;
        public static string BarcodeSpec = "";
        public static bool WakeUpR = false;
        public static bool ClearCellVoltage = false;
        public static bool WakeUpN = false;
        public static bool StartScanBarcode = true;
        public static bool StartManual = false;
        public static int CellVoltage = 0;
        public static int BarcodeLen = 21;
        public static bool SacanBarcode = true;

        public static int BmuBarcodeLen = 0;

        public static int BarcodeSpecLen = 0;
        public static int BarcodeSpecStartIndex = 0;


        public static Dictionary<string, string> TestStep;
        public static Dictionary<string, string> BasicParameter;
        public static Dictionary<string, string> LogList;
        public static Dictionary<string, string> LogName;
    }
}
