// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.PCP
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public class PCP : IDisposable
  {
    private MessageClient _locMessage;
    private Diagnostic _locDiagnostic;
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
      this._locMessage.OnDiagnostic += new DiagnosticHandler(this._locMessage_OnDiagnostic);
      this._locMessage.OnConfirmationReceived += new ConfirmationReceiveHandler(this._locMessage_OnConfirmationReceived);
      this._locDiagnostic = new Diagnostic(this.Name);
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
        this._name = value;
        this._locMessage.Name = this._name;
        this._locDiagnostic.Name = this._name;
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
      if (!this._activate)
      {
        this._activate = true;
        this._ready = false;
        this._error = false;
        this._readDataValid = false;
        this._readDataError = false;
        this._readDataConfirmation = new byte[0];
        this._writeDataDone = false;
        this._writeDataError = false;
        this._locDiagnostic.Activate = true;
        byte[] Data = new byte[8]
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
        if (flag = this._locMessage.SendRequest(Data))
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
        this._locDiagnostic.Activate = false;
      }
      byte[] Data = new byte[8]
      {
        (byte) 8,
        (byte) 141,
        (byte) 0,
        (byte) 2,
        (byte) 0,
        Convert.ToByte(this._commReference & (int) byte.MaxValue),
        (byte) 0,
        (byte) 0
      };
      bool flag;
      if (flag = this._locMessage.SendRequestOnly(Data))
        this._lastService = PCP.LastService.Abort;
      this._ready = false;
      this._error = false;
      return flag;
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
        byte[] Data1 = new byte[4 + Integer * 2];
        Data1[0] = (byte) 0;
        Data1[1] = (byte) 130;
        Data1[2] = Util.Int32ToByte(Integer, 8);
        Data1[3] = Util.Int32ToByte(Integer, 0);
        Data1[4] = Convert.ToByte(this._invokeID & (int) byte.MaxValue);
        Data1[5] = Convert.ToByte(this._commReference & (int) byte.MaxValue);
        Data1[6] = Util.Int32ToByte(Index, 8);
        Data1[7] = Util.Int32ToByte(Index, 0);
        Data1[8] = Convert.ToByte(Subindex & (int) byte.MaxValue);
        Data1[9] = Convert.ToByte(ByteLength & (int) byte.MaxValue);
        if (num <= Data.Length)
        {
          for (int index = 0; index < num; ++index)
            Data1[10 + index] = Data[index];
        }
        if (num > Data.Length)
        {
          for (int index = 0; index < Data.Length; ++index)
            Data1[10 + index] = Data[index];
        }
        if (flag = this._locMessage.SendRequest(Data1))
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
        byte[] Data = new byte[10]
        {
          (byte) 0,
          (byte) 129,
          (byte) 0,
          (byte) 3,
          Convert.ToByte(this._invokeID & (int) byte.MaxValue),
          Convert.ToByte(this._commReference & (int) byte.MaxValue),
          Util.Int32ToByte(Index, 8),
          Util.Int32ToByte(Index, 0),
          Convert.ToByte(Subindex & (int) byte.MaxValue),
          (byte) 0
        };
        if (flag = this._locMessage.SendRequest(Data))
          this._lastService = PCP.LastService.ReadRequest;
      }
      return flag;
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

    private void _locMessage_OnDiagnostic(object Sender, DiagnosticArgs DiagnosticCode)
    {
      this._locDiagnostic.SetDiagnostic((object) this, (Enum) (PCP_Diagnostic) DiagnosticCode.DiagCode, DiagnosticCode.AddDiagCode);
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
    }

    private void _locMessage_OnConfirmationReceived(object Sender, byte[] Data)
    {
      if (!this._activate)
        return;
      if (Util.ByteToInt32(Data[0], Data[1]) == 32907)
      {
        if (Util.ByteToInt32(Data[6], Data[7]) == 0)
        {
          this._ready = true;
          this._error = false;
          if (this._hdEanbleReady != null)
            this._hdEanbleReady((object) this);
        }
        else
        {
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) PCP_Diagnostic.NegativeInitiateConfirmation, Util.GetByteArrayFromService(Data));
          this._error = true;
          this._ready = false;
        }
        this._lastService = PCP.LastService.Idle;
      }
      else if (Util.ByteToInt32(Data[0], Data[1]) == 32898)
      {
        if (Util.ByteToInt32(Data[6], Data[7]) == 0)
        {
          this._writeDataDone = true;
          if (this._hdWriteConfirmationReceived != null)
            this._hdWriteConfirmationReceived((object) this);
        }
        else
        {
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) PCP_Diagnostic.NegativeWriteConfirmation, Util.GetByteArrayFromService(Data));
          this._writeDataError = true;
        }
        this._lastService = PCP.LastService.Idle;
      }
      else if (Util.ByteToInt32(Data[0], Data[1]) == 32897)
      {
        if (Util.ByteToInt32(Data[6], Data[7]) == 0)
        {
          this._readDataValid = true;
          this._readDataConfirmation = new byte[(int) Data[9]];
          for (int index = 0; index < this._readDataConfirmation.Length; ++index)
            this._readDataConfirmation[index] = Data[index + 10];
          if (this._hdReadConfirmationReceived != null)
            this._hdReadConfirmationReceived((object) this, this._readDataConfirmation);
        }
        else
        {
          this._locDiagnostic.SetDiagnostic((object) this, (Enum) PCP_Diagnostic.NegativeReadConfirmation, Util.GetByteArrayFromService(Data));
          this._readDataError = true;
        }
        this._lastService = PCP.LastService.Idle;
      }
      else
      {
        this._lastService = PCP.LastService.Idle;
        this._locDiagnostic.SetDiagnostic((object) this, (Enum) PCP_Diagnostic.UnexpectedService, Util.GetByteArrayFromService(Data));
        this._error = true;
        this._ready = false;
      }
    }

    public void Dispose()
    {
      if (this._locDiagnostic != null)
      {
        this._locDiagnostic.Dispose();
        this._locDiagnostic = (Diagnostic) null;
      }
      GC.SuppressFinalize((object) this);
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
