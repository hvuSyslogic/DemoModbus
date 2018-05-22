// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Controller_ILB_ETH
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using PhoenixContact.DDI;
using System;
using System.Collections;
using System.Net;
using System.Threading;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public class Controller_ILB_ETH : IDisposable, IController
  {
    private int minPD_AddressInput = int.MaxValue;
    private int minPD_AddressOutput = int.MaxValue;
    private ArrayList _varInput = new ArrayList();
    private ArrayList _varOutput = new ArrayList();
    private string _ctrlName;
    private string _ctrlDescription;
    private string _connectionDTI;
    private string _connection;
    private bool _ctrlActivate;
    private bool _ctrlReady;
    private bool _ctrlError;
    private ControllerStartup _crtlStartup;
    private int _ctrlUpdateTimeDTI;
    private bool _ctrlCpuLoadToHigh;
    private Diagnostic _locDiagnostic;
    private int _ilbIODiagStatusReg;
    private int _ilbIODiagStatusRegOld;
    private WatchdogMonitoringTime _watchdogTimeout;
    private int _netfailTimeout;
    private bool _netfailDeactivate;
    private bool _netfailOccurred;
    private Controller_ILB_ETH.Config _ctrlConfig;
    private int maxPD_AddressInput;
    private int maxPD_AddressOutput;
    private UpdateProcessDataHandler _te_OnUpdateIO;
    private IBS_G4Driver _ctrlClass;
    private ibsTicker _pduTimer;
    private ibsTicker _ctrlTimer;

    public Controller_ILB_ETH(string Name)
    {
      this._locDiagnostic = new Diagnostic(Name);
      this._ctrlActivate = false;
      this._ctrlReady = false;
      this._ctrlError = false;
      this._ctrlName = Name;
      this._ctrlDescription = "";
      this._crtlStartup = ControllerStartup.PhysicalConfiguration;
      this._watchdogTimeout = WatchdogMonitoringTime.Intervall_524ms;
      this._ctrlConfig = new Controller_ILB_ETH.Config();
      this.UpdateProcessDataCycleTime = 20;
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
      }
    }

    public string SvcFileName
    {
      get
      {
        return "";
      }
      set
      {
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
        return 0;
      }
      set
      {
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
        return (ArrayList) null;
      }
    }

    public Controller_ILB_ETH.Config Configuration
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
        return this._netfailOccurred;
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
        return this._netfailDeactivate;
      }
      set
      {
        if (this._ctrlActivate)
          return;
        this._netfailDeactivate = value;
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
        try
        {
          this._connection = !this._ctrlConfig.DNS_NameResolution ? IPAddress.Parse(value).ToString() : Dns.GetHostEntry(value).AddressList[0].ToString();
        }
        catch
        {
          this._connection = "";
          this._connectionDTI = (string) null;
          return;
        }
        if (this._connection.Length > 0)
        {
          this._connectionDTI = "IBETHIP[" + this._connection + "]N1_D";
        }
        else
        {
          this._connection = "";
          this._connectionDTI = (string) null;
        }
      }
    }

    private bool WatchdogEnable()
    {
      Thread.Sleep(10);
      if (this._netfailDeactivate)
        return true;
      lock (this)
      {
        int Integer = this._ctrlClass.ETH_SetNetFailMode(ETH_NetFailModes.ETH_NF_ALARMSTOP_MODE);
        if (Integer != 0)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.EnableNetfail, Util.Int32ToByteArray(Integer, 2));
          this.StopOnError();
          return false;
        }
      }
      Thread.Sleep(10);
      lock (this)
      {
        switch (this._watchdogTimeout)
        {
          case WatchdogMonitoringTime.Intervall_524ms:
            this._netfailTimeout = 524;
            break;
          case WatchdogMonitoringTime.Intervall_1048ms:
            this._netfailTimeout = 1048;
            break;
        }
        int Integer = this._ctrlClass.ETH_SetTimeout(ref this._netfailTimeout);
        if (Integer == 0)
          return true;
        this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.EnableNetfail, Util.Int32ToByteArray(Integer, 2));
        this.StopOnError();
        return false;
      }
    }

    private bool PdWatchdogClear()
    {
      byte[] Data = new byte[2];
      Thread.Sleep(10);
      lock (this)
      {
        int Integer = this._ctrlClass.WriteData(4000, Data);
        if (Integer != 0)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.RetErrWriteData, Util.Int32ToByteArray(Integer, 2));
          this.StopOnError();
          return false;
        }
        this._ctrlError = false;
        return true;
      }
    }

    private bool NetfailClear()
    {
      Thread.Sleep(10);
      lock (this)
      {
        int Integer = this._ctrlClass.ETH_ClearNetFail();
        if (Integer != 0)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.ClearNetfail, Util.Int32ToByteArray(Integer, 2));
          this.StopOnError();
          return false;
        }
        this._ctrlError = false;
        return true;
      }
    }

    public bool WatchdogClear()
    {
      this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.NotSupported, Util.Int32ToByteArray(0, 2));
      return false;
    }

    private bool CheckWatchdogState()
    {
      if (!this._netfailDeactivate && !this._netfailOccurred)
      {
        lock (this)
        {
          int State;
          int Reason;
          int netFailStatus = this._ctrlClass.ETH_GetNetFailStatus(out State, out Reason);
          if (netFailStatus == 0)
          {
            if (State == 0)
              this._netfailOccurred = false;
            if (State == (int) ushort.MaxValue)
            {
              this._netfailOccurred = true;
              this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.NetfailOccurred, Util.Int32ToByteArray(Reason, 2));
              this.StopOnError();
            }
            return true;
          }
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.GetNetfailState, Util.Int32ToByteArray(netFailStatus, 2));
          this.StopOnError();
        }
      }
      return false;
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
        if (this._connectionDTI == null)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.NoValidConnectionString, (byte[]) null);
          this.StopOnError();
          return false;
        }
        int Integer = this._ctrlClass.OpenNode(this._connectionDTI);
        if (Integer != 0)
        {
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.OpenNodeErrorDTI, Util.Int32ToByteArray(Integer, 2));
          this.StopOnError();
          return false;
        }
        if (!this.StartUpdateMX())
          return false;
        this.NetfailClear();
        if (!this.WatchdogEnable())
        {
          this.StopOnError();
          return false;
        }
        if (!this.StartUpdatePD())
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
        if (this._ctrlClass != null)
          this._ctrlClass.Dispose(true);
        if (this._pduTimer != null)
        {
          this._pduTimer.Dispose();
          this._pduTimer = (ibsTicker) null;
        }
        if (this._ctrlTimer != null)
        {
          this._ctrlTimer.Dispose();
          this._ctrlTimer = (ibsTicker) null;
        }
        this._ctrlReady = false;
        this._ctrlError = false;
        this._ctrlActivate = false;
        this._netfailOccurred = false;
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
      if (this._ctrlConfig.UpdateControllerState >= 10 && this._ctrlConfig.UpdateControllerState <= 1000)
      {
        this._ctrlTimer = new ibsTicker(this._ctrlConfig.UpdateControllerState, ThreadPriority.Highest);
        this._ctrlTimer.OnTimerTick += new IBSTickerHandler(this.UpdateCtrl);
        this._ctrlTimer.Enable = true;
        return true;
      }
      this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.ControllerStateCycleTimeOutOfRange, Util.Int32ToByteArray(this._ctrlConfig.UpdateControllerState, 2));
      this.StopOnError();
      return false;
    }

    private bool StartUpdatePD()
    {
      if (this._pduTimer == null)
      {
        if (this._ctrlUpdateTimeDTI >= 5 && this._ctrlUpdateTimeDTI <= 200)
        {
          this._pduTimer = new ibsTicker(this._ctrlUpdateTimeDTI, ThreadPriority.Highest);
          this._pduTimer.OnTimerTick += new IBSTickerHandler(this.UpdateProcessData);
          this._pduTimer.Enable = true;
        }
        else
        {
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.ProcessDataCycleTimeOutOfRange, Util.Int32ToByteArray(this._ctrlUpdateTimeDTI, 2));
          this.StopOnError();
          return false;
        }
      }
      return true;
    }

    private void StopControllerUpdate()
    {
      if (this._pduTimer != null)
        this._pduTimer.Enable = false;
      if (this._ctrlTimer != null)
        this._ctrlTimer.Enable = false;
      Thread.Sleep(this._ctrlUpdateTimeDTI + this._ctrlConfig.UpdateControllerState);
    }

    public bool AutoStart()
    {
      this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.NotSupported, Util.Int32ToByteArray(0, 2));
      return false;
    }

    private void UpdateCtrl(IBSTickerMode Mode)
    {
      byte[] Data = new byte[2];
      if (Mode == IBSTickerMode.Active && this._ctrlActivate && !this._ctrlError)
      {
        this._ctrlConfig.ErrLogActivate = this._locDiagnostic.ErrorLoggingActivate;
        lock (this)
        {
          int Integer = this._ctrlClass.ReadData(10, ref Data);
          if (Integer != 0)
          {
            this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.GetDiagnostic, Util.Int32ToByteArray(Integer, 2));
            this.StopOnError();
          }
          this._ilbIODiagStatusReg = Util.ByteToInt32(Data[0], Data[1]);
          if (this._ilbIODiagStatusReg > 0)
          {
            if (this._ilbIODiagStatusReg != this._ilbIODiagStatusRegOld)
            {
              this._ilbIODiagStatusRegOld = this._ilbIODiagStatusReg;
              this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.IODiagnosticError, Util.Int32ToByteArray(this._ilbIODiagStatusReg, 2));
            }
          }
          else
            this._ilbIODiagStatusRegOld = 0;
        }
        if (this._ctrlConfig.ControlCPU_Load)
        {
          if (this._ctrlClass.CPU_LoadEx > 95 && !this._ctrlCpuLoadToHigh)
          {
            this._ctrlCpuLoadToHigh = true;
            this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.CpuLoadToHigh, Util.Int32ToByteArray(this._ctrlClass.CPU_LoadEx, 2));
          }
          else if (this._ctrlClass.CPU_LoadEx < 90 && this._ctrlCpuLoadToHigh)
          {
            this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.CpuLoadOk, Util.Int32ToByteArray(this._ctrlClass.CPU_LoadEx, 2));
            this._ctrlCpuLoadToHigh = false;
          }
        }
      }
      if (Mode != IBSTickerMode.Stop)
        return;
      this._ilbIODiagStatusReg = 0;
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

    private void UpdateProcessData(IBSTickerMode TickerMode)
    {
      if (TickerMode == IBSTickerMode.Active && this._ctrlActivate && !this._ctrlError)
      {
        this.UpdateInputs(this.minPD_AddressInput, this.InputObjectLength);
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
        this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.UpdateInputBlockStartAddress, Util.Int32ToByteArray(StartAddress, 2));
        this.StopOnError();
      }
      else if (Length < 1 || Length - 1 > this.InputObjectEndAddress)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.UpdateInputBlockLength, Util.Int32ToByteArray(Length, 2));
        this.StopOnError();
      }
      else
      {
        lock (this)
        {
          byte[] Data = new byte[Length];
          int Integer = this._ctrlClass.ReadData(StartAddress, ref Data);
          switch (Integer)
          {
            case 0:
              IEnumerator enumerator = this._varInput.GetEnumerator();
              try
              {
                while (enumerator.MoveNext())
                {
                  VarInput current = (VarInput) enumerator.Current;
                  if (current.BaseAddress >= StartAddress && current.BaseAddress < StartAddress + Length)
                  {
                    if (current.VarType == VarType.Boolean)
                      current.SetValue = this.GetDataFromBuffer(Data, current.BaseAddress - StartAddress, current.ByteLength, current.BitOffset);
                    if (current.VarType == VarType.UInt63)
                      current.SetValue = this.GetDataFromBuffer(Data, current.BaseAddress - StartAddress, current.ByteLength, current.BitOffset, current.MaxValue);
                    if (current.VarType == VarType.ByteArray)
                      current.SetByteArray = this.GetByteFromBuffer(Data, current.BaseAddress - StartAddress, current.ByteLength);
                  }
                }
                return;
              }
              finally
              {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                  disposable.Dispose();
              }
            case 134:
              this.CheckWatchdogState();
              break;
          }
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.RetErrReadData, Util.Int32ToByteArray(Integer, 2));
          this.StopOnError();
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
        this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.UpdateOutputBlockStartAddress, Util.Int32ToByteArray(StartAddress, 2));
        this.StopOnError();
      }
      else if (Length < 1 || Length - 1 > this.OutputObjectEndAddress)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.UpdateOutputBlockLength, Util.Int32ToByteArray(Length, 2));
        this.StopOnError();
      }
      else
      {
        lock (this)
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
          int Integer = this._ctrlClass.WriteData(StartAddress, Data);
          switch (Integer)
          {
            case 0:
              return;
            case 134:
              this.CheckWatchdogState();
              break;
          }
          this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.RetErrWriteData, Util.Int32ToByteArray(Integer, 2));
          this.StopOnError();
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
        this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.ParaErrGetByteFromBuffer, (byte[]) null);
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
        this._locDiagnostic.SetDiagnostic((object) this, (System.Enum) ControllerDiagnostic.ParaErrPutByteToBuffer, (byte[]) null);
        this.StopOnError();
      }
    }

    private void DeleteOutputBuffer()
    {
      if (this.maxPD_AddressOutput <= 0 || this.OutputObjectLength <= 0)
        return;
      byte[] Data = new byte[this.OutputObjectLength];
      lock (this)
        this._ctrlClass.WriteData(this.minPD_AddressOutput, Data);
    }

    public int AddObject(MessageClient MessageObject)
    {
      return -1;
    }

    public bool RemoveObject(MessageClient MessageObject)
    {
      return false;
    }

    public event UpdateMailboxHandler OnUpdateMailbox
    {
      add
      {
      }
      remove
      {
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
      this._varInput = (ArrayList) null;
      this._varOutput = (ArrayList) null;
      GC.SuppressFinalize((object) this);
    }

    public class Config
    {
      private bool _writeValues;
      private bool _controlCPU_Load;
      private int _updateCotrollerState;
      private bool _errLoggingActivate;
      private string _errLoggingFilename;
      private bool _DNS_NameResolution;

      public Config()
      {
        this._writeValues = true;
        this._controlCPU_Load = true;
        this._updateCotrollerState = 100;
        this._errLoggingActivate = false;
        this._errLoggingFilename = "";
        this._DNS_NameResolution = false;
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

      public bool DNS_NameResolution
      {
        get
        {
          return this._DNS_NameResolution;
        }
        set
        {
          if (!this._writeValues)
            return;
          this._DNS_NameResolution = value;
        }
      }
    }
  }
}
