// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Utility.Trace
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System.Diagnostics;

namespace uPLibrary.Networking.M2Mqtt.Utility
{
  public static class Trace
  {
    public static TraceLevel TraceLevel;
    public static WriteTrace TraceListener;

    [Conditional("DEBUG")]
    public static void Debug(string format, params object[] args)
    {
      if (Trace.TraceListener == null)
        return;
      Trace.TraceListener(format, args);
    }

    public static void WriteLine(TraceLevel level, string format)
    {
      if (Trace.TraceListener == null || (level & Trace.TraceLevel) <= (TraceLevel) 0)
        return;
      Trace.TraceListener(format, new object[0]);
    }

    public static void WriteLine(TraceLevel level, string format, object arg1)
    {
      if (Trace.TraceListener == null || (level & Trace.TraceLevel) <= (TraceLevel) 0)
        return;
      Trace.TraceListener(format, new object[1]{ arg1 });
    }

    public static void WriteLine(TraceLevel level, string format, object arg1, object arg2)
    {
      if (Trace.TraceListener == null || (level & Trace.TraceLevel) <= (TraceLevel) 0)
        return;
      Trace.TraceListener(format, new object[2]
      {
        arg1,
        arg2
      });
    }

    public static void WriteLine(TraceLevel level, string format, object arg1, object arg2, object arg3)
    {
      if (Trace.TraceListener == null || (level & Trace.TraceLevel) <= (TraceLevel) 0)
        return;
      Trace.TraceListener(format, new object[3]
      {
        arg1,
        arg2,
        arg3
      });
    }
  }
}
