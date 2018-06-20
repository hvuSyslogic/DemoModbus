// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublish
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgPublish : MqttMsgBase
  {
    private string topic;
    private byte[] message;

    public string Topic
    {
      get
      {
        return this.topic;
      }
      set
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
      set
      {
        this.message = value;
      }
    }

    public MqttMsgPublish()
    {
      this.type = (byte) 3;
    }

    public MqttMsgPublish(string topic, byte[] message)
      : this(topic, message, false, (byte) 0, false)
    {
    }

    public MqttMsgPublish(string topic, byte[] message, bool dupFlag, byte qosLevel, bool retain)
    {
      this.type = (byte) 3;
      this.topic = topic;
      this.message = message;
      this.dupFlag = dupFlag;
      this.qosLevel = qosLevel;
      this.retain = retain;
      this.messageId = (ushort) 0;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int index1 = 0;
      if (this.topic.IndexOf('#') != -1 || this.topic.IndexOf('+') != -1)
        throw new MqttClientException(MqttClientErrorCode.TopicWildcard);
      if (this.topic.Length < 1 || this.topic.Length > (int) ushort.MaxValue)
        throw new MqttClientException(MqttClientErrorCode.TopicLength);
      if (this.qosLevel > (byte) 2)
        throw new MqttClientException(MqttClientErrorCode.QosNotAllowed);
      byte[] bytes = Encoding.UTF8.GetBytes(this.topic);
      int num4 = checked (num1 + bytes.Length + 2);
      if (this.qosLevel == (byte) 1 || this.qosLevel == (byte) 2)
        checked { num4 += 2; }
      if (this.message != null)
        checked { num2 += this.message.Length; }
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
      buffer[index1] = checked ((byte) (48 | (int) this.qosLevel << 1));
      buffer[index1] |= this.dupFlag ? (byte) 8 : (byte) 0;
      buffer[index1] |= this.retain ? (byte) 1 : (byte) 0;
      int index2 = checked (index1 + 1);
      int num7 = this.encodeRemainingLength(remainingLength, buffer, index2);
      byte[] numArray1 = buffer;
      int index3 = num7;
      int num8 = checked (index3 + 1);
      int num9 = (int) checked ((byte) (bytes.Length >> 8 & (int) byte.MaxValue));
      numArray1[index3] = (byte) num9;
      byte[] numArray2 = buffer;
      int index4 = num8;
      int destinationIndex1 = checked (index4 + 1);
      int num10 = (int) checked ((byte) (bytes.Length & (int) byte.MaxValue));
      numArray2[index4] = (byte) num10;
      Array.Copy((Array) bytes, 0, (Array) buffer, destinationIndex1, bytes.Length);
      int destinationIndex2 = checked (destinationIndex1 + bytes.Length);
      if (this.qosLevel == (byte) 1 || this.qosLevel == (byte) 2)
      {
        if (this.messageId == (ushort) 0)
          throw new MqttClientException(MqttClientErrorCode.WrongMessageId);
        byte[] numArray3 = buffer;
        int index5 = destinationIndex2;
        int num11 = checked (index5 + 1);
        int num12 = (int) checked ((byte) ((int) this.messageId >> 8 & (int) byte.MaxValue));
        numArray3[index5] = (byte) num12;
        byte[] numArray4 = buffer;
        int index6 = num11;
        destinationIndex2 = checked (index6 + 1);
        int num13 = (int) checked ((byte) ((int) this.messageId & (int) byte.MaxValue));
        numArray4[index6] = (byte) num13;
      }
      if (this.message != null)
      {
        Array.Copy((Array) this.message, 0, (Array) buffer, destinationIndex2, this.message.Length);
        int num11 = checked (destinationIndex2 + this.message.Length);
      }
      return buffer;
    }

    public static MqttMsgPublish Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      int num1 = 0;
      MqttMsgPublish mqttMsgPublish1 = new MqttMsgPublish();
      int length1 = MqttMsgBase.decodeRemainingLength(channel);
      byte[] buffer = new byte[length1];
      int num2 = channel.Receive(buffer);
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num3 = checked (index1 + 1);
      int num4 = (int) numArray1[index1] << 8 & 65280;
      byte[] numArray2 = buffer;
      int index2 = num3;
      int sourceIndex1 = checked (index2 + 1);
      int num5 = (int) numArray2[index2];
      int length2 = num4 | num5;
      byte[] bytes = new byte[length2];
      Array.Copy((Array) buffer, sourceIndex1, (Array) bytes, 0, length2);
      int sourceIndex2 = checked (sourceIndex1 + length2);
      mqttMsgPublish1.topic = new string(Encoding.UTF8.GetChars(bytes));
      mqttMsgPublish1.qosLevel = checked ((byte) (((int) fixedHeaderFirstByte & 6) >> 1));
      if (mqttMsgPublish1.qosLevel > (byte) 2)
        throw new MqttClientException(MqttClientErrorCode.QosNotAllowed);
      mqttMsgPublish1.dupFlag = ((int) fixedHeaderFirstByte & 8) >> 3 == 1;
      mqttMsgPublish1.retain = ((int) fixedHeaderFirstByte & 1) == 1;
      if (mqttMsgPublish1.qosLevel == (byte) 1 || mqttMsgPublish1.qosLevel == (byte) 2)
      {
        MqttMsgPublish mqttMsgPublish2 = mqttMsgPublish1;
        byte[] numArray3 = buffer;
        int index3 = sourceIndex2;
        int num6 = checked (index3 + 1);
        int num7 = (int) checked ((ushort) ((int) numArray3[index3] << 8 & 65280));
        mqttMsgPublish2.messageId = (ushort) num7;
        MqttMsgPublish mqttMsgPublish3 = mqttMsgPublish1;
        int messageId = (int) mqttMsgPublish3.messageId;
        byte[] numArray4 = buffer;
        int index4 = num6;
        sourceIndex2 = checked (index4 + 1);
        int num8 = (int) numArray4[index4];
        mqttMsgPublish3.messageId = (ushort) (messageId | num8);
      }
      int length3 = checked (length1 - sourceIndex2);
      int num9 = length3;
      int destinationIndex1 = 0;
      mqttMsgPublish1.message = new byte[length3];
      Array.Copy((Array) buffer, sourceIndex2, (Array) mqttMsgPublish1.message, destinationIndex1, checked (num2 - sourceIndex2));
      int num10 = checked (num9 - num2 - sourceIndex2);
      int destinationIndex2 = checked (destinationIndex1 + num2 - sourceIndex2);
      while (num10 > 0)
      {
        int length4 = channel.Receive(buffer);
        Array.Copy((Array) buffer, 0, (Array) mqttMsgPublish1.message, destinationIndex2, length4);
        checked { num10 -= length4; }
        checked { destinationIndex2 += length4; }
      }
      return mqttMsgPublish1;
    }

    public override string ToString()
    {
      return this.GetTraceString("PUBLISH", new object[3]{ (object) "messageId", (object) "topic", (object) "message" }, new object[3]{ (object) this.messageId, (object) this.topic, (object) this.message });
    }
  }
}
