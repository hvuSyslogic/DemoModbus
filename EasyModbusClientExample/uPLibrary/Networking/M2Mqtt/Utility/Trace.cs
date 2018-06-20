// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Utility.Trace
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

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
