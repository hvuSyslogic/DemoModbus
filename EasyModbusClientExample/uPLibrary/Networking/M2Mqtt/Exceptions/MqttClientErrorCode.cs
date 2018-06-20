// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Exceptions.MqttClientErrorCode
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

namespace uPLibrary.Networking.M2Mqtt.Exceptions
{
  public enum MqttClientErrorCode
  {
    WillWrong = 1,
    KeepAliveWrong = 2,
    TopicWildcard = 3,
    TopicLength = 4,
    QosNotAllowed = 5,
    TopicsEmpty = 6,
    QosLevelsEmpty = 7,
    TopicsQosLevelsNotMatch = 8,
    WrongBrokerMessage = 9,
    WrongMessageId = 10, // 0x0000000A
    InflightQueueFull = 11, // 0x0000000B
    InvalidFlagBits = 12, // 0x0000000C
    InvalidConnectFlags = 13, // 0x0000000D
    InvalidClientId = 14, // 0x0000000E
    InvalidProtocolName = 15, // 0x0000000F
  }
}
