// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgContext
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

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
