using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProgram
{
    public class ChannelNumber : INotifyPropertyChanged
    {
        private Int64 PassNumber=0;
        private Int64 SUMNumber=0;
        public Int64 PNumber
        {
            get
            {
                return PassNumber;
            }
            set
            {
                PassNumber = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("PNumber"));
                }
            }
        }
        public Int64 SNumber
        {
            get
            {
                return SUMNumber;
            }
            set
            {
                SUMNumber = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("SNumber"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    }
