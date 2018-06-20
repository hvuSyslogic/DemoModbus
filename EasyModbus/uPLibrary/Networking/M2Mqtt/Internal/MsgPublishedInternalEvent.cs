// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Internal.MsgPublishedInternalEvent
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

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
