// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.Diagnostic
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Globalization;
using System.Text;

namespace PhoenixContact.PxC_Library.Util
{
  public class Diagnostic : IDisposable
  {
    public static string ExDataDateTime = "DateTime";
    public static string ExDataAddMessage = "AddMessage";
    public static int GetExceptionCount = 0;
    private readonly object _syncObj = new object();
    private bool _exHandlingActive;
    private bool _exception;
    private ExceptionHandler _hdOnException;
    private bool _ready;
    private string _uName;
    private bool disposed;

    public Diagnostic(string Name)
    {
      this._uName = Name;
      this._ready = false;
      this._exception = false;
    }

    public override string ToString()
    {
      return this._uName;
    }

    public static DateTime GetDateTime(System.Exception ExceptionData)
    {
      object obj;
      if ((obj = ExceptionData.Data[(object) Diagnostic.ExDataDateTime]) != null)
        return (DateTime) obj;
      return DateTime.MinValue;
    }

    public static byte[] GetAddMessage(System.Exception ExceptionData)
    {
      object obj;
      if ((obj = ExceptionData.Data[(object) Diagnostic.ExDataAddMessage]) != null)
        return (byte[]) obj;
      return new byte[0];
    }

    public static string GetExceptionMessage(System.Exception ExceptionData)
    {
      if (ExceptionData == null)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(ExceptionData.Message);
      stringBuilder.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.ErrorSource, (object) ExceptionData.Source));
      stringBuilder.AppendLine();
      if (Diagnostic.GetAddMessage(ExceptionData).Length != 0)
      {
        string hexStringW = PhoenixContact.PxC_Library.Util.Util.ByteArrayToHexStringW(Diagnostic.GetAddMessage(ExceptionData), ' ');
        stringBuilder.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.AddErrorCode, (object) hexStringW));
        stringBuilder.AppendLine();
      }
      if (ExceptionData.InnerException != null)
      {
        stringBuilder.AppendLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.InnerErrors, (object) ++Diagnostic.GetExceptionCount));
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(Diagnostic.GetExceptionMessage(ExceptionData.InnerException));
      }
      else
      {
        if (Diagnostic.GetDateTime(ExceptionData) != DateTime.MinValue)
          stringBuilder.Append(string.Format((IFormatProvider) CultureInfo.InvariantCulture, PxCUtilStrings.Timestamp, (object) Diagnostic.GetDateTime(ExceptionData).ToString()));
        Diagnostic.GetExceptionCount = 0;
      }
      return stringBuilder.ToString();
    }

    public static System.Exception NewException(string Name, string Message)
    {
      return Diagnostic.NewException(Name, Message, (byte[]) null, (System.Exception) null);
    }

    public static System.Exception NewException(string Name, string Message, System.Exception InnerException)
    {
      return Diagnostic.NewException(Name, Message, (byte[]) null, InnerException);
    }

    public static System.Exception NewException(string Name, string Message, byte[] Data)
    {
      return Diagnostic.NewException(Name, Message, Data, (System.Exception) null);
    }

    public static System.Exception NewException(string Name, string Message, byte[] Data, System.Exception InnerException)
    {
      System.Exception exception = InnerException == null ? new System.Exception(Message) : new System.Exception(Message, InnerException);
      exception.Source = Name;
      exception.Data[(object) Diagnostic.ExDataDateTime] = (object) DateTime.Now;
      if (Data == null)
        exception.Data[(object) Diagnostic.ExDataAddMessage] = (object) new byte[0];
      else
        exception.Data[(object) Diagnostic.ExDataAddMessage] = (object) Data;
      return exception;
    }

    public bool Ready
    {
      get
      {
        return this._ready;
      }
    }

    public bool Exception
    {
      get
      {
        return this._exception;
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
        lock (this._syncObj)
          this._uName = value;
      }
    }

    public bool Enable()
    {
      if (this._exHandlingActive)
        return false;
      lock (this._syncObj)
      {
        this._ready = true;
        this._exception = false;
        this._exHandlingActive = true;
        return true;
      }
    }

    public void Disable()
    {
      lock (this._syncObj)
      {
        this._ready = false;
        this._exception = false;
        this._exHandlingActive = false;
      }
    }

    public bool Quit()
    {
      if (!this._exHandlingActive)
        return false;
      lock (this._syncObj)
      {
        this._exception = false;
        return true;
      }
    }

    public bool Throw(Enum Message, byte[] AddMessage)
    {
      return this.Throw(Message.ToString(), AddMessage);
    }

    public bool Throw(string Message, byte[] AddMessage)
    {
      if (this._exHandlingActive && this._hdOnException != null)
      {
        lock (this._syncObj)
        {
          if (!this._exception)
          {
            System.Exception ExceptionData = new System.Exception(Message);
            ExceptionData.Source = this._uName;
            ExceptionData.Data[(object) Diagnostic.ExDataDateTime] = (object) DateTime.Now;
            ExceptionData.Data[(object) Diagnostic.ExDataAddMessage] = (object) AddMessage;
            this._exception = true;
            this._hdOnException(ExceptionData);
            return true;
          }
        }
      }
      return false;
    }

    public bool Throw(System.Exception ExceptionData)
    {
      if (this._exHandlingActive && this._hdOnException != null)
      {
        lock (this._syncObj)
        {
          if (!this._exception)
          {
            System.Exception ExceptionData1 = ExceptionData.InnerException != null ? new System.Exception(ExceptionData.Message, ExceptionData.InnerException) : new System.Exception(ExceptionData.Message);
            try
            {
              ExceptionData1.Source = ExceptionData.Source;
            }
            catch
            {
            }
            try
            {
              ExceptionData1.Data[(object) Diagnostic.ExDataDateTime] = ExceptionData.Data[(object) Diagnostic.ExDataDateTime];
            }
            catch
            {
            }
            try
            {
              ExceptionData1.Data[(object) Diagnostic.ExDataAddMessage] = ExceptionData.Data[(object) Diagnostic.ExDataAddMessage];
            }
            catch
            {
            }
            this._exception = true;
            this._hdOnException(ExceptionData1);
            return true;
          }
        }
      }
      return false;
    }

    public bool Throw(Enum Message, byte[] AddMessage, System.Exception InnerException)
    {
      return this.Throw(Message.ToString(), AddMessage, InnerException);
    }

    public bool Throw(string Message, byte[] AddMessage, System.Exception InnerException)
    {
      if (this._exHandlingActive && this._hdOnException != null)
      {
        lock (this._syncObj)
        {
          if (!this._exception)
          {
            System.Exception ExceptionData = new System.Exception(Message, InnerException);
            ExceptionData.Source = this._uName;
            ExceptionData.Data[(object) Diagnostic.ExDataDateTime] = (object) DateTime.Now;
            ExceptionData.Data[(object) Diagnostic.ExDataAddMessage] = (object) AddMessage;
            this._exception = true;
            this._hdOnException(ExceptionData);
            return true;
          }
        }
      }
      return false;
    }

    public event ExceptionHandler OnException
    {
      add
      {
        this._hdOnException += value;
      }
      remove
      {
        lock (this._syncObj)
          this._hdOnException -= value;
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        this._exHandlingActive = false;
        this._ready = false;
        this._exception = false;
        this._hdOnException = (ExceptionHandler) null;
      }
      this.disposed = true;
    }
  }
}
