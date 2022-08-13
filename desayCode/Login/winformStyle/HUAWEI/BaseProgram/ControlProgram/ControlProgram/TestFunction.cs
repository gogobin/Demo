using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ControlProgram
{
    public class TestFunction
    {
        TcpClient Client = new TcpClient();
        byte[] ReceiveBuffer;
        public enum CurrentMode
        {
            highAccuracy,
            lowAccuracy
        }
        private static byte[] SplitHexString(string sOriginalString, char chSeparator)
        {
            string[] sResults;
            sResults = sOriginalString.Split(chSeparator);
            byte[] lstResult = new byte[sResults.Length];
            for (int i = 0; i < sResults.Length; i++)
            {
                lstResult[i] = Convert.ToByte(sResults[i], 16);
            }
            return lstResult;
        }

        public bool OpenConnt(string ip, int port)
        {
            if (!Client.Open(ip, port))
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            Client.Close();
        }

        public bool SendString(string cmd)
        {
            ReceiveBuffer = new byte[cmd.Split(new char[] { '-' }).Count()];
            if (!Client.writeCMD(cmd, out ReceiveBuffer))
            {
                return false;
            }
            if (!JudgeCMD(ReceiveBuffer, cmd))
            {
                return false;
            }
            return true;
        }

        public bool SendStringNoReturn(string cmd)
        {
            if (!Client.writeCMDnoReturn(cmd))
            {
                return false;
            }
            return true;
        }

        public bool JudgeCMD(byte[] Buffer,string cmd)
        {
            List<byte> list = Buffer.ToList();
            string ReturnValue = "";
            int count=cmd.Split(new char[] { '-' }).Count();
            for (int i = 0; i < count; i++)
            {
                ReturnValue += Buffer[i].ToString("X2")+"-";
            }
            if (cmd + "-" == ReturnValue)
            {
                return true;
            }
            return false;
        }

        static byte SLE95250_crc_cal(byte[]arr1)
        {
            int i;
            int len = 2;
            byte crc_mul = 0x31;
            byte a = 0x00;
            int Number = 0;
            while ((len--)!=0)
            {
                a ^= (arr1[Number]);
                Number++;
                for (i = 8; i>0; i--)
                {
                    a <<= 1;
                    if (Convert.ToBoolean(a & 0x08))
                    {
                        a ^= crc_mul;
                    }
                }
            }
            return a;
        }

        public Dictionary<string,string> WriteBarcode(string BarCode)
        {
            ReceiveBuffer = new byte[1024];
            byte[] ByteList = System.Text.ASCIIEncoding.ASCII.GetBytes(BarCode);
            Dictionary<string, string> ReturnDic = new Dictionary<string, string>();
            byte[] CheckSUM=System.Text.ASCIIEncoding.ASCII.GetBytes(BarCode.Substring(4, 2));
            int Page7CheckSUM=SLE95250_crc_cal(CheckSUM);
            if (!Client.writeCMD("11-22-33-07-"+CheckSUM[0].ToString("X2")+"-"+CheckSUM[1].ToString("X2")+"-"+Page7CheckSUM.ToString("X2")+"-00", out ReceiveBuffer))
            {
                return ReturnDic;
            } 
            if (!Client.writeCMD("11-22-33-07-41-59-16-00",out ReceiveBuffer))
            {
                return ReturnDic;
            }
            string ReturnValue = System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer);
            string[] Value = Regex.Split(ReturnValue, "\r\n");
            if (!System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("page7"))
            {
                return ReturnDic;
            }
            if (System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("Failed"))
            {
                return ReturnDic;
            }
            string Page7 = GetUID(Value[1]);
            ReturnDic.Add("Page7", Page7);
            if (!Client.writeCMD("11-22-33-12-" + ByteList[0].ToString("X2") + "-" + ByteList[1].ToString("X2") + "-" + ByteList[2].ToString("X2") + "-" + ByteList[3].ToString(), out ReceiveBuffer))
            {
                return ReturnDic;
            }
            if (!System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("page12"))
            {
                return ReturnDic;
            }
            if(System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("Failed"))
            {
                return ReturnDic;
            }
            ReturnValue = System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer);
            Value= Regex.Split(ReturnValue, "\r\n");
            string Page12 = GetUID(Value[1]);
            ReturnDic.Add("Page12", Page12);
            if (!Client.writeCMD("11-22-13-AA-" + ByteList[4].ToString("X2") + "-" + ByteList[5].ToString("X2") + "-" + ByteList[6].ToString("X2") + "-" + Convert.ToInt32("0x" + BarCode.Substring(7, 2), 16).ToString("X2"), out ReceiveBuffer))
            {
                return ReturnDic;
            }
            if(!System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("page13"))
            {
                return ReturnDic;
            }
            if (System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("Failed"))
            {
                return ReturnDic;
            }
            string PageList=ByteList[10].ToString("X2");
            string byte5 = BarCode.Substring(9, 1) + PageList.Substring(0, 1);
            string byte6 = PageList.Substring(1, 1) + BarCode.Substring(11, 1);
            if (!Client.writeCMD("11-22-13-BB-"+byte5+"-"+byte6+"-"+BarCode.Substring(12,2)+"-"+BarCode.Substring(14,2),out ReceiveBuffer))
            {
                return ReturnDic;
            }
            if (!System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("page13"))
            {
                return ReturnDic;
            }
            if (System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("Failed"))
            {
                return ReturnDic;
            }
            ReturnValue = System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer);
            Value = Regex.Split(ReturnValue, "\r\n");
            string Page13 = GetUID(Value[1]);
            ReturnDic.Add("Page13", Page13);
            return ReturnDic;
        }


        public bool LifeSpanF(string cmd,out string LifeF)
        {
            ReceiveBuffer = new byte[1024];
            LifeF = "";
            if (!Client.writeCMD(cmd, out ReceiveBuffer))
            {
                return false;
            }
            if (System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("fffff"))
            {
                LifeF = System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer);
                return true;
            }
            return false;
        }

        public bool LifeSpanX(string cmd,out string LifeX)
        {
            ReceiveBuffer = new byte[1024];
            LifeX = "";
            if (!Client.writeCMD(cmd, out ReceiveBuffer))
            {
                return false;
            }
            if (System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Contains("186a0"))
            {
                LifeX= System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer);
                return true;
            }
            return false;
        }

        public bool ResetRelay()
        {
            for (int i = 0; i < 10; i++)
            {
                if (SendString("2A-11-22-21"))
                {
                    return true;
                }
                Thread.Sleep(10);
            }
            return false;
        }


        ushort[] wCRCTalbeAbs =
        {
            0x0000, 0xCC01, 0xD801, 0x1400, 0xF001, 0x3C00, 0x2800, 0xE401, 0xA001, 0x6C00, 0x7800, 0xB401, 0x5000, 0x9C01, 0x8801, 0x4400,
        };
        public ushort CRC16_2(byte[] a, int len)
        {
            ushort wCRC = 0xffff;
            byte chChar;

            for (int i = 0; i < len; i++)
            {
                chChar = a[i];
                ushort a1, b1;


                a1 = (ushort)((chChar ^ wCRC) & 15);
                b1 = (ushort)(wCRC >> 4);
                wCRC = (ushort)(wCRCTalbeAbs[a1] ^ b1);

                a1 = (ushort)(((chChar >> 4) ^ wCRC) & 15);
                b1 = (ushort)(wCRC >> 4);
                wCRC = (ushort)(wCRCTalbeAbs[a1] ^ b1);
            }

            return wCRC;
        }
        private string GetCheckSum(string Value)
        {
            int i = 0, j = 0;
            string mid_s = string.Empty;
            string Checksum = "";
            string cp_text = Value.Trim();
            cp_text = cp_text.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
            byte[] array = new byte[cp_text.Length];
            if (cp_text.Length % 2 == 0)
            {
                for (i = 0, j = 0; i < cp_text.Length;)
                {
                    mid_s = cp_text.Substring(i, 2);
                    i += 2;
                    try
                    {
                        array[j++] = Convert.ToByte(mid_s, 16);
                    }
                    catch (Exception)
                    {
                    }
                }

                ushort crc = CRC16_2(array, j);

                byte[] b = BitConverter.GetBytes(crc);

                // tBResult2.Text = string.Empty;
                //tBResult2.Text += Convert.ToString(b[1], 16).PadLeft(2, '0') + " " + Convert.ToString(b[0], 16).PadLeft(2, '0');
                Checksum = string.Empty;
                Checksum = Convert.ToString(b[0], 16).PadLeft(2, '0').ToUpper() + "-" + Convert.ToString(b[1], 16).PadLeft(2, '0').ToUpper();
            }

            return Checksum;
        }

        public bool SetCurrentMode(CurrentMode mode)
        {
            if (mode == CurrentMode.highAccuracy)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (SendString("01-06-10-00-00-00-8D-0A"))
                    {
                        return true;
                    }
                }
                return false;
            }
            else if (mode == CurrentMode.lowAccuracy)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (SendString("01-06-10-00-00-01-4C-CA"))
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
        public bool ClearCurrentRegister()
        {
            for (int i = 0; i < 10; i++)
            {
                if (SendString("01-06-10-1F-00-00-BC-CC"))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ClearVoltageRegister()
        {
            for (int i = 0; i < 10; i++)
            {
                if (SendString("01-06-10-1E-00-00-ED-0C"))
                {
                    return true;
                }

            }
            return false;
        }

        public bool ChargeRelayON()
        {
            for (int i = 0; i < 10; i++)
            {
                if (SendString("01-06-10-04-00-01-0D-0B"))//01 06 10 04 00 01 0D 0B
                {
                    return true;
                }
            }
            return false;
        }

        public bool DisChargeRelayON()
        {
            for (int i = 0; i < 10; i++)
            {
                if (SendString("01-06-10-03-00-01-BC-CA"))//01 06 10 03 00 01 BC CA
                {
                    return true;
                }
            }
            return false;
        }

        public bool ChargeRelayOFF()
        {
            for (int i = 0; i < 10; i++)
            {
                if (SendString("01-06-10-04-00-00-CC-CB"))//01 06 10 04 00 00 CC CB
                {
                    return true;
                }
            }
            return false;
        }
        public bool DisChargeRelayOFF()
        {
            for (int i = 0; i < 10; i++)
            {
                if (SendString("01-06-10-03-00-00-7D-0A"))//01 06 10 03 00 00 7D 0A
                {
                    return true;
                }
            }
            return false;
        }

        public bool SetCurrnet(CurrentMode mode, int Current)
        {
            SetCurrentMode(mode);
            if (CurrentMode.highAccuracy == mode)
            {
                string Cmd = "01-06-10-1F-";
                //int WriteValue = Math.Abs(Convert.ToInt32(2435 + (Current - 1000) * 2.33));
                int WriteValue = Math.Abs(Current);
                string CmdValue = Cmd + (WriteValue / 256).ToString("X2") + "-" + (WriteValue % 256).ToString("X2");
                CmdValue = CmdValue + "-" + GetCheckSum(CmdValue.Replace("-", ""));
                for (int i = 0; i < 20; i++)
                {
                    if (SendString(CmdValue))
                    {
                        return true;
                    }
                    Thread.Sleep(10);
                }
                return false;
            }
            else if (CurrentMode.lowAccuracy == mode)
            {
                string Cmd = "01-06-10-1F-";
                //int WriteValue = Math.Abs(Convert.ToInt32(2435 + (Current - 1000) * 2.33));
                int WriteValue = Math.Abs(Current);
                string CmdValue = Cmd + (WriteValue / 256).ToString("X2") + "-" + (WriteValue % 256).ToString("X2");
                CmdValue = CmdValue + "-" + GetCheckSum(CmdValue.Replace("-", ""));
                for (int i = 0; i < 20; i++)
                {
                    if (SendString(CmdValue))
                    {
                        return true;
                    }
                    Thread.Sleep(10);
                }
                return false;
            }
            return false;
        }
        public bool SetVoltage(Int32 SetVoltage)
        {
            string Cmd = "01-06-10-21-";
            //int WriteValue = Math.Abs(Convert.ToInt32(SetVoltage * 0.203));
            int WriteValue = Math.Abs(SetVoltage);
            string CmdValue = Cmd + (WriteValue / 256).ToString("X2") + "-" + (WriteValue % 256).ToString("X2");
            CmdValue = CmdValue + "-" + GetCheckSum(CmdValue.Replace("-", ""));
            for (int i = 0; i < 20; i++)
            {
                if (SendString(CmdValue))
                {
                    return true;
                }
                Thread.Sleep(20);
            }
            return false;
        }



        public double ReceiveDouble(string cmd)
        {
            ReceiveBuffer = new byte[1024];
            if (!Client.writeCMDACIR(cmd, out ReceiveBuffer))
            {
                return -1;
            }
            try
            {
                return Double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer));
            }
            catch
            {
                string Value = System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Replace("\0", "");
                return double.Parse(System.Text.ASCIIEncoding.ASCII.GetString(ReceiveBuffer).Substring(1, Value.Length));
            }
        }


        public List<string> Writestring(string Cmd, int DelayTime)
        {
            List<string> ReadList = new List<string>();
            try
            {
                Client.write(Cmd,DelayTime,out ReadList);
            }
            catch { }
            return ReadList;
        }


        #region
        Dictionary<string, string> dic;
        public Dictionary<string,string> GetVersion()
        {
            dic = new Dictionary<string, string>();
            try
            {
                cc = new List<byte>();
                string Value = "";
                string[] CNT_ID;
                string[] ID;
                string[] VNUM;
                string[] NVM;
                List<string> BackValue = Writestring("11-22-33-44-55-66-77-88", 5500);
                dic.Add("Page0", PageValue(BackValue[4]));
                dic.Add("Page1", PageValue(BackValue[5]));
                dic.Add("Page2", PageValue(BackValue[6]));
                dic.Add("Page3", PageValue(BackValue[7]));
                dic.Add("Page4", PageValue(BackValue[8]));
                dic.Add("Page5", PageValue(BackValue[9]));
                dic.Add("Page6", PageValue(BackValue[10]));
                dic.Add("Page7", PageValue(BackValue[11]));
                dic.Add("Page11", PageValue(BackValue[12]));
                dic.Add("Page12", PageValue(BackValue[13]));
                dic.Add("Page13", PageValue(BackValue[14]));
                for (int i = 0; i < BackValue.Count; i++)
                {
                    Value += BackValue[i].ToString() + "\r\n";
                }
                CNT_ID = Regex.Split(BackValue[1], "\r\n");
                ID = CNT_ID[1].Split(new char[] { ':' });
                string ValueUID=GetUID(ID[1]);
                GetValue(1, ID);
                GetValue(2, ID);
                GetValue(3, ID);
                GetValue(4, ID);
                string VenderID = cc[0].ToString("X2") + cc[1].ToString("X2");
                string ProductID = cc[2].ToString("X2") + cc[3].ToString("X2");
                dic.Add("VenderID","0x"+ VenderID);
                dic.Add("ProductID","0x"+ ProductID);
                dic.Add("UID", ValueUID);
                if (BackValue[2].Contains("Passed"))
                {
                    dic.Add("ECCE", "PASS");
                }
                else
                { dic.Add("ECCE", "NG"); }
                VNUM = Regex.Split(BackValue[3], "\r\n", RegexOptions.IgnoreCase)[1].Split(new char[] { '=' });
                NVM = Regex.Split(BackValue[3], "\r\n", RegexOptions.IgnoreCase)[2].Split(new char[] { '=' });
                string SWI = VNUM[1].Remove(VNUM[1].Length - 1, 1);
                string NVM1 = NVM[1].Remove(NVM[1].Length - 1, 1);
                dic.Add("SWI", SWI);
                dic.Add("NVM", NVM1);
                return dic;
            }
            catch (Exception ex)
            {
                return dic;
            }
        }

        private string GetUID(string Value)
        {
            string[] SplitValue = Regex.Split(Value," ");
            string BackValue = "";
            for (int i = 0; i < SplitValue.Count(); i++)
            {
                if (SplitValue[i].Count() == 1)
                {
                    SplitValue[i] = "0" + SplitValue[i];
                }
                BackValue += SplitValue[i].ToString();
            }
            return "0x" + BackValue;
        }

        private string PageValue(string Value)
        {
            string[] SplitValue = Regex.Split(Value,"\r\n");
            string[] SpValue = Regex.Split(SplitValue[1].Trim(), " ");
            string BackValue = "";
            for (int i = 0; i < SpValue.Count(); i++)
            {
                if (SpValue[i].Count() == 1)
                {
                    SpValue[i] = "0" + SpValue[i];
                }
                BackValue += SpValue[i].ToString();
            }
            return "0x" + BackValue;
        }

        public static byte[] ConvertHexStringToBytes(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("参数长度不正确");
            }

            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }
        List<Byte> cc = new List<byte>();
        public void GetValue(int n, string[] bbb)
        {
            if (bbb[1].Split(new char[] { ' ' })[n].Length < 2)
            {
                cc.Add(Convert.ToByte(bbb[1].Split(new char[] { ' ' })[n]));
            }
            else
            {
                cc.AddRange(ConvertHexStringToBytes(bbb[1].Split(new char[] { ' ' })[n]));
            }
        }
        #endregion



        public double ACIR()
        {
            double ACIR;
            for (int i = 0; i < 4; i++)
            {
                ACIR = ReceiveDouble("D4-BB-CC");
                if (ACIR != -1)
                {
                    return ACIR;
                }
                Thread.Sleep(20);
            }
            return -1;
        }

        public double ReadResistance()
        {
            double Resistacne;
            for (int i = 0; i < 10; i++)
            {
                if (SendString("EC-BB-DD"))
                {
                    break;
                }
                Thread.Sleep(20);
            }
            Thread.Sleep(100);
            for (int i = 0; i < 3; i++)
            {
                Resistacne = ReceiveDouble("D5-BB-CC");
                if (Resistacne!=-1)
                {
                    return Resistacne;
                }
            }
            return -1;
        }

        public double ReadResistance10k()
        {
            double Resistacne;
            for (int i = 0; i < 10; i++)
            {
                if (SendString("EC-BB-DD"))
                {
                    break;
                }
                Thread.Sleep(20);
            }
            Thread.Sleep(100);
            for (int i = 0; i < 3; i++)
            {
                Resistacne = ReceiveDouble("DE-BB-CC");
                if (Resistacne != -1)
                {
                    return Resistacne;
                }
            }
            return -1;
        }

        public double MeasureVoltageInside()
        {
            for (int i = 0; i < 10; i++)
            {
                double ReturnValue = ReceiveDouble("D3-BB-CC");
                if (ReturnValue != -1)
                {
                    return ReturnValue;
                }
            }
            return -1;
            //return ReceiveDouble("D3-BB-CC");
        }

        public bool QueryProtection()
        {
            byte[] Readbuffer= { };
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    Client.writeCMD("01-03-10-0A-00-01-A0-C8", out Readbuffer);
                    if (Readbuffer.Length != 0)
                    {
                        break;
                    }
                }
                if (Readbuffer[4].ToString("X2") == "01")
                {
                    return true;
                }
            }
            return false;
        }

        public double protectionTime()
        {
            ReceiveBuffer = new byte[1024];
            if (!Client.writeCMD("01-03-10-28-00-01-00-C2", out ReceiveBuffer))
            {
                return -1;
            }
            string Value="0x"+ReceiveBuffer[3].ToString("X2") + ReceiveBuffer[4].ToString("X2");
            return Convert.ToInt32(Value,16);
        }

        public double MeasureCurrentInside()
        {
            for (int i = 0; i < 10; i++)
            {
                double ReturnValue = ReceiveDouble("D9-BB-CC");
                if (ReturnValue != -1)
                {
                    return ReturnValue;
                }
            }
            return -1;
        }

        public double MeasureCurrentInside500uA()
        {
            for (int i = 0; i < 10; i++)
            {
                double ReturnValue = ReceiveDouble("Db-BB-CC");
                if (ReturnValue != -1)
                {
                    return ReturnValue;
                }
            }
            return -1;
        }

        public double MeasureCurrentInside10uA()
        {
            for (int i = 0; i < 10; i++)
            {
                double ReturnValue = ReceiveDouble("DC-BB-CC");
                if (ReturnValue != -1)
                {
                    return ReturnValue;
                }
                Thread.Sleep(30);
            }
            return -1;
        }


        /// <summary>
        /// 去除byte[]数组缓冲区内的尾部空白区;从末尾向前判断;
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] bytesTrimEnd(byte[] bytes)
        {
            List<byte> list = bytes.ToList();
            for (int i = bytes.Length - 1; i >= 0; i--)
            {
                if (bytes[i] == 0x00)
                {
                    list.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
            return list.ToArray();
        }
        //比较两个数组内容
        private bool PasswordEquals(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
                if (b1[i] != b2[i])
                    return false;
            return true;
        }
        public bool ResetEquipment()
        {
            ReceiveBuffer = new byte[3];
            if (!Client.writeCMD("01-06-10-08-00-01-CD-08", out ReceiveBuffer))
            {
                return false;
            }
            if (!Client.writeCMD("E8-BB-DD", out ReceiveBuffer))
            {
                return false;
            }
            if (!PasswordEquals(SplitHexString("E8-BB-DD", '-'), bytesTrimEnd(ReceiveBuffer)))
            {
                return false;
            }
            return true;
        }
        public bool ResetEQ()
        {
            for (int i = 0; i < 10; i++)
            {
                if (ResetEquipment())
                {
                    return true;
                }
            }
            return false;

        }
    }
}
