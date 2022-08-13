using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IC.NOGagueIC.SW
{
    public class SettingTestParameter
    {
        public Dictionary<string, string> RegisterDictionary = new Dictionary<string, string>();
        public Dictionary<string, string> TestStepDictionary = new Dictionary<string, string>();
        public Dictionary<string, string> ItermclassifyStepDictionary = new Dictionary<string, string>();
        public SettingTestParameter()
        {
            TestStepDictionary.Add("0", "Start UP");
            TestStepDictionary.Add("1", "pack_wake");
            TestStepDictionary.Add("2", "pack_verify_i2c");
            TestStepDictionary.Add("3", "pack_cal_voltage");
            TestStepDictionary.Add("4", "pack_i2c_sda_voltage");
            TestStepDictionary.Add("5", "pack_i2c_scl_voltage");
            TestStepDictionary.Add("6", "pack_ocv");
            TestStepDictionary.Add("7", "pack_acir");
            TestStepDictionary.Add("8", "pack_z1hz");
            TestStepDictionary.Add("9", "pack_cocp");
            TestStepDictionary.Add("10", "pack_cocp_delay");
            TestStepDictionary.Add("11", "pack_cocp_release");
            TestStepDictionary.Add("12", "pack_docp");
            TestStepDictionary.Add("13", "pack_docp_delay");
            TestStepDictionary.Add("14", "pack_docp_release");
            TestStepDictionary.Add("15", "pack_gg_current_acc_noload");
            TestStepDictionary.Add("16", "pack_gg_curr_acc_chg_10mA");
            TestStepDictionary.Add("17", "pack_gg_curr_acc_dsg_10mA");
            TestStepDictionary.Add("18", "pack_gg_curr_acc_chg_0A1");
            TestStepDictionary.Add("19", "pack_gg_curr_acc_dsg_0A1");
            TestStepDictionary.Add("20", "pack_gg_temp_acc");
            TestStepDictionary.Add("21", "pack_gg_volt_acc");
            TestStepDictionary.Add("22", "pack_ntc_temp_acc");
            TestStepDictionary.Add("23", "pack_gg_rsoc");
            TestStepDictionary.Add("24", "pack_gg_fcc");
            TestStepDictionary.Add("25", "pack_gg_ncc");
            TestStepDictionary.Add("26", "pack_gg_cycle_count");
            TestStepDictionary.Add("27", "pack_gg_chem_id");
            TestStepDictionary.Add("28", "pack_gg_chem_dfcs");
            TestStepDictionary.Add("29", "pack_gg_prot_cs");
            TestStepDictionary.Add("30", "Enable_IT");
            TestStepDictionary.Add("31", "pack_gg_verify_qen");
            TestStepDictionary.Add("32", "Write_pack_serial_Number");
            TestStepDictionary.Add("33", "pack_sn");
            TestStepDictionary.Add("34", "pack_sn_cell_dates");
            TestStepDictionary.Add("35", "Place_pack_in_fullsleep_mode");
            TestStepDictionary.Add("36", "pack_verify_FS_OP");
            TestStepDictionary.Add("37", "Pack_unseal");
            TestStepDictionary.Add("38", "pack_gg_static_dfcs");
            TestStepDictionary.Add("39", "Pack_OCV_Current");
            TestStepDictionary.Add("40", "Pack_FET_Control");
            TestStepDictionary.Add("41", "oqc_bmu_sn");
            TestStepDictionary.Add("42", "oqc_sip_sn");
            TestStepDictionary.Add("43", "oqc_sip_sn_info");
            TestStepDictionary.Add("44", "LT_reset");
        }
    }
}
