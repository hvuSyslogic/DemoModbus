// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Internal.MsgInternalEvent
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using uPLibrary.Networking.M2Mqtt.Messages;

namespace uPLibrary.Networking.M2Mqtt.Internal
{
  public class MsgInternalEvent : InternalEvent
  {
    protected MqttMsgBase msg;

    public MqttMsgBase Message
    {
      get
      {
        return this.msg;
      }
      set
      {
        this.msg = value;
      }
    }

    public MsgInternalEvent(MqttMsgBase msg)
    {
      this.msg = msg;
    }
  }
}
