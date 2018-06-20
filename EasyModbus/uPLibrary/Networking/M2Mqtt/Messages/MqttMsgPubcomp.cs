// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPubcomp
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgPubcomp : MqttMsgBase
  {
    public MqttMsgPubcomp()
    {
      this.type = (byte) 7;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = checked (num1 + 2);
      int remainingLength = checked (num3 + num5 + num2);
      int num6 = 1;
      int num7 = remainingLength;
      do
      {
        checked { ++num6; }
        num7 /= 128;
      }
      while (num7 > 0);
      byte[] buffer = new byte[checked (num6 + num5 + num2)];
      int index1;
      if (protocolVersion == (byte) 4)
      {
        byte[] numArray = buffer;
        int index2 = num4;
        index1 = checked (index2 + 1);
        int num8 = 112;
        numArray[index2] = (byte) num8;
      }
      else
      {
        byte[] numArray = buffer;
        int index2 = num4;
        index1 = checked (index2 + 1);
        int num8 = 112;
        numArray[index2] = (byte) num8;
      }
      int num9 = this.encodeRemainingLength(remainingLength, buffer, index1);
      byte[] numArray1 = buffer;
      int index3 = num9;
      int num10 = checked (index3 + 1);
      int num11 = (int) checked ((byte) ((int) this.messageId >> 8 & (int) byte.MaxValue));
      numArray1[index3] = (byte) num11;
      byte[] numArray2 = buffer;
      int index4 = num10;
      int num12 = checked (index4 + 1);
      int num13 = (int) checked ((byte) ((int) this.messageId & (int) byte.MaxValue));
      numArray2[index4] = (byte) num13;
      return buffer;
    }

    public static MqttMsgPubcomp Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      int num1 = 0;
      MqttMsgPubcomp mqttMsgPubcomp1 = new MqttMsgPubcomp();
      if (protocolVersion == (byte) 4 && ((uint) fixedHeaderFirstByte & 15U) > 0U)
        throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
      byte[] buffer = new byte[MqttMsgBase.decodeRemainingLength(channel)];
      channel.Receive(buffer);
      MqttMsgPubcomp mqttMsgPubcomp2 = mqttMsgPubcomp1;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = checked (index1 + 1);
      int num3 = (int) checked ((ushort) ((int) numArray1[index1] << 8 & 65280));
      mqttMsgPubcomp2.messageId = (ushort) num3;
      MqttMsgPubcomp mqttMsgPubcomp3 = mqttMsgPubcomp1;
      int messageId = (int) mqttMsgPubcomp3.messageId;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = checked (index2 + 1);
      int num5 = (int) numArray2[index2];
      mqttMsgPubcomp3.messageId = (ushort) (messageId | num5);
      return mqttMsgPubcomp1;
    }

    public override string ToString()
    {
      return this.GetTraceString("PUBCOMP", new object[1]{ (object) "messageId" }, new object[1]{ (object) this.messageId });
    }
  }
}
