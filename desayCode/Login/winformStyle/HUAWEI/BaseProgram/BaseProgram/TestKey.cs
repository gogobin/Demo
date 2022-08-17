using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProgram
{
    public class TestKey
    {
        public Dictionary<string, Key> dc = new Dictionary<string, Key>();
        public bool GetValue(string TestStepName)
        {
            int Retry = 0;
            int Delay = 1;
            string[] Value = Convert.ToString(CParamter.BasicParameter[TestStepName]).Split(new char[] { ',' });
            bool ITest = Convert.ToBoolean(Value[6]);
            bool bGon = Convert.ToBoolean(Value[29]);
            string ErrCode = Value[4].ToString();
            string LogName = Value[5].ToString();
            if (Value[2].ToString().Trim().Length > 0)
            {
                Retry = Convert.ToInt16(Value[2].ToString());
            }
            if (Value[3].ToString().Trim().Length > 0)
            {
                Delay = Convert.ToInt16(Value[3].ToString());
            }
            double minRes = 0;
            if (Value[7].ToString().Trim().Length > 0)
            {
                minRes = Convert.ToDouble(Value[7]);
            }
            double maxRes = 0;
            if (Value[8].ToString().Trim().Length > 0)
            {
                maxRes = Convert.ToDouble(Value[8]);
            }
            double set = 500;
            if (Value[9].ToString().Trim().Length > 0)
            {
                set = Convert.ToDouble(Value[9]);
            }
            Key TK = new Key(Delay, Retry, TestStepName, ErrCode, LogName, ITest, bGon, minRes, maxRes, set);
            if (dc.ContainsKey(TestStepName))
            {
                dc.Clear();
            }
            dc.Add(TestStepName, TK);
            return ITest;
        }
    }
    public class Key
    {
        public int Delay;
        public int Retry;
        public string key;
        public string ErrorCode;
        public string LogName;
        public double minValue;
        public double maxValue;
        public bool ITest;
        public bool bGon;
        public double set;
        public Key(int Delay, int Retry, string TestKey, string ErrorCode, string LogName, bool ITest, bool bGon, double minValue, double maxValue, double set)
        {
            this.Delay = Delay;
            this.Retry = Retry;
            this.key = TestKey;
            this.ErrorCode = ErrorCode;
            this.LogName = LogName;
            this.ITest = ITest;
            this.bGon = bGon;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.set = set;
        }
    }
}
