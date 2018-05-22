// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Diagnostic
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;
using System.IO;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public class Diagnostic : IDisposable
  {
    private string _uName;
    private bool _diagValid;
    private bool _devDiagActive;
    private DiagnosticArgs _actualError;
    private DiagnosticHandler _dhOnDiagnostic;
    private bool _activateLogging;
    private string _logFileName;

    public Diagnostic(string Name)
    {
      this._uName = Name;
      this._actualError.Name = Name;
      this._diagValid = false;
      this._activateLogging = false;
      this._logFileName = Name + ".log";
    }

    public override string ToString()
    {
      return this._uName;
    }

    public bool Activate
    {
      get
      {
        return this._devDiagActive;
      }
      set
      {
        if (value && !this._devDiagActive)
          this.LogMessage(this._logFileName, "Activate logging");
        if (!value && this._devDiagActive)
          this.LogMessage(this._logFileName, "Deactivate logging");
        this._devDiagActive = value;
        this.GetDiagnostic();
      }
    }

    public bool MessageValid
    {
      get
      {
        return this._diagValid;
      }
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
        this._actualError.Name = this._uName;
        this._logFileName = this._uName;
      }
    }

    public int DiagCode
    {
      get
      {
        int num = 0;
        if (this._actualError.DiagCode != null)
          num = Convert.ToInt32((object) this._actualError.DiagCode);
        return num;
      }
    }

    public bool ErrorLoggingActivate
    {
      get
      {
        return this._activateLogging;
      }
      set
      {
        this._activateLogging = value;
      }
    }

    public string ErrorLoggingFileName
    {
      get
      {
        return this._logFileName;
      }
      set
      {
        this._logFileName = value;
      }
    }

    public bool SetDiagnostic(object Sender, DiagnosticArgs DiagnosticCode)
    {
      return this.setDiagnostic(Sender, DiagnosticCode.DiagCode, DiagnosticCode.AddDiagCode, DiagnosticCode.Name);
    }

    public bool SetDiagnostic(object Sender, Enum DiagCode, byte[] AddDiagCode)
    {
      return this.setDiagnostic(Sender, DiagCode, AddDiagCode, this.Name);
    }

    private bool setDiagnostic(object Sender, Enum DiagCode, byte[] AddDiagCode, string Name)
    {
      bool flag = false;
      lock (this)
      {
        if (DiagCode != null)
        {
          if (this._devDiagActive)
          {
            this._actualError.Name = Name;
            this._actualError.DiagCode = DiagCode;
            this._actualError.AddDiagCode = AddDiagCode;
            this._actualError.DateTime = DateTime.Now;
            this._diagValid = true;
            if (this._activateLogging)
              this.LogError(this._logFileName, Sender, this._actualError);
            if (this._dhOnDiagnostic != null)
            {
              flag = true;
              this._dhOnDiagnostic(Sender, this.GetDiagnostic());
            }
          }
        }
      }
      return flag;
    }

    private DiagnosticArgs GetDiagnostic()
    {
      DiagnosticArgs diagnosticArgs;
      diagnosticArgs.Name = "";
      diagnosticArgs.DiagCode = (Enum) null;
      diagnosticArgs.AddDiagCode = new byte[0];
      diagnosticArgs.DateTime = DateTime.Now;
      if (this._actualError.DiagCode != null)
      {
        diagnosticArgs.Name = this._actualError.Name;
        diagnosticArgs.DiagCode = this._actualError.DiagCode;
        diagnosticArgs.AddDiagCode = this._actualError.AddDiagCode;
        diagnosticArgs.DateTime = this._actualError.DateTime;
      }
      this._actualError.DiagCode = !this._devDiagActive ? (Enum) Diagnostic.DiagnosticStates.Idle : (Enum) Diagnostic.DiagnosticStates.Active;
      this._actualError.Name = this._uName;
      this._actualError.AddDiagCode = new byte[0];
      this._actualError.DateTime = DateTime.Now;
      if (diagnosticArgs.DiagCode == null)
      {
        diagnosticArgs.Name = this._actualError.Name;
        diagnosticArgs.DiagCode = this._actualError.DiagCode;
        diagnosticArgs.AddDiagCode = this._actualError.AddDiagCode;
        diagnosticArgs.DateTime = this._actualError.DateTime;
      }
      this._diagValid = false;
      return diagnosticArgs;
    }

    private bool LogMessage(string FileName, string Message)
    {
      if (FileName == null || !this._activateLogging)
        return false;
      if (FileName.Length == 0)
        return false;
      try
      {
        FileStream fileStream = new FileStream(FileName, FileMode.Append);
        StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
        if (streamWriter != null)
        {
          streamWriter.WriteLine("//****************************************************************");
          streamWriter.WriteLine("Text    : " + Message);
          streamWriter.WriteLine("Date    : " + DateTime.Now.ToString());
          streamWriter.WriteLine();
        }
        streamWriter.Flush();
        streamWriter.Close();
        fileStream.Close();
        return true;
      }
      catch
      {
        this._activateLogging = false;
        return false;
      }
    }

    private bool LogError(string FileName, object Sender, DiagnosticArgs ErrorCode)
    {
      if (FileName == null || !this._activateLogging)
        return false;
      if (FileName.Length == 0)
        return false;
      try
      {
        FileStream fileStream = new FileStream(FileName, FileMode.Append);
        StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
        if (streamWriter != null)
        {
          streamWriter.WriteLine("//****************************************************************");
          streamWriter.WriteLine("Sender           : " + Sender.ToString());
          streamWriter.WriteLine("Name             : " + ErrorCode.Name);
          streamWriter.WriteLine("Date             : " + ErrorCode.DateTime.ToString());
          streamWriter.WriteLine("DiagCode         : " + ErrorCode.DiagCode.ToString());
          streamWriter.WriteLine("AddDiagCode (hex): " + Util.ByteArrayToHexStringW(ErrorCode.AddDiagCode, ' '));
          streamWriter.WriteLine();
        }
        streamWriter.Flush();
        streamWriter.Close();
        fileStream.Close();
        return true;
      }
      catch
      {
        this._activateLogging = false;
        return false;
      }
    }

    public event DiagnosticHandler OnDiagnostic
    {
      add
      {
        this._dhOnDiagnostic += value;
      }
      remove
      {
        this._dhOnDiagnostic -= value;
      }
    }

    public void Dispose()
    {
      this._diagValid = false;
      this._actualError.DiagCode = (Enum) null;
      this._dhOnDiagnostic = (DiagnosticHandler) null;
      GC.SuppressFinalize((object) this);
    }

    private enum DiagnosticStates
    {
      Idle = 0,
      Active = 33536, // 0x00008300
    }
  }
}
