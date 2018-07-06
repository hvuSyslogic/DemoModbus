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
        //#region *** Create input variables

        //private VarInput _module2Input = new VarInput(0, PD_Length.Word, 8, 0);
        //private VarInput _module4Input = new VarInput(1, PD_Length.Word, 8, 0);
        //private VarInput _module5Input = new VarInput(2, PD_Length.Word, 8, 0);
        //private VarInput _module6Input = new VarInput(3, PD_Length.Word, 8, 0);
        //#endregion
        //#region *** Create output variables

        //private VarOutput _module1Output = new VarOutput(0, PD_Length.Word, 4, 0);
        //private VarOutput _module3Output = new VarOutput(2, PD_Length.Word, 8, 0);
        //private VarOutput _module7Output = new VarOutput(6, PD_Length.Word, 8, 0);
        //private VarOutput _module8Output = new VarOutput(7, PD_Length.Word, 8, 0);
        //private VarOutput _module9Output = new VarOutput(8, PD_Length.Word, 8, 0);
        //private VarOutput _module10Output = new VarOutput(9, PD_Length.Word, 8, 0);
        //#endregion
        public IOScanner(ICollection<int> Inputs, ICollection<int> Outputs)
        {


        }

        public bool Initialize()
        {
            return false;
        }
        Dictionary<int, Variable> _IOlist;
        private byte[] _in_buffer;
        private byte[] _out_buffer;
        private byte[] _recvData;
    }
}
