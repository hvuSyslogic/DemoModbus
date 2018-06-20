// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.MqttSslUtility
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Security.Authentication;

namespace uPLibrary.Networking.M2Mqtt
{
  public static class MqttSslUtility
  {
    public static SslProtocols ToSslPlatformEnum(MqttSslProtocols mqttSslProtocol)
    {
      switch (mqttSslProtocol)
      {
        case MqttSslProtocols.None:
          return SslProtocols.None;
        case MqttSslProtocols.SSLv3:
          return SslProtocols.Ssl3;
        case MqttSslProtocols.TLSv1_0:
          return SslProtocols.Tls;
        default:
          throw new ArgumentException("SSL/TLS protocol version not supported");
      }
    }
  }
}
