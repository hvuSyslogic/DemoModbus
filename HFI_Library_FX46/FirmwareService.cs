// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.FirmwareService
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using System;

namespace PhoenixContact.HFI.Inline
{
  internal class FirmwareService : ICloneable
  {
    private readonly object accessLock;

    public FirmwareService.State ActualState { get; private set; }

    public byte[] Command { get; private set; }

    public int PreSendDelay { get; set; }

    public InterbusHandlingState TargetState { get; private set; }

    public FirmwareService(byte[] data = null, InterbusHandlingState state = InterbusHandlingState.DoNotChangeState, int preSendDelay = 0)
    {
      this.accessLock = new object();
      this.TargetState = state;
      this.PreSendDelay = preSendDelay;
      if (data == null)
        this.Command = new byte[0];
      else
        this.SetCommandData(data);
    }

    public bool SetCommandData(byte[] command)
    {
      lock (this.accessLock)
      {
        if (this.ActualState == FirmwareService.State.Idle && this.ActualState == FirmwareService.State.Done || command == null)
          return false;
        this.Command = new byte[command.Length];
        Buffer.BlockCopy((Array) command, 0, (Array) this.Command, 0, this.Command.Length);
        this.ActualState = FirmwareService.State.Idle;
        return true;
      }
    }

    public void SetActualState(FirmwareService.State newState)
    {
      lock (this.accessLock)
        this.ActualState = newState;
    }

    public object Clone()
    {
      lock (this.accessLock)
        return this.MemberwiseClone();
    }

    public enum State
    {
      Idle,
      PreSendDelay,
      Send,
      WaitForConfirmation,
      Done,
      Error,
    }
  }
}
