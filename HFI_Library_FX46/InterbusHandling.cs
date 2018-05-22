// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.InterbusHandling
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using PhoenixContact.PxC_Library.Util;
using System;

namespace PhoenixContact.HFI.Inline
{
  public sealed class InterbusHandling
  {
    private readonly FirmwareServiceList firmwareServiceList = new FirmwareServiceList();
    private bool cycleTimeReceived = true;
    private readonly Diagnostic diagnostic;
    private readonly MessageClient msgClient;
    private string name;
    private bool enable;
    private FirmwareService activeFirmwareService;
    private DateTime preSendDelayEstimatedTime;

    internal InterbusHandling(string name)
    {
      this.name = name.Length != 0 ? "InterbusHandling " + name : nameof (InterbusHandling);
      this.diagnostic = new Diagnostic(this.Name);
      this.msgClient = new MessageClient(this.Name);
      this.msgClient.Timeout = 5;
      this.msgClient.OnException += new ExceptionHandler(this.MsgClient_OnException);
      this.HandlingState = InterbusHandlingState.Idle;
      this.RevisionInfo = new RevisionInformation();
      this.FirmwareSercivePositiveConfirmationReceived += new ConfirmationReceiveHandler(this.OnPDCycleTimeHandler);
      this.FirmwareSercivePositiveConfirmationReceived += new ConfirmationReceiveHandler(this.OnGetVersionInfoHandler);
      this.activeFirmwareService = (FirmwareService) null;
    }

    private event ConfirmationReceiveHandler FirmwareSercivePositiveConfirmationReceived;

    public RevisionInformation RevisionInfo { get; private set; }

    public double CurrentCycleTime { get; private set; }

    public InterbusHandlingState HandlingState { get; private set; }

    internal MessageClient MessageClient
    {
      get
      {
        return this.msgClient;
      }
    }

