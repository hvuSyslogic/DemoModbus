// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPubrel
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgPubrel : MqttMsgBase
  {
    public MqttMsgPubrel()
    {
      this.type = (byte) 6;
      this.qosLevel = (byte) 1;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int index1 = 0;
      int num4 = checked (num1 + 2);
      int remainingLength = checked (num3 + num4 + num2);
      int num5 = 1;
      int num6 = remainingLength;
      do
      {
        checked { ++num5; }
        num6 /= 128;
      }
      while (num6 > 0);
      byte[] buffer = new byte[checked (num5 + num4 + num2)];
      int index2;
      if (protocolVersion == (byte) 4)
      {
        byte[] numArray = buffer;
        int index3 = index1;
        index2 = checked (index3 + 1);
        int num7 = 98;
        numArray[index3] = (byte) num7;
      }
      else
      {
        buffer[index1] = checked ((byte) (96 | (int) this.qosLevel << 1));
        buffer[index1] |= this.dupFlag ? (byte) 8 : (byte) 0;
        index2 = checked (index1 + 1);
      }
      int num8 = this.encodeRemainingLength(remainingLength, buffer, index2);
      byte[] numArray1 = buffer;
      int index4 = num8;
      int num9 = checked (index4 + 1);
      int num10 = (int) checked ((byte) ((int) this.messageId >> 8 & (int) byte.MaxValue));
      numArray1[index4] = (byte) num10;
      byte[] numArray2 = buffer;
      int index5 = num9;
      int num11 = checked (index5 + 1);
      int num12 = (int) checked ((byte) ((int) this.messageId & (int) byte.MaxValue));
      numArray2[index5] = (byte) num12;
      return buffer;
    }

    public static MqttMsgPubrel Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      int num1 = 0;
      MqttMsgPubrel mqttMsgPubrel1 = new MqttMsgPubrel();
      if (protocolVersion == (byte) 4 && ((int) fixedHeaderFirstByte & 15) != 2)
        throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
      byte[] buffer = new byte[MqttMsgBase.decodeRemainingLength(channel)];
      channel.Receive(buffer);
      if (protocolVersion == (byte) 3)
      {
        mqttMsgPubrel1.qosLevel = checked ((byte) (((int) fixedHeaderFirstByte & 6) >> 1));
        mqttMsgPubrel1.dupFlag = ((int) fixedHeaderFirstByte & 8) >> 3 == 1;
      }
      MqttMsgPubrel mqttMsgPubrel2 = mqttMsgPubrel1;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = checked (index1 + 1);
      int num3 = (int) checked ((ushort) ((int) numArray1[index1] << 8 & 65280));
      mqttMsgPubrel2.messageId = (ushort) num3;
      MqttMsgPubrel mqttMsgPubrel3 = mqttMsgPubrel1;
      int messageId = (int) mqttMsgPubrel3.messageId;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = checked (index2 + 1);
      int num5 = (int) numArray2[index2];
      mqttMsgPubrel3.messageId = (ushort) (messageId | num5);
      return mqttMsgPubrel1;
    }

    public override string ToString()
    {
      return this.GetTraceString("PUBREL", new object[1]{ (object) "messageId" }, new object[1]{ (object) this.messageId });
    }
  }
}
