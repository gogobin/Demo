using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseProgram
{
    public partial class Parameter : Form
    {
        public Parameter(string filePath)
        {
            InitializeComponent();
            ShowTestPara(filePath, skinDataGridView1);
        }
        public DataGridView ShowTestPara(string FilePath, DataGridView DG)
        {
            LoadFileClass.LoadFile(FilePath);
            int Row = 0;
            int[] DieabelShow = { 46 };
            DG.RowCount = CParamter.TestStep.Count;
            foreach (string Var in CParamter.TestStep.Keys)
            {
                if (Var.ToString() == "Start UP")
                {
                    continue;
                }
                DG.Rows[Row].Cells[0].Value = Var.ToString();
                string[] Value = CParamter.BasicParameter[Var].Split(new char[] { ',' });
                int iStep = Convert.ToInt16(CParamter.TestStep[Var]);
                DG.Rows[Row].Cells[1].Value = Value[7];
                DG.Rows[Row].Cells[2].Value = Value[8];
                DG.Rows[Row].Cells[3].Value = Value[4];
                Row = Row + 1;
            }
            return DG;
        }
    }
}
