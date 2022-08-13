using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IC.NOGagueIC.SW;
using System.Xml;
using System.Xml.Linq;
using DataDisplay;
using CCWin;
using System.IO;

namespace IC.NOGagueIC.SW
{
    public partial class frmSttingParamter : Form
    {
        public frmSttingParamter()
        {
            InitializeComponent();
        }
        private static frmSttingParamter frmchild = null;
        public static int RowInbdex;
        public static Form MyParaent = null;//= new DataGridView();
        //public static DataGridView MyParaent = new DataGridView();
        //public 
        public static frmSttingParamter CreateInstance(int index, Form muForm)
        {
            //if (frmchild == null || frmchild.IsDisposed)
            //{
            frmchild = new frmSttingParamter();
            //}
            muForm = MyParaent;
            RowInbdex = index;
            frmchild.BringToFront();
            return frmchild;
        }
        SettingTestParameter SettingTestParameter = new SettingTestParameter();
        private void frmSttingParamter_Load(object sender, EventArgs e)
        {
            try
            {
                //if (UserMsg.Dep.Contains("技术") || UserMsg.Admin == true)
                //{

                foreach (string Value in SettingTestParameter.TestStepDictionary.Keys)
                {
                    string Kes = SettingTestParameter.TestStepDictionary[Value].Trim();
                    //string[] ValueList = SettingTestParameter.ItermclassifyStepDictionary[Value].ToString().Split(new char[] { ',' });
                    TreeNode tn = new TreeNode();
                    tn.Text = Kes;
                    ////tn.ImageIndex = 1;
                    //for (int i = 0; i < ValueList.Length; i++)// sKeys in ValueList )
                    //{
                    //TreeNode Chid = new TreeNode();
                    //Chid.Text = SettingTestParameter.ItermclassifyStepDictionary[Value].ToString().Trim ();
                    //Chid.ImageIndex = 2;

                    //}
                    treeView1.Nodes.Add(tn);



                }

                //}
                //if (UserMsg.Dep.Contains("品质"))
                //{
                //    foreach (string Value in SettingTestParameter.OQCStepDictionary.Keys)
                //    {
                //        treeListViewTestStep.Nodes.Add(SettingTestParameter.OQCStepDictionary[Value].ToString());
                //    }
                //}

                //skinDataGridView1.Rows.Clear();
                ShowFatherData();
                //cboStation.Text = "OQC";
                //rdoSacanBarcode.Visible  ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.Close();
            }
        }


