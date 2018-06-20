// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.MqttSslUtility
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

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
