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
//    Creation Date: 06/27/2018
//    Description: Utility Extensions class to work with the assembly 
//
//************************************************************************

namespace Qti.Autotron.ModbusAutotronAPI
{
    static class UtilExtensions
    {
        public static bool Compare(this byte[] pByte, byte[] pByteArray)
        {
            if (pByte == null || pByteArray == null || (pByte.Length != pByteArray.Length || pByteArray.Length == 0))
                return false;
            for (int index = 0; index < pByteArray.Length; ++index)
            {
                if ((int)pByte[index] != (int)pByteArray[index])
                    return false;
            }
            return true;
        }
    }
}
