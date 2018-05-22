// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.Logging.TraceLoggingFactory
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Diagnostics;
using System.IO;

namespace PhoenixContact.Common.Logging
{
  public static class TraceLoggingFactory
  {
    private static readonly object ExtLock = new object();

    public static string LogFilePath { get; private set; }

    public static TraceLogging GetLogger(Type type)
    {
      lock (TraceLoggingFactory.ExtLock)
        return new TraceLogging(type);
    }

    public static bool SetLogFile(string filePathAndName)
    {
      if (string.IsNullOrEmpty(filePathAndName))
        return false;
      try
      {
        Trace.Listeners.Add((TraceListener) new TextWriterTraceListener((Stream) File.Create(filePathAndName)));
        TraceLoggingFactory.LogFilePath = filePathAndName;
        return true;
      }
      catch (Exception ex)
      {
        TraceLoggingFactory.LogFilePath = (string) null;
        return false;
      }
    }
  }
}
