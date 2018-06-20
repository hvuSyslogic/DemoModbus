// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishedEventArgs
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgPublishedEventArgs : EventArgs
  {
    private ushort messageId;
    private bool isPublished;

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

    public bool IsPublished
    {
      get
      {
        return this.isPublished;
      }
      internal set
      {
        this.isPublished = value;
      }
    }

    public MqttMsgPublishedEventArgs(ushort messageId)
      : this(messageId, true)
    {
    }

    public MqttMsgPublishedEventArgs(ushort messageId, bool isPublished)
    {
      this.messageId = messageId;
      this.isPublished = isPublished;
    }
  }
}
