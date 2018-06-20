// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Exceptions.MqttClientErrorCode
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

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
