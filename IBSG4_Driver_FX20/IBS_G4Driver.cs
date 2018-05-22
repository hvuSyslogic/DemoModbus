// Decompiled with JetBrains decompiler
// Type: PhoenixContact.DDI.IBS_G4Driver
// Assembly: IBSG4_Driver_FX20, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3ba9beb416a0ed83
// MVID: 066AFE0C-D702-4CB2-814E-202CA622D4F8
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\IBSG4_Driver_FX20.dll

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PhoenixContact.DDI
{
  [StructLayout(LayoutKind.Sequential)]
  public class IBS_G4Driver : IDisposable
  {
    private double _glbCPULoadEx = 50.0;
    private double _glbCPULoadDisableEvent = 200.0;
    private const string import_File = "dn2ddi.dll";
    private const int USIGN8 = 255;
    private const int USIGN16 = 65535;
    private const long USIGN32 = 4294967295;
    private string name;
    private int dtiHandle;
    private int mxiHandle;
    private bool threadExitFlag;
    private string dtiConnectionString;
    private string mxiConnectionString;
    private long callBackCounter;
    private long callBackCounterTO;
    private Thread thrHdlIB;
    private Thread thrHdlTO;
    private int _glbCounterTo10;
    private double _glbCPULoad;
    private bool _glbEventDisableFlag;
    private long saveIdleTime;
    private long saveKernelTime;
    private long saveUserTime;
    private int _EvtHdlTO;
    private int _EvtHdlIB;
    private int _createEvent;

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_GetInfo(int nodeHd, int cmd, char* vendor, char* name, char* revision, char* datetime, int* revNumber);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe void DN_DDI_GetVersion(char* VersionInfo, int maxLen);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_DevOpenNode(char[] devName, int perm, int* nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_DDI_DevCloseNode(int nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_DTI_WriteData(int ibsHdl, int length, int addr, int dCons, byte* msgBuf);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_DTI_ReadData(int ibsHdl, int length, int addr, int dCons, byte* msgBuf);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_MXI_SndMsg(int ibsHdl, int userID, int length, int msgType, byte* msgBuf);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_MXI_RcvMsg(int ibsHdl, byte* msgBuf, byte* msgBuf2);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_DDI_CreateEvent(int nodeHd, int eventType, int eventTime, int evtHdl_IB, int evtHdl_TO);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_DDI_DeleteEvent(int nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_GetIBSDiagnostic(int nodeHd, int* state, int* diagPara);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_GetIBSDiagnosticEx(int nodeHd, int* state, int* diagPara, int* addInfo);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_DDI_GetSlaveDiagnostic(int nodeHd, int* state, int* diagPara, int* addInfo);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_DDI_EnableWatchDogEx(int nodeHd, int wdTimeOut);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_DDI_GetWatchDogState(int nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_DDI_ClearWatchDog(int nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_ETH_SetDTITimeoutCtrl(int nodeHd, int* Time);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_ETH_ClearDTITimeoutCtrl(int nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_ETH_SetNetFail(int nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_ETH_GetNetFailStatus(int nodeHd, int* State, int* Reason);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_ETH_ClrNetFailStatus(int nodeHd);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int DN_ETH_SetNetFailMode(int nodeHd, int Mode);

    [DllImport("dn2ddi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern unsafe int DN_ETH_GetNetFailMode(int nodeHd, int* Mode);

    [DllImport("kernel32.dll")]
    private static extern unsafe int CreateEvent(void* nix, bool b1, bool b2, char[] nameOfEvent);

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(int Handle);

    [DllImport("kernel32.dll")]
    private static extern int WaitForSingleObject(int evHdl, int evtTO);

    [DllImport("kernel32.dll")]
    private static extern unsafe bool GetSystemTimes(long* ft1, long* ft2, long* ft3);

    private event UpdateIBSHandler evOnIBSCycle;

    public IBS_G4Driver(string Name)
    {
      this.name = Name;
      this.threadExitFlag = false;
      this.callBackCounter = 0L;
      this.callBackCounterTO = 0L;
      this.threadExitFlag = false;
      this.dtiConnectionString = "";
      this.mxiConnectionString = "";
    }

    public int CPU_Load
    {
      get
      {
        return (int) this._glbCPULoad;
      }
    }

    public int CPU_LoadEx
    {
      get
      {
        return (int) this._glbCPULoadEx;
      }
      set
      {
        this._glbCPULoadEx = (double) value;
      }
    }

    public int DTI_Handle
    {
      get
      {
        return this.dtiHandle;
      }
    }

    public int MXI_Handle
    {
      get
      {
        return this.mxiHandle;
      }
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public long CallBackInterbus
    {
      get
      {
        return this.callBackCounter;
      }
    }

    public long CallBackTimeout
    {
      get
      {
        return this.callBackCounterTO;
      }
    }

    public int MaxCPULoad
    {
      get
      {
        return (int) this._glbCPULoadDisableEvent;
      }
      set
      {
        this._glbCPULoadDisableEvent = (double) value;
      }
    }

    public bool EventDisable
    {
      get
      {
        return this._glbEventDisableFlag;
      }
      set
      {
        this._glbEventDisableFlag = value;
      }
    }

    public unsafe VersionInfo GetInfo(VersionInfoCmd cmd)
    {
      VersionInfo versionInfo = new VersionInfo();
      char[] chArray1 = new char[80];
      char[] chArray2 = new char[80];
      char[] chArray3 = new char[80];
      char[] chArray4 = new char[80];
      int num;
      fixed (char* vendor = chArray1)
        fixed (char* name = chArray2)
          fixed (char* revision = chArray3)
            fixed (char* datetime = chArray4)
              IBS_G4Driver.DN_DDI_GetInfo(this.mxiHandle, (int) cmd, vendor, name, revision, datetime, &num);
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

    public unsafe string DN2DDI_GetVersion()
    {
      char[] chArray = new char[256];
      fixed (char* VersionInfo = chArray)
        IBS_G4Driver.DN_DDI_GetVersion(VersionInfo, 256);
      int length = 0;
      while (length < 256 && chArray[length] != char.MinValue)
        ++length;
      return new string(chArray, 0, length);
    }

    public int OpenNode(string OpenNodeString)
    {
      int num;
      if (OpenNodeString != null)
      {
        string rightString = this.GetRightString(OpenNodeString, 4);
        if (rightString == "N1_D")
        {
          if (this.dtiHandle <= 0 || this.dtiHandle > (int) ushort.MaxValue)
          {
            this.dtiConnectionString = OpenNodeString;
            num = this.DDI_DevOpenNode(this.dtiConnectionString, ref this.dtiHandle);
            Thread.Sleep(10);
          }
          else
            num = this.dtiHandle;
        }
        else if (rightString == "N1_M")
        {
          if (this.mxiHandle <= 0 || this.mxiHandle > (int) ushort.MaxValue)
          {
            this.mxiConnectionString = OpenNodeString;
            num = this.DDI_DevOpenNode(this.mxiConnectionString, ref this.mxiHandle);
            Thread.Sleep(10);
          }
          else
            num = this.mxiHandle;
        }
        else
        {
          this.SetError(65281, 1);
          num = -1;
        }
      }
      else
      {
        this.SetError(65281, 2);
        num = -1;
      }
      return num;
    }

    private unsafe int DDI_DevOpenNode(string OpenNodeString, ref int Handle)
    {
      int num1;
      int num2 = IBS_G4Driver.DN_DDI_DevOpenNode(OpenNodeString.ToCharArray(), 3, &num1);
      Handle = num1;
      return num2;
    }

    private int DDI_DevCloseNode(int Handle)
    {
      return IBS_G4Driver.DN_DDI_DevCloseNode(Handle);
    }

    public unsafe int WriteData(int Address, byte[] Data)
    {
      int num1 = -1;
      if (this.dtiHandle <= 0 || this.dtiHandle > (int) ushort.MaxValue)
        return num1;
      int length = Data.Length;
      int num2;
      fixed (byte* msgBuf = Data)
        num2 = IBS_G4Driver.DN_DDI_DTI_WriteData(this.dtiHandle, length, Address, 0, msgBuf);
      return num2;
    }

    public unsafe int ReadData(int Address, ref byte[] Data)
    {
      int num1 = -1;
      if (this.dtiHandle <= 0 || this.dtiHandle > (int) ushort.MaxValue)
        return num1;
      int length = Data.Length;
      int num2;
      fixed (byte* msgBuf = Data)
        num2 = IBS_G4Driver.DN_DDI_DTI_ReadData(this.dtiHandle, length, Address, 0, msgBuf);
      return num2;
    }

    public unsafe int SendMessage(int UserID, int Length, int MsgType, byte[] Message)
    {
      int num1 = -1;
      if (this.mxiHandle <= 0 || this.mxiHandle > (int) ushort.MaxValue || Length > Message.Length)
        return num1;
      int num2;
      fixed (byte* msgBuf = Message)
        num2 = IBS_G4Driver.DN_DDI_MXI_SndMsg(this.mxiHandle, UserID, Length, MsgType, msgBuf);
      return num2;
    }

    public unsafe int ReceiveMessage(out int UserID, out int Length, out int MsgType, ref byte[] Message)
    {
      byte[] numArray = new byte[6];
      if (this.mxiHandle <= 0 || this.mxiHandle > (int) ushort.MaxValue)
      {
        UserID = 0;
        Length = 0;
        MsgType = 0;
        Message = new byte[0];
        return -1;
      }
      Length = Message.Length;
      numArray[2] = Convert.ToByte(Length & (int) byte.MaxValue);
      numArray[3] = Convert.ToByte(Length >> 8 & (int) byte.MaxValue);
      int num;
      fixed (byte* msgBuf = Message)
        fixed (byte* msgBuf2 = numArray)
          num = IBS_G4Driver.DN_DDI_MXI_RcvMsg(this.mxiHandle, msgBuf, msgBuf2);
      UserID = (int) numArray[1];
      UserID = UserID << 8 | (int) numArray[0];
      Length = (int) numArray[3];
      Length = Length << 8 | (int) numArray[2];
      MsgType = (int) numArray[5];
      MsgType = MsgType << 8 | (int) numArray[4];
      return num;
    }

    public unsafe int GetDiagnostic(out int DiagState, out int DiagPara)
    {
      int num1 = -1;
      DiagState = 0;
      DiagPara = 0;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
      {
        int num2;
        int num3;
        num1 = IBS_G4Driver.DN_DDI_GetIBSDiagnostic(this.mxiHandle, &num2, &num3);
        DiagState = num2;
        DiagPara = num3;
      }
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
      {
        int num2;
        int num3;
        num1 = IBS_G4Driver.DN_DDI_GetIBSDiagnostic(this.dtiHandle, &num2, &num3);
        DiagState = num2;
        DiagPara = num3;
      }
      return num1;
    }

    public unsafe int GetDiagnosticEx(out int DiagState, out int DiagPara, out int DiagAddInfo)
    {
      int num1 = -1;
      DiagState = 0;
      DiagPara = 0;
      DiagAddInfo = 0;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
      {
        int num2;
        int num3;
        int num4;
        num1 = IBS_G4Driver.DN_DDI_GetIBSDiagnosticEx(this.mxiHandle, &num2, &num3, &num4);
        DiagState = num2;
        DiagPara = num3;
        DiagAddInfo = num4;
      }
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
      {
        int num2;
        int num3;
        int num4;
        num1 = IBS_G4Driver.DN_DDI_GetIBSDiagnosticEx(this.dtiHandle, &num2, &num3, &num4);
        DiagState = num2;
        DiagPara = num3;
        DiagAddInfo = num4;
      }
      return num1;
    }

    public unsafe int GetSlaveDiagnostic(out int DiagState, out int DiagPara, out int DiagAddInfo)
    {
      int num1 = -1;
      DiagState = 0;
      DiagPara = 0;
      DiagAddInfo = 0;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
      {
        int num2;
        int num3;
        int num4;
        num1 = IBS_G4Driver.DN_DDI_GetSlaveDiagnostic(this.mxiHandle, &num2, &num3, &num4);
        DiagState = num2;
        DiagPara = num3;
        DiagAddInfo = num4;
      }
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
      {
        int num2;
        int num3;
        int num4;
        num1 = IBS_G4Driver.DN_DDI_GetSlaveDiagnostic(this.dtiHandle, &num2, &num3, &num4);
        DiagState = num2;
        DiagPara = num3;
        DiagAddInfo = num4;
      }
      return num1;
    }

    public int EnableWatchDogEx(WatchdogMonitoringTime TimeOut)
    {
      int num = -1;
      if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_DDI_EnableWatchDogEx(this.dtiHandle, (int) TimeOut);
      return num;
    }

    public int GetWatchDogState()
    {
      int num = -1;
      if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_DDI_GetWatchDogState(this.dtiHandle);
      return num;
    }

    public int ClearWatchDog()
    {
      int num = -1;
      if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_DDI_ClearWatchDog(this.dtiHandle);
      return num;
    }

    public unsafe int ETH_SetTimeout(ref int TimeOut)
    {
      int num1 = -1;
      int num2 = TimeOut;
      if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
      {
        num1 = IBS_G4Driver.DN_ETH_SetDTITimeoutCtrl(this.dtiHandle, &num2);
        TimeOut = num2;
      }
      return num1;
    }

    public int ETH_ClearTimeout()
    {
      int num = -1;
      if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_ETH_ClearDTITimeoutCtrl(this.dtiHandle);
      return num;
    }

    public int ETH_SetNetFail()
    {
      int num = -1;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_ETH_SetNetFail(this.mxiHandle);
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_ETH_SetNetFail(this.dtiHandle);
      return num;
    }

    public unsafe int ETH_GetNetFailStatus(out int State, out int Reason)
    {
      int num1 = -1;
      int num2 = 0;
      int num3 = 0;
      State = 0;
      Reason = 0;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
      {
        num1 = IBS_G4Driver.DN_ETH_GetNetFailStatus(this.mxiHandle, &num2, &num3);
        State = num2;
        Reason = num3;
      }
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
      {
        num1 = IBS_G4Driver.DN_ETH_GetNetFailStatus(this.dtiHandle, &num2, &num3);
        State = num2;
        Reason = num3;
      }
      return num1;
    }

    public int ETH_ClearNetFail()
    {
      int num = -1;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_ETH_ClrNetFailStatus(this.mxiHandle);
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_ETH_ClrNetFailStatus(this.dtiHandle);
      return num;
    }

    public int ETH_SetNetFailMode(ETH_NetFailModes Mode)
    {
      int num = -1;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_ETH_SetNetFailMode(this.mxiHandle, (int) Mode);
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
        num = IBS_G4Driver.DN_ETH_SetNetFailMode(this.dtiHandle, (int) Mode);
      return num;
    }

    public unsafe int ETH_GetNetFailMode(out ETH_NetFailModes Mode)
    {
      int num1 = -1;
      int num2 = 0;
      Mode = ETH_NetFailModes.ETH_NF_STD_MODE;
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
      {
        num1 = IBS_G4Driver.DN_ETH_GetNetFailMode(this.mxiHandle, &num2);
        Mode = (ETH_NetFailModes) num2;
      }
      else if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
      {
        num1 = IBS_G4Driver.DN_ETH_GetNetFailMode(this.dtiHandle, &num2);
        Mode = (ETH_NetFailModes) num2;
      }
      return num1;
    }

    public event UpdateIBSHandler OnIBSUpdate
    {
      add
      {
        this.evOnIBSCycle += value;
      }
      remove
      {
        this.evOnIBSCycle -= value;
      }
    }

    private void eventWorkerThread_IB()
    {
      do
      {
        if (IBS_G4Driver.WaitForSingleObject(this._EvtHdlIB, 100) != 258)
          this.OnIBEvent();
      }
      while (!this.threadExitFlag);
    }

    private void eventWorkerThread_TO()
    {
      do
      {
        if (IBS_G4Driver.WaitForSingleObject(this._EvtHdlTO, 2000) != 258)
          this.OnTOEvent();
      }
      while (!this.threadExitFlag);
    }

    public int StartCallbackEvent(PxCEventType eventType)
    {
      return this.StartCallbackEvent(eventType, 100);
    }

    public unsafe int StartCallbackEvent(PxCEventType eventType, int timeOutTime)
    {
      string str1 = "EVN_IB_" + this.mxiHandle.ToString("X");
      string str2 = "EVN_TO_" + this.mxiHandle.ToString("X");
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
      {
        int evtHdl_IB = IBS_G4Driver.CreateEvent((void*) null, false, false, str1.ToCharArray());
        int evtHdl_TO = IBS_G4Driver.CreateEvent((void*) null, false, false, str2.ToCharArray());
        this._EvtHdlIB = evtHdl_IB;
        this._EvtHdlTO = evtHdl_TO;
        this.thrHdlIB = new Thread(new ThreadStart(this.eventWorkerThread_IB));
        this.thrHdlIB.Start();
        this.thrHdlIB.Priority = ThreadPriority.Highest;
        this.thrHdlTO = new Thread(new ThreadStart(this.eventWorkerThread_TO));
        this.thrHdlTO.Start();
        this.thrHdlTO.Priority = ThreadPriority.Highest;
        this._createEvent = IBS_G4Driver.DN_DDI_CreateEvent(this.mxiHandle, (int) eventType, timeOutTime, evtHdl_IB, evtHdl_TO);
      }
      return this._createEvent;
    }

    private void OnIBEvent()
    {
      if (this.evOnIBSCycle == null)
        return;
      if (!this._glbEventDisableFlag)
        this.evOnIBSCycle(CallbackSource.IBS_Cycle);
      ++this._glbCounterTo10;
      if (this._glbCounterTo10 != 10)
        return;
      this._glbCounterTo10 = 0;
      this.GetCpuLoad();
    }

    private void OnTOEvent()
    {
      if (this.evOnIBSCycle != null && !this._glbEventDisableFlag)
        this.evOnIBSCycle(CallbackSource.IBS_Timeout);
      this.GetCpuLoad();
    }

    private unsafe void GetCpuLoad()
    {
      long num1;
      long num2;
      long num3;
      IBS_G4Driver.GetSystemTimes(&num1, &num2, &num3);
      long num4 = Convert.ToInt64(num1) - Convert.ToInt64(this.saveIdleTime);
      long num5 = Convert.ToInt64(num2) - Convert.ToInt64(this.saveKernelTime);
      long num6 = Convert.ToInt64(num3) - Convert.ToInt64(this.saveUserTime);
      this._glbCPULoad = (double) num4 + (double) num5 + (double) num6 == 0.0 ? 100.0 : (double) ((num5 + num6 - num4) * 100L / (num5 + num6));
      this._glbCPULoadEx += (this._glbCPULoad - this._glbCPULoadEx) / 100.0;
      if (this._glbCPULoadEx >= this._glbCPULoadDisableEvent)
        this._glbEventDisableFlag = true;
      this.saveIdleTime = num1;
      this.saveKernelTime = num2;
      this.saveUserTime = num3;
    }

    private void SetError(int DiagCode, int AddDiagCode)
    {
    }

    private void SetError(long ErrorCode)
    {
    }

    private string GetRightString(string String, int Count)
    {
      int length = String.Length;
      if (length >= Count)
        return String.Substring(length - Count, Count);
      return String;
    }

    public void Dispose()
    {
      if (this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
      {
        if (this._createEvent != 0)
          IBS_G4Driver.DN_DDI_DeleteEvent(this.mxiHandle);
        this.DDI_DevCloseNode(this.mxiHandle);
        this.mxiHandle = 0;
      }
      if (this.dtiHandle > 0 && this.dtiHandle <= (int) ushort.MaxValue)
      {
        this.DDI_DevCloseNode(this.dtiHandle);
        this.dtiHandle = 0;
      }
      this.threadExitFlag = true;
      Thread.Sleep(500);
      this.thrHdlIB = (Thread) null;
      this.thrHdlTO = (Thread) null;
      IBS_G4Driver.CloseHandle(this._EvtHdlIB);
      IBS_G4Driver.CloseHandle(this._EvtHdlTO);
      GC.Collect();
      Thread.Sleep(200);
    }

    public void Dispose(bool Alarmstop)
    {
      if (Alarmstop && this.mxiHandle > 0 && this.mxiHandle <= (int) ushort.MaxValue)
        this.SendMessage(0, 4, 0, new byte[4]
        {
          (byte) 19,
          (byte) 3,
          (byte) 0,
          (byte) 0
        });
      this.Dispose();
    }
  }
}
