using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ClassLibrary1;

namespace ControlProgram
{
    public partial class ToolForm : Form
    {
        public ToolForm()
        {
            InitializeComponent();
            System.Windows.Forms.Application.EnableVisualStyles();
            tabControl1.Enabled = false;
            groupBox_Meter.Enabled = false;
            button_discharge.BackColor = Color.Red;
            button_Charge.BackColor = Color.Red;
            this.txtSettingGrid.ScrollToCaret();
            txtIP.Text = "192.168.1.1";
        }
        TestFunction TFunction = new TestFunction();
        private CAgilent agilent;


        private  void button_Connect_Click(object sender, EventArgs e)
        {
            //await Task.Run(new Action(() =>
            //{
                if (txtIP.Text.Trim() != "")
                {
                    if (TFunction.OpenConnt(txtIP.Text.Trim(), 10008))
                    {
                        tabControl1.Enabled = true;
                        tableLayoutPanel2.Enabled = false;
                    }
                }
            if (txtMeter.Text.Trim() != "")
            {
                try
                {
                    if (txtMeter.Text.Trim().Contains("USB"))
                    {
                        agilent = new CAgilent(txtMeter.Text.Trim(), CAgilent.ControlMode.USBMode);
                    }
                    else
                    {
                        agilent = new CAgilent(txtMeter.Text.Trim(), CAgilent.ControlMode.TCPMode);
                    }
                    groupBox_Meter.Enabled = true;
                }
                catch(Exception ex)
                {
                }
            }
            //}));

        }

        private void button_Temperature_Click(object sender, EventArgs e)
        {
            txtSettingGrid.Text += "温度:" + TFunction.ReceiveDouble("F6-BB-DD") + "\r\n";
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            if (TFunction.ResetEQ())
            {
                ButtonReset();
                button_Charge.BackColor = Color.Red;
                button_discharge.BackColor = Color.Red;
            }
        }

        private async void ButtonReset()
        {
            await Task.Run(new Action(() =>
            {
                button_reset.BackColor = Color.Lime;
                Thread.Sleep(1000);
                button_reset.BackColor = Color.Transparent;
            }));
        }

        private void button_voltage_Click(object sender, EventArgs e)
        {
            txtSettingGrid.Text += "VoltageInside:" + TFunction.MeasureVoltageInside().ToString() + "\r\n";
        }

        private void button_ACIR_Click(object sender, EventArgs e)
        {
            txtSettingGrid.Text += "ACIR:" + TFunction.ReceiveDouble("D4-BB-CC").ToString() + "\r\n";
        }

        private void button_Charge_Click(object sender, EventArgs e)
        {
            if (button_Charge.BackColor == Color.Red)
            {
                button_Charge.BackColor = Color.Lime;
                TFunction.ClearCurrentRegister();
                TFunction.ClearVoltageRegister();
                TFunction.SendString("E1-BB-DD");//大电流继电器
                TFunction.ChargeRelayON();
                TFunction.SetCurrnet(CurrMode_CheckBox.Checked ? TestFunction.CurrentMode.lowAccuracy : TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(txtCurrent.Text.Trim()));
                TFunction.SetVoltage(Convert.ToInt32(txtVoltage.Text.Trim()));
            }
            else if (button_Charge.BackColor == Color.Lime)
            {
                TFunction.ChargeRelayOFF();
                button_Charge.BackColor = Color.Red;
            }
        }

        private void button_discharge_Click(object sender, EventArgs e)
        {
            if (button_discharge.BackColor == Color.Red)
            {
                button_discharge.BackColor = Color.Lime;
                TFunction.ClearCurrentRegister();
                TFunction.ClearVoltageRegister();
                TFunction.SetCurrnet(CurrMode_CheckBox.Checked ? TestFunction.CurrentMode.lowAccuracy : TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(txtCurrent.Text.Trim()));
                TFunction.DisChargeRelayON();
            }
            else if (button_discharge.BackColor == Color.Lime)
            {
                TFunction.DisChargeRelayOFF();
                button_discharge.BackColor = Color.Red;
            }
        }

        private void button_CurrentInside_Click(object sender, EventArgs e)
        {
            txtSettingGrid.Text += "CurrentInside:" + TFunction.MeasureCurrentInside().ToString() + "\r\n";
        }

