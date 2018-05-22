// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.MessageClient
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public class MessageClient
  {
    private static int _uIdCounter;
    private int _uID;
    private string _uName;
    private MessageClientState _state;
    private byte[] _sendData;
    private int _sendDataLength;
    private DateTime _sendDataTime;
    private byte[] _recvData;
    private int _recvDataTimeout;
    private Diagnostic _locDiagnostic;
    private ConfirmationReceiveHandler _confirmationReceived;

    public MessageClient(string Name)
    {
      ++MessageClient._uIdCounter;
      this._uID = MessageClient._uIdCounter;
      this._uName = Name != null ? Name : "MSG_" + MessageClient._uIdCounter.ToString();
      this._state = MessageClientState.Idle;
      this._sendDataTime = DateTime.Now;
      this._recvDataTimeout = 2;
      this._locDiagnostic = new Diagnostic(this._uName);
      this._sendData = new byte[0];
      this._recvData = new byte[0];
    }

    public override string ToString()
    {
      return this._uName;
    }

    public string Name
    {
      get
      {
        return this._uName;
      }
      set
      {
        this._uName = value;
        this._locDiagnostic.Name = this._uName;
      }
    }

    public MessageClientState State
    {
      get
      {
        return this._state;
      }
    }

    internal MessageClientState _State
    {
      get
      {
        return this._state;
      }
      set
      {
        this._state = value;
      }
    }

    internal int ID
    {
      get
      {
        return this._uID;
      }
    }

    public int SendDataLength
    {
      get
      {
        return this._sendDataLength;
      }
    }

    public byte[] SendData
    {
      get
      {
        lock (this._sendData)
          return this._sendData;
      }
    }

    public DateTime SendDataTime
    {
      get
      {
        return this._sendDataTime;
      }
      set
      {
        this._sendDataTime = value;
      }
    }

    public DateTime EstimatedReceiveDataTime
    {
      get
      {
        return this._sendDataTime.AddSeconds((double) this._recvDataTimeout);
      }
    }

    public int Timeout
    {
      get
      {
        return this._recvDataTimeout;
      }
      set
      {
        if (value <= 0)
          return;
        this._recvDataTimeout = value;
      }
    }

    internal byte[] _ReceiveData
    {
      get
      {
        lock (this._recvData)
          return this._recvData;
      }
      set
      {
        if (value == null)
          return;
        lock (this._recvData)
          this._recvData = value;
      }
    }

    public byte[] ReceiveRequest
    {
      get
      {
        lock (this._recvData)
          return this._recvData;
      }
    }

    internal void SetActiv()
    {
      this.ClearSendData();
      this.ClearReceiveData();
      this.ActivateDiagnostic = true;
    }

    internal void SetInactiv()
    {
      this.ActivateDiagnostic = false;
    }

    public bool SendRequest(byte[] Data, int Length)
    {
      if (this._state == MessageClientState.SendRequest || this._state == MessageClientState.SendRequestOnly || (this._state == MessageClientState.WaitingForConfirmation || Data == null) || Length <= 0)
        return false;
      lock (this._sendData)
      {
        this._sendData = Data;
        this._sendDataLength = Length;
        this._state = MessageClientState.SendRequest;
      }
      return true;
    }

    public bool SendRequest(byte[] Data)
    {
      return this.SendRequest(Data, Data.Length);
    }

    public bool SendRequestOnly(byte[] Data, int Length)
    {
      if (this._state == MessageClientState.SendRequest || this._state == MessageClientState.SendRequestOnly || (this._state == MessageClientState.WaitingForConfirmation || Data == null) || Length <= 0)
        return false;
      lock (this._sendData)
      {
        this._sendData = Data;
        this._sendDataLength = Length;
        this._state = MessageClientState.SendRequestOnly;
      }
      return true;
    }

    public bool SendRequestOnly(byte[] Data)
    {
      return this.SendRequestOnly(Data, Data.Length);
    }

    public void ClearReceiveData()
    {
      lock (this._recvData)
      {
        if (this._recvData.Length > 0)
          this._recvData = new byte[0];
        this._state = MessageClientState.Idle;
      }
    }

    internal void ClearSendData()
    {
      lock (this._sendData)
      {
        if (this._sendData.Length > 0)
          this._sendData = new byte[0];
        this._sendDataLength = 0;
        this._sendDataTime = DateTime.Now;
        this._state = MessageClientState.Idle;
      }
    }

    public event ConfirmationReceiveHandler OnConfirmationReceived
    {
      add
      {
        this._confirmationReceived += value;
      }
      remove
      {
        this._confirmationReceived -= value;
      }
    }

    internal void CallConfirmationReceivedEvent(object Sender)
    {
      if (this._confirmationReceived == null)
        return;
      this._confirmationReceived(Sender, this.ReceiveRequest);
    }

    internal bool ConfirmationReceivedDelegateValid
    {
      get
      {
        return this._confirmationReceived != null;
      }
    }

    public bool ActivateDiagnostic
    {
      get
      {
        return this._locDiagnostic.Activate;
      }
      set
      {
        this._locDiagnostic.Activate = value;
      }
    }

    internal void SetDiagnostic(MessageClientDiagnostic DiagCode, byte[] AddDiagCode)
    {
      if (this._locDiagnostic.SetDiagnostic((object) this, (Enum) DiagCode, AddDiagCode))
        this._state = MessageClientState.Idle;
      else
        this._state = MessageClientState.Error;
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
  }
}
