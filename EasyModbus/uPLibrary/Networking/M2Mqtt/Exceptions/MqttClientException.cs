// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Exceptions.MqttClientException
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;

namespace uPLibrary.Networking.M2Mqtt.Exceptions
{
  public class MqttClientException : Exception
  {
    private MqttClientErrorCode errorCode;

    public MqttClientException(MqttClientErrorCode errorCode)
    {
      this.errorCode = errorCode;
    }

    public MqttClientErrorCode ErrorCode
    {
      get
      {
        return this.errorCode;
      }
      set
      {
        this.errorCode = value;
      }
    }
  }
}
