﻿namespace winformStyle
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            BunifuAnimatorNS.Animation animation5 = new BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelChildForm = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bunifuTransition1 = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.SumPanel = new System.Windows.Forms.Panel();
            this.panel_Manager = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button_Administrator = new System.Windows.Forms.Button();
            this.button_Manager = new System.Windows.Forms.Button();
            this.panel_seismometer = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.Button_seismometer = new System.Windows.Forms.Button();
            this.MenuPanel = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.btnHuawei = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button_Menu = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelChildForm.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SumPanel.SuspendLayout();
            this.panel_Manager.SuspendLayout();
            this.panel_seismometer.SuspendLayout();
            this.MenuPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelChildForm
            // 
            this.panelChildForm.Controls.Add(this.panel1);
            this.bunifuTransition1.SetDecoration(this.panelChildForm, BunifuAnimatorNS.DecorationType.None);
            this.panelChildForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChildForm.Location = new System.Drawing.Point(0, 0);
            this.panelChildForm.Name = "panelChildForm";
            this.panelChildForm.Size = new System.Drawing.Size(956, 546);
            this.panelChildForm.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SumPanel);
            this.panel1.Controls.Add(this.pictureBox1);
            this.bunifuTransition1.SetDecoration(this.panel1, BunifuAnimatorNS.DecorationType.None);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(956, 546);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.bunifuTransition1.SetDecoration(this.pictureBox1, BunifuAnimatorNS.DecorationType.None);
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::winformStyle.Properties.Resources._660691;
            this.pictureBox1.InitialImage = global::winformStyle.Properties.Resources._660691;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(956, 546);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // bunifuTransition1
            // 
            this.bunifuTransition1.AnimationType = BunifuAnimatorNS.AnimationType.VertSlide;
            this.bunifuTransition1.Cursor = null;
            animation5.AnimateOnlyDifferences = true;
            animation5.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation5.BlindCoeff")));
            animation5.LeafCoeff = 0F;
            animation5.MaxTime = 1F;
            animation5.MinTime = 0F;
            animation5.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation5.MosaicCoeff")));
            animation5.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation5.MosaicShift")));
            animation5.MosaicSize = 0;
            animation5.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            animation5.RotateCoeff = 0F;
            animation5.RotateLimit = 0F;
            animation5.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation5.ScaleCoeff")));
            animation5.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation5.SlideCoeff")));
            animation5.TimeCoeff = 0F;
            animation5.TransparencyCoeff = 0F;
            this.bunifuTransition1.DefaultAnimation = animation5;
            // 
            // panel3
            // 
            this.bunifuTransition1.SetDecoration(this.panel3, BunifuAnimatorNS.DecorationType.None);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(956, 20);
            this.panel3.TabIndex = 2;
            // 
            // SumPanel
            // 
            this.SumPanel.AutoScroll = true;
            this.SumPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.SumPanel.Controls.Add(this.panel_Manager);
            this.SumPanel.Controls.Add(this.button_Manager);
            this.SumPanel.Controls.Add(this.panel_seismometer);
            this.SumPanel.Controls.Add(this.Button_seismometer);
            this.SumPanel.Controls.Add(this.MenuPanel);
            this.SumPanel.Controls.Add(this.button_Menu);
            this.SumPanel.Controls.Add(this.panel2);
            this.bunifuTransition1.SetDecoration(this.SumPanel, BunifuAnimatorNS.DecorationType.None);
            this.SumPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.SumPanel.Location = new System.Drawing.Point(0, 0);
            this.SumPanel.Name = "SumPanel";
            this.SumPanel.Size = new System.Drawing.Size(200, 546);
            this.SumPanel.TabIndex = 1;
            // 
            // panel_Manager
            // 
            this.panel_Manager.Controls.Add(this.button1);
            this.panel_Manager.Controls.Add(this.button7);
            this.panel_Manager.Controls.Add(this.button_Administrator);
            this.bunifuTransition1.SetDecoration(this.panel_Manager, BunifuAnimatorNS.DecorationType.None);
            this.panel_Manager.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Manager.Location = new System.Drawing.Point(0, 555);
            this.panel_Manager.Name = "panel_Manager";
            this.panel_Manager.Size = new System.Drawing.Size(183, 120);
            this.panel_Manager.TabIndex = 6;
            // 
            // button1
            // 
            this.bunifuTransition1.SetDecoration(this.button1, BunifuAnimatorNS.DecorationType.None);
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button1.Location = new System.Drawing.Point(0, 80);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button1.Size = new System.Drawing.Size(183, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "Users";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.bunifuTransition1.SetDecoration(this.button7, BunifuAnimatorNS.DecorationType.None);
            this.button7.Dock = System.Windows.Forms.DockStyle.Top;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button7.Location = new System.Drawing.Point(0, 40);
            this.button7.Name = "button7";
            this.button7.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button7.Size = new System.Drawing.Size(183, 40);
            this.button7.TabIndex = 1;
            this.button7.Text = "Engineer";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button_Administrator
            // 
            this.bunifuTransition1.SetDecoration(this.button_Administrator, BunifuAnimatorNS.DecorationType.None);
            this.button_Administrator.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_Administrator.FlatAppearance.BorderSize = 0;
            this.button_Administrator.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Administrator.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Administrator.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button_Administrator.Location = new System.Drawing.Point(0, 0);
            this.button_Administrator.Name = "button_Administrator";
            this.button_Administrator.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button_Administrator.Size = new System.Drawing.Size(183, 40);
            this.button_Administrator.TabIndex = 0;
            this.button_Administrator.Text = "Administrator";
            this.button_Administrator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Administrator.UseVisualStyleBackColor = true;
            // 
            // button_Manager
            // 
            this.bunifuTransition1.SetDecoration(this.button_Manager, BunifuAnimatorNS.DecorationType.None);
            this.button_Manager.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_Manager.FlatAppearance.BorderSize = 0;
            this.button_Manager.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Manager.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Manager.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button_Manager.Location = new System.Drawing.Point(0, 510);
            this.button_Manager.Name = "button_Manager";
            this.button_Manager.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button_Manager.Size = new System.Drawing.Size(183, 45);
            this.button_Manager.TabIndex = 5;
            this.button_Manager.Text = "Manager";
            this.button_Manager.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Manager.UseVisualStyleBackColor = true;
            this.button_Manager.Click += new System.EventHandler(this.button_Manager_Click);
            // 
            // panel_seismometer
            // 
            this.panel_seismometer.Controls.Add(this.button8);
            this.panel_seismometer.Controls.Add(this.button9);
            this.panel_seismometer.Controls.Add(this.button10);
            this.bunifuTransition1.SetDecoration(this.panel_seismometer, BunifuAnimatorNS.DecorationType.None);
            this.panel_seismometer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_seismometer.Location = new System.Drawing.Point(0, 390);
            this.panel_seismometer.Name = "panel_seismometer";
            this.panel_seismometer.Size = new System.Drawing.Size(183, 120);
            this.panel_seismometer.TabIndex = 4;
            // 
            // button8
            // 
            this.bunifuTransition1.SetDecoration(this.button8, BunifuAnimatorNS.DecorationType.None);
            this.button8.Dock = System.Windows.Forms.DockStyle.Top;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button8.Location = new System.Drawing.Point(0, 80);
            this.button8.Name = "button8";
            this.button8.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button8.Size = new System.Drawing.Size(183, 40);
            this.button8.TabIndex = 2;
            this.button8.Text = "Itech";
            this.button8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.bunifuTransition1.SetDecoration(this.button9, BunifuAnimatorNS.DecorationType.None);
            this.button9.Dock = System.Windows.Forms.DockStyle.Top;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button9.Location = new System.Drawing.Point(0, 40);
            this.button9.Name = "button9";
            this.button9.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button9.Size = new System.Drawing.Size(183, 40);
            this.button9.TabIndex = 1;
            this.button9.Text = "Hoki";
            this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.bunifuTransition1.SetDecoration(this.button10, BunifuAnimatorNS.DecorationType.None);
            this.button10.Dock = System.Windows.Forms.DockStyle.Top;
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button10.Location = new System.Drawing.Point(0, 0);
            this.button10.Name = "button10";
            this.button10.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button10.Size = new System.Drawing.Size(183, 40);
            this.button10.TabIndex = 0;
            this.button10.Text = "Aglient";
            this.button10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.UseVisualStyleBackColor = true;
            // 
            // Button_seismometer
            // 
            this.bunifuTransition1.SetDecoration(this.Button_seismometer, BunifuAnimatorNS.DecorationType.None);
            this.Button_seismometer.Dock = System.Windows.Forms.DockStyle.Top;
            this.Button_seismometer.FlatAppearance.BorderSize = 0;
            this.Button_seismometer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Button_seismometer.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_seismometer.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.Button_seismometer.Location = new System.Drawing.Point(0, 345);
            this.Button_seismometer.Name = "Button_seismometer";
            this.Button_seismometer.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.Button_seismometer.Size = new System.Drawing.Size(183, 45);
            this.Button_seismometer.TabIndex = 3;
            this.Button_seismometer.Text = "seismometer";
            this.Button_seismometer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Button_seismometer.UseVisualStyleBackColor = true;
            this.Button_seismometer.Click += new System.EventHandler(this.Button_seismometer_Click_1);
            // 
            // MenuPanel
            // 
            this.MenuPanel.Controls.Add(this.button6);
            this.MenuPanel.Controls.Add(this.btnHuawei);
            this.MenuPanel.Controls.Add(this.button4);
            this.MenuPanel.Controls.Add(this.button3);
            this.MenuPanel.Controls.Add(this.button2);
            this.bunifuTransition1.SetDecoration(this.MenuPanel, BunifuAnimatorNS.DecorationType.None);
            this.MenuPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.MenuPanel.Location = new System.Drawing.Point(0, 145);
            this.MenuPanel.Name = "MenuPanel";
            this.MenuPanel.Size = new System.Drawing.Size(183, 200);
            this.MenuPanel.TabIndex = 2;
            // 
            // button6
            // 
            this.bunifuTransition1.SetDecoration(this.button6, BunifuAnimatorNS.DecorationType.None);
            this.button6.Dock = System.Windows.Forms.DockStyle.Top;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button6.Location = new System.Drawing.Point(0, 160);
            this.button6.Name = "button6";
            this.button6.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button6.Size = new System.Drawing.Size(183, 40);
            this.button6.TabIndex = 4;
            this.button6.Text = "Others";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // btnHuawei
            // 
            this.bunifuTransition1.SetDecoration(this.btnHuawei, BunifuAnimatorNS.DecorationType.None);
            this.btnHuawei.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnHuawei.FlatAppearance.BorderSize = 0;
            this.btnHuawei.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuawei.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuawei.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnHuawei.Location = new System.Drawing.Point(0, 120);
            this.btnHuawei.Name = "btnHuawei";
            this.btnHuawei.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.btnHuawei.Size = new System.Drawing.Size(183, 40);
            this.btnHuawei.TabIndex = 3;
            this.btnHuawei.Text = "Huawei";
            this.btnHuawei.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHuawei.UseVisualStyleBackColor = true;
            this.btnHuawei.Click += new System.EventHandler(this.btnHuawei_Click_1);
            // 
            // button4
            // 
            this.bunifuTransition1.SetDecoration(this.button4, BunifuAnimatorNS.DecorationType.None);
            this.button4.Dock = System.Windows.Forms.DockStyle.Top;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button4.Location = new System.Drawing.Point(0, 80);
            this.button4.Name = "button4";
            this.button4.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button4.Size = new System.Drawing.Size(183, 40);
            this.button4.TabIndex = 2;
            this.button4.Text = "Google";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.bunifuTransition1.SetDecoration(this.button3, BunifuAnimatorNS.DecorationType.None);
            this.button3.Dock = System.Windows.Forms.DockStyle.Top;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button3.Location = new System.Drawing.Point(0, 40);
            this.button3.Name = "button3";
            this.button3.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button3.Size = new System.Drawing.Size(183, 40);
            this.button3.TabIndex = 1;
            this.button3.Text = "FaceBook";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.bunifuTransition1.SetDecoration(this.button2, BunifuAnimatorNS.DecorationType.None);
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.button2.Size = new System.Drawing.Size(183, 40);
            this.button2.TabIndex = 0;
            this.button2.Text = "Apple";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button_Menu
            // 
            this.bunifuTransition1.SetDecoration(this.button_Menu, BunifuAnimatorNS.DecorationType.None);
            this.button_Menu.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_Menu.FlatAppearance.BorderSize = 0;
            this.button_Menu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Menu.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Menu.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button_Menu.Location = new System.Drawing.Point(0, 100);
            this.button_Menu.Name = "button_Menu";
            this.button_Menu.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.button_Menu.Size = new System.Drawing.Size(183, 45);
            this.button_Menu.TabIndex = 1;
            this.button_Menu.Text = "Menu";
            this.button_Menu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_Menu.UseVisualStyleBackColor = true;
            this.button_Menu.Click += new System.EventHandler(this.button_Menu_Click_1);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.bunifuTransition1.SetDecoration(this.panel2, BunifuAnimatorNS.DecorationType.None);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(183, 100);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.bunifuTransition1.SetDecoration(this.label1, BunifuAnimatorNS.DecorationType.None);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("MV Boli", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 100);
            this.label1.TabIndex = 0;
            this.label1.Text = "Desay";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.ClientSize = new System.Drawing.Size(956, 546);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panelChildForm);
            this.bunifuTransition1.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelChildForm.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.SumPanel.ResumeLayout(false);
            this.panel_Manager.ResumeLayout(false);
            this.panel_seismometer.ResumeLayout(false);
            this.MenuPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelChildForm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private BunifuAnimatorNS.BunifuTransition bunifuTransition1;
        private System.Windows.Forms.Panel SumPanel;
        private System.Windows.Forms.Panel panel_Manager;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button_Administrator;
        private System.Windows.Forms.Button button_Manager;
        private System.Windows.Forms.Panel panel_seismometer;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button Button_seismometer;
        private System.Windows.Forms.Panel MenuPanel;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnHuawei;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_Menu;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
    }
}
