// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgUnsubscribedEventArgs
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgUnsubscribedEventArgs : EventArgs
  {
    private ushort messageId;

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

    public MqttMsgUnsubscribedEventArgs(ushort messageId)
    {
      this.messageId = messageId;
    }
  }
}
