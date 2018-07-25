// Decompiled with JetBrains decompiler
// Type: PhoenixContact.DDI.DDI
// Assembly: IBSG4_Driver_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: BA38E233-77EA-4C5F-9C3F-E03C7CD687CE
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\IBSG4_Driver_FX46.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PhoenixContact.DDI
{
    [StructLayout(LayoutKind.Sequential)]
    public static class DDI
    {
        private static object l_OpenNode = new object();
        private static object l_CloseNode = new object();
        private static object l_ReadData = new object();
        private static object l_WriteData = new object();
        private static object l_SendMessage = new object();
        private static object l_ReceiveMessage = new object();
        private static object l_GetVersionDDI = new object();
        private static object l_GetVersionDn2DDI = new object();
        private static object l_GetDiagnostic = new object();
        private static object l_GetDiagnosticEx = new object();
        private static object l_GetSlaveDiagnostic = new object();
        private static object l_EnableWatchdogEx = new object();
        private static object l_GetWatchdogState = new object();
        private static object l_ClearWatchdog = new object();
        private static object l_EthSetTimeout = new object();
        private static object l_EthClearTimeout = new object();
        private static object l_EthSetNetfail = new object();
        private static object l_EthGetNetfailStatus = new object();
        private static object l_EthClearNetfail = new object();
        private static object l_EthSetNetfailMode = new object();
        private static object l_EthGetNetfailMode = new object();
        private static object l_CreateIB_Event = new object();
        private static object l_DeleteIB_Event = new object();
        private static object l_CreateEvent = new object();
        private static object l_CloseEvent = new object();
        private static object l_SetEvent = new object();
        private const string import_File = "dn2ddi.dll";
        private const int USIGN8 = 255;
        private const int USIGN16 = 65535;
        private const long USIGN32 = 4294967295;

        [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_GetInfo(int nodeHd, int cmd, char* vendor, char* name, char* revision, char* datetime, int* revNumber);

        [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe void DN_DDI_GetVersion(char* VersionInfo, int maxLen);

        [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_DevOpenNode(char[] devName, int perm, int* nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_DDI_DevCloseNode(int nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_DTI_WriteData(int ibsHdl, int length, int addr, int dCons, byte* msgBuf);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_DTI_ReadData(int ibsHdl, int length, int addr, int dCons, byte* msgBuf);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_MXI_SndMsg(int ibsHdl, int userID, int length, int msgType, byte* msgBuf);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_MXI_RcvMsg(int ibsHdl, byte* msgBuf, byte* msgBuf2);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_DDI_CreateEvent(int nodeHd, int eventType, int eventTime, int evtHdl_IB, int evtHdl_TO);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_DDI_DeleteEvent(int nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_GetIBSDiagnostic(int nodeHd, int* state, int* diagPara);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_GetIBSDiagnosticEx(int nodeHd, int* state, int* diagPara, int* addInfo);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_DDI_GetSlaveDiagnostic(int nodeHd, int* state, int* diagPara, int* addInfo);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_DDI_EnableWatchDogEx(int nodeHd, int wdTimeOut);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_DDI_GetWatchDogState(int nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_DDI_ClearWatchDog(int nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_ETH_SetDTITimeoutCtrl(int nodeHd, int* Time);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_ETH_ClearDTITimeoutCtrl(int nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_ETH_SetNetFail(int nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_ETH_GetNetFailStatus(int nodeHd, int* State, int* Reason);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_ETH_ClrNetFailStatus(int nodeHd);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern int DN_ETH_SetNetFailMode(int nodeHd, int Mode);

        [DllImport("dn2ddi.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern unsafe int DN_ETH_GetNetFailMode(int nodeHd, int* Mode);

        [DllImport("kernel32.dll")]
        private static extern unsafe int CreateEvent(void* nix, bool b1, bool b2, char[] nameOfEvent);

        [DllImport("kernel32.dll")]
        private static extern bool SetEvent(int Handle);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(int Handle);

        [DllImport("kernel32.dll")]
        private static extern int WaitForSingleObject(int evHdl, int evtTO);

        public static unsafe int OpenNode(string OpenNodeString, out int Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_OpenNode)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_OpenNode" + OpenNodeString + "\n");
                int num1;
                int num2 = PhoenixContact.DDI.DDI.DN_DDI_DevOpenNode(OpenNodeString.ToCharArray(), 3, &num1);
                Handle = num1;
                return num2;
            }
        }

        public static int CloseNode(int Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_CloseNode)
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_CloseNode\n");
            return PhoenixContact.DDI.DDI.DN_DDI_DevCloseNode(Handle);
        }

        public static unsafe int WriteData(int DTI_Handle, int Address, byte[] Data)
        {
            lock (PhoenixContact.DDI.DDI.l_WriteData)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_WriteData\n");
                int num1 = -1;
                if (DTI_Handle <= 0 || DTI_Handle > (int)ushort.MaxValue)
                    return num1;
                int length = Data.Length;
                int num2;
                fixed (byte* msgBuf = Data)
                    num2 = PhoenixContact.DDI.DDI.DN_DDI_DTI_WriteData(DTI_Handle, length, Address, 1, msgBuf);
                return num2;
            }
        }

        public static unsafe int ReadData(int DTI_Handle, int Address, ref byte[] Data)
        {
            lock (PhoenixContact.DDI.DDI.l_ReadData)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_ReadData\n");
                int num1 = -1;
                if (DTI_Handle <= 0 || DTI_Handle > (int)ushort.MaxValue)
                    return num1;
                int length = Data.Length;
                int num2;
                fixed (byte* msgBuf = Data)
                    num2 = PhoenixContact.DDI.DDI.DN_DDI_DTI_ReadData(DTI_Handle, length, Address, 1, msgBuf);
                return num2;
            }
        }

        public static unsafe int SendMessage(int MXI_Handle, int UserID, int Length, int MsgType, byte[] Message)
        {
            lock (PhoenixContact.DDI.DDI.l_SendMessage)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_SendMessage\n");
                if (MXI_Handle <= 0 || MXI_Handle > (int)ushort.MaxValue)
                    return -2;
                if (Length > Message.Length)
                    return -3;
                int num;
                fixed (byte* msgBuf = Message)
                    num = PhoenixContact.DDI.DDI.DN_DDI_MXI_SndMsg(MXI_Handle, UserID, Length, MsgType, msgBuf);
                Trace.WriteLine(string.Format("In PhoenixContact.DDI.DDI.l_SendMessage MsgType {0} Msg[] = {1} , Msg[1] = {2}\n", MsgType, Message[0], Message[1]));

                return num;
            }
        }

        public static unsafe int ReceiveMessage(int MXI_Handle, out int UserID, out int Length, out int MsgType, ref byte[] Message)
        {
            lock (PhoenixContact.DDI.DDI.l_ReceiveMessage)
            {
                Trace.WriteLine(string.Format("In PhoenixContact.DDI.DDI.l_ReceiveMessage handle {0} \n",MXI_Handle) );
                byte[] numArray = new byte[6];
                if (MXI_Handle <= 0 || MXI_Handle > (int)ushort.MaxValue)
                {
                    Trace.WriteLine("**shoudl not see this ! DDI.l_ReceiveMessage\n");
                    UserID = 0;
                    Length = 0;
                    MsgType = 0;
                    Message = new byte[0];
                    return -1;
                }
                Length = Message.Length;
                numArray[2] = Convert.ToByte(Length & (int)byte.MaxValue);
                numArray[3] = Convert.ToByte(Length >> 8 & (int)byte.MaxValue);
                int num;
                fixed (byte* msgBuf = Message)
                fixed (byte* msgBuf2 = numArray)
                    num = PhoenixContact.DDI.DDI.DN_DDI_MXI_RcvMsg(MXI_Handle, msgBuf, msgBuf2);
                UserID = (int)numArray[1];
                UserID = UserID << 8 | (int)numArray[0];
                Length = (int)numArray[3];
                Length = Length << 8 | (int)numArray[2];
                MsgType = (int)numArray[5];
                MsgType = MsgType << 8 | (int)numArray[4];
                Trace.WriteLine(string.Format("In PhoenixContact.DDI.DDI.l_ReceiveMessage handle{0} UserID {1} MsgType {2} \n", MXI_Handle, UserID, MsgType));
                return num;
            }
        }

        public static unsafe VersionInfo GetVersionDDI(int Handle, VersionInfoType cmd)
        {
            lock (PhoenixContact.DDI.DDI.l_GetVersionDDI)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_GetVersionDDI\n");
                VersionInfo versionInfo = new VersionInfo();
                char[] chArray1 = new char[80];
                char[] chArray2 = new char[80];
                char[] chArray3 = new char[80];
                char[] chArray4 = new char[80];
                if (Handle <= 0 || Handle > (int)ushort.MaxValue)
                    return versionInfo;
                int num;
                fixed (char* vendor = chArray1)
                fixed (char* name = chArray2)
                fixed (char* revision = chArray3)
                fixed (char* datetime = chArray4)
                    PhoenixContact.DDI.DDI.DN_DDI_GetInfo(Handle, (int)cmd, vendor, name, revision, datetime, &num);
                int length1 = 0;
                while (length1 < chArray1.Length && chArray1[length1] != char.MinValue)
                    ++length1;
                versionInfo.Vendor = new string(chArray1, 0, length1);
                int length2 = 0;
                while (length2 < chArray2.Length && chArray2[length2] != char.MinValue)
                    ++length2;
                versionInfo.Name = new string(chArray2, 0, length2);
                int length3 = 0;
                while (length3 < chArray3.Length && chArray3[length3] != char.MinValue)
                    ++length3;
                versionInfo.Revision = new string(chArray3, 0, length3);
                versionInfo.RevNumber = num;
                return versionInfo;
            }
        }

        public static unsafe string GetVersionDn2DDI()
        {
            lock (PhoenixContact.DDI.DDI.l_GetVersionDn2DDI)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_GetVersionDn2DDI\n");
                char[] chArray = new char[256];
                fixed (char* VersionInfo = chArray)
                    PhoenixContact.DDI.DDI.DN_DDI_GetVersion(VersionInfo, 256);
                int length = 0;
                while (length < 256 && chArray[length] != char.MinValue)
                    ++length;
                if (length > 0)
                    return new string(chArray, 0, length);
                return "No data available!";
            }
        }

        public static unsafe int GetDiagnostic(int Handle, out int DiagState, out int DiagPara)
        {
            lock (PhoenixContact.DDI.DDI.l_GetDiagnostic)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_GetDiagnostic\n");
                int num1 = -1;
                DiagState = 0;
                DiagPara = 0;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                {
                    int num2;
                    int num3;
                    num1 = PhoenixContact.DDI.DDI.DN_DDI_GetIBSDiagnostic(Handle, &num2, &num3);
                    DiagState = num2;
                    DiagPara = num3;
                }
                return num1;
            }
        }

        public static unsafe int GetDiagnosticEx(int Handle, out int DiagState, out int DiagPara, out int DiagAddInfo)
        {
            lock (PhoenixContact.DDI.DDI.l_GetDiagnosticEx)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_GetDiagnosticEx\n");
                int num1 = -1;
                DiagState = 0;
                DiagPara = 0;
                DiagAddInfo = 0;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                {
                    int num2;
                    int num3;
                    int num4;
                    num1 = PhoenixContact.DDI.DDI.DN_DDI_GetIBSDiagnosticEx(Handle, &num2, &num3, &num4);
                    DiagState = num2;
                    DiagPara = num3;
                    DiagAddInfo = num4;
                }
                return num1;
            }
        }

        public static unsafe int GetSlaveDiagnostic(int Handle, out int DiagState, out int DiagPara, out int DiagAddInfo)
        {
            lock (PhoenixContact.DDI.DDI.l_GetSlaveDiagnostic)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_GetSlaveDiagnostic\n");
                int num1 = -1;
                DiagState = 0;
                DiagPara = 0;
                DiagAddInfo = 0;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                {
                    int num2;
                    int num3;
                    int num4;
                    num1 = PhoenixContact.DDI.DDI.DN_DDI_GetSlaveDiagnostic(Handle, &num2, &num3, &num4);
                    DiagState = num2;
                    DiagPara = num3;
                    DiagAddInfo = num4;
                }
                return num1;
            }
        }

        public static int EnableWatchDogEx(int DTI_Handle, WatchdogMonitoringTime TimeOut)
        {
            lock (PhoenixContact.DDI.DDI.l_EnableWatchdogEx)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_EnableWatchdogEx\n");
                int num = -1;
                if (DTI_Handle > 0 && DTI_Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_DDI_EnableWatchDogEx(DTI_Handle, (int)TimeOut);
                return num;
            }
        }

        public static int GetWatchDogState(int DTI_Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_GetWatchdogState)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_GetWatchdogState\n");
                int num = -1;
                if (DTI_Handle > 0 && DTI_Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_DDI_GetWatchDogState(DTI_Handle);
                return num;
            }
        }

        public static int ClearWatchDog(int DTI_Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_ClearWatchdog)
            {
                Trace.WriteLine("In PhoenixContact.DDI.DDI.l_ClearWatchdog\n");
                int num = -1;
                if (DTI_Handle > 0 && DTI_Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_DDI_ClearWatchDog(DTI_Handle);
                return num;
            }
        }

        public static unsafe int ETH_SetTimeout(int DTI_Handle, ref int TimeOut)
        {
            lock (PhoenixContact.DDI.DDI.l_EthSetTimeout)
            {
                int num1 = -1;
                int num2 = TimeOut;
                if (DTI_Handle > 0 && DTI_Handle <= (int)ushort.MaxValue)
                {
                    num1 = PhoenixContact.DDI.DDI.DN_ETH_SetDTITimeoutCtrl(DTI_Handle, &num2);
                    TimeOut = num2;
                }
                return num1;
            }
        }

        public static int ETH_ClearTimeout(int DTI_Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_EthClearTimeout)
            {
                int num = -1;
                if (DTI_Handle > 0 && DTI_Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_ETH_ClearDTITimeoutCtrl(DTI_Handle);
                return num;
            }
        }

        public static int ETH_SetNetFail(int Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_EthSetNetfail)
            {
                int num = -1;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_ETH_SetNetFail(Handle);
                return num;
            }
        }

        public static unsafe int ETH_GetNetFailStatus(int Handle, out int State, out int Reason)
        {
            lock (PhoenixContact.DDI.DDI.l_EthGetNetfailStatus)
            {
                int num1 = -1;
                int num2 = 0;
                int num3 = 0;
                State = 0;
                Reason = 0;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                {
                    num1 = PhoenixContact.DDI.DDI.DN_ETH_GetNetFailStatus(Handle, &num2, &num3);
                    State = num2;
                    Reason = num3;
                }
                return num1;
            }
        }

        public static int ETH_ClearNetfail(int Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_EthClearNetfail)
            {
                int num = -1;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_ETH_ClrNetFailStatus(Handle);
                return num;
            }
        }

        public static int ETH_SetNetFailMode(int Handle, ETH_NetFailModes Mode)
        {
            lock (PhoenixContact.DDI.DDI.l_EthSetNetfailMode)
            {
                int num = -1;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_ETH_SetNetFailMode(Handle, (int)Mode);
                return num;
            }
        }

        public static unsafe int ETH_GetNetfailMode(int Handle, out ETH_NetFailModes Mode)
        {
            lock (PhoenixContact.DDI.DDI.l_EthGetNetfailMode)
            {
                int num1 = -1;
                int num2 = 0;
                Mode = ETH_NetFailModes.ETH_NF_STD_MODE;
                if (Handle > 0 && Handle <= (int)ushort.MaxValue)
                {
                    num1 = PhoenixContact.DDI.DDI.DN_ETH_GetNetFailMode(Handle, &num2);
                    Mode = (ETH_NetFailModes)num2;
                }
                return num1;
            }
        }

        public static int CreateIB_Event(int DTI_Handle, int EventType, int EventTime, int EventHandleIB, int EventHandleTO)
        {
            lock (PhoenixContact.DDI.DDI.l_CreateIB_Event)
            {
                int num = -1;
                if (DTI_Handle > 0 && DTI_Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_DDI_CreateEvent(DTI_Handle, EventType, EventTime, EventHandleIB, EventHandleTO);
                return num;
            }
        }

        public static int DeleteIB_Event(int DTI_Handle)
        {
            lock (PhoenixContact.DDI.DDI.l_DeleteIB_Event)
            {
                int num = -1;
                if (DTI_Handle > 0 && DTI_Handle <= (int)ushort.MaxValue)
                    num = PhoenixContact.DDI.DDI.DN_DDI_DeleteEvent(DTI_Handle);
                return num;
            }
        }

        public static unsafe int CreateNamedEvent(string Name)
        {
            lock (PhoenixContact.DDI.DDI.l_CreateEvent)
                return PhoenixContact.DDI.DDI.CreateEvent((void*)null, false, false, Name.ToCharArray());
        }

        public static bool CloseNamedEvent(int EvHandle)
        {
            lock (PhoenixContact.DDI.DDI.l_CloseEvent)
            {
                bool flag = false;
                if (EvHandle > 0 && EvHandle <= (int)ushort.MaxValue)
                    flag = PhoenixContact.DDI.DDI.CloseHandle(EvHandle);
                return flag;
            }
        }

        public static bool SetNamedEvent(int EvHandle)
        {
            lock (PhoenixContact.DDI.DDI.l_SetEvent)
            {
                bool flag = false;
                if (EvHandle > 0 && EvHandle <= (int)ushort.MaxValue)
                    flag = PhoenixContact.DDI.DDI.SetEvent(EvHandle);
                return flag;
            }
        }

        public static bool WaitForNamedEvent(int EvHandle, int Timeout)
        {
            return EvHandle > 0 && EvHandle <= (int)ushort.MaxValue && PhoenixContact.DDI.DDI.WaitForSingleObject(EvHandle, Timeout) == 0;
        }
    }
}
