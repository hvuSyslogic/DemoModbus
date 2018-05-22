// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.PCP
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using PhoenixContact.PxC_Library.Util;
using System;

namespace PhoenixContact.HFI.Inline
{
  public class PCP
  {
    private MessageClient _locMessage;
    private Diagnostic _diagnostic;
    private string _name;
    private bool _activate;
    private bool _ready;
    private bool _error;
    private PCP.LastService _lastService;
    private bool _writeDataDone;
    private bool _writeDataError;
    private bool _readDataValid;
    private bool _readDataError;
    private byte[] _readDataConfirmation;
    private int _commReference;
    private int _invokeID;
    private ReadConfirmationReceiveHandler _hdReadConfirmationReceived;
    private WriteConfirmationReceiveHandler _hdWriteConfirmationReceived;
    private EnableReadyHandler _hdEanbleReady;

    public PCP(string Name, int CR)
    {
      if (CR > 1)
        this._commReference = CR;
      this._locMessage = new MessageClient(Name);
      if (this._name == null)
        this._name = this._locMessage.Name;
      this._locMessage.OnException += new ExceptionHandler(this._locMessage_OnException);
      this._locMessage.OnConfirmationReceived += new ConfirmationReceiveHandler(this._locMessage_OnConfirmationReceived);
      this._diagnostic = new Diagnostic(this.Name);
    }

    public override string ToString()
    {
      return this._name;
    }

