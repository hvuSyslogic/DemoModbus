//************************************************************************
//
//    This work contains valuable confidential and proprietary
//    information.  Disclosure, use or reproduction without the
//    written authorization of the company is prohibited.  This
//    unpublished work by the company is protected by the laws of the
//    United States and other countries.  If publication of the work
//    should occur the following notice shall apply:
//    Copyright Baldwin Technology Company, Inc. 1998/2018 All Rights Reserved
//
//    Creation Date: 07/02/2018
//    Description: EasyModbusWrapper to work with Dn2ddi driver and/or Easymodbus.dll
//    this class abstract the IO periodically read and write IO values to put in the Mem shared from DDIDriver
//    which is using to read and write to the IOs with it backplan bus (Interbus).
//
//************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qti.Autotron.ModbusAutotronAPI
{

    class IOScanner : IIOScanner
    {
        //* original
        //#region *** Create input variables
        /// <summary>
        /// VarInput( in ByteAdress(BaseAddress), enumtype Process_Data_Length, Bit_Length_OfTheProcessDataItem,
        /// BitOffset__OfTheProcessDataItem)
        /// </summary>
        /// <param name="Inputs"></param>
        /// <param name="Outputs"></param>
        private VarInput MODULE_2_IN = new VarInput(0, PD_Length.Word, 8, 0);
        //#endregion
        //#region *** Create output variables
        private VarOutput MODULE_1_OUT = new VarOutput(0, PD_Length.Word, 4, 0);
        private VarOutput MODULE_3_OUT = new VarOutput(2, PD_Length.Word, 8, 0);

        //#endregion 
        public IOScanner(string ConnectionDTI = @"IBETHIP[192.168.0.1]N1_D", string ConnectionMXI = @"IBETHIP[192.168.0.1]N1_M")
        {
            _sConnectionDTI = ConnectionDTI;
            _sConnectionMXI = ConnectionMXI;

        }

        public bool Initialize()
        {
            return false;
        }
        string _sConnectionDTI = string.Empty;//@"IBETHIP[192.168.0.1]N1_D";
        string _sConnectionMXI = string.Empty;
        string s_sVersionInfo = string.Empty;
    }
}
