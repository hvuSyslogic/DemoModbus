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
//    Creation Date: 07/5/2018
//    Description: IEasyModbusItem interface for EasyModbusItem to work with Easymodbus.dll
//    should be collection member resides in the EasyModbusWrapper
//
//************************************************************************
using System;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public interface IEasyModbusItem : IComparable
    {
        int FunctionCode { get;}
        int StartingAddress { get; }
        int Quantity { get; }
        bool ShouldRetry { get; }
        int[] DataSource { get;}
        byte[] RxBuffer { get; set; }
        byte[] TxBuffer { get; set; }
    }
}