        private void ShowDataGridViewCol(string item)
        {
            frmParaMter ower = (frmParaMter)this.Owner;
            if (ower.dgvTestStep.RowCount == 0)
            {
                ower.dgvTestStep.RowCount = 1;
            }
            tabControlParaSetting.TabPages.Clear();

            if (item != "46")
            {
                if (dgvTestStep.ColumnCount > 1)
                {
                    dgvTestStep.Columns[dgvTestStep.ColumnCount - 1].ReadOnly = true;
                }
            }
            //if (dgvTestStep.Columns.Count > 2)
            //{
            dgvTestStep.DataSource = null;
            dgvTestStep.Columns.Clear();
            dgvTestStep.Rows.Clear();
            dgvTestStep.RowCount = 1;
            //}
            //txtMsg.Text = " 电流单位:mA\r\n 电压单位mV\r\n 时间单位:ms ";
            Dictionary<string, string> HeaderList = new Dictionary<string, string>();
            HeaderList.Add("46", "");
            HeaderList.Add("1", "Voltage_Min(mV),Voltage_Max(mV),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("2", "Resistance_Min(omh),Resistance_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("3", "Resistance_Min(omh),Resistance_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("4", "Resistance_Min(omh),Reistacne_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("5", "Resistance_Min(omh),Reistacne_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("6", "ChargeCurrent(mA),Current_Min(mA),Current_Max(mA),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("7", "Voltage_Min(mV),Voltage_Max(mV),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("8", "Current_Min(mA),Current_Max(mA),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("9", "Delay_Min(ms),Delay_Max(ms),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("10", "Voltage_Min(mV),Voltage_Max(mV),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("11", "Voltage_Min(mV),Voltage_Max(mV),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("12", "Resistance_Min(omh),Resistacne_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("13", "Resistance_Min(omh),Resistance_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("14", "Resistance_Min(omh),Resistance_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("15", "Resistance_Min(omh),Resistance_Max(omh),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("16", "DisChargeCurrent(mA),Current_Min(mA),Current_Max(mA),Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("17", "DeltaVoltage_Min(mV),DeltaVoltage_Max(mV),Retry,Delay,Errorcode,LogName");
            HeaderList.Add("31", "Vender_ID,Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("32", "Product_ID,Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("33", "ECC_Verify,Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("34", "LifeSpanCounter,Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("35", "NVM_Number,Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("36", "IC_Version,Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("37", "Lock_Status,Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("38", "Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("18", "Current(mA),Current_min(uA),Current_max(uA),Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("19", "Current(mA),Current_min(uA),Current_max(uA),Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("20", "Current(mA),Current_min(uA),Current_max(uA),Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("21", "Temperature_Min(℃),Tempature_Max(℃),Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("22", "DeltaTemperature_Min(℃),DeltaTemperature(℃),Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("23", "DeltaTemperature_Min(℃),DeltaTemperature(℃),Retry,Delay,ErrorCode,LogName");
            HeaderList.Add("40", "Retry,Delay,ErrorCode,LogName");
            foreach (string keys in HeaderList.Keys)
            {
                if (keys != item)
                {
                    continue;
                }
                string[] Header = HeaderList[keys].Split(new char[] { ',' });

                dgvTestStep.ColumnCount = Header.Length;
                for (int i = 0; i < Header.Length; i++)
                {
                    dgvTestStep.Columns[i].HeaderText = Header[i];
                }
                if (item == "46")
                {
                    tabPageParaSettingConnect.Parent = tabControlParaSetting;
                    tabControlParaSetting.SelectedTab = tabPageParaSettingConnect;
                    dgvTestStep.ColumnCount = 3;
                    dgvTestStep.Columns[0].HeaderText = "产品型号";
                    dgvTestStep.Columns[1].HeaderText = "条码长度";
                    dgvTestStep.Columns[2].HeaderText = "条码规则";

                    txtExplain.Text = "如:*A*\r\n *:表示任意字符\r\n A:表示固定码\r\n 规格长度与Pack长度一致";
                    if (Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[0].Value).Trim() == txtBoxSetpIndex.Text.Trim())
                    {
                        dgvTestStep.Rows[0].Cells[0].Value = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[7].Value);
                        dgvTestStep.Rows[0].Cells[1].Value = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[8].Value);
                        dgvTestStep.Rows[0].Cells[2].Value = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[9].Value);

                        if (Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[13].Value).Trim().Length > 0)
                        {
                            chkWakeUPR.Checked = Convert.ToBoolean(ower.dgvTestStep.Rows[RowInbdex].Cells[13].Value);
                        }
                        if (Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[14].Value).Trim().Length > 0)
                        {
                            chkWakeUPN.Checked = Convert.ToBoolean(ower.dgvTestStep.Rows[RowInbdex].Cells[14].Value);
                        }
                        txtCellVoltage.Text = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[15].Value);
                        if (Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[21].Value).Trim().Length > 0)
                        {
                            chkClearCellVoltage.Checked = Convert.ToBoolean(ower.dgvTestStep.Rows[RowInbdex].Cells[21].Value);
                        }
                        if (Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[23].Value).Trim().Length > 0)
                        {
                            chkStartSCanBarcode.Checked = Convert.ToBoolean(ower.dgvTestStep.Rows[RowInbdex].Cells[22].Value);
                        } if (Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[23].Value).Trim().Length > 0)
                        {
                            chkSManualstart.Checked = Convert.ToBoolean(ower.dgvTestStep.Rows[RowInbdex].Cells[23].Value); ;
                        }
                    }
                }
                else if (item == "22" || item == "23" || item == "24")
                {
                    dgvTestStep.Rows[0].Cells[0].Value = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[7].Value);
                }
                else
                {
                    int ColLen = dgvTestStep.ColumnCount - 4;
                    for (int i = 0; i < ColLen; i++)
                    {
                        dgvTestStep.Rows[0].Cells[i].Value = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[7 + i].Value);
                    }
                    int Col =4;
                    for (int i = 0; i < 4; i++)
                    {
                        dgvTestStep.Rows[0].Cells[i + ColLen].Value = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[i + 2].Value);
                        Col--;
                    }
                }
            }

            for (int i = 0; i < dgvTestStep.Rows.Count; i++)
            {
                for (int Col = 0; Col < dgvTestStep.ColumnCount; Col++)
                {
                    if (dgvTestStep.Columns[Col].HeaderText == "Retry")
                    {
                        if (Convert.ToString(dgvTestStep.Rows[i].Cells[Col].Value).Trim().Length == 0)
                        {
                            dgvTestStep.Rows[i].Cells[Col].Value = "3";
                        }
                    }
                    if (dgvTestStep.Columns[Col].HeaderText == "Delay")
                    {
                        if (Convert.ToString(dgvTestStep.Rows[i].Cells[Col].Value).Trim().Length == 0)
                        {
                            dgvTestStep.Rows[i].Cells[Col].Value = "10";
                        }
                    }
                }
            }
        }

        public void CreatXmlTree(string xmlPath)
        {
            //string sTestStep = "";

            //需要指定编码格式，否则在读取时会抛：根级别上的数据无效。 第 1 行 位置 1异常
            XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            XmlWriter xw = System.Xml.XmlWriter.Create(xmlPath, settings);
            //xElement.Save(xw);
            //写入文件
            xw.Flush();
            xw.Close();
        }



        private void btnEdittheLatStep_Click(object sender, EventArgs e)
        {
            frmParaMter ower = (frmParaMter)this.Owner;
            if (RowInbdex <= 0)
            {
                proEvent(false, 0, 1, "");
                ShowFatherData();
                return;
            }
            else
            {
                RowInbdex--;
                ShowFatherData();
                proEvent(false, RowInbdex, 1, "");
            }

        }
        private void ShowFatherData()
        {
            frmParaMter ower = (frmParaMter)this.Owner;
            tabControlParaSetting.TabPages.Clear();
            //if (Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[0].Value).Trim() == txtBoxSetpIndex.Text.Trim())
            //{
            txtBoxSetpIndex.Text = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[0].Value).Trim();
            txtStepName.Text = Convert.ToString(ower.dgvTestStep.Rows[RowInbdex].Cells[1].Value).Trim();
            //proEvent(true, RowInbdex, 16, cboStation.Text);
            //proEvent(true, RowInbdex, 17, cboBQType.Text);
            //proEvent(true, RowInbdex, 18, cbo运输方式.Text);
            //proEvent(true, RowInbdex, 19, cboTestDevice.Text);
            cboStation.Text = Convert.ToString(ower.dgvTestStep.Rows[0].Cells[16].Value).Trim();
            cboBQType.Text = Convert.ToString(ower.dgvTestStep.Rows[0].Cells[17].Value).Trim();
            cbo运输方式.Text = Convert.ToString(ower.dgvTestStep.Rows[0].Cells[18].Value).Trim();
            cboTestDevice.Text = Convert.ToString(ower.dgvTestStep.Rows[0].Cells[19].Value).Trim();
            ShowDataGridViewCol(txtBoxSetpIndex.Text);
        }
        private void btnEditTheNexStep_Click(object sender, EventArgs e)
        {
            frmParaMter ower = (frmParaMter)this.Owner;
            if (Convert.ToString(ower.dgvTestStep.Rows[0].Cells[0].Value).Trim() != "46" && txtBoxSetpIndex.Text.Trim() != "46")
            {
                MessageBox.Show("Please set the basic parameters first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvTestStep.ColumnCount < 1)
            {
                MessageBox.Show("Please choose the test steps.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //if (txtBoxSetpIndex.Text.Trim() != "46" && txtBoxSetpIndex.Text.Trim() != "12")
            //{
            //    for (int i = 2; i < ower.dgvTestStep.RowCount; i++)
            //    {
            //        if (RowInbdex == i || Convert.ToString(ower.dgvTestStep.Rows[i].Cells[0].Value).Trim().Length == 0)
            //        {
            //            continue;
            //        }
            //        if (Convert.ToString(ower.dgvTestStep.Rows[i].Cells[0].Value).Trim() == txtBoxSetpIndex.Text.Trim())
            //        {
            //            MessageBox.Show("This Test Step Already Exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }
            //    }
            //}

            ShowDataGridViewRowValue(txtBoxSetpIndex.Text.Trim());

            RowInbdex++;

            ShowFatherData();

            txtStepName.Text = "";
            txtBoxSetpIndex.Text = "";
            dgvTestStep.Rows.Clear();
            dgvTestStep.RowCount = 1;

            ShowFatherData();

        }

        private void ShowDataGridViewRowValue(string item)
        {
            if (item.Trim().Length == 0)
            {
                return;
            }
            proEvent(true, RowInbdex, 0, txtBoxSetpIndex.Text.Trim());
            proEvent(true, RowInbdex, 1, txtStepName.Text.Trim());

            if (item == "46")
            {
                if (Convert.ToString(dgvTestStep.Rows[0].Cells[1].EditedFormattedValue.ToString()).Trim().Length > 0)
                {
                    if (Convert.ToInt16(dgvTestStep.Rows[0].Cells[1].EditedFormattedValue) != Convert.ToString(dgvTestStep.Rows[0].Cells[2].EditedFormattedValue.ToString()).Length)
                    {
                        MessageBox.Show("条码长度和规格设置长度不一致!", "错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!Convert.ToString(dgvTestStep.Rows[0].Cells[2].EditedFormattedValue).Contains("*"))
                    {
                        MessageBox.Show("条码规格设置请用 * 代替任意字符!", "错误提示:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                proEvent(true, RowInbdex, 7, Convert.ToString(dgvTestStep.Rows[0].Cells[0].EditedFormattedValue.ToString()));
                proEvent(true, RowInbdex, 8, Convert.ToString(dgvTestStep.Rows[0].Cells[1].EditedFormattedValue.ToString()));
                proEvent(true, RowInbdex, 9, Convert.ToString(dgvTestStep.Rows[0].Cells[2].EditedFormattedValue.ToString()));
                proEvent(true, RowInbdex, 13, chkWakeUPR.Checked.ToString());
                proEvent(true, RowInbdex, 14, chkWakeUPN.Checked.ToString());
                proEvent(true, RowInbdex, 15, txtCellVoltage.Text.Trim().ToString());
                proEvent(true, RowInbdex, 16, cboStation.Text);
                proEvent(true, RowInbdex, 17, cboBQType.Text);
                proEvent(true, RowInbdex, 18, cbo运输方式.Text);
                proEvent(true, RowInbdex, 19, cboTestDevice.Text);
                //proEvent(true, RowInbdex, 20, Convert.ToString(dgvTestStep.Rows[0].Cells[1].EditedFormattedValue.ToString()) + "-" + Convert.ToString(dgvTestStep.Rows[0].Cells[0].EditedFormattedValue.ToString()) +
                //    "-" + cboBQType.Text.Trim() + "-" + cbo运输方式.Text.Trim() + "-" + cboTestDevice.Text + "-" + cboBQType.Text.Trim());
                proEvent(true, RowInbdex, 21, chkClearCellVoltage.Checked.ToString());
                proEvent(true, RowInbdex, 22, chkStartSCanBarcode.Checked.ToString());
                proEvent(true, RowInbdex, 23, chkSManualstart.Checked.ToString());

            }
            //else if (item == "22" || item == "23" || item == "24")
            //{
            //    proEvent(true, RowInbdex, 7, Convert.ToString(dgvTestStep.Rows[0].Cells[0].EditedFormattedValue.ToString()));
            //}
            else if (item == "83" || item == "84")
            {
                int Col = 5;
                for (int i = dgvTestStep.ColumnCount - 1; i >= dgvTestStep.ColumnCount - 4; i--)
                {
                    proEvent(true, RowInbdex, Col, Convert.ToString(dgvTestStep.Rows[0].Cells[i].EditedFormattedValue.ToString()));
                    Col--;
                }

                for (int i = 0; i < dgvTestStep.ColumnCount - 4; i++)
                {
                    proEvent(true, RowInbdex, 7 + i, Convert.ToString(dgvTestStep.Rows[0].Cells[i].EditedFormattedValue.ToString()));
                }
            }
            else
            {
                int Col = 5;
                for (int i = dgvTestStep.ColumnCount - 1; i >= dgvTestStep.ColumnCount - 4; i--)
                {
                    proEvent(true, RowInbdex, Col, Convert.ToString(dgvTestStep.Rows[0].Cells[i].EditedFormattedValue.ToString()));
                    Col--;
                }

                for (int i = 0; i < dgvTestStep.ColumnCount - 4; i++)
                {
                    proEvent(true, RowInbdex, 7 + i, Convert.ToString(dgvTestStep.Rows[0].Cells[i].EditedFormattedValue.ToString()));
                }
            }
        }


        public delegate void EventDelegate(bool IsNext, int Row, int Col, string Message);
        public EventDelegate proEvent;

        private void chkWakeUp_CheckedChanged(object sender, EventArgs e)
        {
            frmParaMter ower = (frmParaMter)this.Owner;

        }
        private void treeListViewTestStep_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            txtStepName.Text = e.Node.Text.Trim();

            foreach (string key in SettingTestParameter.TestStepDictionary.Keys)
            {
                if (SettingTestParameter.TestStepDictionary[key].Trim() == e.Node.Text.Trim())
                {
                    txtBoxSetpIndex.Text = key.ToString();
                    ShowDataGridViewCol(key.ToString());
                }
            }
            for (int i = 0; i < dgvTestStep.Rows.Count; i++)
            {
                for (int Col = 0; Col < dgvTestStep.ColumnCount; Col++)
                {
                    if (dgvTestStep.Columns[Col].HeaderText == "Retry")
                    {
                        if (Convert.ToString(dgvTestStep.Rows[i].Cells[Col].Value).Trim().Length == 0)
                        {
                            dgvTestStep.Rows[i].Cells[Col].Value = "3";
                        }
                    }
                    if (dgvTestStep.Columns[Col].HeaderText == "Delay")
                    {
                        if (Convert.ToString(dgvTestStep.Rows[i].Cells[Col].Value).Trim().Length == 0)
                        {
                            dgvTestStep.Rows[i].Cells[Col].Value = "10";
                        }
                    }
                    else
                    {
                        dgvTestStep.Rows[i].Cells[Col].Value = " ";
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            frmParaMter ower = (frmParaMter)this.Owner;
            if (Convert.ToString(ower.dgvTestStep.Rows[0].Cells[0].Value).Trim() != "46" && txtBoxSetpIndex.Text.Trim() != "46")
            {
                MessageBox.Show("Please set the basic parameters first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvTestStep.ColumnCount < 1 && txtBoxSetpIndex.Text.Trim() != "12")
            {
                MessageBox.Show("Please choose the test steps.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //if (txtBoxSetpIndex.Text.Trim() != "46" && txtBoxSetpIndex.Text.Trim() != "12")
            //{
            //    for (int i = 2; i < ower.dgvTestStep.RowCount; i++)
            //    {
            //        if (RowInbdex == i || Convert.ToString(ower.dgvTestStep.Rows[i].Cells[0].Value).Trim().Length == 0)
            //        {
            //            continue;
            //        }
            //        if (Convert.ToString(ower.dgvTestStep.Rows[i].Cells[5].Value).Trim() == Convert.ToString(dgvTestStep.Rows[0].Cells[dgvTestStep.Columns.Count - 1].EditedFormattedValue.ToString()).Trim())
            //        {
            //            MessageBox.Show("This Log name already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            return;
            //        }

            //    }
            //}
            //for (int i = 2; i < ower.dgvTestStep.RowCount; i++)
            //{
            //    if (RowInbdex == i || Convert.ToString(ower.dgvTestStep.Rows[i].Cells[0].Value).Trim().Length == 0)
            //    {
            //        continue;
            //    }
            //    if (Convert.ToString(ower.dgvTestStep.Rows[i].Cells[0].Value).Trim() == txtBoxSetpIndex.Text.Trim())
            //    {
            //        MessageBox.Show("This Test Step Already Exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //}
            ShowDataGridViewRowValue(txtBoxSetpIndex.Text.Trim());
            RowInbdex++;

            ShowFatherData();

            txtStepName.Text = "";
            txtBoxSetpIndex.Text = "";
            dgvTestStep.Rows.Clear();
            dgvTestStep.RowCount = 1;

            ShowFatherData();
        }

        private void cboOCCPProtectMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            skinLabel5.Visible = false;
            txtOCCPProtectMode.Enabled = false;
            if (cboOCCPProtectMode.Text.Trim() == "Check Voltage")
            {
                skinLabel5.Visible = true;
                txtOCCPProtectMode.Enabled = true;
                skinLabel5.Text = "Voltage Min(Unit:mV):";
            }

            if (cboOCCPProtectMode.Text.Trim() == "Check Current")
            {
                skinLabel5.Text = "Current Min(Unit:mA):";
                skinLabel5.Visible = true;
                txtOCCPProtectMode.Enabled = true;
            }
        }

        private void cboOCCPRecoverMode_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboOCCPRecoverMode.Text.Trim() == "DisCharge Recovery")
            {
                skinLabel7.Visible = true;
                txtOCCPRecoverDischargeCurrent.Enabled = true;
                skinLabel7.Text = "Current(Unit:mA)";
            }
            else
            {
                skinLabel7.Visible = false;
                txtOCCPRecoverDischargeCurrent.Enabled = false;
            }
        }

        private void cboODCPProtectMode_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            skinLabel10.Visible = false;
            txtODCPProtectCondition.Enabled = false;
            if (cboODCPProtectMode.Text.Trim() == "Check Voltage")
            {
                skinLabel10.Visible = true;
                txtODCPProtectCondition.Enabled = true;
                skinLabel10.Text = "Voltage Min(Unit:mV):";
            }

            if (cboODCPProtectMode.Text.Trim() == "Check Current")
            {
                skinLabel10.Text = "Voltage Min(Unit:mA):";
                skinLabel10.Visible = true;
                txtODCPProtectCondition.Enabled = true;
            }
        }

        private void cboODCPRecoverMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboODCPRecoverMode.Text.Trim() == "Charge Recovery")
            //{
            //    skinLabel8.Visible = true;
            //    skinLabel13.Visible = true;
            //    txtODCPRecoverChargeVoltage.Visible = true;
            //    txtODCPRecoverChargeCurrent.Visible = true;
            //    skinLabel8.Text = "ChargeVoltage(Unit:mV):";
            //    txtODCPRecoverChargeVoltage.Enabled = true;
            //    skinLabel13.Text = "ChargeCurrent(Unit:mA):";
            //    txtODCPRecoverChargeCurrent.Enabled = true;
            //}
            //else
            //{
            //    skinLabel8.Visible = false;
            //    skinLabel13.Visible = false;
            //    txtODCPRecoverChargeVoltage.Visible = false;
            //    txtODCPRecoverChargeCurrent.Visible = false;
            //}
        }

        private void rdoreadBarcode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoSacanBarcode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnLoadEEParamter_Click(object sender, EventArgs e)
        {

            OpenFileDialog saveFileDialog1 = new System.Windows.Forms.OpenFileDialog();//打开一个保存对话框
            saveFileDialog1.Filter = "log文件(*.log)|*.log";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)//判断是否选择了一个文件路径
            {
                string FileName = saveFileDialog1.FileNames[0];
                LoadFile(FileName);
            }
        }

        private void LoadFile(string FileName)
        {
            frmParaMter ower = (frmParaMter)this.Owner;
            string BQType = Convert.ToString(ower.dgvTestStep.Rows[0].Cells[12].Value);
            if (BQType.Trim().Length == 0)
            {
                return;
            }
            string Value = "";
            using (StreamReader sr = new StreamReader(FileName))
            {
                Value = sr.ReadToEnd();
                string[] File = Value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                int dgvCount = 0;
                for (int i = 0; i < File.Length; i++)
                {
                    if (File[i].Trim().Length > 0)
                    {
                        string[] ShowValue = File[i].Split(new char[] { ',' });
                        if (BQType.Trim() == "bq20z451" || BQType.Trim() == "bq275xx")
                        {
                            if (ShowValue[2].Trim().Length > 0)
                            {
                                dgvCount++;
                            }
                        }
                        else
                        {
                            if (ShowValue[1].Trim().Length > 0)
                            {
                                dgvCount++;
                            }
                        }
                    }
                }

                int RowCount = 0;
                for (int i = 0; i < File.Length; i++)
                {
                    if (File[i].Trim().Length == 0)
                    {
                        continue;
                    }
                    string[] ShowValue = File[i].Split(new char[] { ',' });
                    if (BQType.Trim() == "bq20z451" || BQType.Trim() == "bq275xx")
                    {
                        if (ShowValue[2].Trim().Length == 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (ShowValue[1].Trim().Length == 0)
                        {
                            continue;
                        }
                    }

                }
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkCellProtectRecoveryWUR.Enabled = false;
            chkCellProtectRecoveryWUN.Enabled = false;
            txtCellProtectRecoveryTime.Enabled = false;
            if (comboBox1.Text.Trim() == "WakeUP Recovery")
            {
                chkCellProtectRecoveryWUR.Enabled = true;
                chkCellProtectRecoveryWUN.Enabled = true;
            }
            else
            {
                chkCellProtectRecoveryWUR.Enabled = false;
                chkCellProtectRecoveryWUN.Enabled = false;
                chkCellProtectRecoveryWUR.Checked = false;
                chkCellProtectRecoveryWUN.Checked = false;
            }
            if (comboBox1.Text.Trim() == "Auto Recovery")
            { txtCellProtectRecoveryTime.Enabled = true; }
            else
            {
                txtCellProtectRecoveryTime.Enabled = false;
                txtCellProtectRecoveryTime.Clear();
            }

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            txtStepName.Text = e.Node.Text.Trim();

            foreach (string key in SettingTestParameter.TestStepDictionary.Keys)
            {
                if (SettingTestParameter.TestStepDictionary[key].Trim() == e.Node.Text.Trim())
                {
                    txtBoxSetpIndex.Text = key.ToString();
                    ShowDataGridViewCol(key.ToString());
                }
            }
            for (int i = 0; i < dgvTestStep.Rows.Count; i++)
            {
                for (int Col = 0; Col < dgvTestStep.ColumnCount; Col++)
                {
                    if (dgvTestStep.Columns[Col].HeaderText == "Retry")
                    {
                        if (Convert.ToString(dgvTestStep.Rows[i].Cells[Col].Value).Trim().Length == 0)
                        {
                            dgvTestStep.Rows[i].Cells[Col].Value = "3";
                        }
                    }
                    if (dgvTestStep.Columns[Col].HeaderText == "Delay")
                    {
                        if (Convert.ToString(dgvTestStep.Rows[i].Cells[Col].Value).Trim().Length == 0)
                        {
                            dgvTestStep.Rows[i].Cells[Col].Value = "10";
                        }
                    }
                    else
                    {
                        dgvTestStep.Rows[i].Cells[Col].Value = " ";
                    }
                }
            }
        }



        //private void skinCheckBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //   
        //}
    }
}
