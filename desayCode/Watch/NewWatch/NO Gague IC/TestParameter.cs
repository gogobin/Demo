using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace IC.NOGagueIC.SW
{
    public class TestParameter
    {
        public bool LoadFile(string FilePth)
        {
            XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”
            xmlDocument.Load(FilePth);//载入路径这个xml
            try
            {
                #region  TestStep
                XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("/Test/TestStep").ChildNodes;//选择class为根结点并得到旗下所有子节点
                Paramter.TestStep = new Dictionary<string, string>();
                Paramter.BasicParameter = new Dictionary<string, string>();
                foreach (XmlNode xmlNode in xmlNodeList)//遍历class的所有节点
                {
                    int Repeat = 0;
                    XmlElement xmlElement = (XmlElement)xmlNode;

                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim().Length == 0 || xmlElement.ChildNodes.Item(0).InnerText.Trim() == "46")
                    {
                        continue;
                    }

                    foreach (string key in Paramter.TestStep.Keys)
                    {
                        if (key == (xmlElement.ChildNodes.Item(1).InnerText.Trim()))
                        {
                            Repeat++;
                        }
                        if (key.Contains((xmlElement.ChildNodes.Item(1).InnerText.Trim() + "_")))
                        {
                            Repeat++;
                        }
                    }

                    if (Repeat == 0)
                    {
                        Paramter.TestStep.Add(xmlElement.ChildNodes.Item(1).InnerText, xmlElement.ChildNodes.Item(0).InnerText);
                    }
                    else
                    {
                        Paramter.TestStep.Add(xmlElement.ChildNodes.Item(1).InnerText + "_" + Repeat.ToString(), xmlElement.ChildNodes.Item(0).InnerText);
                    }
                }

                foreach (XmlNode xmlNode in xmlNodeList)//遍历class的所有节点
                {
                    int Repeat = 0;
                    XmlElement xmlElement = (XmlElement)xmlNode;

                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim().Length == 0 || xmlElement.ChildNodes.Item(0).InnerText.Trim() == "46")
                    {
                        continue;
                    }

                    foreach (string key in Paramter.BasicParameter.Keys)
                    {
                        if (key == (xmlElement.ChildNodes.Item(1).InnerText.Trim()))
                        {
                            Repeat++;
                        }
                        if (key.Contains((xmlElement.ChildNodes.Item(1).InnerText.Trim() + "_")))
                        {
                            Repeat++;
                        }
                    }

                    string Value = "";
                    for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
                    {
                        Value += xmlElement.ChildNodes.Item(i).InnerText + ",";
                    }
                    if (Repeat == 0)
                    {
                        Paramter.BasicParameter.Add(xmlElement.ChildNodes.Item(1).InnerText, Value);
                    }
                    else
                    {
                        Paramter.BasicParameter.Add(xmlElement.ChildNodes.Item(1).InnerText + "_" + Repeat.ToString(), Value);
                    }
                }
                #endregion

                foreach (XmlNode xmlNode in xmlNodeList)//遍历class的所有节点
                {
                    XmlElement xmlElement = (XmlElement)xmlNode;

                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim().Length == 0)
                    {
                        continue;
                    }

                    if (xmlElement.ChildNodes.Item(0).InnerText.Trim() == "46")
                    {
                        Paramter.ProductName = xmlElement.ChildNodes.Item(7).InnerText.Trim();
                        Paramter.ProductNo = xmlElement.ChildNodes.Item(8).InnerText.Trim();

                        if (xmlElement.ChildNodes.Item(9).InnerText.Trim().Length > 0)
                        {
                            Paramter.CellCount = Convert.ToInt16(xmlElement.ChildNodes.Item(9).InnerText.Trim());
                        }
                        Paramter.CellType = xmlElement.ChildNodes.Item(10).InnerText.Trim();

                        if (xmlElement.ChildNodes.Item(11).InnerText.Trim().Length > 0)
                        {
                            Paramter.BarcodeLen = Convert.ToInt16(xmlElement.ChildNodes.Item(11).InnerText.Trim());
                        }

                        if (xmlElement.ChildNodes.Item(12).InnerText.Trim().Length > 0)
                        {
                            Paramter.BarcodeSpec = Convert.ToString(xmlElement.ChildNodes.Item(21).InnerText.Trim());
                            Paramter.BarcodeSpecStartIndex  =GetBarcodeSpecIndex( Convert.ToString(xmlElement.ChildNodes.Item(21).InnerText.Trim()));
                            Paramter.BarcodeSpecLen = GetBarcodeSpecLength(Convert.ToString(xmlElement.ChildNodes.Item(21).InnerText.Trim()));
                        }

                        if (xmlElement.ChildNodes.Item(13).InnerText.Trim().Length > 0)
                        {
                            Paramter.BmuBarcodeLen = Convert.ToInt16(xmlElement.ChildNodes.Item(13).InnerText.Trim());
                        }
                        Paramter.Station = xmlElement.ChildNodes.Item(14).InnerText.Trim();
                        Paramter.BQType = xmlElement.ChildNodes.Item(15).InnerText.Trim();
                        Paramter.sTraffic = xmlElement.ChildNodes.Item(16).InnerText.Trim();
                        Paramter.sDevice = xmlElement.ChildNodes.Item(17).InnerText.Trim();
                        break;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Load FileFail");
                return false;
            }

            return true;
        }
        private int GetBarcodeSpecIndex(string BarcodeSpec)
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
            return Index + 1;
        }
        private int GetBarcodeSpecLength(string BarcodeSpec)
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
    }
    public static class Paramter
    {
        public static string BQType = "";
        public static string Station = "";
        public static string sDevice = "";
        public static string sTraffic;
        public static string ProductName = "";
        public static string ProductNo = "";
        public static string CellType = "";
        public static int CellCount = 0;
        public static string BarcodeSpec = "";
        public static int BarcodeLen = 0;
        public static int BmuBarcodeLen = 0;

        public static int BarcodeSpecLen = 0;
        public static int BarcodeSpecStartIndex = 0;


        public static Dictionary<string, string> TestStep;
        public static Dictionary<string, string> BasicParameter;
        public static Dictionary<string, string> LogList;
        public static Dictionary<string, string> LogName;
        public static List<string> Errorcode;
    }
}