    public MessageClient ControllerConnection
    {
      get
      {
        return this._locMessage;
      }
    }

    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        if (string.IsNullOrEmpty(value))
          return;
        this._name = value;
        this._locMessage.Name = this._name;
        this._diagnostic.Name = this._name;
      }
    }

    public int CommReference
    {
      get
      {
        return this._commReference;
      }
      set
      {
        if (value <= 1 || value >= 256)
          return;
        this._commReference = value;
      }
    }

    public int InvokeID
    {
      get
      {
        return this._invokeID;
      }
      set
      {
        if (value < 0 || value >= 256)
          return;
        this._invokeID = value;
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

    public bool WriteDataDone
    {
      get
      {
        return this._writeDataDone;
      }
    }

    public bool WriteDataError
    {
      get
      {
        return this._writeDataError;
      }
    }

    public bool ReadDataValid
    {
      get
      {
        return this._readDataValid;
      }
    }

    public bool ReadDataError
    {
      get
      {
        return this._readDataError;
      }
    }

    public byte[] ReadData
    {
      get
      {
        return this._readDataConfirmation;
      }
    }

    public int Timeout
    {
      get
      {
        return this._locMessage.Timeout;
      }
      set
      {
        this._locMessage.Timeout = value;
      }
    }

    public event ReadConfirmationReceiveHandler OnReadConfirmationReceived
    {
      add
      {
        this._hdReadConfirmationReceived += value;
      }
      remove
      {
        this._hdReadConfirmationReceived -= value;
      }
    }

    public event WriteConfirmationReceiveHandler OnWriteConfirmationReceived
    {
      add
      {
        this._hdWriteConfirmationReceived += value;
      }
      remove
      {
        this._hdWriteConfirmationReceived -= value;
      }
    }

    public event EnableReadyHandler OnEnableReady
    {
      add
      {
        this._hdEanbleReady += value;
      }
      remove
      {
        this._hdEanbleReady -= value;
      }
    }

    public bool Enable()
    {
      bool flag = false;
      if (!this._activate && this._locMessage.ControllerId != 0)
      {
        this._activate = true;
        this._ready = false;
        this._error = false;
        this._readDataValid = false;
        this._readDataError = false;
        this._readDataConfirmation = new byte[0];
        this._writeDataDone = false;
        this._writeDataError = false;
        this._diagnostic.Enable();
        byte[] data = new byte[8]
        {
          (byte) 0,
          (byte) 139,
          (byte) 0,
          (byte) 2,
          (byte) 0,
          Convert.ToByte(this._commReference & (int) byte.MaxValue),
          (byte) 0,
          (byte) 0
        };
        if (flag = this._locMessage.SendRequest(data))
          this._lastService = PCP.LastService.Initiate;
      }
      return flag;
    }

    public bool Disable()
    {
      if (this._activate)
      {
        this._activate = false;
        this._readDataValid = false;
        this._readDataError = false;
        this._readDataConfirmation = new byte[0];
        this._writeDataDone = false;
        this._writeDataError = false;
        this._diagnostic.Disable();
      }
      int num = this._locMessage.SendRequestOnly(new byte[8]
      {
        (byte) 8,
        (byte) 141,
        (byte) 0,
        (byte) 2,
        (byte) 0,
        Convert.ToByte(this._commReference & (int) byte.MaxValue),
        (byte) 0,
        (byte) 0
      }) ? 1 : 0;
      if (num != 0)
        this._lastService = PCP.LastService.Abort;
      this._ready = false;
      this._error = false;
      return num != 0;
    }

    public bool WriteRequest(int Index, int Subindex, int ByteLength, byte[] Data)
    {
      bool flag = false;
      int num = 0;
      if (this._activate && this._lastService == PCP.LastService.Idle)
      {
        this._writeDataDone = false;
        this._writeDataError = false;
        if (ByteLength > 0 && Data != null)
          num = ByteLength % 2 != 0 ? ByteLength + 1 : ByteLength;
        int Integer = 3 + num / 2;
        byte[] data = new byte[4 + Integer * 2];
        data[0] = (byte) 0;
        data[1] = (byte) 130;
        data[2] = PhoenixContact.PxC_Library.Util.Util.Int32ToByte(Integer, 8);
        data[3] = PhoenixContact.PxC_Library.Util.Util.Int32ToByte(Integer, 0);
        data[4] = Convert.ToByte(this._invokeID & (int) byte.MaxValue);
        data[5] = Convert.ToByte(this._commReference & (int) byte.MaxValue);
        data[6] = PhoenixContact.PxC_Library.Util.Util.Int32ToByte(Index, 8);
        data[7] = PhoenixContact.PxC_Library.Util.Util.Int32ToByte(Index, 0);
        data[8] = Convert.ToByte(Subindex & (int) byte.MaxValue);
        data[9] = Convert.ToByte(ByteLength & (int) byte.MaxValue);
        if (num <= Data.Length)
        {
          for (int index = 0; index < num; ++index)
            data[10 + index] = Data[index];
        }
        if (num > Data.Length)
        {
          for (int index = 0; index < Data.Length; ++index)
            data[10 + index] = Data[index];
        }
        if (flag = this._locMessage.SendRequest(data))
          this._lastService = PCP.LastService.WriteRequest;
      }
      return flag;
    }

    public bool WriteRequest(int Index, int Subindex, byte[] Data)
    {
      if (Data != null)
        return this.WriteRequest(Index, Subindex, Data.Length, Data);
      return false;
    }

    public bool ReadRequest(int Index, int Subindex)
    {
      bool flag = false;
      if (this._activate && this._lastService == PCP.LastService.Idle)
      {
        this._readDataValid = false;
        this._readDataError = false;
        byte[] data = new byte[10]
        {
          (byte) 0,
          (byte) 129,
          (byte) 0,
          (byte) 3,
          Convert.ToByte(this._invokeID & (int) byte.MaxValue),
          Convert.ToByte(this._commReference & (int) byte.MaxValue),
          PhoenixContact.PxC_Library.Util.Util.Int32ToByte(Index, 8),
          PhoenixContact.PxC_Library.Util.Util.Int32ToByte(Index, 0),
          Convert.ToByte(Subindex & (int) byte.MaxValue),
          (byte) 0
        };
        if (flag = this._locMessage.SendRequest(data))
          this._lastService = PCP.LastService.ReadRequest;
      }
      return flag;
    }

    public event ExceptionHandler OnException
    {
      add
      {
        this._diagnostic.OnException += value;
      }
      remove
      {
        this._diagnostic.OnException -= value;
      }
    }

    private void _locMessage_OnException(Exception ExceptionData)
    {
      switch (this._lastService)
      {
        case PCP.LastService.ReadRequest:
          this._readDataError = true;
          this._readDataValid = false;
          break;
        case PCP.LastService.WriteRequest:
          this._writeDataError = true;
          this._writeDataDone = false;
          break;
        default:
          this._error = true;
          this._ready = false;
          break;
      }
      this._lastService = PCP.LastService.Idle;
      this._diagnostic.Throw((Enum) PCP_Diagnostic.MessageClientDiagnostic, new byte[0], ExceptionData);
      this._diagnostic.Quit();
    }

    private void _locMessage_OnConfirmationReceived(object Sender, byte[] Data)
    {
      if (!this._activate)
        return;
      if (PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Data[0], Data[1]) == 32907)
      {
        if (PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Data[6], Data[7]) == 0)
        {
          this._ready = true;
          this._error = false;
          this._lastService = PCP.LastService.Idle;
          if (this._hdEanbleReady == null)
            return;
          this._hdEanbleReady((object) this);
        }
        else
        {
          this._error = true;
          this._ready = false;
          this._diagnostic.Throw((Enum) PCP_Diagnostic.NegativeInitiateConfirmation, PhoenixContact.PxC_Library.Util.Util.GetByteArrayFromService(Data));
          this._diagnostic.Quit();
          this._lastService = PCP.LastService.Idle;
        }
      }
      else if (PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Data[0], Data[1]) == 32898)
      {
        if (PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Data[6], Data[7]) == 0)
        {
          this._writeDataDone = true;
          this._writeDataError = false;
          this._lastService = PCP.LastService.Idle;
          if (this._hdWriteConfirmationReceived == null)
            return;
          this._hdWriteConfirmationReceived((object) this);
        }
        else
        {
          this._writeDataDone = false;
          this._writeDataError = true;
          this._diagnostic.Throw((Enum) PCP_Diagnostic.NegativeWriteConfirmation, PhoenixContact.PxC_Library.Util.Util.GetByteArrayFromService(Data));
          this._diagnostic.Quit();
          this._lastService = PCP.LastService.Idle;
        }
      }
      else if (PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Data[0], Data[1]) == 32897)
      {
        if (PhoenixContact.PxC_Library.Util.Util.ByteToInt32(Data[6], Data[7]) == 0)
        {
          this._readDataConfirmation = new byte[(int) Data[9]];
          for (int index = 0; index < this._readDataConfirmation.Length; ++index)
            this._readDataConfirmation[index] = Data[index + 10];
          this._readDataValid = true;
          this._readDataError = false;
          this._lastService = PCP.LastService.Idle;
          if (this._hdReadConfirmationReceived == null)
            return;
          this._hdReadConfirmationReceived((object) this, this._readDataConfirmation);
        }
        else
        {
          this._readDataValid = false;
          this._readDataError = true;
          this._diagnostic.Throw((Enum) PCP_Diagnostic.NegativeReadConfirmation, PhoenixContact.PxC_Library.Util.Util.GetByteArrayFromService(Data));
          this._diagnostic.Quit();
          this._lastService = PCP.LastService.Idle;
        }
      }
      else
      {
        this._lastService = PCP.LastService.Idle;
        this._error = true;
        this._ready = false;
        this._diagnostic.Throw((Enum) PCP_Diagnostic.UnexpectedService, PhoenixContact.PxC_Library.Util.Util.GetByteArrayFromService(Data));
        this._diagnostic.Quit();
      }
    }

    private enum LastService
    {
      Idle,
      Initiate,
      ReadRequest,
      WriteRequest,
      Abort,
    }
  }
}
