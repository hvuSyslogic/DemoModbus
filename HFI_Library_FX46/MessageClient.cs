// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.MessageClient
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using PhoenixContact.PxC_Library.Util;
using System;

namespace PhoenixContact.HFI.Inline
{
  public class MessageClient : IComparable, ICloneable
  {
    private static uint instanceCounter;
    private readonly object sendDataLock;
    private readonly Diagnostic diagnostic;
    private string name;
    private byte[] sendData;
    private byte[] recvData;
    private int recvDataTimeout;
    private ConfirmationReceiveHandler confirmationReceived;

    public MessageClient(string newName)
    {
      if (MessageClient.instanceCounter >= uint.MaxValue)
        throw new OverflowException("No more MessageClients available.");
      ++MessageClient.instanceCounter;
      this.ControllerId = 0;
      this.name = !string.IsNullOrEmpty(newName) ? newName : "MSG_" + (object) MessageClient.instanceCounter;
      this.sendDataLock = new object();
      this.SetState(MessageClientState.Idle);
      this.SendDataTime = DateTime.Now;
      this.recvDataTimeout = 2;
      this.diagnostic = new Diagnostic(this.name);
      this.sendData = new byte[0];
      this.recvData = new byte[0];
    }

    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this.name = value;
        this.diagnostic.Name = this.name;
      }
    }

    public string ControllerName { get; private set; }

    public MessageClientState State { get; private set; }

    public int ControllerId { get; private set; }

    public int SendDataLength
    {
      get
      {
        if (this.sendData != null)
          return this.sendData.Length;
        return 0;
      }
    }

    public DateTime SendDataTime { get; private set; }

    public DateTime EstimatedReceiveDataTime { get; private set; }

    public int Timeout
    {
      get
      {
        return this.recvDataTimeout;
      }
      set
      {
        if (value <= 0)
          return;
        this.recvDataTimeout = value;
      }
    }

    internal bool IsDataToSend
    {
      get
      {
        if (this.State != MessageClientState.SendRequest)
          return this.State == MessageClientState.SendRequestOnly;
        return true;
      }
    }

    internal void CheckTimeout()
    {
      if (!(DateTime.Now > this.EstimatedReceiveDataTime))
        return;
      this.SetDiagnostic(MessageClientDiagnostic.ConfirmationTimeout, this.GetSendData());
    }

    public byte[] GetSendData()
    {
      lock (this.sendDataLock)
        return this.sendData;
    }

    public byte[] GetReceiveRequest()
    {
      lock (this.sendDataLock)
        return this.recvData;
    }

    internal bool SetSendDone()
    {
      lock (this.sendDataLock)
      {
        this.SendDataTime = DateTime.Now;
        this.EstimatedReceiveDataTime = DateTime.Now.AddSeconds((double) this.Timeout);
        if (this.State == MessageClientState.SendRequest)
        {
          this.SetState(MessageClientState.WaitingForConfirmation);
          return true;
        }
        if (this.State != MessageClientState.SendRequestOnly)
          return false;
        this.SetState(MessageClientState.SendRequestOnlyDone);
        return true;
      }
    }

    internal void SetError()
    {
      lock (this.sendDataLock)
        this.SetState(MessageClientState.Error);
    }

    internal void SetState(MessageClientState newState)
    {
      lock (this.sendDataLock)
        this.State = newState;
    }

    internal void SetReceiveData(byte[] data, int length)
    {
      lock (this.sendDataLock)
      {
        if (data == null)
          return;
        this.recvData = new byte[length];
        Buffer.BlockCopy((Array) data, 0, (Array) this.recvData, 0, length);
        this.SetState(MessageClientState.ConfirmationReceived);
      }
    }

    internal void AssignController(string controllerName, int controllerId)
    {
      this.ControllerId = controllerId;
      this.ControllerName = controllerName;
      this.ClearSendData();
      this.ClearReceiveData();
      this.ActivateDiagnostic = true;
    }

    internal void ClearController()
    {
      this.ControllerId = 0;
      this.ControllerName = string.Empty;
      this.ActivateDiagnostic = false;
      this.SetState(MessageClientState.Idle);
    }

    public bool SendRequest(byte[] data, int length)
    {
      if (this.State == MessageClientState.SendRequest || this.State == MessageClientState.SendRequestOnly || (this.State == MessageClientState.WaitingForConfirmation || data == null) || length <= 0)
        return false;
      lock (this.sendDataLock)
      {
        this.sendData = new byte[length];
        Buffer.BlockCopy((Array) data, 0, (Array) this.sendData, 0, length);
        this.SetState(MessageClientState.SendRequest);
      }
      return true;
    }

    public bool SendRequest(byte[] data)
    {
      return this.SendRequest(data, data.Length);
    }

    public bool SendRequestOnly(byte[] data, int length)
    {
      if (this.State == MessageClientState.SendRequest || this.State == MessageClientState.SendRequestOnly || (this.State == MessageClientState.WaitingForConfirmation || data == null) || length <= 0)
        return false;
      lock (this.sendDataLock)
      {
        this.sendData = new byte[length];
        Buffer.BlockCopy((Array) data, 0, (Array) this.sendData, 0, length);
        this.SetState(MessageClientState.SendRequestOnly);
      }
      return true;
    }

    public bool SendRequestOnly(byte[] data)
    {
      return this.SendRequestOnly(data, data.Length);
    }

    public void ClearReceiveData()
    {
      lock (this.sendDataLock)
      {
        if (this.recvData.Length != 0)
          this.recvData = new byte[0];
        this.SetState(MessageClientState.Idle);
      }
    }

    internal void ClearSendData()
    {
      lock (this.sendDataLock)
      {
        if (this.sendData.Length != 0)
          this.sendData = new byte[0];
        this.SendDataTime = DateTime.MinValue;
        this.SetState(MessageClientState.Idle);
      }
    }

    public override string ToString()
    {
      return this.name;
    }

    public event ConfirmationReceiveHandler OnConfirmationReceived
    {
      add
      {
        this.confirmationReceived += value;
      }
      remove
      {
        this.confirmationReceived -= value;
      }
    }

    internal void CallConfirmationReceivedEvent(object Sender)
    {
      if (this.confirmationReceived == null)
        return;
      this.confirmationReceived(Sender, this.GetReceiveRequest());
    }

    internal bool ConfirmationReceivedDelegateValid
    {
      get
      {
        return this.confirmationReceived != null;
      }
    }

    public bool ActivateDiagnostic
    {
      get
      {
        return this.diagnostic.Ready;
      }
      set
      {
        if (value && !this.diagnostic.Ready)
          this.diagnostic.Enable();
        if (value || !this.diagnostic.Ready)
          return;
        this.diagnostic.Disable();
      }
    }

    internal void SetDiagnostic(MessageClientDiagnostic DiagCode, byte[] AddDiagCode)
    {
      lock (this.sendDataLock)
      {
        this.diagnostic.Throw((Enum) DiagCode, AddDiagCode);
        this.SetState(MessageClientState.Error);
        this.diagnostic.Quit();
      }
    }

    public event ExceptionHandler OnException
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

    public int CompareTo(object obj)
    {
      MessageClient pMessageClient = obj as MessageClient;
      if (this.ControllerId > pMessageClient.ControllerId)
        return 1;
      if (this.ControllerId < pMessageClient.ControllerId)
        return -1;
      return this.CompareName(pMessageClient);
    }

    private int CompareName(MessageClient pMessageClient)
    {
      return this.Name.CompareTo(pMessageClient.Name);
    }

    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}
