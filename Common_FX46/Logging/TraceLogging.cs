// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.Logging.TraceLogging
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Diagnostics;
using System.Text;

namespace PhoenixContact.Common.Logging
{
  public class TraceLogging
  {
    private static readonly object ExtLock = new object();
    private readonly Type _usingClassType;
    private StringBuilder _writeBuffer;

    internal TraceLogging(Type type)
    {
      lock (TraceLogging.ExtLock)
        this._usingClassType = type;
    }

    public void Info(object message, Exception ex = null)
    {
      lock (TraceLogging.ExtLock)
        this.WriteMessage(TraceLogging.Level.Information, message, ex);
    }

    public void Warn(object message, Exception ex = null)
    {
      lock (TraceLogging.ExtLock)
        this.WriteMessage(TraceLogging.Level.Warning, message, ex);
    }

    public void Error(object message, Exception ex = null)
    {
      lock (TraceLogging.ExtLock)
        this.WriteMessage(TraceLogging.Level.Error, message, ex);
    }

    public void StartWriteBlock(string text)
    {
      lock (TraceLogging.ExtLock)
      {
        if (this._writeBuffer == null)
          this._writeBuffer = new StringBuilder();
        this._writeBuffer.Append(text);
      }
    }

    public void EndWriteBlock()
    {
      lock (TraceLogging.ExtLock)
      {
        if (this._writeBuffer == null)
          return;
        Trace.WriteLine(this._writeBuffer.ToString());
        Trace.Flush();
        this._writeBuffer = (StringBuilder) null;
      }
    }

    private void WriteMessage(TraceLogging.Level level, object message, Exception ex)
    {
      string message1;
      if (ex == null)
        message1 = string.Format("[{0}]: {1} | {2} | {3}", new object[4]
        {
          (object) level,
          (object) DateTime.Now,
          (object) this._usingClassType,
          message
        });
      else
        message1 = string.Format("[{0}]: {1} | {2} | {3} | {4}", (object) level, (object) DateTime.Now, (object) this._usingClassType, message, (object) ex);
      Trace.WriteLine(message1);
      Trace.Flush();
    }

    public enum Level
    {
      Information,
      Warning,
      Error,
    }
  }
}
