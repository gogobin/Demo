namespace ControlProgram
{
    partial class ToolForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button_Connect = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtIP = new CCWin.SkinControl.SkinWaterTextBox();
            this.txtMeter = new CCWin.SkinControl.SkinWaterTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button_version = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CurrMode_CheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVoltage = new CCWin.SkinControl.SkinWaterTextBox();
            this.button_discharge = new System.Windows.Forms.Button();
            this.button_Charge = new System.Windows.Forms.Button();
            this.txtCurrent = new CCWin.SkinControl.SkinWaterTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_CurrentInside = new System.Windows.Forms.Button();
            this.button_Temperature = new System.Windows.Forms.Button();
            this.button_ACIR = new System.Windows.Forms.Button();
            this.txtSettingGrid = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_voltage = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.groupBox_Meter = new System.Windows.Forms.GroupBox();
            this.button_CurrentMeter = new System.Windows.Forms.Button();
            this.button_MeterVoltage = new System.Windows.Forms.Button();
            this.txtMeterShow = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox_Meter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.407407F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.59259F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(698, 490);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.50377F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.49623F));
            this.tableLayoutPanel2.Controls.Add(this.button_Connect, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(692, 30);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // button_Connect
            // 
            this.button_Connect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Connect.Font = new System.Drawing.Font("Rockwell", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Connect.Location = new System.Drawing.Point(573, 3);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(116, 24);
            this.button_Connect.TabIndex = 0;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtIP);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtMeter);
            this.splitContainer1.Size = new System.Drawing.Size(564, 24);
            this.splitContainer1.SplitterDistance = 298;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtIP
            // 
            this.txtIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIP.Font = new System.Drawing.Font("微软雅黑 Light", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP.Location = new System.Drawing.Point(0, 0);
            this.txtIP.Multiline = true;
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(298, 24);
            this.txtIP.TabIndex = 1;
            this.txtIP.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtIP.WaterText = "请输入ip地址";
            // 
            // txtMeter
            // 
            this.txtMeter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMeter.Font = new System.Drawing.Font("微软雅黑 Light", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMeter.Location = new System.Drawing.Point(0, 0);
            this.txtMeter.Multiline = true;
            this.txtMeter.Name = "txtMeter";
            this.txtMeter.Size = new System.Drawing.Size(262, 24);
            this.txtMeter.TabIndex = 2;
            this.txtMeter.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtMeter.WaterText = "请输入Meter地址";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Rockwell", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(692, 448);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel3);
            this.tabPage1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(684, 421);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setting";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox4, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox_Meter, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(678, 415);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.button_version);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(342, 210);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(333, 202);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Communication";
            // 
            // button_version
            // 
            this.button_version.Location = new System.Drawing.Point(6, 26);
            this.button_version.Name = "button_version";
            this.button_version.Size = new System.Drawing.Size(114, 23);
            this.button_version.TabIndex = 1;
            this.button_version.Text = "CNT";
            this.button_version.UseVisualStyleBackColor = true;
            this.button_version.Click += new System.EventHandler(this.button_version_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CurrMode_CheckBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtVoltage);
            this.groupBox1.Controls.Add(this.button_discharge);
            this.groupBox1.Controls.Add(this.button_Charge);
            this.groupBox1.Controls.Add(this.txtCurrent);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 201);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Charge&&Discharge";
            // 
            // CurrMode_CheckBox
            // 
            this.CurrMode_CheckBox.AutoSize = true;
            this.CurrMode_CheckBox.Location = new System.Drawing.Point(221, 91);
            this.CurrMode_CheckBox.Name = "CurrMode_CheckBox";
            this.CurrMode_CheckBox.Size = new System.Drawing.Size(63, 21);
            this.CurrMode_CheckBox.TabIndex = 12;
            this.CurrMode_CheckBox.Text = "低精度";
            this.CurrMode_CheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "电压设置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "电流设置";
            // 
            // txtVoltage
            // 
            this.txtVoltage.Location = new System.Drawing.Point(68, 62);
            this.txtVoltage.Name = "txtVoltage";
            this.txtVoltage.Size = new System.Drawing.Size(146, 23);
            this.txtVoltage.TabIndex = 9;
            this.txtVoltage.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtVoltage.WaterText = "电压设置";
            // 
            // button_discharge
            // 
            this.button_discharge.Location = new System.Drawing.Point(220, 61);
            this.button_discharge.Name = "button_discharge";
            this.button_discharge.Size = new System.Drawing.Size(114, 24);
            this.button_discharge.TabIndex = 8;
            this.button_discharge.Text = "放电";
            this.button_discharge.UseVisualStyleBackColor = true;
            this.button_discharge.Click += new System.EventHandler(this.button_discharge_Click);
            // 
            // button_Charge
            // 
            this.button_Charge.Location = new System.Drawing.Point(220, 31);
            this.button_Charge.Name = "button_Charge";
            this.button_Charge.Size = new System.Drawing.Size(114, 24);
            this.button_Charge.TabIndex = 7;
            this.button_Charge.Text = "充电";
            this.button_Charge.UseVisualStyleBackColor = true;
            this.button_Charge.Click += new System.EventHandler(this.button_Charge_Click);
            // 
            // txtCurrent
            // 
            this.txtCurrent.Location = new System.Drawing.Point(68, 31);
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.Size = new System.Drawing.Size(146, 23);
            this.txtCurrent.TabIndex = 6;
            this.txtCurrent.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtCurrent.WaterText = "电流设置";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button_CurrentInside);
            this.groupBox2.Controls.Add(this.button_Temperature);
            this.groupBox2.Controls.Add(this.button_ACIR);
            this.groupBox2.Controls.Add(this.txtSettingGrid);
            this.groupBox2.Controls.Add(this.button_voltage);
            this.groupBox2.Controls.Add(this.button_reset);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(342, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 201);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Measure";
            // 
            // button_CurrentInside
            // 
            this.button_CurrentInside.Location = new System.Drawing.Point(6, 81);
            this.button_CurrentInside.Name = "button_CurrentInside";
            this.button_CurrentInside.Size = new System.Drawing.Size(114, 24);
            this.button_CurrentInside.TabIndex = 5;
            this.button_CurrentInside.Text = "CurrentInside";
            this.button_CurrentInside.UseVisualStyleBackColor = true;
            this.button_CurrentInside.Click += new System.EventHandler(this.button_CurrentInside_Click);
            // 
            // button_Temperature
            // 
            this.button_Temperature.Location = new System.Drawing.Point(6, 140);
            this.button_Temperature.Name = "button_Temperature";
            this.button_Temperature.Size = new System.Drawing.Size(114, 23);
            this.button_Temperature.TabIndex = 0;
            this.button_Temperature.Text = "Temperature";
            this.button_Temperature.UseVisualStyleBackColor = true;
            this.button_Temperature.Click += new System.EventHandler(this.button_Temperature_Click);
            // 
            // button_ACIR
            // 
            this.button_ACIR.Location = new System.Drawing.Point(6, 110);
            this.button_ACIR.Name = "button_ACIR";
            this.button_ACIR.Size = new System.Drawing.Size(114, 24);
            this.button_ACIR.TabIndex = 4;
            this.button_ACIR.Text = "ACIR";
            this.button_ACIR.UseVisualStyleBackColor = true;
            this.button_ACIR.Click += new System.EventHandler(this.button_ACIR_Click);
            // 
            // txtSettingGrid
            // 
            this.txtSettingGrid.ContextMenuStrip = this.contextMenuStrip1;
            this.txtSettingGrid.Location = new System.Drawing.Point(144, 22);
            this.txtSettingGrid.Multiline = true;
            this.txtSettingGrid.Name = "txtSettingGrid";
            this.txtSettingGrid.Size = new System.Drawing.Size(183, 155);
            this.txtSettingGrid.TabIndex = 3;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 26);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // button_voltage
            // 
            this.button_voltage.Location = new System.Drawing.Point(6, 52);
            this.button_voltage.Name = "button_voltage";
            this.button_voltage.Size = new System.Drawing.Size(114, 24);
            this.button_voltage.TabIndex = 2;
            this.button_voltage.Text = "VoltageInside";
            this.button_voltage.UseVisualStyleBackColor = true;
            this.button_voltage.Click += new System.EventHandler(this.button_voltage_Click);
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(6, 22);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(114, 23);
            this.button_reset.TabIndex = 1;
            this.button_reset.Text = "Reset";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // groupBox_Meter
            // 
            this.groupBox_Meter.Controls.Add(this.button_CurrentMeter);
            this.groupBox_Meter.Controls.Add(this.button_MeterVoltage);
            this.groupBox_Meter.Controls.Add(this.txtMeterShow);
            this.groupBox_Meter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Meter.Location = new System.Drawing.Point(3, 210);
            this.groupBox_Meter.Name = "groupBox_Meter";
            this.groupBox_Meter.Size = new System.Drawing.Size(333, 202);
            this.groupBox_Meter.TabIndex = 3;
            this.groupBox_Meter.TabStop = false;
            this.groupBox_Meter.Text = "Meter";
            // 
            // button_CurrentMeter
            // 
            this.button_CurrentMeter.Location = new System.Drawing.Point(6, 66);
            this.button_CurrentMeter.Name = "button_CurrentMeter";
            this.button_CurrentMeter.Size = new System.Drawing.Size(114, 27);
            this.button_CurrentMeter.TabIndex = 6;
            this.button_CurrentMeter.Text = "Current";
            this.button_CurrentMeter.UseVisualStyleBackColor = true;
            this.button_CurrentMeter.Click += new System.EventHandler(this.button_CurrentMeter_Click);
            // 
            // button_MeterVoltage
            // 
            this.button_MeterVoltage.Location = new System.Drawing.Point(6, 22);
            this.button_MeterVoltage.Name = "button_MeterVoltage";
            this.button_MeterVoltage.Size = new System.Drawing.Size(114, 27);
            this.button_MeterVoltage.TabIndex = 5;
            this.button_MeterVoltage.Text = "Voltage";
            this.button_MeterVoltage.UseVisualStyleBackColor = true;
            this.button_MeterVoltage.Click += new System.EventHandler(this.button_MeterVoltage_Click);
            // 
            // txtMeterShow
            // 
            this.txtMeterShow.Location = new System.Drawing.Point(144, 15);
            this.txtMeterShow.Multiline = true;
            this.txtMeterShow.Name = "txtMeterShow";
            this.txtMeterShow.Size = new System.Drawing.Size(183, 181);
            this.txtMeterShow.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(684, 421);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Meter";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 169);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "isolateRes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(144, 178);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "reset";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(135, 26);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(114, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "NTC";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 490);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ToolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox_Meter.ResumeLayout(false);
            this.groupBox_Meter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button_Connect;
        private CCWin.SkinControl.SkinWaterTextBox txtIP;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button_Temperature;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.TextBox txtSettingGrid;
        private System.Windows.Forms.Button button_voltage;
        private System.Windows.Forms.Button button_ACIR;
        private System.Windows.Forms.Button button_discharge;
        private System.Windows.Forms.Button button_Charge;
        private CCWin.SkinControl.SkinWaterTextBox txtCurrent;
        private CCWin.SkinControl.SkinWaterTextBox txtVoltage;
        private System.Windows.Forms.Button button_CurrentInside;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CurrMode_CheckBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private CCWin.SkinControl.SkinWaterTextBox txtMeter;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox_Meter;
        private System.Windows.Forms.Button button_MeterVoltage;
        private System.Windows.Forms.TextBox txtMeterShow;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.Button button_CurrentMeter;
        private System.Windows.Forms.Button button_version;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

