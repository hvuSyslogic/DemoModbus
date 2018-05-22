// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.InterbusHandling
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;
using System.Collections;

namespace PhoenixContact.HFI
{
  public sealed class InterbusHandling
  {
    internal ArrayList _firmwareServiceList = new ArrayList();
    private bool _CycleTimeReceived = true;
    private string _name;
    private bool _activate;
    private InterbusHandlingState _actuaState;
    private int _firmwareServiceState;
    private Diagnostic _locDiagnostic;
    private IController _locController;
    private MessageClient _msgClient;
    private SvcWriter _ctrlSvcFileWriter;
    private double _CurrentCycleTime;
    private DateTime _PreSendDelayStart;
    private InterbusHandling.RevisionInformation _RevisionInformation;

    private event ConfirmationReceiveHandler _firmwareSercivePositiveConfirmationReceived;

    internal InterbusHandling(string Name, IController Controller)
    {
      if (Controller == null)
        return;
      this._locController = Controller;
      this._name = Name;
      this._locDiagnostic = new Diagnostic(Name);
      this._msgClient = new MessageClient("Internal " + Name);
      this._msgClient.OnDiagnostic += new DiagnosticHandler(this._msgClient_OnDiagnostic);
      this._locController.AddObject(this._msgClient);
      this._actuaState = InterbusHandlingState.Idle;
      this._RevisionInformation.Delete();
      this._firmwareSercivePositiveConfirmationReceived += new ConfirmationReceiveHandler(this.OnPDCycleTimeHandler);
      this._firmwareSercivePositiveConfirmationReceived += new ConfirmationReceiveHandler(this.OnGetVersionInfoHandler);
    }

    public InterbusHandling.RevisionInformation RevisionInfo
    {
      get
      {
        return this._RevisionInformation;
      }
    }

    public double CurrentCycleTime
    {
      get
      {
        return this._CurrentCycleTime;
      }
    }

    public InterbusHandlingState HandlingState
    {
      get
      {
        return this._actuaState;
      }
    }

