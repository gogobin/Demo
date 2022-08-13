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
            TestStepDictionary.Add("46", "Start UP");
            TestStepDictionary.Add("1", "OCV_Left");
            TestStepDictionary.Add("2", "ACIR_Left");
            TestStepDictionary.Add("3", "ISolate_Resistance_Left");
            TestStepDictionary.Add("4", "ID_Resistance_Left");
            TestStepDictionary.Add("5","NTC_Left");
            TestStepDictionary.Add("6", "ChargeCurrent_Left");
            TestStepDictionary.Add("7", "Charge_deltaVoltage_Left");
            TestStepDictionary.Add("8", "Docp_Left");
            TestStepDictionary.Add("9", "Docp_Delay");
            TestStepDictionary.Add("10","ShortCirCuit_Protection");
            TestStepDictionary.Add("11", "OCV_Right");
            TestStepDictionary.Add("12", "ACIR_Right");
            TestStepDictionary.Add("13", "ID_Resistance_Right");
            TestStepDictionary.Add("14", "ISolate_Resistance_Right");
            TestStepDictionary.Add("15", "NTC_Right");
            TestStepDictionary.Add("16", "DischargeCurrent_Right");
            TestStepDictionary.Add("17", "Discharge_deltaVoltage_Right");
            TestStepDictionary.Add("31", "Vendor_ID");
            TestStepDictionary.Add("32", "Product_ID");
            TestStepDictionary.Add("33", "ECC_Verify");
            TestStepDictionary.Add("34", "Life_Span_Counter");
            TestStepDictionary.Add("35", "NVM_BarCode");
            TestStepDictionary.Add("36", "IC_Version");
            TestStepDictionary.Add("37", "Lock_Status");
            TestStepDictionary.Add("38", "BarCode_Compare");
            TestStepDictionary.Add("18", "Chargeing_Leakage_Current_Right");
            TestStepDictionary.Add("19", "Chargeing_Leakage_Current_Left");
            TestStepDictionary.Add("20", "DisChargeing_Leakage_Current_Left");
            TestStepDictionary.Add("21", "TempSample");
            TestStepDictionary.Add("22", "TempDelta2");
            TestStepDictionary.Add("23", "TempDelta1");
        }
    }
}
