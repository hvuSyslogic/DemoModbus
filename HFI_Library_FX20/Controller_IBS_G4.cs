// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Controller_IBS_G4
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using PhoenixContact.DDI;
using System;
using System.Collections;
using System.Threading;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public class Controller_IBS_G4 : IDisposable, IController, I_IBS_G4
  {
    private string _ctrlSvcFileName = "";
    private string _ctrlSvcLogFile = "";
    private int minPD_AddressInput = int.MaxValue;
    private int minPD_AddressOutput = int.MaxValue;
    private ArrayList _varInput = new ArrayList();
    private ArrayList _varOutput = new ArrayList();
    private ArrayList _messageObj = new ArrayList();
    private string _ctrlName;
    private string _ctrlDescription;
    private string _connectionDTI;
    private string _connectionMXI;
    private string _connection;
    private bool _ctrlActivate;
    private bool _ctrlReady;
    private bool _ctrlError;
    private ControllerStartup _crtlStartup;
    private int _ctrlUpdateTimeDTI;
    private int _ctrlUpdateTimeMXI;
    private int _mxiSleep;
    private bool _ctrlCpuLoadToHigh;
    private Diagnostic _locDiagnostic;
    private IBS_Diagnostic _ibsDiagnostic;
    private int _ibsDiagStatusReg;
    private int _ibsDiagStatusRegOld;
    private int _ibsDiagParaReg;
    private int _ibsDiagParaRegOld;
    private int _ibsDiagParaReg2;
    private bool _ibsRunFlag;
    private WatchdogMonitoringTime _watchdogTimeout;
    private bool _watchdogDeactivate;
    private bool _watchdogOccurred;
    private Controller_IBS_G4.Config _ctrlConfig;
    private int maxPD_AddressInput;
    private int maxPD_AddressOutput;
    private OnIBS_DiagChangeHandler _hd_OnIbsDiagnostic;
    private UpdateProcessDataHandler _te_OnUpdateIO;
    private UpdateMailboxHandler _um_OnUpdateMX;
    private IBS_G4Driver _ctrlClass;
    private ibsTicker _pduTimer;
    private ibsTicker _mxTimer;
    private ibsTicker _ctrlTimer;
    private InterbusHandling _locBusHandling;

    public Controller_IBS_G4(string Name)
    {
      this._locDiagnostic = new Diagnostic(Name);
      this._ibsDiagnostic = new IBS_Diagnostic();
      this._locBusHandling = new InterbusHandling(Name, (IController) this);
      this._locBusHandling.OnDiagnostic += new DiagnosticHandler(this._ctrlBus_OnDiagnostic);
      this._ctrlActivate = false;
      this._ctrlReady = false;
      this._ctrlError = false;
      this._ctrlName = Name;
      this._ctrlDescription = "";
      this._crtlStartup = ControllerStartup.PhysicalConfiguration;
      this._watchdogTimeout = WatchdogMonitoringTime.Intervall_524ms;
      this._ctrlConfig = new Controller_IBS_G4.Config();
      this.UpdateProcessDataCycleTime = 5;
      this.UpdateMailboxTime = 50;
    }

    public override string ToString()
    {
      return this._ctrlName;
    }

    public string VersionInfo
    {
      get
      {
        string str1 = "" + this.GetType().Assembly.FullName + "\r\n";
        string str2;
        if (this._ctrlClass != null)
        {
          string str3 = str1 + this._ctrlClass.GetType().Assembly.FullName + "\r\n";
          if (this._ctrlActivate)
          {
            string str4 = str3 + "dn2ddi.dll, Version " + this._ctrlClass.DN2DDI_GetVersion() + "\r\n";
            PhoenixContact.DDI.VersionInfo info = this._ctrlClass.GetInfo(VersionInfoCmd.DDI_Info);
            string str5 = str4 + info.Name + ", Revision " + info.Revision + "\r\n";
            info = this._ctrlClass.GetInfo(VersionInfoCmd.Driver_Info);
            str2 = str5 + info.Name + ", Revision " + info.Revision;
          }
          else
            str2 = str3 + "dn2ddi.dll, Version only in active controller state available\r\n" + "DDI, Version only in active controller state available\r\n" + "Driver, Version only in active controller state available\r\n";
        }
        else
          str2 = str1 + "IBS_G4_Driver, Version only in enabled controller available\r\n" + "DDI, Version only in active controller state available\r\n" + "Driver, Version only in active controller state available\r\n";
        return str2;
      }
    }

    public string Name
    {
      get
      {
        return this._ctrlName;
      }
      set
      {
        this._ctrlName = value;
      }
    }

    public string Description
    {
      get
      {
        return this._ctrlDescription;
      }
      set
      {
        this._ctrlDescription = value;
      }
    }

    public ControllerStartup Startup
    {
      get
      {
        return this._crtlStartup;
      }
      set
      {
        this._crtlStartup = value;
      }
    }

    public string SvcFileName
    {
      get
      {
        return this._ctrlSvcFileName;
      }
      set
      {
        this._ctrlSvcFileName = value;
      }
    }

    public string SvcLogFileName
    {
      get
      {
        return this._ctrlSvcLogFile;
      }
      set
      {
        this._ctrlSvcLogFile = value;
      }
    }

    public int UpdateProcessDataCycleTime
    {
      get
      {
        return this._ctrlUpdateTimeDTI;
      }
      set
      {
        this._ctrlUpdateTimeDTI = value;
      }
    }

    public int UpdateMailboxTime
    {
      get
      {
        return this._ctrlUpdateTimeMXI;
      }
      set
      {
        this._ctrlUpdateTimeMXI = value;
      }
    }

    public bool Ready
    {
      get
      {
        return this._ctrlReady;
      }
    }

    public bool Error
    {
      get
      {
        return this._ctrlError;
      }
    }

    public IBS_Diagnostic IBS_Diag
    {
      get
      {
        lock (this._ibsDiagnostic)
          return this._ibsDiagnostic;
      }
    }

    public ArrayList InputObjectList
    {
      get
      {
        return this._varInput;
      }
    }

    public ArrayList OutputObjectList
    {
      get
      {
        return this._varOutput;
      }
    }

    public ArrayList MessageObjectList
    {
      get
      {
        return this._messageObj;
      }
    }

    public Controller_IBS_G4.Config Configuration
    {
      get
      {
        return this._ctrlConfig;
      }
    }

    public bool WatchdogOccurred
    {
      get
      {
        return this._watchdogOccurred;
      }
    }

    public WatchdogMonitoringTime WatchdogTimeout
    {
      get
      {
        return this._watchdogTimeout;
      }
      set
      {
        this._watchdogTimeout = value;
      }
    }

    public bool WatchdogDeactivate
    {
      get
      {
        return this._watchdogDeactivate;
      }
      set
      {
        if (this._ctrlActivate)
          return;
        this._watchdogDeactivate = value;
      }
    }

    public string Connection
    {
      get
      {
        return this._connection;
      }
      set
      {
        this._connectionDTI = "IBB";
        this._connectionMXI = "IBB";
        int num;
        try
        {
          num = int.Parse(value);
        }
        catch (Exception ex)
        {
          num = 0;
        }
        this._connection = num.ToString();
        if (num > 0 && num < 11)
        {
          this._connectionDTI = this._connectionDTI + num.ToString() + "N1_D";
          this._connectionMXI = this._connectionMXI + num.ToString() + "N1_M";
        }
        else
        {
          this._connectionDTI = (string) null;
          this._connectionMXI = (string) null;
        }
      }
    }

    private bool WatchdogEnable()
    {
      if (this._watchdogDeactivate)
        return true;
      int Integer = this._ctrlClass.EnableWatchDogEx((PhoenixContact.DDI.WatchdogMonitoringTime) this._watchdogTimeout);
      if (Integer == 0)
        return true;
      this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ControllerStateCycleTimeOutOfRange, Util.Int32ToByteArray(Integer, 2));
      this.StopOnError();
      return false;
    }

    private bool WatchdogDisable()
    {
      int Integer = this._ctrlClass.ClearWatchDog();
      if (Integer != 0)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ControllerStateCycleTimeOutOfRange, Util.Int32ToByteArray(Integer, 2));
        this.StopOnError();
        return false;
      }
      this._ctrlError = false;
      return true;
    }

    public bool WatchdogClear()
    {
      if (!this._watchdogOccurred || !this.WatchdogDisable() || !this.WatchdogEnable())
        return false;
      this._watchdogOccurred = false;
      return true;
    }

    private bool WatchdogTrigger()
    {
      if (!this._watchdogDeactivate && !this._watchdogOccurred)
      {
        int watchDogState = this._ctrlClass.GetWatchDogState();
        switch (watchDogState)
        {
          case 0:
            this._watchdogOccurred = false;
            return true;
          case 1:
            if (!this._watchdogOccurred)
            {
              this._watchdogOccurred = true;
              this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.WatchdogOccurred, Util.Int32ToByteArray((int) this._watchdogTimeout, 2));
              break;
            }
            break;
          default:
            this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.GetWatchdogState, Util.Int32ToByteArray(watchDogState, 2));
            this.StopOnError();
            break;
        }
      }
      return false;
    }

    public event OnIBS_DiagChangeHandler OnIBS_DiagnosticChange
    {
      add
      {
        this._hd_OnIbsDiagnostic += value;
      }
      remove
      {
        this._hd_OnIbsDiagnostic -= value;
      }
    }

    public InterbusHandling Bus
    {
      get
      {
        return this._locBusHandling;
      }
      set
      {
        this._locBusHandling = value;
      }
    }

    public bool Enable()
    {
      if (this._ctrlActivate)
        return false;
      lock (this)
      {
        this._ctrlActivate = true;
        this._ctrlConfig.WriteValue = false;
        this._locDiagnostic.ErrorLoggingActivate = this._ctrlConfig.ErrLogActivate;
        if (this._ctrlConfig.ErrLogFilename.Length > 0)
          this._locDiagnostic.ErrorLoggingFileName = this._ctrlConfig.ErrLogFilename;
        this._locDiagnostic.Activate = true;
        this._ctrlClass = new IBS_G4Driver(this.Name);
        if (this._connectionDTI == null || this._connectionMXI == null)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.NoValidConnectionString, (byte[]) null);
          this.StopOnError();
          return false;
        }
        int Integer1 = this._ctrlClass.OpenNode(this._connectionDTI);
        if (Integer1 != 0)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.OpenNodeErrorDTI, Util.Int32ToByteArray(Integer1, 2));
          this.StopOnError();
          return false;
        }
        int Integer2 = this._ctrlClass.OpenNode(this._connectionMXI);
        if (Integer2 != 0)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.OpenNodeErrorMXI, Util.Int32ToByteArray(Integer2, 2));
          this.StopOnError();
          return false;
        }
        this._locBusHandling.ResetFirmwareService();
        this._locBusHandling.Activate = true;
        if (!this.StartUpdateMX())
          return false;
        if (this._ctrlConfig.GetRevisionInfo)
          this._locBusHandling.FetchRevisionInfo();
        this.WatchdogDisable();
        if (!this.WatchdogEnable())
        {
          this.StopOnError();
          return false;
        }
        if (!this.AutoStart() || !this.StartUpdatePD())
          return false;
        this._ctrlReady = true;
        this._ctrlError = false;
        return true;
      }
    }

    public void Disable()
    {
      if (!this._ctrlActivate)
        return;
      lock (this)
      {
        this._locDiagnostic.Activate = false;
        this.StopControllerUpdate();
        this.DeleteOutputBuffer();
        if (this._ctrlClass != null)
          this._ctrlClass.Dispose(true);
        if (this._pduTimer != null)
        {
          this._pduTimer.Dispose();
          this._pduTimer = (ibsTicker) null;
        }
        if (this._mxTimer != null)
        {
          this._mxTimer.Dispose();
          this._mxTimer = (ibsTicker) null;
        }
        if (this._ctrlTimer != null)
        {
          this._ctrlTimer.Dispose();
          this._ctrlTimer = (ibsTicker) null;
        }
        this._ctrlReady = false;
        this._ctrlError = false;
        this._ctrlActivate = false;
        this._locBusHandling.Activate = false;
        this._watchdogOccurred = false;
        this._ctrlConfig.WriteValue = true;
      }
    }

    private void StopOnError()
    {
      if (!this._ctrlActivate)
        return;
      this._ctrlError = true;
      this._ctrlReady = false;
      this._locDiagnostic.Activate = false;
      this.StopControllerUpdate();
    }

    private bool StartUpdateMX()
    {
      if (this._ctrlUpdateTimeMXI >= 10 && this._ctrlUpdateTimeMXI <= 1000)
      {
        this._mxTimer = new ibsTicker(this._ctrlUpdateTimeMXI, ThreadPriority.Highest);
        this._mxTimer.OnTimerTick += new IBSTickerHandler(this.UpdateMX);
        this._mxTimer.Enable = true;
        if (this._ctrlConfig.UpdateControllerState >= 10 && this._ctrlConfig.UpdateControllerState <= 1000)
        {
          this._ctrlTimer = new ibsTicker(this._ctrlConfig.UpdateControllerState, ThreadPriority.Highest);
          this._ctrlTimer.OnTimerTick += new IBSTickerHandler(this.UpdateCtrl);
          this._ctrlTimer.Enable = true;
          return true;
        }
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ControllerStateCycleTimeOutOfRange, Util.Int32ToByteArray(this._ctrlConfig.UpdateControllerState, 2));
        this.StopOnError();
        return false;
      }
      this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.MailboxDataCycleTimeOutOfRange, Util.Int32ToByteArray(this._ctrlUpdateTimeMXI, 2));
      this.StopOnError();
      return false;
    }

    private bool StartUpdatePD()
    {
      if (this._ctrlConfig.EnableIBS_Sync)
      {
        this._ctrlClass.OnIBSUpdate += new UpdateIBSHandler(this.UpdateProcessData);
        this._ctrlClass.StartCallbackEvent(PxCEventType.PxC_irq);
        this._ctrlClass.EventDisable = false;
      }
      else if (this._pduTimer == null)
      {
        if (this._ctrlUpdateTimeDTI >= 5 && this._ctrlUpdateTimeDTI <= 200)
        {
          this._pduTimer = new ibsTicker(this._ctrlUpdateTimeDTI, ThreadPriority.Highest);
          this._pduTimer.OnTimerTick += new IBSTickerHandler(this.UpdateProcessData);
          this._pduTimer.Enable = true;
        }
        else
        {
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ProcessDataCycleTimeOutOfRange, Util.Int32ToByteArray(this._ctrlUpdateTimeDTI, 2));
          this.StopOnError();
          return false;
        }
      }
      return true;
    }

    private void StopControllerUpdate()
    {
      if (!this._ctrlClass.EventDisable)
        this._ctrlClass.EventDisable = true;
      if (this._pduTimer != null)
        this._pduTimer.Enable = false;
      if (this._ctrlTimer != null)
        this._ctrlTimer.Enable = false;
      if (this._mxTimer != null)
        this._mxTimer.Enable = false;
      Thread.Sleep(this._ctrlUpdateTimeDTI + this._ctrlUpdateTimeMXI + this._ctrlConfig.UpdateControllerState);
    }

    public bool AutoStart()
    {
      if (this._ctrlConfig.EnableIBS_Sync)
        this._locBusHandling.SetIBS_Sync();
      switch (this._crtlStartup)
      {
        case ControllerStartup.PhysicalConfiguration:
          this._locBusHandling.AlarmStop();
          this._locBusHandling.CreateConfiguration();
          if (this._ctrlConfig.EnableIBS_Sync)
          {
            if (this._ctrlUpdateTimeDTI >= 5 && this._ctrlUpdateTimeDTI <= 99)
            {
              this._locBusHandling.SetIbsCycleTime(this._ctrlUpdateTimeDTI * 1000);
            }
            else
            {
              this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ProcessDataCycleTimeOutOfRange, Util.Int32ToByteArray(this._ctrlUpdateTimeDTI, 2));
              this.StopOnError();
              return false;
            }
          }
          this._locBusHandling.StartDataTransfer();
          break;
        case ControllerStartup.LogicalConfiguration:
          this._locBusHandling.AlarmStop();
          this._locBusHandling.ActivateConfiguration();
          if (this._ctrlConfig.EnableIBS_Sync)
          {
            if (this._ctrlUpdateTimeDTI >= 5 && this._ctrlUpdateTimeDTI <= 99)
            {
              this._locBusHandling.SetIbsCycleTime(this._ctrlUpdateTimeDTI * 1000);
            }
            else
            {
              this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ProcessDataCycleTimeOutOfRange, Util.Int32ToByteArray(this._ctrlConfig.UpdateControllerState, 2));
              this.StopOnError();
              return false;
            }
          }
          this._locBusHandling.StartDataTransfer();
          break;
        case ControllerStartup.SvcFileConfiguration:
          this._locBusHandling.AlarmStop();
          if (this._ctrlSvcFileName.Length > 0)
          {
            this._locBusHandling.SvcFileDownload(this._ctrlSvcFileName, this._ctrlSvcLogFile);
            break;
          }
          break;
      }
      return true;
    }

    private void UpdateCtrl(IBSTickerMode Mode)
    {
      if (Mode == IBSTickerMode.Active && this._ctrlActivate && !this._ctrlError)
      {
        this._ctrlConfig.ErrLogActivate = this._locDiagnostic.ErrorLoggingActivate;
        lock (this._ibsDiagnostic)
        {
          int diagnosticEx = this._ctrlClass.GetDiagnosticEx(out this._ibsDiagStatusReg, out this._ibsDiagParaReg, out this._ibsDiagParaReg2);
          if (diagnosticEx != 0)
          {
            this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.GetDiagnostic, Util.Int32ToByteArray(diagnosticEx, 2));
            this.StopOnError();
          }
          else
          {
            if (this._ibsDiagStatusReg == this._ibsDiagStatusRegOld)
            {
              if (this._ibsDiagParaReg == this._ibsDiagParaRegOld)
                goto label_10;
            }
            this._ibsDiagStatusRegOld = this._ibsDiagStatusReg;
            this._ibsDiagParaRegOld = this._ibsDiagParaReg;
            this._ibsDiagnostic.SetStatusRegister = this._ibsDiagStatusReg;
            this._ibsDiagnostic.SetParameterRegister = this._ibsDiagParaReg;
            this._ibsDiagnostic.SetExtendedParameterRegister = this._ibsDiagParaReg2;
            if (this._hd_OnIbsDiagnostic != null)
              this._hd_OnIbsDiagnostic((object) this, this._ibsDiagnostic);
          }
        }
label_10:
        if (this._ctrlConfig.ControlCPU_Load)
        {
          if (this._ctrlClass.CPU_LoadEx > 95 && !this._ctrlCpuLoadToHigh)
          {
            this._ctrlCpuLoadToHigh = true;
            this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.CpuLoadToHigh, Util.Int32ToByteArray(this._ctrlClass.CPU_LoadEx, 2));
          }
          else if (this._ctrlClass.CPU_LoadEx < 90 && this._ctrlCpuLoadToHigh)
          {
            this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.CpuLoadOk, Util.Int32ToByteArray(this._ctrlClass.CPU_LoadEx, 2));
            this._ctrlCpuLoadToHigh = false;
          }
        }
        if (this._ctrlConfig.Read_IBS_Cycletime)
        {
          if (this._ibsDiagnostic.StatusRegister.RUN)
          {
            if (!this._ibsRunFlag && !this._ibsDiagnostic.StatusRegister.DETECT)
            {
              this._ibsRunFlag = true;
              this._locBusHandling.FetchRealCycleTime();
            }
          }
          else
            this._ibsRunFlag = false;
        }
      }
      if (Mode != IBSTickerMode.Stop)
        return;
      this._ibsDiagnostic.SetStatusRegister = 0;
      this._ibsDiagnostic.SetParameterRegister = 0;
      this._ibsDiagnostic.SetExtendedParameterRegister = 0;
    }

    public int AddObject(VarInput InputObject)
    {
      if (this._ctrlActivate)
        return -10;
      if (InputObject == null)
        return -1;
      if (InputObject.BaseAddress < 0 || InputObject.VarType == VarType.Unknown)
        return -2;
      if (InputObject.BaseAddress < this.minPD_AddressInput)
        this.minPD_AddressInput = InputObject.BaseAddress;
      if (InputObject.BaseAddress + InputObject.ByteLength >= this.maxPD_AddressInput)
        this.maxPD_AddressInput = InputObject.BaseAddress + InputObject.ByteLength - 1;
      return this._varInput.Add((object) InputObject);
    }

    public bool RemoveObject(VarInput InputObject)
    {
      if (this._ctrlActivate || InputObject == null)
        return false;
      this._varInput.Remove((object) InputObject);
      return true;
    }

    public int AddObject(VarOutput OutputObject)
    {
      if (this._ctrlActivate)
        return -10;
      if (OutputObject == null)
        return -1;
      if (OutputObject.BaseAddress < 0 || OutputObject.VarType == VarType.Unknown)
        return -2;
      if (OutputObject.BaseAddress < this.minPD_AddressOutput)
        this.minPD_AddressOutput = OutputObject.BaseAddress;
      if (OutputObject.BaseAddress + OutputObject.ByteLength >= this.maxPD_AddressOutput)
        this.maxPD_AddressOutput = OutputObject.BaseAddress + OutputObject.ByteLength - 1;
      return this._varOutput.Add((object) OutputObject);
    }

    public bool RemoveObject(VarOutput OutputObject)
    {
      if (this._ctrlActivate || OutputObject == null)
        return false;
      this._varOutput.Remove((object) OutputObject);
      return true;
    }

    public int InputObjectCount
    {
      get
      {
        return this._varInput.Count;
      }
    }

    public int InputObjectStartAddress
    {
      get
      {
        return this.minPD_AddressInput;
      }
    }

    public int InputObjectEndAddress
    {
      get
      {
        return this.maxPD_AddressInput;
      }
    }

    public int InputObjectLength
    {
      get
      {
        return this.maxPD_AddressInput - this.minPD_AddressInput + 1;
      }
    }

    public int OutputObjectCount
    {
      get
      {
        return this._varOutput.Count;
      }
    }

    public int OutputObjectStartAddress
    {
      get
      {
        return this.minPD_AddressOutput;
      }
    }

    public int OutputObjectEndAddress
    {
      get
      {
        return this.maxPD_AddressOutput;
      }
    }

    public int OutputObjectLength
    {
      get
      {
        return this.maxPD_AddressOutput - this.minPD_AddressOutput + 1;
      }
    }

    private void UpdateProcessData(CallbackSource State)
    {
      if (!this._ctrlActivate || this._ctrlError)
        return;
      this.UpdateInputs(this.minPD_AddressInput, this.InputObjectLength);
      this.WatchdogTrigger();
      if (this._te_OnUpdateIO != null)
        this._te_OnUpdateIO((object) this);
      this.UpdateOutputs(this.minPD_AddressOutput, this.OutputObjectLength);
    }

    private void UpdateProcessData(IBSTickerMode TickerMode)
    {
      if (TickerMode == IBSTickerMode.Active && this._ctrlActivate && !this._ctrlError)
      {
        this.UpdateInputs(this.minPD_AddressInput, this.InputObjectLength);
        this.WatchdogTrigger();
        if (this._te_OnUpdateIO != null)
          this._te_OnUpdateIO((object) this);
        this.UpdateOutputs(this.minPD_AddressOutput, this.OutputObjectLength);
      }
      if (TickerMode != IBSTickerMode.Stop)
        return;
      this.DeleteOutputBuffer();
    }

    private void UpdateInputs(int StartAddress, int Length)
    {
      if (this._varInput.Count <= 0)
        return;
      if (StartAddress < 0 || StartAddress + Length - 1 > this.InputObjectEndAddress)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.UpdateInputBlockStartAddress, Util.Int32ToByteArray(StartAddress, 2));
        this.StopOnError();
      }
      else if (Length < 1 || Length - 1 > this.InputObjectEndAddress)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.UpdateInputBlockLength, Util.Int32ToByteArray(Length, 2));
        this.StopOnError();
      }
      else
      {
        lock (this._varInput)
        {
          byte[] Data = new byte[Length];
          this._ctrlClass.ReadData(StartAddress, ref Data);
          foreach (VarInput varInput in this._varInput)
          {
            if (varInput.BaseAddress >= StartAddress && varInput.BaseAddress < StartAddress + Length)
            {
              if (varInput.VarType == VarType.Boolean)
                varInput.SetValue = this.GetDataFromBuffer(Data, varInput.BaseAddress - StartAddress, varInput.ByteLength, varInput.BitOffset);
              if (varInput.VarType == VarType.UInt63)
                varInput.SetValue = this.GetDataFromBuffer(Data, varInput.BaseAddress - StartAddress, varInput.ByteLength, varInput.BitOffset, varInput.MaxValue);
              if (varInput.VarType == VarType.ByteArray)
                varInput.SetByteArray = this.GetByteFromBuffer(Data, varInput.BaseAddress - StartAddress, varInput.ByteLength);
            }
          }
        }
      }
    }

    public event UpdateProcessDataHandler OnUpdateProcessData
    {
      add
      {
        this._te_OnUpdateIO += value;
      }
      remove
      {
        this._te_OnUpdateIO -= value;
      }
    }

    private void UpdateOutputs(int StartAddress, int Length)
    {
      if (this._varOutput.Count <= 0)
        return;
      if (StartAddress < 0 || StartAddress + Length - 1 > this.OutputObjectEndAddress)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.UpdateOutputBlockStartAddress, Util.Int32ToByteArray(StartAddress, 2));
        this.StopOnError();
      }
      else if (Length < 1 || Length - 1 > this.OutputObjectEndAddress)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.UpdateOutputBlockLength, Util.Int32ToByteArray(Length, 2));
        this.StopOnError();
      }
      else
      {
        lock (this._varOutput)
        {
          byte[] Data = new byte[Length];
          foreach (VarOutput varOutput in this._varOutput)
          {
            if (varOutput.BaseAddress >= StartAddress && varOutput.BaseAddress < StartAddress + Length)
            {
              if (varOutput.VarType == VarType.Boolean)
                this.PutDataToBuffer(varOutput.GetValue, ref Data, varOutput.BaseAddress - StartAddress, varOutput.ByteLength, varOutput.BitOffset);
              if (varOutput.VarType == VarType.UInt63)
                this.PutDataToBuffer(varOutput.GetValue, ref Data, varOutput.BaseAddress - StartAddress, varOutput.ByteLength, varOutput.BitOffset, varOutput.MaxValue);
              if (varOutput.VarType == VarType.ByteArray)
                this.PutByteToBuffer(varOutput.ByteArray, ref Data, varOutput.BaseAddress - StartAddress);
            }
          }
          this._ctrlClass.WriteData(StartAddress, Data);
        }
      }
    }

    private long GetDataFromBuffer(byte[] Data, int Address, int ByteLength, int BitOffset, long MaxValue)
    {
      long num = 0;
      for (int index = 0; index < ByteLength; ++index)
      {
        num |= (long) Data[Address + index];
        if (index < ByteLength - 1)
          num <<= 8;
      }
      if (BitOffset > 0)
        num >>= BitOffset;
      return num & MaxValue;
    }

    private long GetDataFromBuffer(byte[] Data, int Address, int ByteLength, int BitOffset)
    {
      long num = 0;
      for (int index = 0; index < ByteLength; ++index)
      {
        num |= (long) Data[Address + index];
        if (index < ByteLength - 1)
          num <<= 8;
      }
      return num >> BitOffset & 1L;
    }

    private byte[] GetByteFromBuffer(byte[] Data, int Address, int Length)
    {
      byte[] numArray = new byte[0];
      if (Data != null && Address >= 0 && Length > 0)
      {
        numArray = new byte[Length];
        for (int index = 0; index < Length; ++index)
          numArray[index] = Data[Address + index];
      }
      else
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ParaErrGetByteFromBuffer, (byte[]) null);
        this.StopOnError();
      }
      return numArray;
    }

    private bool PutDataToBuffer(long Variable, ref byte[] Data, int Address, int ByteLength, int BitOffset, long MaxValue)
    {
      if (BitOffset > 0)
      {
        Variable = (Variable & MaxValue) << BitOffset;
        MaxValue <<= BitOffset;
      }
      for (int index = ByteLength; index > 0; --index)
      {
        byte num1 = Convert.ToByte(Variable & (long) byte.MaxValue);
        byte num2 = Convert.ToByte(~MaxValue & (long) byte.MaxValue);
        Data[Address + (index - 1)] = Convert.ToByte((int) Data[Address + (index - 1)] & (int) num2 | (int) num1);
        Variable >>= 8;
        MaxValue >>= 8;
      }
      return true;
    }

    private void PutDataToBuffer(long Variable, ref byte[] Data, int Address, int ByteLength, int BitOffset)
    {
      long num1 = 1;
      if (BitOffset > 0)
        num1 <<= BitOffset;
      for (int index = ByteLength; index > 0; --index)
      {
        byte num2 = Convert.ToByte(num1 & (long) byte.MaxValue);
        byte num3 = Convert.ToByte(~num1 & (long) byte.MaxValue);
        Data[Address + (index - 1)] = Variable == 0L ? Convert.ToByte((int) Data[Address + (index - 1)] & (int) num3) : Convert.ToByte((int) Data[Address + (index - 1)] & (int) num3 | (int) num2);
        num1 >>= 8;
      }
    }

    private void PutByteToBuffer(byte[] ByteArray, ref byte[] Data, int Address)
    {
      if (ByteArray != null && Data != null && Address >= 0)
      {
        for (int index = 0; index < ByteArray.Length; ++index)
          Data[Address + index] = ByteArray[index];
      }
      else
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ParaErrPutByteToBuffer, (byte[]) null);
        this.StopOnError();
      }
    }

    private void DeleteOutputBuffer()
    {
      if (this.maxPD_AddressOutput <= 0 || this.OutputObjectLength <= 0)
        return;
      byte[] Data = new byte[this.OutputObjectLength];
      lock (this._varOutput)
        this._ctrlClass.WriteData(this.minPD_AddressOutput, Data);
    }

    public int AddObject(MessageClient MessageObject)
    {
      int num = -1;
      lock (this._messageObj)
      {
        if (MessageObject != null)
        {
          MessageObject.SetActiv();
          num = this._messageObj.Add((object) MessageObject);
        }
        else
          num = -1;
      }
      return num;
    }

    public bool RemoveObject(MessageClient MessageObject)
    {
      bool flag = false;
      lock (this._messageObj)
      {
        if (MessageObject != null)
        {
          if (MessageObject.State != MessageClientState.SendRequestOnlyDone && MessageObject.State != MessageClientState.ConfirmationReceived && MessageObject.State != MessageClientState.Error)
          {
            if (MessageObject.State != MessageClientState.Idle)
              goto label_6;
          }
          this._messageObj.Remove((object) MessageObject);
          MessageObject.SetInactiv();
          flag = true;
        }
      }
label_6:
      return flag;
    }

    private void UpdateMX(IBSTickerMode Mode)
    {
      if (Mode == IBSTickerMode.Active && this._ctrlActivate && !this._ctrlError)
      {
        this.UpdateReceiveMXI();
        this._locBusHandling.Run();
        if (this._um_OnUpdateMX != null)
          this._um_OnUpdateMX((object) this);
        this.UpdateSendMXI();
      }
    }

    private void UpdateReceiveMXI()
    {
      int Length = 0;
      if (this._messageObj.Count <= 0)
        return;
      lock (this._messageObj)
      {
        byte[] Message = new byte[1024];
        int UserID;
        int MsgType;
        int Integer = this._ctrlClass.ReceiveMessage(out UserID, out Length, out MsgType, ref Message) & (int) ushort.MaxValue;
        switch (Integer)
        {
          case 0:
          case 155:
            foreach (MessageClient messageClient in this._messageObj)
            {
              if ((Integer & (int) ushort.MaxValue) == 0)
              {
                if (messageClient.ID == UserID)
                {
                  if (messageClient._State == MessageClientState.WaitingForConfirmation)
                  {
                    messageClient._ReceiveData = new byte[Length];
                    for (int index = 0; index < Length; ++index)
                      messageClient._ReceiveData[index] = Message[index];
                    messageClient._State = MessageClientState.ConfirmationReceived;
                    Integer = 155;
                    messageClient.CallConfirmationReceivedEvent((object) this);
                  }
                  else
                  {
                    messageClient.SetDiagnostic(MessageClientDiagnostic.UnexpectedService, Util.GetByteArrayFromService(Message));
                    Integer = 155;
                  }
                }
                else if (((int) Message[0] & 64) != 0)
                {
                  if (this.Configuration.EnableIBS_Indications)
                    this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ControllerIndication, Util.GetByteArrayFromService(Message));
                  Integer = 155;
                }
              }
              else if (messageClient.State == MessageClientState.WaitingForConfirmation && DateTime.Now > messageClient.EstimatedReceiveDataTime)
                messageClient.SetDiagnostic(MessageClientDiagnostic.ConfirmationTimeout, (byte[]) null);
            }
            if (Integer != 0)
              break;
            this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.NoValidMessageObject, Util.GetByteArrayFromService(Message));
            break;
          default:
            this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.RetErrReceiveMessage, Util.Int32ToByteArray(Integer, 2));
            this.StopOnError();
            break;
        }
      }
    }

    public event UpdateMailboxHandler OnUpdateMailbox
    {
      add
      {
        this._um_OnUpdateMX += value;
      }
      remove
      {
        this._um_OnUpdateMX -= value;
      }
    }

    private void UpdateSendMXI()
    {
      if (this._messageObj.Count <= 0)
        return;
      lock (this._messageObj)
      {
        foreach (MessageClient messageClient in this._messageObj)
        {
          if (messageClient.State == MessageClientState.SendRequest || messageClient.State == MessageClientState.SendRequestOnly)
          {
            if (messageClient.SendDataLength > 0 && messageClient.SendData != null)
            {
              int Integer = this._ctrlClass.SendMessage(messageClient.ID, messageClient.SendDataLength, 0, messageClient.SendData) & (int) ushort.MaxValue;
              switch (Integer)
              {
                case 0:
                  if (messageClient.State == MessageClientState.SendRequest)
                  {
                    messageClient.ClearReceiveData();
                    messageClient._State = MessageClientState.WaitingForConfirmation;
                    messageClient.SendDataTime = DateTime.Now;
                  }
                  if (messageClient.State == MessageClientState.SendRequestOnly)
                  {
                    messageClient.ClearReceiveData();
                    messageClient._State = MessageClientState.SendRequestOnlyDone;
                    break;
                  }
                  break;
                case 156:
                case 157:
                  ++this._mxiSleep;
                  break;
                default:
                  messageClient.ClearReceiveData();
                  messageClient._State = MessageClientState.Error;
                  this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.RetErrSendMessage, Util.Int32ToByteArray(Integer, 2));
                  break;
              }
              Thread.Sleep(this._mxiSleep);
            }
            else
              this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ParaErrMessageClient, (byte[]) null);
          }
        }
      }
    }

    public event DiagnosticHandler OnDiagnostic
    {
      add
      {
        this._locDiagnostic.OnDiagnostic += value;
      }
      remove
      {
        this._locDiagnostic.OnDiagnostic -= value;
      }
    }

    private void _ctrlBus_OnDiagnostic(object Sender, DiagnosticArgs DiagnosticCode)
    {
      this._locDiagnostic.SetDiagnostic(Sender, DiagnosticCode);
    }

    public void Dispose()
    {
      this.Disable();
      if (this._locDiagnostic != null)
      {
        this._locDiagnostic.Dispose();
        this._locDiagnostic = (Diagnostic) null;
      }
      if (this._locBusHandling != null)
      {
        this._locBusHandling.Dispose();
        this._locBusHandling = (InterbusHandling) null;
      }
      this._varInput = (ArrayList) null;
      this._varOutput = (ArrayList) null;
      this._messageObj = (ArrayList) null;
      GC.SuppressFinalize((object) this);
    }

    public class Config
    {
      private bool _writeValues;
      private bool _enableIBS_Sync;
      private bool _enableIndications;
      private bool _read_IBS_Cycletime;
      private bool _controlCPU_Load;
      private bool _getRevisionInfo;
      private int _updateCotrollerState;
      private bool _errLoggingActivate;
      private string _errLoggingFilename;

      public Config()
      {
        this._writeValues = true;
        this._enableIBS_Sync = true;
        this._controlCPU_Load = true;
        this._enableIndications = false;
        this._updateCotrollerState = 50;
        this._read_IBS_Cycletime = true;
        this._getRevisionInfo = true;
        this._errLoggingActivate = false;
        this._errLoggingFilename = "";
      }

      internal bool WriteValue
      {
        get
        {
          return this._writeValues;
        }
        set
        {
          this._writeValues = value;
        }
      }

      public bool EnableIBS_Sync
      {
        get
        {
          return this._enableIBS_Sync;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._enableIBS_Sync = value;
        }
      }

      public bool ControlCPU_Load
      {
        get
        {
          return this._controlCPU_Load;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._controlCPU_Load = value;
        }
      }

      public bool EnableIBS_Indications
      {
        get
        {
          return this._enableIndications;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._enableIndications = value;
        }
      }

      public int UpdateControllerState
      {
        get
        {
          return this._updateCotrollerState;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._updateCotrollerState = value;
        }
      }

      public bool Read_IBS_Cycletime
      {
        get
        {
          return this._read_IBS_Cycletime;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._read_IBS_Cycletime = value;
        }
      }

      public bool GetRevisionInfo
      {
        get
        {
          return this._getRevisionInfo;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._getRevisionInfo = value;
        }
      }

      public bool ErrLogActivate
      {
        get
        {
          return this._errLoggingActivate;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._errLoggingActivate = value;
        }
      }

      public string ErrLogFilename
      {
        get
        {
          return this._errLoggingFilename;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._errLoggingFilename = value;
        }
      }
    }
  }
}
