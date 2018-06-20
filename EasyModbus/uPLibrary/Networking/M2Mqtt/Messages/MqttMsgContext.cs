// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgContext
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgContext
  {
    public MqttMsgBase Message { get; set; }

    public MqttMsgState State { get; set; }

    public MqttMsgFlow Flow { get; set; }

    public int Timestamp { get; set; }

    public int Attempt { get; set; }

    public string Key
    {
      get
      {
        return ((int) this.Flow).ToString() + "_" + (object) this.Message.MessageId;
      }
    }
  }
}
