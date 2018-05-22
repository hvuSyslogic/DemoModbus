// Decompiled with JetBrains decompiler
// Type: PhoenixContact.DDI.IBS_G4_Drv
// Assembly: IBSG4_Driver_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: BA38E233-77EA-4C5F-9C3F-E03C7CD687CE
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\IBSG4_Driver_FX46.dll

using PhoenixContact.Common.Ticker;
using PhoenixContact.PxC_Library.Util;
using System;
using System.Threading;

namespace PhoenixContact.DDI
{
  public class IBS_G4_Drv : ITick, IDisposable
  {
    private static ManualResetEvent mrEvent = new ManualResetEvent(false);
    private object syncObject = new object();
    private string _name = "";
    private ThreadPriority _tPriority = ThreadPriority.Normal;
    private int writeDataStartAddress = int.MaxValue;
    private bool _ready;
    private bool _error;
    private int _tIntervall;
    private TickerHandler _hdOnTick;
    private TickerHandler _hdOnStart;
    private TickerHandler _hdOnDisable;
    private TickerDiagnosticHandler _hdOnDiagnostic;
    private Diagnostic diagnostic;
    private PhoenixContact.Common.Ticker.Ticker ticker;
    private string _connectionDTI;
    private string _connectionMXI;
    private int _dtiHandle;
    private int _mxiHandle;
    private int writeDataLength;
    private bool _ibSync;
    private Thread thrHdlIB;
    private Thread thrHdlTO;
    private bool doWorker_IB;
    private bool doWorker_TO;
    private int evHandel_IB;
    private int evHandel_TO;
    private int hdCreateEvent;
    private TickerHandler hdTicker;
    private bool disposed;
    private IBS_G4_Drv.DriverState aktState;
    private IBS_G4_Drv.DriverState oldState;

    public IBS_G4_Drv(string Name)
    {
      this._name = Name != null ? Name : nameof (IBS_G4_Drv);
      this.diagnostic = new Diagnostic(this._name);
      this.diagnostic.OnException += new ExceptionHandler(this._diagnostic_OnException);
      this._connectionDTI = "";
      this._connectionMXI = "";
      this.ticker = TickerFactory.Create(Name, this._tIntervall, this._tPriority);
      this.hdTicker = new TickerHandler(this._ticker_OnTick);
      this.ChangeState(IBS_G4_Drv.DriverState.Idle);
    }

    private IBS_G4_Drv.DriverState GetState()
    {
      return this.aktState;
    }

    private IBS_G4_Drv.DriverState GetOldState()
    {
      return this.oldState;
    }

    private void ChangeState(IBS_G4_Drv.DriverState NewState)
    {
      if (this.aktState == NewState)
        return;
      this.oldState = this.aktState;
      this.aktState = NewState;
      switch (this.aktState)
      {
        case IBS_G4_Drv.DriverState.Inactive:
        case IBS_G4_Drv.DriverState.Idle:
        case IBS_G4_Drv.DriverState.Disable:
        case IBS_G4_Drv.DriverState.GoingToDispose:
          this._ready = false;
          this._error = false;
          break;
        case IBS_G4_Drv.DriverState.Start:
        case IBS_G4_Drv.DriverState.Active:
          this._ready = true;
          break;
        case IBS_G4_Drv.DriverState.ErrorDetect:
        case IBS_G4_Drv.DriverState.Error:
          this._error = true;
          this._ready = false;
          break;
      }
    }

    public string Name
    {
      get
      {
        return this._name;
      }
    }

    public bool Ready
    {
      get
      {
        return this._ready;
      }
    }

    public bool Error
    {
      get
      {
        return this._error;
      }
    }

    public IBS_G4_Drv.DriverState InternalState
    {
      get
      {
        return this.GetState();
      }
    }

