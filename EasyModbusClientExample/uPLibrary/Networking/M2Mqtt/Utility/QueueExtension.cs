// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Utility.QueueExtension
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System.Collections;

namespace uPLibrary.Networking.M2Mqtt.Utility
{
  internal static class QueueExtension
  {
    internal static object Get(this Queue queue, QueueExtension.QueuePredicate predicate)
    {
      foreach (object obj in queue)
      {
        if (predicate(obj))
          return obj;
      }
      return (object) null;
    }

    internal delegate bool QueuePredicate(object item);
  }
}
