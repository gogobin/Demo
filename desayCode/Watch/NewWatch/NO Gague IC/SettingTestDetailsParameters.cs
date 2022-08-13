using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
using System.Xml;
//using Bll;
using IC.NOGagueIC.SW;
using System.IO;
using System.Net;

namespace IC.NOGagueIC.SW
{
    public partial class frmParaMter : Form
    {
        public frmParaMter()
        {
            InitializeComponent();
        }
        private static frmParaMter frmchild = null;
        public static frmParaMter CreateInstance()
        {
            if (frmchild == null || frmchild.IsDisposed)
            {
                frmchild = new frmParaMter();
            }
            frmchild.BringToFront();
            return frmchild;
        }
        private void SettingTestDetailsParameters_Load(object sender, EventArgs e)
        {
            dgvTestStep.RowCount = 10;
            for (int i = 0; i < dgvTestStep.ColumnCount; i++)
            {
                dgvTestStep.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;// DataGridViewAutoSizeColumnMode.NotSet;
            }
            dgvTestStep.ScrollBars = ScrollBars.Both;
        }

        private void dgvTestParamter_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1 && e.ColumnIndex != -1)
            //{
            //    frmSttingParamter frmSttingParamter = new frmSttingParamter();
            //    frmSttingParamter.ShowDialog();
            //}
        }

        private void dgvTestParamter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                int Row = e.RowIndex;
                frmSttingParamter frmSttingParamter = frmSttingParamter.CreateInstance(Row, this);
                frmSttingParamter.Owner = this;
                frmSttingParamter.proEvent = new frmSttingParamter.EventDelegate(EventPro);
                frmSttingParamter.ShowDialog();
            }
        }
        void EventPro(bool IsNext, int Row, int Col, string Message)
        {
            if (Row < 0 || Col < 0)
            {
                return;
            }
            if (Message.Trim().Length > 0)
            {
                //if (Col != 5)
                //{
                dgvTestStep.Rows[Row].Cells[Col].Value = Message;
                //}
                //else
                //{
                //    dgvTestStep.Rows[Row].Cells[Col].Value = Message;

                //    int Retry = 0;
                //    for (int i = 0; i < Row; i++)
                //    {
                //        if (dgvTestStep.Rows[Row].Cells[0].Value.ToString() == dgvTestStep.Rows[i].Cells[0].Value.ToString())
                //        {
                //            Retry++;
                //        }
                //    }
                //    if (Retry > 0)
                //    {
                //        dgvTestStep.Rows[Row].Cells[Col].Value = Message + "_" + Retry.ToString();
                //    }
                //}
            }
            if (Row == dgvTestStep.Rows.Count - 1)
            {
                dgvTestStep.Rows.Add();
            }
            if (IsNext)
            {
                dgvTestStep.Rows[Row + 1].Selected = true;
            }
            else
            {
                dgvTestStep.Rows[Row].Selected = true;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            //if (cboBqType.Text.Trim().Length == 0)
            //{
            //    MessageBox.Show("Plaese Write BQ Type", "Warning:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //SaveToXml();
        }



        private void Open_Click(object sender, EventArgs e)
        {
            //frmQueryParamter frmQueryParamter = new frmQueryParamter();
            //frmQueryParamter.ShowDialog();
            //if (frmQueryParamter.LoadOK)
            //{
            //    LoadFile(frmQueryParamter.XmlValue);
            //}
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

        public string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }

        private bool LoadFile(string xml_FilePath)
        {
            if (xml_FilePath.Trim().Length == 0)
            {
                return false;
            }
            //OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();//一个打开文件的对话框
            //openFileDialog1.Filter = "xml(*.xml)|*.xml";//设置允许打开的扩展名
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)//判断是否选择了文件  
            //{
            //xml_FilePath = openFileDialog1.FileName;//记录用户选择的文件路径
            XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”
            xmlDocument.LoadXml(xml_FilePath);//载入路径这个xml
            try
            {
                XmlNodeList xmlSeal = xmlDocument.SelectSingleNode("/Test/TestStep").ChildNodes;//选择class为根结点并得到旗下所有子节点
                dgvTestStep.Rows.Clear();//清空dataGridView1，防止和上次处理的数据混乱
                foreach (XmlNode xmlNode in xmlSeal)//遍历class的所有节点
                {
                    XmlElement xmlElement = (XmlElement)xmlNode;//对于任何一个元素，其实就是每一个<student>
                    //旗下的子节点<name>和<number>分别放入dataGridView1
                    int index = dgvTestStep.Rows.Add();//在dataGridView1新加一行，并拿到改行的行标
                    for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
                    {
                        dgvTestStep.Rows[index].Cells[i].Value = xmlElement.ChildNodes.Item(i).InnerText;//各个单元格分别添加
                    }
                }

            }
            catch
            {
                MessageBox.Show("XML format is not correct.");
            }

            return true;
        }

        private void toolStripMenuItemUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvTestStep.CurrentRow != null)
                {
                    //获取当前所选择的记录行号 
                    int index = this.dgvTestStep.CurrentRow.Index;
                    if (index == 0)
                    {
                        return;
                    }
                    for (int i = 0; i < dgvTestStep.ColumnCount; i++)
                    {
                        string _rowData = Convert.ToString(dgvTestStep.Rows[index - 1].Cells[i].EditedFormattedValue.ToString());

                        dgvTestStep.Rows[index - 1].Cells[i].Value = dgvTestStep.Rows[index].Cells[i].EditedFormattedValue.ToString();
                        dgvTestStep.Rows[index].Cells[i].Value = _rowData;
                    }

                    //选择的光标同时上移一行 
                    this.dgvTestStep.CurrentCell = this.dgvTestStep[this.dgvTestStep.CurrentCell.ColumnIndex, this.dgvTestStep.CurrentCell.RowIndex - 1];
                }
            }
            catch
            {
            }
        }

        private void toolStripMenuItemTestStep_Click(object sender, EventArgs e)
        {
            int Row = this.dgvTestStep.CurrentRow.Index;
            if (Row >= 0)
            {
                frmSttingParamter frmSttingParamter = frmSttingParamter.CreateInstance(Row, this);
                frmSttingParamter.Owner = this;
                frmSttingParamter.proEvent = new frmSttingParamter.EventDelegate(EventPro);
                frmSttingParamter.ShowDialog();
            }
        }

        private void toolStripMenuItemDown_Click(object sender, EventArgs e)
        {
            if (this.dgvTestStep.CurrentRow != null)
            {
                //获取当前所选择的记录行号 
                int index = this.dgvTestStep.CurrentRow.Index;
                if (index == dgvTestStep.RowCount - 1)
                {
                    return;
                }
                for (int i = 0; i < dgvTestStep.ColumnCount; i++)
                {
                    string _rowData = Convert.ToString(dgvTestStep.Rows[index + 1].Cells[i].EditedFormattedValue.ToString());

                    dgvTestStep.Rows[index + 1].Cells[i].Value = dgvTestStep.Rows[index].Cells[i].EditedFormattedValue.ToString();
                    dgvTestStep.Rows[index].Cells[i].Value = _rowData;
                }
                //选择的光标同时下移一行
                this.dgvTestStep.CurrentCell = this.dgvTestStep[this.dgvTestStep.CurrentCell.ColumnIndex, this.dgvTestStep.CurrentCell.RowIndex + 1];
            }
        }

        private void toolStripMenuItemDelect_Click(object sender, EventArgs e)
        {
            if (this.dgvTestStep.CurrentRow == null)
            {
                return;
            }
            int index = this.dgvTestStep.CurrentRow.Index;
            if (index < 0)
            {
                MessageBox.Show("Please select the data to be removed.！", "Warning:", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (index >= 0)
            {
                dgvTestStep.Rows.RemoveAt(index);
            }
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            int index = this.dgvTestStep.CurrentRow.Index; ;
            if (index >= 0)
            {
                dgvTestStep.Rows.Insert(index);
                dgvTestStep.Rows[this.dgvTestStep.CurrentRow.Index - 1].Selected = true;
            }
        }



        private void toolStripMenuItemSaveLocal_Click(object sender, EventArgs e)
        {
            SaveToXmlForLocal();

        }
        private bool SaveToXmlForLocal()
        {
            XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”

            SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();//打开一个保存对话框
            saveFileDialog1.Filter = "xml文件(*.xml)|*.xml";//设置允许打开的扩展名
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)//判断是否选择了一个文件路径
            {
                try
                {
                    XmlElement xmlElement_Main = xmlDocument.CreateElement("Test");//创建一个<class>节点

                    int row = 0;
                    int Col = 0;
                    #region TestStep
                    XmlElement xmlElement_class = xmlDocument.CreateElement("TestStep");//创建一个<class>节点
                    xmlElement_Main.AppendChild(xmlElement_class);

                    row = dgvTestStep.Rows.Count;//得到总行数    
                    Col = dgvTestStep.ColumnCount;
                    //int cell = dataGridView1.Rows[1].Cells.Count;//得到总列数    
                    for (int i = 0; i < row; i++)//得到总行数并在之内循环    
                    {
                        XmlElement xmlElement_student = xmlDocument.CreateElement("Row");

                        for (int iCol = 0; iCol < Col; iCol++)
                        {
                            XmlElement xmlElement_Test = xmlDocument.CreateElement("Col" + iCol.ToString());
                            xmlElement_Test.InnerText = dgvTestStep.Rows[i].Cells[iCol].EditedFormattedValue.ToString();
                            xmlElement_student.AppendChild(xmlElement_Test);

                        }
                        //同上，创建一个个<student>节点，并且附到<class>之下
                        xmlElement_class.AppendChild(xmlElement_student);
                    }
                    #endregion

                    xmlElement_Main.AppendChild(xmlElement_class);
                    xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", ""));//编写文件头
                    xmlDocument.AppendChild(xmlElement_Main);//将这个<class>附到总文件头，而且设置为根结点
                    string[] fineName = saveFileDialog1.FileName.Split(new char[] { '.' });


                    xmlDocument.Save(saveFileDialog1.FileName);//保存这个xml文件
                    MessageBox.Show("Save File OK");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return true;
                }
            }

            return true;
        }

        private void toolStripMenuItemOpenLocalFile_Click(object sender, EventArgs e)
        {
            LoadLocalFile();
        }
        private bool LoadLocalFile()
        {
            string xml_FilePath = "";
            OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();//一个打开文件的对话框
            openFileDialog1.Filter = "xml(*.xml)|*.xml";//设置允许打开的扩展名
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//判断是否选择了文件  
            {
                xml_FilePath = openFileDialog1.FileName;//记录用户选择的文件路径
                XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器”
                xmlDocument.Load(xml_FilePath);//载入路径这个xml
                try
                {
                    XmlNodeList xmlSeal = xmlDocument.SelectSingleNode("/Test/TestStep").ChildNodes;//选择class为根结点并得到旗下所有子节点
                    dgvTestStep.Rows.Clear();//清空dataGridView1，防止和上次处理的数据混乱
                    foreach (XmlNode xmlNode in xmlSeal)//遍历class的所有节点
                    {
                        XmlElement xmlElement = (XmlElement)xmlNode;//对于任何一个元素，其实就是每一个<student>
                        //旗下的子节点<name>和<number>分别放入dataGridView1
                        int index = dgvTestStep.Rows.Add();//在dataGridView1新加一行，并拿到改行的行标
                        for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
                        {
                            dgvTestStep.Rows[index].Cells[i].Value = xmlElement.ChildNodes.Item(i).InnerText;//各个单元格分别添加
                        }
                    }

                }
                catch
                {
                    MessageBox.Show("XML format is not correct.");
                }
            }
            else
            {
                //MessageBox.Show("请打开XML文件");
            }
            return true;
        }

        private void dgvTestStep_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTestStep_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTestStep_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                int Row = e.RowIndex;
                frmSttingParamter frmSttingParamter = frmSttingParamter.CreateInstance(Row, this);
                frmSttingParamter.Owner = this;
                frmSttingParamter.proEvent = new frmSttingParamter.EventDelegate(EventPro);
                frmSttingParamter.ShowDialog();
            }
        }
    }
}
