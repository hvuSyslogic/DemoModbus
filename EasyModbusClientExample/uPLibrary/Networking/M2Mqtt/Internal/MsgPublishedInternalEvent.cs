// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Internal.MsgPublishedInternalEvent
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using uPLibrary.Networking.M2Mqtt.Messages;

namespace uPLibrary.Networking.M2Mqtt.Internal
{
  public class MsgPublishedInternalEvent : MsgInternalEvent
  {
    private bool isPublished;

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

    public MsgPublishedInternalEvent(MqttMsgBase msg, bool isPublished)
      : base(msg)
    {
      this.isPublished = isPublished;
    }
  }
}