    internal string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
        this._locDiagnostic.Name = value;
        this._msgClient.Name = "Internal " + value;
      }
    }

    internal bool Activate
    {
      get
      {
        return this._activate;
      }
      set
      {
        if (!value && this._activate)
        {
          this.ResetFirmwareService();
          this._RevisionInformation.Delete();
          this._CurrentCycleTime = 0.0;
          this._CycleTimeReceived = true;
        }
        this._activate = value;
        this._locDiagnostic.Activate = value;
      }
    }

    internal event DiagnosticHandler OnDiagnostic
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

    public void SetCycleTime(int Time)
    {
      this.SetIbsCycleTime(Time * 1000);
    }

    internal void SetIBS_Sync()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(12);
      ctrlFirmwareService.TargetState = InterbusHandlingState.DoNotChangeState;
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 80;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 4;
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 1;
      ctrlFirmwareService.Command[6] = (byte) 34;
      ctrlFirmwareService.Command[7] = (byte) 0;
      ctrlFirmwareService.Command[8] = (byte) 6;
      ctrlFirmwareService.Command[9] = (byte) 0;
      ctrlFirmwareService.Command[10] = (byte) 0;
      ctrlFirmwareService.Command[11] = (byte) 0;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    internal void SetIbsCycleTime(int Time)
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      if (Time < 0 || Time > 130000)
      {
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.ProcessDataCycleTimeOutOfRange, Util.Int32ToByteArray(Time, 2));
      }
      else
      {
        InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(12);
        ctrlFirmwareService.TargetState = InterbusHandlingState.DoNotChangeState;
        ctrlFirmwareService.Command[0] = (byte) 7;
        ctrlFirmwareService.Command[1] = (byte) 80;
        ctrlFirmwareService.Command[2] = (byte) 0;
        ctrlFirmwareService.Command[3] = (byte) 4;
        ctrlFirmwareService.Command[4] = (byte) 0;
        ctrlFirmwareService.Command[5] = (byte) 1;
        ctrlFirmwareService.Command[6] = (byte) 34;
        ctrlFirmwareService.Command[7] = (byte) 16;
        string str1 = Time.ToString("x").PadLeft(8, '0');
        for (int index = 0; index < 4; ++index)
        {
          string str2 = str1.Substring(index * 2, 2);
          ctrlFirmwareService.Command[8 + index] = Convert.ToByte(str2, 16);
        }
        this._firmwareServiceList.Add((object) ctrlFirmwareService);
      }
    }

    internal void PlugAndPlayDeactivate()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(12);
      ctrlFirmwareService.TargetState = InterbusHandlingState.DoNotChangeState;
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 80;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 4;
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 1;
      ctrlFirmwareService.Command[6] = (byte) 34;
      ctrlFirmwareService.Command[7] = (byte) 64;
      ctrlFirmwareService.Command[8] = (byte) 0;
      ctrlFirmwareService.Command[9] = (byte) 0;
      ctrlFirmwareService.Command[10] = (byte) 0;
      ctrlFirmwareService.Command[11] = (byte) 0;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    internal void ExpertModeActivate()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(12);
      ctrlFirmwareService.TargetState = InterbusHandlingState.DoNotChangeState;
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 80;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 4;
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 1;
      ctrlFirmwareService.Command[6] = (byte) 34;
      ctrlFirmwareService.Command[7] = (byte) 117;
      ctrlFirmwareService.Command[8] = (byte) 0;
      ctrlFirmwareService.Command[9] = (byte) 0;
      ctrlFirmwareService.Command[10] = (byte) 0;
      ctrlFirmwareService.Command[11] = (byte) 1;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public void AlarmStop()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(4);
      ctrlFirmwareService.TargetState = InterbusHandlingState.BusStopped;
      ctrlFirmwareService.Command[0] = (byte) 19;
      ctrlFirmwareService.Command[1] = (byte) 3;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 0;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public void CreateConfiguration()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(6);
      ctrlFirmwareService.TargetState = InterbusHandlingState.CreateConfiguration;
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 16;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 1;
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 1;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public void ConfirmPeripheralFault()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(8);
      ctrlFirmwareService.TargetState = InterbusHandlingState.DoNotChangeState;
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 20;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 2;
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 4;
      ctrlFirmwareService.Command[6] = (byte) 0;
      ctrlFirmwareService.Command[7] = (byte) 0;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public void ConfirmPeripheralFault(int[] Modules)
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive || Modules.Length > (int) ushort.MaxValue)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(8 + 2 * Modules.Length);
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 20;
      ctrlFirmwareService.Command[2] = Convert.ToByte(Modules.Length + 2 >> 8 & (int) byte.MaxValue);
      ctrlFirmwareService.Command[3] = Convert.ToByte(Modules.Length + 2 & (int) byte.MaxValue);
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 3;
      ctrlFirmwareService.Command[6] = Convert.ToByte(Modules.Length >> 8 & (int) byte.MaxValue);
      ctrlFirmwareService.Command[7] = Convert.ToByte(Modules.Length & (int) byte.MaxValue);
      for (int index = 0; index < Modules.Length; ++index)
      {
        ctrlFirmwareService.Command[8 + index * 2] = Convert.ToByte(Modules[index] >> 8 & (int) byte.MaxValue);
        ctrlFirmwareService.Command[8 + index * 2 + 1] = Convert.ToByte(Modules[index] & (int) byte.MaxValue);
      }
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public void ConfirmPeripheralFault(int Module)
    {
      this.ConfirmPeripheralFault(new int[1]{ Module });
    }

    public void ActivateConfiguration()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(6);
      ctrlFirmwareService.TargetState = InterbusHandlingState.ActivateConfiguration;
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 17;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 1;
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 1;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public void StartDataTransfer()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(4);
      ctrlFirmwareService.TargetState = InterbusHandlingState.BusActivate;
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 1;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 0;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public void ConfirmDiagnostics()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(4);
      ctrlFirmwareService.Command[0] = (byte) 7;
      ctrlFirmwareService.Command[1] = (byte) 96;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 0;
      ctrlFirmwareService.PreSendDelay = 200;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    internal void FetchRealCycleTime()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive || !this._CycleTimeReceived)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(8);
      ctrlFirmwareService.Command[0] = (byte) 3;
      ctrlFirmwareService.Command[1] = (byte) 81;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 2;
      ctrlFirmwareService.Command[4] = (byte) 0;
      ctrlFirmwareService.Command[5] = (byte) 1;
      ctrlFirmwareService.Command[6] = (byte) 34;
      ctrlFirmwareService.Command[7] = (byte) 22;
      this._CycleTimeReceived = false;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    internal void FetchRevisionInfo()
    {
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return;
      InterbusHandling.ctrlFirmwareService ctrlFirmwareService = new InterbusHandling.ctrlFirmwareService(4);
      ctrlFirmwareService.Command[0] = (byte) 3;
      ctrlFirmwareService.Command[1] = (byte) 42;
      ctrlFirmwareService.Command[2] = (byte) 0;
      ctrlFirmwareService.Command[3] = (byte) 0;
      this._firmwareServiceList.Add((object) ctrlFirmwareService);
    }

    public bool SvcFileDownload(string FileName, string LogFileName)
    {
      bool flag = false;
      if (this._actuaState == InterbusHandlingState.SvcDownloadActive)
        return flag;
      if (this._ctrlSvcFileWriter == null)
      {
        this._ctrlSvcFileWriter = new SvcWriter(this.Name, this._locController);
        this._ctrlSvcFileWriter.OnStart += new SvcWriterStartHandler(this._ctrlSvcFileWriter_OnStart);
        this._ctrlSvcFileWriter.OnFinish += new SvcWriterReadyHandler(this._ctrlSvcFileWriter_OnFinish);
        this._ctrlSvcFileWriter.OnDiagnostic += new DiagnosticHandler(this._ctrlSvcFileWriter_OnDiagnostic);
      }
      if (FileName.Length > 0)
      {
        this.ResetFirmwareService();
        if (this._ctrlSvcFileWriter.SvcFileDownload(FileName, LogFileName))
          flag = true;
      }
      return flag;
    }

    internal void Run()
    {
      if (!this._activate)
        return;
      this.UpdateFirmwareService();
    }

    private void UpdateFirmwareService()
    {
      switch (this._firmwareServiceState)
      {
        case 0:
          if (this._firmwareServiceList.Count <= 0)
            break;
          this._msgClient.ClearReceiveData();
          InterbusHandling.ctrlFirmwareService firmwareService1 = (InterbusHandling.ctrlFirmwareService) this._firmwareServiceList[0];
          if (firmwareService1.PreSendDelay != 0)
          {
            this._firmwareServiceState = 2;
            this._PreSendDelayStart = DateTime.Now;
            break;
          }
          if (!this._msgClient.SendRequest(firmwareService1.Command))
            break;
          this._firmwareServiceState = 1;
          break;
        case 1:
          if (this._msgClient._State != MessageClientState.ConfirmationReceived || this._msgClient.ReceiveRequest.Length <= 0 || ((int) this._msgClient.ReceiveRequest[0] & 128) != 128)
            break;
          this._msgClient._State = MessageClientState.Idle;
          if (Util.ByteToInt32(this._msgClient.ReceiveRequest[4], this._msgClient.ReceiveRequest[5]) == 0)
          {
            if (this._msgClient.ReceiveRequest[0] == (byte) 131 && this._firmwareSercivePositiveConfirmationReceived != null)
              this._firmwareSercivePositiveConfirmationReceived((object) this, this._msgClient.ReceiveRequest);
            InterbusHandling.ctrlFirmwareService firmwareService2 = (InterbusHandling.ctrlFirmwareService) this._firmwareServiceList[0];
            if (firmwareService2.TargetState != InterbusHandlingState.DoNotChangeState)
              this._actuaState = firmwareService2.TargetState;
            this._firmwareServiceList.Remove(this._firmwareServiceList[0]);
            this._firmwareServiceState = 0;
            break;
          }
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.NegConfSendMessage, Util.GetByteArrayFromService(this._msgClient.ReceiveRequest));
          while (this._firmwareServiceList.Count > 0)
            this._firmwareServiceList.Remove(this._firmwareServiceList[0]);
          this._firmwareServiceState = 0;
          break;
        case 2:
          InterbusHandling.ctrlFirmwareService firmwareService3 = (InterbusHandling.ctrlFirmwareService) this._firmwareServiceList[0];
          if ((DateTime.Now - this._PreSendDelayStart).TotalMilliseconds <= (double) firmwareService3.PreSendDelay)
            break;
          firmwareService3.PreSendDelay = 0;
          this._firmwareServiceList[0] = (object) firmwareService3;
          this._firmwareServiceState = 0;
          break;
        default:
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) ControllerDiagnostic.FirmwareServiceStateError, Util.Int32ToByteArray(this._firmwareServiceState, 2));
          while (this._firmwareServiceList.Count > 0)
            this._firmwareServiceList.Remove(this._firmwareServiceList[0]);
          this._firmwareServiceState = 0;
          break;
      }
    }

    internal void ResetFirmwareService()
    {
      while (this._firmwareServiceList.Count > 0)
      {
        if (this._firmwareServiceList[0] != null)
          this._firmwareServiceList.Remove(this._firmwareServiceList[0]);
      }
      this._msgClient._State = MessageClientState.Idle;
      this._msgClient.ClearSendData();
      this._msgClient.ClearReceiveData();
      this._firmwareServiceState = 0;
      this._actuaState = InterbusHandlingState.Idle;
    }

    private void _ctrlSvcFileWriter_OnStart(object sender)
    {
      this._actuaState = InterbusHandlingState.SvcDownloadActive;
    }

    private void _ctrlSvcFileWriter_OnFinish(object sender)
    {
      this._actuaState = InterbusHandlingState.SvcDownloadReady;
    }

    private void _ctrlSvcFileWriter_OnDiagnostic(object Sender, DiagnosticArgs DiagnosticCode)
    {
      this._locDiagnostic.SetDiagnostic(Sender, DiagnosticCode);
      this._actuaState = InterbusHandlingState.SvcDownloadError;
    }

    private void _msgClient_OnDiagnostic(object Sender, DiagnosticArgs DiagnosticCode)
    {
      this._locDiagnostic.SetDiagnostic((object) this, (Enum) (ControllerDiagnostic) DiagnosticCode.DiagCode, DiagnosticCode.AddDiagCode);
    }

    private void OnPDCycleTimeHandler(object Sender, byte[] Data)
    {
      if (Data[1] != (byte) 81 || Data[2] != (byte) 0 || (Data[3] != (byte) 5 || Data[6] != (byte) 0) || (Data[7] != (byte) 1 || Data[8] != (byte) 34 || Data[9] != (byte) 22))
        return;
      this._CurrentCycleTime = Math.Round((double) (Util.ByteToInt32(Data[10], Data[11]) << 16 | Util.ByteToInt32(Data[12], Data[13])) / 1000.0, 2);
      this._CycleTimeReceived = true;
    }

    private void OnGetVersionInfoHandler(object Sender, byte[] Data)
    {
      if (Data[2] != (byte) 0 || Data[3] != (byte) 85 || (Data[4] != (byte) 0 || Data[5] != (byte) 0))
        return;
      this._RevisionInformation.Firmware.Version = this.CopyConfToFwInfo(6, 9, Data);
      this._RevisionInformation.Firmware.State = this.CopyConfToFwInfo(10, 15, Data);
      this._RevisionInformation.Firmware.Date = this.CopyConfToFwInfo(16, 21, Data);
      this._RevisionInformation.Firmware.Time = this.CopyConfToFwInfo(22, 27, Data);
      this._RevisionInformation.Host.Type = this.CopyConfToFwInfo(28, 47, Data);
      this._RevisionInformation.Host.Version = this.CopyConfToFwInfo(48, 51, Data);
      this._RevisionInformation.Host.State = this.CopyConfToFwInfo(52, 57, Data);
      this._RevisionInformation.Host.Date = this.CopyConfToFwInfo(58, 63, Data);
      this._RevisionInformation.Host.Time = this.CopyConfToFwInfo(64, 69, Data);
      this._RevisionInformation.StartFirmware.Version = this.CopyConfToFwInfo(70, 73, Data);
      this._RevisionInformation.StartFirmware.State = this.CopyConfToFwInfo(74, 79, Data);
      this._RevisionInformation.StartFirmware.Date = this.CopyConfToFwInfo(80, 85, Data);
      this._RevisionInformation.StartFirmware.Time = this.CopyConfToFwInfo(86, 91, Data);
      this._RevisionInformation.Hardware.ArticleNumber = this.CopyConfToFwInfo(92, 99, Data);
      this._RevisionInformation.Hardware.Name = this.CopyConfToFwInfo(100, 129, Data);
      this._RevisionInformation.Hardware.MotherboardID = this.CopyConfToFwInfo(130, 133, Data);
      this._RevisionInformation.Hardware.Version = this.CopyConfToFwInfo(134, 135, Data);
      this._RevisionInformation.Hardware.Vendor = this.CopyConfToFwInfo(136, 155, Data);
      this._RevisionInformation.Hardware.SerialNumber = this.CopyConfToFwInfo(156, 167, Data);
      this._RevisionInformation.Hardware.Date = this.CopyConfToFwInfo(168, 173, Data);
    }

    private string CopyConfToFwInfo(int FromWord, int ToWord, byte[] Data)
    {
      string str = "";
      if (FromWord >= ToWord || ToWord > Data.Length)
        return str;
      for (int index = FromWord; index <= ToWord && Data[index] != (byte) 0; ++index)
        str += ((char) Data[index]).ToString();
      return str;
    }

    internal void Dispose()
    {
      if (this._locDiagnostic != null)
      {
        this._locDiagnostic.Dispose();
        this._locDiagnostic = (Diagnostic) null;
      }
      this._firmwareServiceList = (ArrayList) null;
      this._msgClient = (MessageClient) null;
      GC.SuppressFinalize((object) this);
    }

    public struct RevisionInformation
    {
      public InterbusHandling.FirmwareInformation Firmware;
      public InterbusHandling.HostInformation Host;
      public InterbusHandling.StartFirmwareInformation StartFirmware;
      public InterbusHandling.HardwareInformation Hardware;

      internal void Delete()
      {
        this.Firmware.Delete();
        this.Host.Delete();
        this.StartFirmware.Delete();
        this.Hardware.Delete();
      }

      public override string ToString()
      {
        return this.Firmware.ToString() + "\r\n" + this.Host.ToString() + "\r\n" + this.StartFirmware.ToString() + "\r\n" + this.Hardware.ToString();
      }
    }

    public struct FirmwareInformation
    {
      public string Version;
      public string State;
      public string Date;
      public string Time;

      internal void Delete()
      {
        this.Version = "";
        this.State = "";
        this.Date = "";
        this.Time = "";
      }

      public override string ToString()
      {
        return "Firmware:\r\nVersion: " + this.Version + "\r\nState: " + this.State + "\r\nDate: " + this.Date + "\r\nTime: " + this.Time;
      }
    }

    public struct HostInformation
    {
      public string Type;
      public string Version;
      public string State;
      public string Date;
      public string Time;

      internal void Delete()
      {
        this.Type = "";
        this.Version = "";
        this.State = "";
        this.Date = "";
        this.Time = "";
      }

      public override string ToString()
      {
        return "Host:\r\nType: " + this.Type + "\r\nVersion: " + this.Version + "\r\nState: " + this.State + "\r\nDate: " + this.Date + "\r\nTime: " + this.Time;
      }
    }

    public struct StartFirmwareInformation
    {
      public string Version;
      public string State;
      public string Date;
      public string Time;

      internal void Delete()
      {
        this.Version = "";
        this.State = "";
        this.Date = "";
        this.Time = "";
      }

      public override string ToString()
      {
        return "Start Firmware:\r\nVersion: " + this.Version + "\r\nState: " + this.State + "\r\nDate: " + this.Date + "\r\nTime: " + this.Time;
      }
    }

    public struct HardwareInformation
    {
      public string ArticleNumber;
      public string Name;
      public string MotherboardID;
      public string Version;
      public string Vendor;
      public string SerialNumber;
      public string Date;

      internal void Delete()
      {
        this.ArticleNumber = "";
        this.Name = "";
        this.MotherboardID = "";
        this.Version = "";
        this.Vendor = "";
        this.SerialNumber = "";
        this.Date = "";
      }

      public override string ToString()
      {
        return "Hardware:\r\nArticleNumber: " + this.ArticleNumber + "\r\nName: " + this.Name + "\r\nMotherboard ID: " + this.MotherboardID + "\r\nVersion: " + this.Version + "\r\nDate: " + this.Date;
      }
    }

    internal struct ctrlFirmwareService
    {
      internal InterbusHandlingState TargetState;
      internal byte[] Command;
      internal int PreSendDelay;

      internal ctrlFirmwareService(int CommandLength)
      {
        this.TargetState = InterbusHandlingState.DoNotChangeState;
        this.Command = new byte[CommandLength];
        this.PreSendDelay = 0;
      }
    }
  }
}
