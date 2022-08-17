
namespace wpfSetting
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.skinMenuStrip1 = new CCWin.SkinControl.SkinMenuStrip();
            this.参数设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.调试工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数设定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载档案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testControl1 = new wpfSetting.TestControl();
            this.skinMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinMenuStrip1
            // 
            this.skinMenuStrip1.Arrow = System.Drawing.Color.Black;
            this.skinMenuStrip1.Back = System.Drawing.Color.White;
            this.skinMenuStrip1.BackRadius = 4;
            this.skinMenuStrip1.BackRectangle = new System.Drawing.Rectangle(10, 10, 10, 10);
            this.skinMenuStrip1.Base = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(200)))), ((int)(((byte)(254)))));
            this.skinMenuStrip1.BaseFore = System.Drawing.Color.Black;
            this.skinMenuStrip1.BaseForeAnamorphosis = false;
            this.skinMenuStrip1.BaseForeAnamorphosisBorder = 4;
            this.skinMenuStrip1.BaseForeAnamorphosisColor = System.Drawing.Color.White;
            this.skinMenuStrip1.BaseHoverFore = System.Drawing.Color.White;
            this.skinMenuStrip1.BaseItemAnamorphosis = true;
            this.skinMenuStrip1.BaseItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.BaseItemBorderShow = true;
            this.skinMenuStrip1.BaseItemDown = ((System.Drawing.Image)(resources.GetObject("skinMenuStrip1.BaseItemDown")));
            this.skinMenuStrip1.BaseItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.BaseItemMouse = ((System.Drawing.Image)(resources.GetObject("skinMenuStrip1.BaseItemMouse")));
            this.skinMenuStrip1.BaseItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.BaseItemRadius = 4;
            this.skinMenuStrip1.BaseItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinMenuStrip1.BaseItemSplitter = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.DropDownImageSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.skinMenuStrip1.Fore = System.Drawing.Color.Black;
            this.skinMenuStrip1.HoverFore = System.Drawing.Color.White;
            this.skinMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.skinMenuStrip1.ItemAnamorphosis = true;
            this.skinMenuStrip1.ItemBorder = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.ItemBorderShow = true;
            this.skinMenuStrip1.ItemHover = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.ItemPressed = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(148)))), ((int)(((byte)(212)))));
            this.skinMenuStrip1.ItemRadius = 4;
            this.skinMenuStrip1.ItemRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.参数设置ToolStripMenuItem,
            this.调试工具ToolStripMenuItem,
            this.参数设定ToolStripMenuItem,
            this.加载档案ToolStripMenuItem,
            this.查看参数ToolStripMenuItem});
            this.skinMenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.skinMenuStrip1.Name = "skinMenuStrip1";
            this.skinMenuStrip1.RadiusStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinMenuStrip1.Size = new System.Drawing.Size(1373, 30);
            this.skinMenuStrip1.SkinAllColor = true;
            this.skinMenuStrip1.TabIndex = 0;
            this.skinMenuStrip1.Text = "skinMenuStrip1";
            this.skinMenuStrip1.TitleAnamorphosis = true;
            this.skinMenuStrip1.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(228)))), ((int)(((byte)(236)))));
            this.skinMenuStrip1.TitleRadius = 4;
            this.skinMenuStrip1.TitleRadiusStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // 参数设置ToolStripMenuItem
            // 
            this.参数设置ToolStripMenuItem.Name = "参数设置ToolStripMenuItem";
            this.参数设置ToolStripMenuItem.Size = new System.Drawing.Size(83, 26);
            this.参数设置ToolStripMenuItem.Text = "参数设置";
            // 
            // 调试工具ToolStripMenuItem
            // 
            this.调试工具ToolStripMenuItem.Name = "调试工具ToolStripMenuItem";
            this.调试工具ToolStripMenuItem.Size = new System.Drawing.Size(83, 26);
            this.调试工具ToolStripMenuItem.Text = "调试工具";
            this.调试工具ToolStripMenuItem.Click += new System.EventHandler(this.调试工具ToolStripMenuItem_Click);
            // 
            // 参数设定ToolStripMenuItem
            // 
            this.参数设定ToolStripMenuItem.Name = "参数设定ToolStripMenuItem";
            this.参数设定ToolStripMenuItem.Size = new System.Drawing.Size(83, 26);
            this.参数设定ToolStripMenuItem.Text = "参数设定";
            this.参数设定ToolStripMenuItem.Click += new System.EventHandler(this.参数设定ToolStripMenuItem_Click);
            // 
            // 加载档案ToolStripMenuItem
            // 
            this.加载档案ToolStripMenuItem.Name = "加载档案ToolStripMenuItem";
            this.加载档案ToolStripMenuItem.Size = new System.Drawing.Size(83, 26);
            this.加载档案ToolStripMenuItem.Text = "加载档案";
            // 
            // 查看参数ToolStripMenuItem
            // 
            this.查看参数ToolStripMenuItem.Name = "查看参数ToolStripMenuItem";
            this.查看参数ToolStripMenuItem.Size = new System.Drawing.Size(83, 26);
            this.查看参数ToolStripMenuItem.Text = "查看参数";
            // 
            // testControl1
            // 
            this.testControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testControl1.Location = new System.Drawing.Point(0, 30);
            this.testControl1.Name = "testControl1";
            this.testControl1.Size = new System.Drawing.Size(1373, 626);
            this.testControl1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1373, 656);
            this.Controls.Add(this.testControl1);
            this.Controls.Add(this.skinMenuStrip1);
            this.MainMenuStrip = this.skinMenuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.skinMenuStrip1.ResumeLayout(false);
            this.skinMenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinMenuStrip skinMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 参数设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 调试工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数设定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载档案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看参数ToolStripMenuItem;
        private TestControl testControl1;
    }
}