    public int Intervall
    {
      get
      {
        return this._tIntervall;
      }
      set
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Idle && this.GetState() != IBS_G4_Drv.DriverState.Disable)
          return;
        this._tIntervall = value;
        this.ticker.Intervall = value;
      }
    }

    public ThreadPriority Priority
    {
      get
      {
        return this._tPriority;
      }
      set
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Idle && this.GetState() != IBS_G4_Drv.DriverState.Disable)
          return;
        this._tPriority = value;
        this.ticker.Priority = value;
      }
    }

    public string ConnectionDTI
    {
      get
      {
        return this._connectionDTI;
      }
      set
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Idle && this.GetState() != IBS_G4_Drv.DriverState.Disable)
          return;
        this._connectionDTI = value;
      }
    }

    public string ConnectionMXI
    {
      get
      {
        return this._connectionMXI;
      }
      set
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Idle && this.GetState() != IBS_G4_Drv.DriverState.Disable)
          return;
        this._connectionMXI = value;
      }
    }

    public int HandleDTI
    {
      get
      {
        return this._dtiHandle;
      }
    }

    public int HandleMXI
    {
      get
      {
        return this._mxiHandle;
      }
    }

    public bool IB_Sync
    {
      get
      {
        return this._ibSync;
      }
      set
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Idle && this.GetState() != IBS_G4_Drv.DriverState.Disable)
          return;
        this._ibSync = value;
      }
    }

    public bool Enable()
    {
      lock (this.syncObject)
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Idle)
        {
          if (this.GetState() != IBS_G4_Drv.DriverState.Disable)
            goto label_20;
        }
        this.ChangeState(IBS_G4_Drv.DriverState.GoingToEnable);
        this.diagnostic.Enable();
        if (this._connectionDTI.Length != 0)
        {
          int Integer = PhoenixContact.DDI.DDI.OpenNode(this._connectionDTI, out this._dtiHandle);
          if (Integer != 0)
          {
            this.diagnostic.Throw((Enum) IBS_G4_Drv.DiagnosticCodes.OpenNodeErrorDTI, IBS_G4_Drv.Int32ToByteArray(Integer, 2));
            return false;
          }
        }
        if (this._connectionMXI.Length != 0)
        {
          int Integer = PhoenixContact.DDI.DDI.OpenNode(this._connectionMXI, out this._mxiHandle);
          if (Integer != 0)
          {
            this.diagnostic.Throw((Enum) IBS_G4_Drv.DiagnosticCodes.OpenNodeErrorMXI, IBS_G4_Drv.Int32ToByteArray(Integer, 2));
            return false;
          }
        }
        if (this._ibSync)
        {
          if (this._mxiHandle > 0)
          {
            this.hdCreateEvent = this.StartCallbackEvent(257, 100);
          }
          else
          {
            this.diagnostic.Throw((Enum) IBS_G4_Drv.DiagnosticCodes.NoValidMailboxHandel, IBS_G4_Drv.Int32ToByteArray(this._mxiHandle, 2));
            return false;
          }
        }
        else if (this._dtiHandle > 0 || this._mxiHandle > 0)
        {
          this.ticker.OnTick += this.hdTicker;
          this.ticker.Enable();
        }
        else
        {
          this.diagnostic.Throw((Enum) IBS_G4_Drv.DiagnosticCodes.NoValidHandel, new byte[0]);
          return false;
        }
        this.ChangeState(IBS_G4_Drv.DriverState.Start);
        return true;
      }
label_20:
      return false;
    }

    public bool Disable()
    {
      lock (this.syncObject)
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Active && this.GetState() != IBS_G4_Drv.DriverState.Start && this.GetState() != IBS_G4_Drv.DriverState.Error)
        {
          if (this.GetState() != IBS_G4_Drv.DriverState.ErrorDetect)
            goto label_7;
        }
        this.ChangeState(IBS_G4_Drv.DriverState.GoingToDisable);
        new Thread(new ThreadStart(this.RunDisable))
        {
          Priority = ThreadPriority.Highest
        }.Start();
        return true;
      }
