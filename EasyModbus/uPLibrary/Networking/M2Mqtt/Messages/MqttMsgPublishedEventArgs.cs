// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishedEventArgs
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

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
