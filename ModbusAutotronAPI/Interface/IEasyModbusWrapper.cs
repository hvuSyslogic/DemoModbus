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
//    Description: IEasyModbusWrapper interface for EasyModbusWrapper to work with Easymodbus.dll

//
//************************************************************************
namespace Qti.Autotron.ModbusAutotronAPI
{
    public interface IEasyModbusWrapper
    {
        void CheckStatus(object stateInfo);
        void Connect();
    }
}