        private void button_MeterVoltage_Click(object sender, EventArgs e)
        {
            TFunction.SendString("F0-BB-DD");
            float Voltage = 0;
            agilent.MeasureVoltage10V(out Voltage);
            txtMeterShow.Text +="Voltage:"+ Voltage.ToString() + "\r\n";
            TFunction.ResetEQ();
            button_discharge.BackColor = Color.Red;
            button_Charge.BackColor = Color.Red;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtSettingGrid.Clear();
            txtMeterShow.Clear();
        }

        private void button_CurrentMeter_Click(object sender, EventArgs e)
        {
            TFunction.SendString("E3-BB-DD");
            float Current = 0;
            agilent.MeasureCurrent(out Current);
            txtMeterShow.Text += "Current:"+Current.ToString() + "\r\n";
        }

        private void ToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().Close();
            }
            catch
            { }
        }
        private void button_version_Click(object sender, EventArgs e)
        {
            //List<string> BackValue=TFunction.Writestring("11-22-33-44-55-66-77-88",5500);
            TFunction.GetVersion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TFunction.SendString("2A-11-22-04");
            Thread.Sleep(150);
            txtSettingGrid.Text +=TFunction.ReadResistance().ToString()+"\r\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TFunction.ResetRelay();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TFunction.SendString("2A-11-22-07");
            Thread.Sleep(150);
            txtSettingGrid.Text += TFunction.ReadResistance().ToString()+"\r\n";
        }

        private void button_100mA_Click(object sender, EventArgs e)
        {
            //TFunction.SendString("01-06-10-0C-00-01-8C-C9");
            //TFunction.DisChargeRelayON();
            ////TFunction.SetVoltage(4500);
            TFunction.SendString("01-06-10-0B-00-01-3D-08");
            TFunction.ChargeRelayON();
            TFunction.SetVoltage(4500);
        }

        private void button_10mA_Click(object sender, EventArgs e)
        {
            TFunction.SendString("01-06-10-0B-00-01-3D-08");
            TFunction.DisChargeRelayON();
            //TFunction.SetVoltage(4500);
        }

        private void button_Read_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            byte[] Rec=TFunction.I2C_Read(0xaa,Convert.ToByte(txtStartRegister.Text,16), txtNumberRead.Text==""?4:Convert.ToInt32(txtNumberRead.Text));
            if(Rec==null)
            {
                return;
            }
            foreach (var re in Rec)
            {
                txtLog.Text += re.ToString("X2")+" ";
            }
        }
        byte[] ls = { 0x02, 0x00 };
        List<byte> vs1;
        private void button_Write_Click(object sender, EventArgs e)
        {
            string[] vs=txtBytetoWrite.Text.Split(' ');
            vs1 = new List<byte>();
            foreach (var vd in vs)
            {
                vs1.Add(Convert.ToByte(vd, 16));
            }
            byte[] lst=vs1.ToArray();
            TFunction.I2C_Write(0xaa, Convert.ToByte(txtStartRegister.Text, 16), lst);
        }
        byte[] ps = { 0x18, 0x35 };
        byte[] pss = { 0x01, 0x56 };
        byte[] full_unseal = { 0xFF, 0xFF };
        private void btnunseal_Click(object sender, EventArgs e)
        {
            TFunction.I2C_Write(0xaa, 0x00, pss);
            TFunction.I2C_Write(0xaa, 0x00, ps);
        }

        private void btn_fullunseal_Click(object sender, EventArgs e)
        {
            TFunction.I2C_Write(0xaa, 0x00, full_unseal);
            TFunction.I2C_Write(0xaa, 0x00, full_unseal);
        }

        private void btn_CurrentAglient_Click(object sender, EventArgs e)
        {
            TFunction.SendString("E3-BB-DD");
            //float Current=agilent.MeasureCurrent();
            //txtSettingGrid.Text += Current.ToString()+"\r\n";
        }

        private void btn_VoltageAglient_Click(object sender, EventArgs e)
        {
            TFunction.SendString("F0-BB-DD");
            //float Voltage = 0;
            //agilent.MeasureVoltage10V(out Voltage);
            //txtSettingGrid.Text += Voltage.ToString() + "\r\n";
        }

        private void btn_sda_Click(object sender, EventArgs e)
        {
            TFunction.SendString("ED-BB-DD");
            TFunction.SendString("EE-BB-DD");
            txtSettingGrid.Text+=TFunction.MeasureVoltageInside().ToString()+"\r\n";
            TFunction.ResetEQ();
        }

        private void btn_scl_Click(object sender, EventArgs e)
        {
            TFunction.SendString("ED-BB-DD");
            TFunction.SendString("EF-BB-DD");
            txtSettingGrid.Text += TFunction.MeasureVoltageInside().ToString() + "\r\n";
            TFunction.ResetEQ();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            TFunction.Cocp_setRange_Min(1000);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            TFunction.SetCurrnet(CurrMode_CheckBox.Checked ? TestFunction.CurrentMode.lowAccuracy : TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(txtCurrent.Text.Trim()));
        }

        private void btn_Convert_Click(object sender, EventArgs e)
        {
            ClassLibrary1.BitConverterEV2300 converterEV2300 = new BitConverterEV2300();
            string Value=txtConvert.Text.Trim();
            string[] Values=Value.Split(' ');
            if (Values.Count() > 1)
            {
                List<byte> ByteList = new List<byte>();
                foreach (var ls in Values)
                {
                    ByteList.Add(Convert.ToByte(ls,16));
                }
                byte[] arry=ByteList.ToArray();
                txtConvert.Text=(5595388/converterEV2300.ByteToDouble(arry[0],arry[1],arry[2],arry[3])).ToString();
            }
            else if (Values.Count() == 1)
            {
                byte[] arry=converterEV2300.DoubleToByteArray(5595388/Convert.ToDouble(txtConvert.Text));
                txtConvert.Text = "";
                foreach (var vs in arry)
                {
                    txtConvert.Text += vs.ToString("X2") + " ";
                }
            }
        }

        private void btnHex_Click(object sender, EventArgs e)
        {
            string hexValue=txtHex.Text.Trim();
            string val = "";
            foreach (var vs in hexValue)
            {
                val+=Convert.ToString(Convert.ToByte(vs.ToString(), 16), 2);
            }
            if (val.Substring(0, 1) == "1")
            {
                int Result = Convert.ToInt32(val.Substring(1, val.Length - 1), 2);
                Result = 128 - Result;
                Result = -Result;
                txtHex.Text = Result.ToString();
            }
            else if (val.Substring(0, 1) == "0")
            {
                int Result = Convert.ToInt32(val.Substring(1, val.Length - 1), 2);
                txtHex.Text = Result.ToString();
            }
        }

        private void btn_Inter_Click(object sender, EventArgs e)
        {
            string Result = txtHex.Text.ToString();
            int value=Convert.ToInt32(Result);
            int temp = 0;
            if (value < 0)
            {
                temp = 128 + value;
                string values=Convert.ToString(temp, 2);
                string finalValue="1"+values.PadLeft(7, '0');
                byte val=Convert.ToByte(finalValue,2);
                txtHex.Text = val.ToString("X2");
            }
            if (value >= 0 && value <= 127)
            {
                temp = value;
                string values = Convert.ToString(temp, 2);
                string finalValue = "0" + values.PadLeft(7, '0');
                byte val = Convert.ToByte(finalValue, 2);
                txtHex.Text = val.ToString("X2");
            }
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
        private void btnVolCal_Click(object sender, EventArgs e)
        {
            TFunction.SendString("E6-BB-DD");
            byte[] vol=TFunction.I2C_Read(0xaa, 0x08, 4);
            int VoltageIC=Convert.ToInt32(vol[2].ToString("X2") + vol[1].ToString("X2"), 16);
            TFunction.SendString("F0-BB-DD");
            float Voltage = 0;
            agilent.MeasureVoltage10V(out Voltage);
            TFunction.ResetEQ();
            int deltaVol=VoltageIC - Convert.ToInt32(Voltage);
            TFunction.I2C_Write(0xaa, 0x61, new byte[] { 0x00 });
            Thread.Sleep(500);
            TFunction.I2C_Write(0xaa, 0x3e, new byte[] { 0x68 });
            Thread.Sleep(500);
            TFunction.I2C_Write(0xaa, 0x3f, new byte[] { 0x00 });
            Thread.Sleep(500);
            byte[] Register=TFunction.I2C_Read(0xaa, 0x40, 32);
            byte[] ReadRegister=Register.Skip(1).ToArray();
            byte[] Voffset=ReadRegister.Skip(13).Take(1).ToArray();
            int byteVoltage = Voffset[0] - deltaVol;
            int value = byteVoltage;
            int temp = 0;
            byte val = 0;
            if (value < 0)
            {
                temp = 128 + value;
                string values = Convert.ToString(temp, 2);
                string finalValue = "1" + values.PadLeft(7, '0');
                val = Convert.ToByte(finalValue, 2);
            }
            if (value >= 0 && value <= 127)
            {
                temp = value;
                string values = Convert.ToString(temp, 2);
                string finalValue = "0" + values.PadLeft(7, '0');
                val = Convert.ToByte(finalValue, 2);
            }
            ReadRegister[13] = val;
            TFunction.I2C_Write(0xaa, 0x40, ReadRegister);
            int CheckSum = EECheckSUM(ReadRegister);
            Thread.Sleep(200);
            TFunction.I2C_Write(0xaa, 0x60, new byte[] { Convert.ToByte(CheckSum), 0x00 });
        }

        private void btnCurrentCal_Click(object sender, EventArgs e)
        {
            TFunction.SendString("E3-BB-DD");
            float Current = 0;
            int CurrentIC;
            agilent.MeasureCurrent(out Current);
            byte[] current=TFunction.I2C_Read(0xaa, 0x14, 4);
            if (current[2] == 0xff)
            {
                current[1] = Convert.ToByte(255 - Convert.ToInt32(current[1]));
                CurrentIC = -Convert.ToInt32(current[1]);
            }
            else
            {
                CurrentIC = Convert.ToInt32(current[1]);
            }
            TFunction.I2C_Write(0xaa, 0x61, new byte[] { 0x00 });
            Thread.Sleep(500);
            TFunction.I2C_Write(0xaa, 0x3e, new byte[] { 0x68 });
            Thread.Sleep(500);
            TFunction.I2C_Write(0xaa, 0x3f, new byte[] { 0x00 });
            Thread.Sleep(500);
            byte[] Register = TFunction.I2C_Read(0xaa, 0x40, 32);
            byte[] ReadRegister = Register.Skip(1).ToArray();
            byte[] CC_Gain= ReadRegister.Take(4).ToArray();
            byte[] CC_Delta = ReadRegister.Skip(4).Take(4).ToArray();
            ClassLibrary1.BitConverterEV2300 converterEV2300 = new BitConverterEV2300();
            double ICGain= 4.7095 / converterEV2300.ByteToDouble(CC_Gain[0], CC_Gain[1], CC_Gain[2], CC_Gain[3]);
            double ICDelta = 5595388 / converterEV2300.ByteToDouble(CC_Delta[0], CC_Delta[1], CC_Delta[2], CC_Delta[3]);
            double newGain = (CurrentIC * ICGain) / Current;
            double newDelta = (CurrentIC * ICDelta) / Current;
            byte[] arryGain = converterEV2300.DoubleToByteArray(4.7095 /newGain);
            byte[] arryDelta = converterEV2300.DoubleToByteArray(5595388 / newDelta);
            ReadRegister[0] = arryGain[0];
            ReadRegister[1] = arryGain[1];
            ReadRegister[2] = arryGain[2];
            ReadRegister[3] = arryGain[3];
            ReadRegister[4] = arryDelta[0];
            ReadRegister[5] = arryDelta[1];
            ReadRegister[6] = arryDelta[2];
            ReadRegister[7] = arryDelta[3];
            TFunction.I2C_Write(0xaa, 0x40, ReadRegister);
            int CheckSum = EECheckSUM(ReadRegister);
            Thread.Sleep(200);
            TFunction.I2C_Write(0xaa, 0x60, new byte[] { Convert.ToByte(CheckSum), 0x00 });
        }
    }
}
