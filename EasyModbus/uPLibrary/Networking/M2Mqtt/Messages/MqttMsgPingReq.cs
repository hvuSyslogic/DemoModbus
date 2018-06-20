// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPingReq
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgPingReq : MqttMsgBase
  {
    public MqttMsgPingReq()
    {
      this.type = (byte) 12;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      byte[] numArray1 = new byte[2];
      int num1 = 0;
      int num2;
      if (protocolVersion == (byte) 4)
      {
        byte[] numArray2 = numArray1;
        int index = num1;
        num2 = checked (index + 1);
        int num3 = 192;
        numArray2[index] = (byte) num3;
      }
      else
      {
        byte[] numArray2 = numArray1;
        int index = num1;
        num2 = checked (index + 1);
        int num3 = 192;
        numArray2[index] = (byte) num3;
      }
      byte[] numArray3 = numArray1;
      int index1 = num2;
      int num4 = checked (index1 + 1);
      int num5 = 0;
      numArray3[index1] = (byte) num5;
      return numArray1;
    }

    public static MqttMsgPingReq Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      MqttMsgPingReq mqttMsgPingReq = new MqttMsgPingReq();
      if (protocolVersion == (byte) 4 && ((uint) fixedHeaderFirstByte & 15U) > 0U)
        throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
      MqttMsgBase.decodeRemainingLength(channel);
      return mqttMsgPingReq;
    }

    public override string ToString()
    {
      return this.GetTraceString("PINGREQ", (object[]) null, (object[]) null);
    }
  }
}
