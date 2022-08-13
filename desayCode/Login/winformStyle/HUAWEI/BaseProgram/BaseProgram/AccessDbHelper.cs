using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADOX;


namespace BaseProgram
{
    public static class AccessDbHelper
    {
        public static bool CreateAccessDb_OneTable(string filePath, List<string> LogKes)
        {
            ADOX.Catalog catalog = new Catalog();
            if (!File.Exists(filePath))
            {
                try
                {
                    catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + @filePath);//+ ";jet oledb:database password=123");       
                    ADOX.Column[] columns = {
                                     new ADOX.Column(){Name="Fid"},
                                     new ADOX.Column(){Name="col1"},
                                     new ADOX.Column(){Name="col2"}
                                 };
                    CreateAccessOneTable(filePath, "TestData", LogKes, columns);
                }
                catch (System.Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 在access数据库中创建表
        /// </summary>
        /// <param name="filePath">数据库表文件全路径如D:\\NewDb.mdb 没有则创建 </param> 
        /// <param name="tableName">表名</param>
        /// <param name="colums">ADOX.Column对象数组</param>
        public static void CreateAccessOneTable(string filePath, string tableName, List<string> LogKes, params ADOX.Column[] colums)
        {
            ADOX.Catalog catalog = new Catalog();
            //数据库文件不存在则创建
            if (!File.Exists(filePath))
            {
                try
                {
                    catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath);//+ ";jet oledb:database password=123");
                }
                catch (System.Exception ex)
                {
                }
            }
            ADODB.Connection cn = new ADODB.Connection();
            cn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath, null, null, -1);
            catalog.ActiveConnection = cn;
            ADOX.Table table = new ADOX.Table();
            table.Name = tableName;
            Column col = new ADOX.Column() { Name = "Fid", Type = DataTypeEnum.adInteger };
            table.Columns.Append(col);
            col.ParentCatalog = catalog;
            col.Properties["AutoIncrement"].Value = true; //设置自动增长
            table.Keys.Append("FirstTablePrimaryKey", KeyTypeEnum.adKeyPrimary, col, null, null); //定义主键

            //Column col0 = new ADOX.Column() { Name = "BMU_Serial_Number", Type = DataTypeEnum.adVarWChar };
            //table.Columns.Append(col0);
            ////Barcode
            Column Barcode = new ADOX.Column() { Name = "BMU_Serial_Nnumber", Type = DataTypeEnum.adLongVarWChar };
            //Barcode.Properties["Jet OLEDB:Allow Zero Length"].Value = true;
            table.Columns.Append(Barcode);

            Column col2 = new ADOX.Column() { Name = "BMU_Barcode", Type = DataTypeEnum.adLongVarWChar };
            //col2.Properties["Jet OLEDB:Allow Zero Length"].Value = true;
            table.Columns.Append(col2);
            Column Test_User = new ADOX.Column() { Name = "Test_Config_Name", Type = DataTypeEnum.adLongVarWChar };
            table.Columns.Append(Test_User);

            Column Test_Channel_No = new ADOX.Column() { Name = "FDate", Type = DataTypeEnum.adDate };
            table.Columns.Append(Test_Channel_No);
            Column Line_No = new ADOX.Column() { Name = "Test_User", Type = DataTypeEnum.adLongVarWChar };
            table.Columns.Append(Line_No);

            //
            Column col3 = new ADOX.Column() { Name = "Test_Channel_No", Type = DataTypeEnum.adLongVarWChar };
            //col3.Properties["Jet OLEDB:Allow Zero Length"].Value = true;
            table.Columns.Append(col3);

            Column col55 = new ADOX.Column() { Name = "StationId", Type = DataTypeEnum.adInteger };
            //col3.Properties["Jet OLEDB:Allow Zero Length"].Value = true;
            table.Columns.Append(col55);

            Column WorkPosition = new ADOX.Column() { Name = "Line_No", Type = DataTypeEnum.adLongVarWChar };
            table.Columns.Append(WorkPosition);

            Column col4 = new ADOX.Column() { Name = "Host_Name", Type = DataTypeEnum.adLongVarWChar };
            table.Columns.Append(col4);


            col4.Attributes = ColumnAttributesEnum.adColNullable;
            Column Rrrorcode = new ADOX.Column() { Name = "WorkPosition", Type = DataTypeEnum.adLongVarWChar };
            table.Columns.Append(Rrrorcode);

            Column Test_Config_Name = new ADOX.Column() { Name = "Result", Type = DataTypeEnum.adInteger };
            table.Columns.Append(Test_Config_Name);
            Column Device_Name = new ADOX.Column() { Name = "Error_Code", Type = DataTypeEnum.adLongVarWChar };
            table.Columns.Append(Device_Name);
            //
            Column col5 = new ADOX.Column() { Name = "Flag", Type = DataTypeEnum.adInteger };
            table.Columns.Append(col5);

            //之后添加的代码
            //Column DeviceName = new ADOX.Column() { Name = "StationName", Type = DataTypeEnum.adVarWChar };
            //table.Columns.Append(DeviceName);

            //Column col6 = new ADOX.Column() { Name = "CellBarcode_HL", Type = DataTypeEnum.adVarWChar };
            //table.Columns.Append(col6);

            //Column col7 = new ADOX.Column() { Name = "CellBarcode_VL", Type = DataTypeEnum.adVarWChar };
            //table.Columns.Append(col7);

            //Column col8 = new ADOX.Column() { Name = "VerifyPackSN_CELL", Type = DataTypeEnum.adVarWChar };
            //table.Columns.Append(col8);

            //Column col9 = new ADOX.Column() { Name = "Barcode", Type = DataTypeEnum.adVarWChar };
            //table.Columns.Append(col9);

            //Column col10 = new ADOX.Column() { Name = "Pack_acir", Type = DataTypeEnum.adInteger };
            //table.Columns.Append(col10);

            //Column col11 = new ADOX.Column() { Name = "Pack_ocv", Type = DataTypeEnum.adInteger };
            //table.Columns.Append(col11);

            //Column col12 = new ADOX.Column() { Name = "Pack_docp_delay", Type = DataTypeEnum.adInteger };
            //table.Columns.Append(col12);

            //Column col13 = new ADOX.Column() { Name = "Pack_i2c_sda_voltage", Type = DataTypeEnum.adInteger };
            //table.Columns.Append(col13);

            //Column col14 = new ADOX.Column() { Name = "Pack_i2c_scl_voltage", Type = DataTypeEnum.adInteger };
            //table.Columns.Append(col14);

            //Column col7 = new ADOX.Column() { Name = "Test_time", Type = DataTypeEnum.adLongVarWChar };
            //table.Columns.Append(col7);
            //List<string> Contains = new List<string>();
            //foreach (string kes in LogKes)
            //{
            //    if (!Contains.Contains(kes))
            //    {
            //        Contains.Add(kes);
            //    }
            //}
            foreach (string kes in LogKes)
            {
                if (kes.Trim().Length > 0)
                {
                    Column col6 = new ADOX.Column() { Name = kes.Trim(), Type = DataTypeEnum.adLongVarWChar };
                    col6.Attributes = ColumnAttributesEnum.adColNullable;
                    table.Columns.Append(col6);
                }
            }
            //Rrrorcode.Attributes = ColumnAttributesEnum.adColNullable; 
            catalog.Tables.Append(table);
            cn.Close();
        }


    }
}
