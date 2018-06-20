// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgPublishEventArgs : EventArgs
  {
    private string topic;
    private byte[] message;
    private bool dupFlag;
    private byte qosLevel;
    private bool retain;

    public string Topic
    {
      get
      {
        return this.topic;
      }
      internal set
      {
        this.topic = value;
      }
    }

    public byte[] Message
    {
      get
      {
        return this.message;
      }
      internal set
      {
        this.message = value;
      }
    }

    public bool DupFlag
    {
      get
      {
        return this.dupFlag;
      }
      set
      {
        this.dupFlag = value;
      }
    }

    public byte QosLevel
    {
      get
      {
        return this.qosLevel;
      }
      internal set
      {
        this.qosLevel = value;
      }
    }

    public bool Retain
    {
      get
      {
        return this.retain;
      }
      internal set
      {
        this.retain = value;
      }
    }

    public MqttMsgPublishEventArgs(string topic, byte[] message, bool dupFlag, byte qosLevel, bool retain)
    {
      this.topic = topic;
      this.message = message;
      this.dupFlag = dupFlag;
      this.qosLevel = qosLevel;
      this.retain = retain;
    }
  }
}
