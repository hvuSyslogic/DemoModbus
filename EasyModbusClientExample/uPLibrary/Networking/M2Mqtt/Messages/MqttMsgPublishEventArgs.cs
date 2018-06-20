// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

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
