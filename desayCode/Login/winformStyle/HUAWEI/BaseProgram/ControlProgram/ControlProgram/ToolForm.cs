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

namespace ControlProgram
{
    public partial class ToolForm : Form
    {
        public ToolForm()
        {
            InitializeComponent();
            tabControl1.Enabled = false;
            groupBox_Meter.Enabled = false;
            button_discharge.BackColor = Color.Red;
            button_Charge.BackColor = Color.Red;
            this.txtSettingGrid.ScrollToCaret();
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
                catch
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
                TFunction.SetCurrnet(CurrMode_CheckBox.Checked ? TestFunction.CurrentMode.lowAccuracy : TestFunction.CurrentMode.highAccuracy, Convert.ToInt32(txtCurrent.Text.Trim()));
                TFunction.ChargeRelayON();
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
            TFunction.SendString("E7-BB-DD");
            float Current = 0;
            agilent.MeasureVoltage10V(out Current);
            txtMeterShow.Text += "Current:"+Current.ToString() + "\r\n";
            TFunction.ResetEQ();
            button_Charge.BackColor = Color.Red;
            button_discharge.BackColor = Color.Red;
        }

        private void ToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                agilent.Close();
                TFunction.Close();
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
    }
}
