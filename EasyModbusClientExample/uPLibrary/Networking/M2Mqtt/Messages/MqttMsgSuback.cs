// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSuback
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgSuback : MqttMsgBase
  {
    private byte[] grantedQosLevels;

    public byte[] GrantedQoSLevels
    {
      get
      {
        return this.grantedQosLevels;
      }
      set
      {
        this.grantedQosLevels = value;
      }
    }

    public MqttMsgSuback()
    {
      this.type = (byte) 9;
    }

    public static MqttMsgSuback Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      int num1 = 0;
      MqttMsgSuback mqttMsgSuback1 = new MqttMsgSuback();
      if (protocolVersion == (byte) 4 && ((uint) fixedHeaderFirstByte & 15U) > 0U)
        throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
      int length = MqttMsgBase.decodeRemainingLength(channel);
      byte[] buffer = new byte[length];
      channel.Receive(buffer);
      MqttMsgSuback mqttMsgSuback2 = mqttMsgSuback1;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = checked (index1 + 1);
      int num3 = (int) checked ((ushort) ((int) numArray1[index1] << 8 & 65280));
      mqttMsgSuback2.messageId = (ushort) num3;
      MqttMsgSuback mqttMsgSuback3 = mqttMsgSuback1;
      int messageId = (int) mqttMsgSuback3.messageId;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = checked (index2 + 1);
      int num5 = (int) numArray2[index2];
      mqttMsgSuback3.messageId = (ushort) (messageId | num5);
      mqttMsgSuback1.grantedQosLevels = new byte[checked (length - 2)];
      int num6 = 0;
      do
      {
        mqttMsgSuback1.grantedQosLevels[checked (num6++)] = buffer[checked (num4++)];
      }
      while (num4 < length);
      return mqttMsgSuback1;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = checked (num1 + 2);
      int num6 = 0;
      while (num6 < this.grantedQosLevels.Length)
      {
        checked { ++num2; }
        checked { ++num6; }
      }
      int remainingLength = checked (num3 + num5 + num2);
      int num7 = 1;
      int num8 = remainingLength;
      do
      {
        checked { ++num7; }
        num8 /= 128;
      }
      while (num8 > 0);
      byte[] buffer = new byte[checked (num7 + num5 + num2)];
      int index1;
      if (protocolVersion == (byte) 4)
      {
        byte[] numArray = buffer;
        int index2 = num4;
        index1 = checked (index2 + 1);
        int num9 = 144;
        numArray[index2] = (byte) num9;
      }
      else
      {
        byte[] numArray = buffer;
        int index2 = num4;
        index1 = checked (index2 + 1);
        int num9 = 144;
        numArray[index2] = (byte) num9;
      }
      int num10 = this.encodeRemainingLength(remainingLength, buffer, index1);
      byte[] numArray1 = buffer;
      int index3 = num10;
      int num11 = checked (index3 + 1);
      int num12 = (int) checked ((byte) ((int) this.messageId >> 8 & (int) byte.MaxValue));
      numArray1[index3] = (byte) num12;
      byte[] numArray2 = buffer;
      int index4 = num11;
      int num13 = checked (index4 + 1);
      int num14 = (int) checked ((byte) ((int) this.messageId & (int) byte.MaxValue));
      numArray2[index4] = (byte) num14;
      int index5 = 0;
      while (index5 < this.grantedQosLevels.Length)
      {
        buffer[checked (num13++)] = this.grantedQosLevels[index5];
        checked { ++index5; }
      }
      return buffer;
    }

    public override string ToString()
    {
      return this.GetTraceString("SUBACK", new object[2]{ (object) "messageId", (object) "grantedQosLevels" }, new object[2]{ (object) this.messageId, (object) this.grantedQosLevels });
    }
  }
}
