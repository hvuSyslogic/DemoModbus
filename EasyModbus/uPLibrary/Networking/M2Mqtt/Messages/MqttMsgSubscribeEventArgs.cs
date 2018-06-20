// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribeEventArgs
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgSubscribeEventArgs : EventArgs
  {
    private ushort messageId;
    private string[] topics;
    private byte[] qosLevels;

    public ushort MessageId
    {
      get
      {
        return this.messageId;
      }
      internal set
      {
        this.messageId = value;
      }
    }

    public string[] Topics
    {
      get
      {
        return this.topics;
      }
      internal set
      {
        this.topics = value;
      }
    }

    public byte[] QoSLevels
    {
      get
      {
        return this.qosLevels;
      }
      internal set
      {
        this.qosLevels = value;
      }
    }

    public MqttMsgSubscribeEventArgs(ushort messageId, string[] topics, byte[] qosLevels)
    {
      this.messageId = messageId;
      this.topics = topics;
      this.qosLevels = qosLevels;
    }
  }
}