label_7:
      return false;
    }

    public bool Disable(bool Alarmstop)
    {
      if (Alarmstop && (this.GetState() == IBS_G4_Drv.DriverState.Active || this.GetState() == IBS_G4_Drv.DriverState.Start || this.GetState() == IBS_G4_Drv.DriverState.Error))
      {
        if (this._dtiHandle > 0)
          PhoenixContact.DDI.DDI.WriteData(this._dtiHandle, this.writeDataStartAddress, new byte[this.writeDataLength]);
        if (this._mxiHandle > 0)
          PhoenixContact.DDI.DDI.SendMessage(this._mxiHandle, 0, 4, 0, new byte[4]
          {
            (byte) 19,
            (byte) 3,
            (byte) 0,
            (byte) 0
          });
      }
      return this.Disable();
    }

    public bool DisableAsync()
    {
      return this.Disable();
    }

    public event TickerHandler OnEnable
    {
      add
      {
        this._hdOnStart += value;
      }
      remove
      {
        this._hdOnStart -= value;
      }
    }

    public event TickerHandler OnDisable
    {
      add
      {
        this._hdOnDisable += value;
      }
      remove
      {
        this._hdOnDisable -= value;
      }
    }

    public event TickerHandler OnTick
    {
      add
      {
        this._hdOnTick += value;
      }
      remove
      {
        this._hdOnTick -= value;
      }
    }

    public event TickerDiagnosticHandler OnDiagnostic
    {
      add
      {
        this._hdOnDiagnostic += value;
      }
      remove
      {
        this._hdOnDiagnostic -= value;
      }
    }

    private void _ticker_OnTick(object Sender)
    {
      this.OnUpdateTick();
    }

    private int StartCallbackEvent(int eventType, int timeOutTime)
    {
      if (this._mxiHandle <= 0)
        return 0;
      this.evHandel_IB = PhoenixContact.DDI.DDI.CreateNamedEvent("EVN_IB_" + this._mxiHandle.ToString("X"));
      this.evHandel_TO = PhoenixContact.DDI.DDI.CreateNamedEvent("EVN_TO_" + this._mxiHandle.ToString("X"));
      if (this.evHandel_IB > 0 && this.evHandel_TO > 0)
      {
        this.doWorker_IB = true;
        this.thrHdlIB = new Thread(new ThreadStart(this.eventWorkerThread_IB));
        this.thrHdlIB.Priority = ThreadPriority.Highest;
        this.thrHdlIB.Start();
        this.doWorker_TO = true;
        this.thrHdlTO = new Thread(new ThreadStart(this.eventWorkerThread_TO));
        this.thrHdlTO.Priority = ThreadPriority.Highest;
        this.thrHdlTO.Start();
        this.hdCreateEvent = PhoenixContact.DDI.DDI.CreateIB_Event(this._mxiHandle, eventType, timeOutTime, this.evHandel_IB, this.evHandel_TO);
      }
      return this.hdCreateEvent;
    }

    private void eventWorkerThread_IB()
    {
      do
      {
        try
        {
          if (this.evHandel_IB <= 0)
          {
            this.diagnostic.Throw((Enum) IBS_G4_Drv.DiagnosticCodes.NoValidEventHandleIB, IBS_G4_Drv.Int32ToByteArray(this.evHandel_IB, 2));
            this.thrHdlIB.Abort();
          }
          if (PhoenixContact.DDI.DDI.WaitForNamedEvent(this.evHandel_IB, 100))
            this.OnUpdateTick();
        }
        catch (ThreadAbortException ex)
        {
          Thread.ResetAbort();
          PhoenixContact.DDI.DDI.SetNamedEvent(this.evHandel_IB);
          this.doWorker_IB = false;
        }
      }
      while (this.doWorker_IB);
    }

    private void eventWorkerThread_TO()
    {
      do
      {
        try
        {
          if (this.evHandel_TO <= 0)
          {
            this.diagnostic.Throw((Enum) IBS_G4_Drv.DiagnosticCodes.NoValidEventHandleTO, IBS_G4_Drv.Int32ToByteArray(this.evHandel_TO, 2));
            this.thrHdlIB.Abort();
          }
          if (PhoenixContact.DDI.DDI.WaitForNamedEvent(this.evHandel_TO, 200))
            this.OnUpdateTick();
        }
        catch (ThreadAbortException ex)
        {
          Thread.ResetAbort();
          PhoenixContact.DDI.DDI.SetNamedEvent(this.evHandel_TO);
          this.doWorker_TO = false;
        }
      }
      while (this.doWorker_TO);
    }

    private void OnUpdateTick()
    {
      if (this.GetState() == IBS_G4_Drv.DriverState.Active)
      {
        this.CallEvent(this._hdOnTick);
      }
      else
      {
        if (this.GetState() != IBS_G4_Drv.DriverState.Start || !this.CallEvent(this._hdOnStart))
          return;
        this.ChangeState(IBS_G4_Drv.DriverState.Active);
      }
    }

    private void _diagnostic_OnException(Exception ExceptionData)
    {
      if (this.GetState() == IBS_G4_Drv.DriverState.ErrorDetect || this.GetState() == IBS_G4_Drv.DriverState.Error)
        return;
      this.ChangeState(IBS_G4_Drv.DriverState.ErrorDetect);
      if (this._hdOnDiagnostic == null)
        return;
      try
      {
        if (ExceptionData != null)
          this._hdOnDiagnostic(ExceptionData);
      }
      catch
      {
      }
      this.ChangeState(IBS_G4_Drv.DriverState.Error);
    }

    private void RunDisable()
    {
      if (this.IB_Sync)
      {
        if (this.thrHdlIB != null)
          this.thrHdlIB.Abort();
        if (this.thrHdlTO != null)
          this.thrHdlTO.Abort();
        if (this._mxiHandle > 0 && this.hdCreateEvent != 0)
        {
          PhoenixContact.DDI.DDI.DeleteIB_Event(this._mxiHandle);
          this.hdCreateEvent = 0;
        }
        if (this.evHandel_IB > 0)
        {
          PhoenixContact.DDI.DDI.CloseNamedEvent(this.evHandel_IB);
          this.evHandel_IB = 0;
        }
        if (this.evHandel_TO > 0)
        {
          PhoenixContact.DDI.DDI.CloseNamedEvent(this.evHandel_TO);
          this.evHandel_TO = 0;
        }
      }
      else if (this.ticker != null)
        this.ticker.Disable();
      Thread.Sleep(50);
      if (this._mxiHandle > 0)
      {
        PhoenixContact.DDI.DDI.CloseNode(this._mxiHandle);
        this._mxiHandle = 0;
      }
      Thread.Sleep(50);
      if (this._dtiHandle > 0)
      {
        PhoenixContact.DDI.DDI.CloseNode(this._dtiHandle);
        this._dtiHandle = 0;
      }
      Thread.Sleep(50);
      this.diagnostic.Disable();
      this.ChangeState(IBS_G4_Drv.DriverState.Disable);
      this.CallEvent(this._hdOnDisable);
    }

    ~IBS_G4_Drv()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        this.ChangeState(IBS_G4_Drv.DriverState.GoingToDispose);
        this.RunDispose();
      }
      this.disposed = true;
    }

    private void RunDispose()
    {
      if (this._mxiHandle > 0 && this.hdCreateEvent != 0)
      {
        PhoenixContact.DDI.DDI.DeleteIB_Event(this._mxiHandle);
        this.hdCreateEvent = 0;
      }
      if (this.evHandel_IB > 0)
      {
        PhoenixContact.DDI.DDI.SetNamedEvent(this.evHandel_IB);
        PhoenixContact.DDI.DDI.CloseNamedEvent(this.evHandel_IB);
        this.evHandel_IB = 0;
      }
      if (this.thrHdlIB != null)
        this.thrHdlIB.Abort();
      if (this.evHandel_TO > 0)
      {
        PhoenixContact.DDI.DDI.SetNamedEvent(this.evHandel_TO);
        PhoenixContact.DDI.DDI.CloseNamedEvent(this.evHandel_TO);
        this.evHandel_TO = 0;
      }
      if (this.thrHdlTO != null)
        this.thrHdlTO.Abort();
      if (this.ticker != null)
      {
        this.ticker.Dispose();
        this.ticker = (PhoenixContact.Common.Ticker.Ticker) null;
      }
      if (this._mxiHandle > 0)
      {
        PhoenixContact.DDI.DDI.CloseNode(this._mxiHandle);
        this._mxiHandle = 0;
      }
      if (this._dtiHandle > 0)
      {
        PhoenixContact.DDI.DDI.CloseNode(this._dtiHandle);
        this._dtiHandle = 0;
      }
      this.diagnostic.Dispose();
    }

    public int WriteData(int Address, byte[] Data)
    {
      if (!this._ready || this._dtiHandle <= 0)
        return -2;
      if (Address < this.writeDataStartAddress)
        this.writeDataStartAddress = Address;
      if (Data != null && Data.Length > this.writeDataLength)
        this.writeDataLength = Data.Length;
      return PhoenixContact.DDI.DDI.WriteData(this._dtiHandle, Address, Data);
    }

    public int ReadData(int Address, ref byte[] Data)
    {
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ReadData(this._dtiHandle, Address, ref Data);
      return -2;
    }

    public int SendMessage(int UserID, int Length, int MsgType, byte[] Message)
    {
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.SendMessage(this._mxiHandle, UserID, Length, MsgType, Message);
      return -2;
    }

    public int ReceiveMessage(out int UserID, out int Length, out int MsgType, ref byte[] Message)
    {
      UserID = 0;
      Length = 0;
      MsgType = 0;
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.ReceiveMessage(this._mxiHandle, out UserID, out Length, out MsgType, ref Message);
      return -2;
    }

    public VersionInfo GetVersionDDI(VersionInfoType InfoType)
    {
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.GetVersionDDI(this._mxiHandle, InfoType);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.GetVersionDDI(this._dtiHandle, InfoType);
      return PhoenixContact.DDI.DDI.GetVersionDDI(0, InfoType);
    }

    public string GetVersionDn2DDI()
    {
      return PhoenixContact.DDI.DDI.GetVersionDn2DDI();
    }

    public int GetDiagnostic(out int DiagState, out int DiagPara)
    {
      DiagState = 0;
      DiagPara = 0;
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.GetDiagnostic(this._mxiHandle, out DiagState, out DiagPara);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.GetDiagnostic(this._dtiHandle, out DiagState, out DiagPara);
      return -2;
    }

    public int GetDiagnosticEx(out int DiagState, out int DiagPara, out int DiagAddInfo)
    {
      DiagState = 0;
      DiagPara = 0;
      DiagAddInfo = 0;
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.GetDiagnosticEx(this._mxiHandle, out DiagState, out DiagPara, out DiagAddInfo);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.GetDiagnosticEx(this._dtiHandle, out DiagState, out DiagPara, out DiagAddInfo);
      return -2;
    }

    public int GetSlaveDiagnostic(out int DiagState, out int DiagPara, out int DiagAddInfo)
    {
      DiagState = 0;
      DiagPara = 0;
      DiagAddInfo = 0;
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.GetSlaveDiagnostic(this._mxiHandle, out DiagState, out DiagPara, out DiagAddInfo);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.GetSlaveDiagnostic(this._dtiHandle, out DiagState, out DiagPara, out DiagAddInfo);
      return -2;
    }

    public int EnableWatchDogEx(WatchdogMonitoringTime TimeOut)
    {
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.EnableWatchDogEx(this._dtiHandle, TimeOut);
      return -2;
    }

    public int GetWatchDogState()
    {
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.GetWatchDogState(this._dtiHandle);
      return -2;
    }

    public int ClearWatchDog()
    {
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ClearWatchDog(this._dtiHandle);
      return -2;
    }

    public int ETH_SetTimeout(ref int TimeOut)
    {
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_SetTimeout(this._dtiHandle, ref TimeOut);
      return -2;
    }

    public int ETH_ClearTimeout()
    {
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_ClearTimeout(this._dtiHandle);
      return -2;
    }

    public int ETH_SetNetFail()
    {
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_SetNetFail(this._dtiHandle);
      return -2;
    }

    public int ETH_GetNetFailStatus(out int State, out int Reason)
    {
      State = 0;
      Reason = 0;
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_GetNetFailStatus(this._mxiHandle, out State, out Reason);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.GetDiagnostic(this._dtiHandle, out State, out Reason);
      return -2;
    }

    public int ETH_ClearNetFail()
    {
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_ClearNetfail(this._mxiHandle);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_ClearNetfail(this._dtiHandle);
      return -2;
    }

    public int ETH_SetNetFailMode(ETH_NetFailModes Mode)
    {
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_SetNetFailMode(this._mxiHandle, Mode);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_SetNetFailMode(this._dtiHandle, Mode);
      return -2;
    }

    public int ETH_GetNetFailMode(out ETH_NetFailModes Mode)
    {
      Mode = ETH_NetFailModes.ETH_NF_STD_MODE;
      if (this._ready && this._mxiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_GetNetfailMode(this._mxiHandle, out Mode);
      if (this._ready && this._dtiHandle > 0)
        return PhoenixContact.DDI.DDI.ETH_GetNetfailMode(this._dtiHandle, out Mode);
      return -2;
    }

    private bool CallEvent(TickerHandler pEvent)
    {
      if (pEvent == null)
        return true;
      try
      {
        pEvent((object) this);
        return true;
      }
      catch (Exception ex)
      {
        this.diagnostic.Throw((Enum) IBS_G4_Drv.DiagnosticCodes.EventException, new byte[0], ex);
        return false;
      }
    }

    public static byte[] Int32ToByteArray(int Integer, int ByteArrayLength)
    {
      byte[] numArray = new byte[0];
      if (ByteArrayLength > 0 && ByteArrayLength <= 4)
      {
        numArray = new byte[ByteArrayLength];
        for (int index = 0; index < ByteArrayLength; ++index)
          numArray[ByteArrayLength - 1 - index] = Convert.ToByte(Integer >> index * 8 & (int) byte.MaxValue);
      }
      return numArray;
    }

    public enum DiagnosticCodes
    {
      Inactive = 0,
      NoError = 33536, // 0x00008300
      NotSupported = 49408, // 0x0000C100
      NoValidDataHandel = 49409, // 0x0000C101
      NoValidMailboxHandel = 49410, // 0x0000C102
      NoValidHandel = 49411, // 0x0000C103
      NoValidEventHandleIB = 49412, // 0x0000C104
      NoValidEventHandleTO = 49413, // 0x0000C105
      OpenNodeErrorDTI = 49424, // 0x0000C110
      OpenNodeErrorMXI = 49425, // 0x0000C111
      EventException = 49433, // 0x0000C119
    }

    public enum DriverState
    {
      Inactive,
      Idle,
      GoingToEnable,
      Start,
      Active,
      ErrorDetect,
      Error,
      GoingToDisable,
      Disable,
      GoingToDispose,
    }
  }
}