    internal string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = "InterbusHandling " + value;
        this.msgClient.Name = this.name;
        this.diagnostic.Name = this.name;
      }
    }

    internal void Enable()
    {
      if (this.enable)
        return;
      this.diagnostic.Enable();
      this.activeFirmwareService = (FirmwareService) null;
      this.firmwareServiceList.DeleteAll();
      this.enable = true;
    }

    internal void Disable()
    {
      if (!this.enable)
        return;
      this.ResetFirmwareService();
      this.RevisionInfo.Delete();
      this.CurrentCycleTime = 0.0;
      this.cycleTimeReceived = true;
      this.diagnostic.Disable();
      this.enable = false;
    }

    public void SetCycleTime(int Time)
    {
      this.SetIbsCycleTime(Time * 1000);
    }

    internal void SetIBS_Sync()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[12]
      {
        (byte) 7,
        (byte) 80,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 1,
        (byte) 34,
        (byte) 0,
        (byte) 6,
        (byte) 0,
        (byte) 0,
        (byte) 0
      }, InterbusHandlingState.DoNotChangeState, 0));
    }

    internal void SetIbsCycleTime(int Time)
    {
      if (Time < 0 || Time > 130000)
      {
        this.diagnostic.Throw((Enum) ControllerDiagnostic.ProcessDataCycleTimeOutOfRange, PhoenixContact.PxC_Library.Util.Util.Int32ToByteArray(Time, 2));
        this.diagnostic.Quit();
      }
      else
      {
        byte[] data = new byte[12]
        {
          (byte) 7,
          (byte) 80,
          (byte) 0,
          (byte) 4,
          (byte) 0,
          (byte) 1,
          (byte) 34,
          (byte) 16,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0
        };
        PhoenixContact.PxC_Library.Util.Util.Int32ToByteArray(Time, 4).CopyTo((Array) data, 8);
        this.firmwareServiceList.AddFirmwareService(new FirmwareService(data, InterbusHandlingState.DoNotChangeState, 0));
      }
    }

    internal void PlugAndPlayDeactivate()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[12]
      {
        (byte) 7,
        (byte) 80,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 1,
        (byte) 34,
        (byte) 64,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      }, InterbusHandlingState.DoNotChangeState, 0));
    }

    internal void ExpertModeActivate()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[12]
      {
        (byte) 7,
        (byte) 80,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 1,
        (byte) 34,
        (byte) 117,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 1
      }, InterbusHandlingState.DoNotChangeState, 0));
    }

    public void AlarmStop()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[4]
      {
        (byte) 19,
        (byte) 3,
        (byte) 0,
        (byte) 0
      }, InterbusHandlingState.BusStopped, 0));
    }

    public void CreateConfiguration()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[6]
      {
        (byte) 7,
        (byte) 16,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 1
      }, InterbusHandlingState.CreateConfiguration, 200));
    }

    public void ConfirmPeripheralFault()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[8]
      {
        (byte) 7,
        (byte) 20,
        (byte) 0,
        (byte) 2,
        (byte) 0,
        (byte) 4,
        (byte) 0,
        (byte) 0
      }, InterbusHandlingState.DoNotChangeState, 0));
    }

    public void ConfirmPeripheralFault(int[] Modules)
    {
      if (Modules.Length > (int) ushort.MaxValue)
        return;
      byte[] data = new byte[8 + 2 * Modules.Length];
      data[0] = (byte) 7;
      data[1] = (byte) 20;
      data[2] = Convert.ToByte(Modules.Length + 2 >> 8 & (int) byte.MaxValue);
      data[3] = Convert.ToByte(Modules.Length + 2 & (int) byte.MaxValue);
      data[4] = (byte) 0;
      data[5] = (byte) 3;
      data[6] = Convert.ToByte(Modules.Length >> 8 & (int) byte.MaxValue);
      data[7] = Convert.ToByte(Modules.Length & (int) byte.MaxValue);
      for (int index = 0; index < Modules.Length; ++index)
      {
        data[8 + index * 2] = Convert.ToByte(Modules[index] >> 8 & (int) byte.MaxValue);
        data[8 + index * 2 + 1] = Convert.ToByte(Modules[index] & (int) byte.MaxValue);
      }
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(data, InterbusHandlingState.DoNotChangeState, 0));
    }

    public void ConfirmPeripheralFault(int Module)
    {
      this.ConfirmPeripheralFault(new int[1]{ Module });
    }

    public void ActivateConfiguration()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[6]
      {
        (byte) 7,
        (byte) 17,
        (byte) 0,
        (byte) 1,
        (byte) 0,
        (byte) 1
      }, InterbusHandlingState.ActivateConfiguration, 0));
    }

    public void StartDataTransfer()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[4]
      {
        (byte) 7,
        (byte) 1,
        (byte) 0,
        (byte) 0
      }, InterbusHandlingState.BusActivate, 0));
    }

    public void ConfirmDiagnostics()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[4]
      {
        (byte) 7,
        (byte) 96,
        (byte) 0,
        (byte) 0
      }, InterbusHandlingState.DoNotChangeState, 200));
    }

    internal void FetchRealCycleTime()
    {
      if (!this.cycleTimeReceived)
        return;
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[8]
      {
        (byte) 3,
        (byte) 81,
        (byte) 0,
        (byte) 2,
        (byte) 0,
        (byte) 1,
        (byte) 34,
        (byte) 22
      }, InterbusHandlingState.DoNotChangeState, 0));
      this.cycleTimeReceived = false;
    }

    internal void FetchRevisionInfo()
    {
      this.firmwareServiceList.AddFirmwareService(new FirmwareService(new byte[4]
      {
        (byte) 3,
        (byte) 42,
        (byte) 0,
        (byte) 0
      }, InterbusHandlingState.DoNotChangeState, 0));
    }

    internal void Run()
    {
      if (!this.enable)
        return;
      this.UpdateFirmwareService();
    }

    internal bool IsServiceActive()
    {
      return this.activeFirmwareService != null && (this.activeFirmwareService.ActualState == FirmwareService.State.Send || this.activeFirmwareService.ActualState == FirmwareService.State.WaitForConfirmation);
    }

    internal event ExceptionHandler OnException
    {
      add
      {
        this.diagnostic.OnException += value;
      }
      remove
      {
        this.diagnostic.OnException -= value;
      }
    }

    private void UpdateFirmwareService()
    {
      if (this.activeFirmwareService != null)
      {
        if (this.activeFirmwareService.ActualState == FirmwareService.State.Done)
          this.activeFirmwareService = this.firmwareServiceList.GetNextFirmwareService();
        else if (this.activeFirmwareService.ActualState == FirmwareService.State.Error)
        {
          this.firmwareServiceList.DeleteAll();
          this.activeFirmwareService = (FirmwareService) null;
          return;
        }
        this.ExecuteService();
      }
      else
      {
        if (this.firmwareServiceList.Count <= 0)
          return;
        this.activeFirmwareService = this.firmwareServiceList.GetNextFirmwareService();
      }
    }

    private void ExecuteService()
    {
      if (this.activeFirmwareService == null)
        return;
      switch (this.activeFirmwareService.ActualState)
      {
        case FirmwareService.State.Idle:
          if (this.activeFirmwareService.PreSendDelay != 0)
          {
            this.activeFirmwareService.SetActualState(FirmwareService.State.PreSendDelay);
            this.preSendDelayEstimatedTime = DateTime.Now.AddMilliseconds((double) this.activeFirmwareService.PreSendDelay);
            break;
          }
          this.activeFirmwareService.SetActualState(FirmwareService.State.Send);
          break;
        case FirmwareService.State.PreSendDelay:
          if (!(this.preSendDelayEstimatedTime < DateTime.Now))
            break;
          this.activeFirmwareService.SetActualState(FirmwareService.State.Send);
          break;
        case FirmwareService.State.Send:
          this.SendFirmwareService();
          break;
        case FirmwareService.State.WaitForConfirmation:
          this.WaitingForConfirmation();
          break;
        case FirmwareService.State.Done:
          break;
        case FirmwareService.State.Error:
          break;
        default:
          this.diagnostic.Throw((Enum) ControllerDiagnostic.FirmwareServiceStateError, PhoenixContact.PxC_Library.Util.Util.Int32ToByteArray((int) (ushort) this.activeFirmwareService.ActualState, 2));
          this.diagnostic.Quit();
          this.activeFirmwareService.SetActualState(FirmwareService.State.Error);
          break;
      }
    }

    private void SendFirmwareService()
    {
      this.msgClient.ClearReceiveData();
      if (!this.msgClient.SendRequest(this.activeFirmwareService.Command))
        return;
      this.activeFirmwareService.SetActualState(FirmwareService.State.WaitForConfirmation);
    }

    private void WaitingForConfirmation()
    {
      if (this.msgClient.State != MessageClientState.ConfirmationReceived)
        return;
      byte[] receiveRequest = this.msgClient.GetReceiveRequest();
      if (receiveRequest.Length == 0 || ((int) receiveRequest[0] & 128) != 128)
        return;
      this.msgClient.SetState(MessageClientState.Idle);
      if (PhoenixContact.PxC_Library.Util.Util.ByteToInt32(receiveRequest[4], receiveRequest[5]) == 0)
      {
        // ISSUE: reference to a compiler-generated field
        if (receiveRequest[0] == (byte) 131 && this.FirmwareSercivePositiveConfirmationReceived != null)
        {
          // ISSUE: reference to a compiler-generated field
          this.FirmwareSercivePositiveConfirmationReceived((object) this, receiveRequest);
        }
        if (this.activeFirmwareService.TargetState != InterbusHandlingState.DoNotChangeState)
          this.HandlingState = this.activeFirmwareService.TargetState;
        this.activeFirmwareService.SetActualState(FirmwareService.State.Done);
      }
      else
      {
        this.diagnostic.Throw((Enum) ControllerDiagnostic.NegConfSendMessage, PhoenixContact.PxC_Library.Util.Util.GetByteArrayFromService(receiveRequest));
        this.diagnostic.Quit();
        this.activeFirmwareService.SetActualState(FirmwareService.State.Error);
      }
    }

    internal void ResetFirmwareService()
    {
      this.firmwareServiceList.DeleteAll();
      this.activeFirmwareService = (FirmwareService) null;
      this.msgClient.ClearSendData();
      this.msgClient.ClearReceiveData();
      this.HandlingState = InterbusHandlingState.Idle;
    }

    private void MsgClient_OnException(Exception ExceptionData)
    {
      this.diagnostic.Throw((Enum) InterbusHandlingState.MessageClientDiagnostic, new byte[0], ExceptionData);
      this.diagnostic.Quit();
      this.activeFirmwareService = (FirmwareService) null;
      this.firmwareServiceList.DeleteAll();
    }

    private void OnPDCycleTimeHandler(object Sender, byte[] Data)
    {
      if (Data[2] != (byte) 0 || Data[3] != (byte) 5 || (Data[6] != (byte) 0 || Data[7] != (byte) 1) || Data[8] != (byte) 34)
        return;
      if (Data[9] != (byte) 22)
        return;
      try
      {
        this.CurrentCycleTime = Math.Round((double) PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Data[10], Data[11], Data[12], Data[13]) / 1000.0, 2);
      }
      catch
      {
      }
      this.cycleTimeReceived = true;
    }

    private void OnGetVersionInfoHandler(object sender, byte[] data)
    {
      if (data[2] != (byte) 0 || data[3] != (byte) 85 || (data[4] != (byte) 0 || data[5] != (byte) 0))
        return;
      this.RevisionInfo.SetData(data);
    }
  }
}
