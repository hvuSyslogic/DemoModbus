//************************************************************************
//
//        This work contains valuable confidential and proprietary
//    information.  Disclosure, use or reproduction without the
//    written authorization of the company is prohibited.  This
//    unpublished work by the company is protected by the laws of the
//    United States and other countries.  If publication of the work
//    should occur the following notice shall apply:
//    Copyright Baldwin Technology Company, Inc. 1998/2018 All Rights Reserved
//
//    Creation Date: 06/18/2018
//    Description: Memory shared between C# and C/C++ dlls, 
//    static class API  function definition FOR interop
//    and Phoenix Contact  IL ETH BK DI8 DO4 2TX-PAC driver wrapper implementation
//	    for both C functions & C++ declaration & EasyModbus dll client
//      from   <package id="EasyModbusTCP" version="5.0.0" targetFramework="net40" />
//************************************************************************
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public class FlatAPIForDDI : IDisposable
    {
        /// <summary>
        /// Called on application SetSharedMem
        /// </summary>
        [DllImport("meb.dll", EntryPoint = "SetSharedMem", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool SetSharedMem(Int64 value);
        public static int EntryPoint(string ignored)
        {
            IntPtr pByteArray = IntPtr.Zero;
            g_GCHandle = GCHandle.Alloc(DataArray, GCHandleType.Pinned);
            pByteArray = g_GCHandle.AddrOfPinnedObject();
            bool bSetOK = SetSharedMem(pByteArray.ToInt64());
            return bSetOK ? 1 : 0;
        }
        // to change the share memory data which shared between Managed and Unmanaged code.
        public static byte[] DataArray
        {
            get
            {
                return s_bydataArray;
            } 
        }

        /// <summary>
        /// a managed static method that gets called from native code
        /// to enable both EasyModbus and dn2ddi Interbus for directly modify 
        /// Input Output module.
        /// </summary>
        public static int Enable(string connectionName)
        {
            if (string.IsNullOrWhiteSpace(connectionName)) return -1;

            if (connectionName.Contains(@"|"))
            {
                //s_ihandle = GetOpenNode();
                string[] vars = connectionName.Split('|');
                if (vars.Length > 1)
                {
                    int PortValue;
                    if (Int32.TryParse(vars[1], out PortValue))
                    {
                        g_ModbusWrapper = new EasyModbusWrapper(vars[0], PortValue);
                    }
                    else
                    {
                        g_ModbusWrapper = new EasyModbusWrapper(vars[0], 502);

                    }
                }
                return 0x0000FFFF;
            }
            else
            {
                s_ihandle = GetOpenNode();
                var test = s_ihandle;
                g_ModbusWrapper = new EasyModbusWrapper("192.168.0.1", 502);
                //_glModbusWrapper.Connect();
                return s_ihandle;
            }

        }
        /// <summary>
        /// a managed static method that gets called from native code
        /// to disalble dn2ddi Interbus driver 
        /// Need to work one to one with Enable method.
        /// If not, memory leak may occurs.
        /// </summary>
        public static int Disable(string connectionNameHandler)
        {
            s_ihandle = CloseNode();
            return s_ihandle;
        }
        /// <summary>
        /// Use to encapsulate dn2ddi read or write data.
        /// </summary>
        /// <param name="args"></param>
        /// ARG IS CONSTRUCT AS FOLLOW:
        /// "X|YY|ZZ|DATA".
        /// where as: X: IS read(1) or write(2)
        /// YY IS ADDRESS
        /// ZZ IS pd(package data) length; valid values are: 1,2,4,8
        /// data is a 64bit value( little endian format).
        /// <returns></returns>
        public static int ManagedPhoenixDN2DDIDriverWrapperOp(string args)
        {
            if (string.IsNullOrWhiteSpace(args)) return -1;
            // Hard code to write to Oput and read from Input slot 1.
            byte[] data = new byte[8] { 0, 7, 0, 0, 0, 0, 0, 0 };
            byte[] readData = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            int addr = 2; int dCons = 1; int tempValue = 0;
            string[] Args = args.Split('|');
            // end of hard code
            switch (args)
            {
                case "1":
                    s_ihandle = ReadData(addr, dCons, readData);
                    Trace.WriteLine(string.Format("*Read: Handler: {0} Address: {1} Data {2} --return value {3}\n", s_dtiHandle, addr, new SoapHexBinary(readData).ToString(), s_ihandle));
                    break;
                case "2":
                    addr = 0; // First DO
                    // this is write to different slot, testing hard code only.
                    for (int i = 0; i < 2; i++)
                    {
                        tempValue = 16 * i;
                        s_ihandle = WriteData(addr, dCons, data);
                        Trace.WriteLine(string.Format("*Write: Handler: {0} Address: {1} Data {2} --return value {3}\n", s_dtiHandle, addr, new SoapHexBinary(data).ToString(), s_ihandle));
                        Thread.Sleep(10);
                    }
                    // end of testing code.
                    break;
                default:
                    GetDDIVersionInfo();
                    break;
            }
            return args.Length;
        }
        /// <summary>
        /// Use to encapsulate Easymodbus read or write data to Modbus entity
        /// It also depends on the hardware supports for Modbus
        /// For Phoenix Contact IL ETH BK DI8 DO4 2TX-PAC, 
        /// It support Function Code FC3, FC4, FC6, FC16, AND FC23
        /// </summary>
        /// <param name="args"></param>
        /// Args is construct as follow:
        /// "Y|XXXX|DATA".
        /// where as: Y: IS FC3, 2 is FC4,... and  5 is FC23
        /// XXXXX Is modbus Input or Output
        /// Data is unsigned long (64BIT -Little Endiant format) for write if applicable.
        /// <returns></returns>
        public static int ManagedModbusServiceWrapperOp(string args)
        {
            int returnValue = -1;
            if (string.IsNullOrWhiteSpace(args)) return returnValue;
            string[] vars = args.Split('|');
            if (g_ModbusWrapper != null) g_ModbusWrapper.Connect();
            if (vars.Length > 1)
            {

                switch (vars[0])
                {
                    case "3":// FC3
                    case "4"://FC4
                        returnValue = EasyModbusReadOp(vars);
                        break;
                    case "6"://FC6
                    case "16"://FC16
                    case "23"://FC23
                    case "65536": // stop Modbus comm. Threads
                        returnValue = EasyModbusWriteOp(vars);
                        break;
                    default:
                        Debug.Assert(true);
                        break;
                }
            }
            else
            {
                return -1;
            }

            return returnValue;
        }
        /// <summary>
        /// Use to encapsulate Easymodbus Mapping IO (Digital & Ananlog Input/Output)
        /// It also depends on the hardware supports for Modbus
        /// For Phoenix Contact IL ETH BK DI8 DO4 2TX-PAC, 
        /// using dynamic table mappings, OR
        /// Process data (dynamic table) starting with di-1 at 8000
       /// 8000 R Local digital inputs FC3, FC4, FC6, FC16, FC23
       /// 8001 ... (8000+x) R Local bus inputs(x words)
       /// (8001+x) R/W Local digital outputs(8002+x)
       /// (8001+x+y) R/W Local bus outputs(y words)
        /// </summary>
        /// <param name="args"></param>
        /// Args is construct as follow:
        /// "di|do|do". is  DIGITAL INPUT 1, THEN DIGITAL OUTPUT 1 FROM INPUT ..., OUTPUT 2 FROM INPUT...
        /// <returns></returns>
        public static int ManagedModbusServiceWrapperMappingIO(string args)
        {
            int returnValue = -1;
            if (string.IsNullOrWhiteSpace(args)) return returnValue;
            string[] vars = args.Split('|');
            if (g_ModbusWrapper != null) g_ModbusWrapper.Connect();
            if (vars.Length > 1)
            {

                switch (vars[0])
                {
                    case "3":// FC3
                    case "4"://FC4
                        returnValue = EasyModbusReadOp(vars);
                        break;
                    case "6"://FC6
                    case "16"://FC16
                    case "23"://FC23
                    case "65536": // stop Modbus comm. Threads
                        returnValue = EasyModbusWriteOp(vars);
                        break;
                    default:
                        Debug.Assert(true);
                        break;
                }
            }
            else
            {
                return -1;
            }

            return args.Length;
        }

        private static int EasyModbusReadOp(string[] vars)
        {
            int FCCode, StartingAddress, QuantityRead;
            long Data;
            if (vars[1].Contains("-"))
            {
                StartingAddress = TranslateIOToAddress(vars[1]);
                if (StartingAddress == -1)
                {
                    s_sLastErrorMessage = "Invalid Address Entered: " + vars[1];
                    return -1;
                }
            }
            else
            {
                if (int.TryParse(vars[1], out StartingAddress))
                {
                    s_sLastErrorMessage = "Invalid Address Entered: " + vars[1];
                    return -1;
                }
            }
            if (int.TryParse(vars[0], out FCCode)
                && long.TryParse(vars[2], out Data))
            {
                QuantityRead = (int)(0x00000000FFFFFFFF & Data);
                g_ModbusWrapper.AddItemToProcess(new EasyModbusItem(FCCode, StartingAddress, QuantityRead));
                return 0;
            }
            else
            {
                s_sLastErrorMessage = "Invalid Address Entered: " + vars.ToString();
                return -1;
            }

        }

        private static int TranslateIOToAddress(string IOInformation)
        {
            string[] IOargs = IOInformation.Split('-');
            if (IOargs != null && IOargs.Length > 1)
            {
                switch (IOInformation.ToLower())
                {
                    case "di-1":
                        return 8000;// this base on the device itself
                    case "do-1":
                        return 8001;
                    case "do-2":
                        return 8002;
                    default:
                        return -1;
                }
            }
            else
                return -1;
        }

        private static int EasyModbusWriteOp(string[] vars)
        {
            int FCCode, StartingAddress, Quantity;
            if (vars[1].Contains("-"))
            {
                StartingAddress = TranslateIOToAddress(vars[1]);
                if (StartingAddress == -1)
                {
                    s_sLastErrorMessage = "Invalid Address Entered: " + vars[1];
                    return -1;
                }
            }
            else
            {
                if (!int.TryParse(vars[1], out StartingAddress))
                {
                    s_sLastErrorMessage = "Invalid Address Entered: " + vars[1];
                    return -1;
                }
            }
            if (int.TryParse(vars[0], out FCCode)
                && int.TryParse(vars[2], out Quantity))
            {

                g_ModbusWrapper.AddItemToProcess(new EasyModbusItem(FCCode, StartingAddress, Quantity));
                return 0;
            }
            else
            {
                s_sLastErrorMessage = "Invalid Address Entered: " + vars.ToString();
                return -1;
            }
        }

        // Public implementation of Dispose pattern callable by consumers.
        ~FlatAPIForDDI()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (s_disposed)
                return;

            if (disposing)
            {
            }

            // Free any unmanaged objects here.
            //
            if (g_GCHandle.IsAllocated) g_GCHandle.Free();
            s_disposed = true;
        }

        #region private
        private static int ReadData(int Address, int dCons, byte[] Data)
        {
            return PhoenixContact.DDI.DDI.ReadData(s_dtiHandle, Address, ref Data);
        }

        private static int CloseNode()
        {
            int Integer = 0;
            if (s_dtiHandle != 0) Integer = PhoenixContact.DDI.DDI.CloseNode(s_dtiHandle);

            Trace.WriteLine(string.Format("In CloseNode : {0} handle :{1}\n", s_dtiHandle));
            return Integer;
        }

        private static int WriteData(int Address, int dCons, byte[] Data)
        {
            return PhoenixContact.DDI.DDI.WriteData(s_dtiHandle, Address, Data);
        }

        private static int GetOpenNode()
        {
            int Integer = PhoenixContact.DDI.DDI.OpenNode(@"IBETHIP[192.168.0.1]N1_D", out s_dtiHandle);
            Trace.WriteLine(string.Format("In GetOpenNode : {0} handle :{1}\n",  s_dtiHandle));

            return Integer;
        }

        private static string GetDDIVersionInfo()
        {
            return PhoenixContact.DDI.DDI.GetVersionDn2DDI();
        }

        // hard code for testing
        static string s_sLastErrorMessage = String.Empty;
        static int s_ihandle, s_dtiHandle;
        static byte[] s_bydataArray = new byte[32] { 32, 0, 0xAA, 0xFF, 0xAA, 0xFF, 0xAA, 0xFF,
                                                      0xAA, 0xFF, 0xAA, 0xFF, 0xAA, 0xFF, 0xAA, 0xFF,
                                                      0xAA, 0xFF, 0xAA, 0xFF, 0xAA, 0xFF, 0xAA, 0xFF,
                                                      0xAA, 0xFF, 0xAA, 0xFF, 0xAA, 0xFF, 0xAA, 0xFF};
        // Flag: Has Dispose already been called?
        bool s_disposed = false;
        // Instantiate a SafeHandle instance.
        static GCHandle g_GCHandle;
        static EasyModbusWrapper g_ModbusWrapper;
        #endregion
    }
    public enum ControllerDiagnosticFlatAPI
    {
        Inactive = 0,
        NoError = 33536, // 0x00008300
        ConfirmationTimeout = 49156, // 0x0000C004
        PingTimeout = 49157, // 0x0000C005
        NotSupported = 49408, // 0x0000C100
        NoValidConnectionString = 49409, // 0x0000C101
        UpdateInputBlockStartAddress = 49412, // 0x0000C104
        UpdateInputBlockLength = 49413, // 0x0000C105
        UpdateOutputBlockStartAddress = 49414, // 0x0000C106
        UpdateOutputBlockLength = 49415, // 0x0000C107
        FirmwareServiceStateError = 49416, // 0x0000C108
        ProcessDataCycleTimeOutOfRange = 49417, // 0x0000C109
        MailboxDataCycleTimeOutOfRange = 49418, // 0x0000C10A
        ControllerStateCycleTimeOutOfRange = 49419, // 0x0000C10B
        GetDiagnostic = 49424, // 0x0000C110
        GetDiagnosticEx = 49425, // 0x0000C111
        GetSlaveDiagnostic = 49426, // 0x0000C112
        EnableNetfail = 49435, // 0x0000C11B
        DisableNetfail = 49436, // 0x0000C11C
        GetNetfailState = 49437, // 0x0000C11D
        NetfailOccurred = 49438, // 0x0000C11E
        ClearNetfail = 49439, // 0x0000C11F
        OnUpdateProcessData = 49440, // 0x0000C120
        OnUpdateMailbox = 49441, // 0x0000C121
        OnStateChangeEvents = 49442, // 0x0000C122
        CreateOnUpdateProcessData = 49443, // 0x0000C123
        EnableWatchdog = 49451, // 0x0000C12B
        DisableWatchdog = 49452, // 0x0000C12C
        GetWatchdogState = 49453, // 0x0000C12D
        WatchdogOccurred = 49454, // 0x0000C12E
        ParaErrGetByteFromBuffer = 49665, // 0x0000C201
        ParaErrPutByteToBuffer = 49666, // 0x0000C202
        ParaErrMessageClient = 49667, // 0x0000C203
        RetErrReceiveMessage = 49921, // 0x0000C301
        RetErrSendMessage = 49922, // 0x0000C302
        RetErrReadData = 49923, // 0x0000C303
        RetErrWriteData = 49924, // 0x0000C304
        NoValidMessageObject = 50176, // 0x0000C400
        NegConfSendMessage = 50177, // 0x0000C401
        ControllerIndication = 50178, // 0x0000C402
        IODiagnosticError = 50432, // 0x0000C500
        InterbusHandlingDiagnostic = 51713, // 0x0000CA01
        InterbusDriverDiagnostic = 51714, // 0x0000CA02
    }
}