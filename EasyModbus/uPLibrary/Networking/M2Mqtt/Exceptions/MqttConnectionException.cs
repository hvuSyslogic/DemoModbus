// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Exceptions.MqttConnectionException
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;

namespace uPLibrary.Networking.M2Mqtt.Exceptions
{
  public class MqttConnectionException : Exception
  {
    public MqttConnectionException(string message, Exception innerException)
      : base(message, innerException)
    {
    }
  }
}
