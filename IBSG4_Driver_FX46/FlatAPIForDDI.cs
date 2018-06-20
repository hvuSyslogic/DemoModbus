using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;

namespace PhoenixContact.DDI
{
    class FlatAPIForDDI
    {
        static string _connectionDTI = @"IBETHIP[192.168.0.1]N1_D";
        static string _connectionMXI = @"IBETHIP[192.168.0.1]N1_D";
        static string _versionInfo;
        static int _handle, _dtiHandle;
        /// <summary>
        /// a managed static method that gets called from native code
        /// </summary>
        public static int Enable(string connectionName)
        {
            _handle = GetOpenNode();
            return _handle;

        }

        public static int Disable(string connectionNameHandler)
        {
            _handle = CloseNode();
            return _handle;
        }
        public static int ManagedMethodCalledFromExtension(string args)
        {
            // need to return an integer: the length of the args string
            byte[] data = new byte[8] { 0,16,0,0,0,0,0,0 };
            byte[] readData = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            int addr = 2; int dCons = 1; int tempValue = 0;
            switch (args)
            {
                case "1":
                    _handle = ReadData(addr, dCons, data);
                    Trace.WriteLine(string.Format("*Read: Handler: {0} Address: {1} Data {2} --return value {3}\n", _dtiHandle, addr, new SoapHexBinary(readData).ToString(), _handle));
                    break;
                case "2":
                    for (int i = 0; i < 2; i++)
                    {
                        tempValue = 16 * i;
                        data[1] += (byte)tempValue;
                        _handle = WriteData(addr, dCons, data);
                        Trace.WriteLine(string.Format("*Write: Handler: {0} Address: {1} Data {2} --return value {3}\n", _dtiHandle, addr, new SoapHexBinary(data).ToString(), _handle));
                        Thread.Sleep(10);
                    }
                    break;
                default:
                    _versionInfo = GetDDIVersionInfo();
                      break;
            }
            return args.Length;
        }

        private static int ReadData(int Address, int dCons, byte[] Data)
        {
            return PhoenixContact.DDI.DDI.ReadData(_dtiHandle, Address,ref Data);
        }

        private static int CloseNode()
        {
            int Integer  =  PhoenixContact.DDI.DDI.CloseNode(_dtiHandle);
            Trace.WriteLine(string.Format("In CloseNode : {0} handle :{1}\n", _connectionDTI, _dtiHandle) );
            return Integer;
        }

        private static int WriteData(int Address, int dCons, byte[] Data)
        {
            return PhoenixContact.DDI.DDI.WriteData(_dtiHandle, Address, Data);
        }

        private static int GetOpenNode()
        {
            int Integer = PhoenixContact.DDI.DDI.OpenNode(_connectionDTI, out _dtiHandle);
            Trace.WriteLine(string.Format("In GetOpenNode : {0} handle :{1}\n", _connectionDTI, _dtiHandle) );

            return Integer;
        }

        private static string GetDDIVersionInfo()
        {
            return PhoenixContact.DDI.DDI.GetVersionDn2DDI();
        }

     
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
