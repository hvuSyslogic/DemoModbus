using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qti.Autotron.ModbusAutotronAPI
{
    class EasyModbusItemComparer : EqualityComparer<EasyModbusItem>
    {
        public override bool Equals(EasyModbusItem modbusItem1, EasyModbusItem modbusItem2)
        {
            return modbusItem1.Equals(modbusItem2);
        }


        public override int GetHashCode(EasyModbusItem modbusItem)
        {
            return base.GetHashCode();
        }
    }   
}
