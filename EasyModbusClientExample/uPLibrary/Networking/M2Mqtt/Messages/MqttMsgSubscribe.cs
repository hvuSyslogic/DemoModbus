// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribe
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgSubscribe : MqttMsgBase
  {
    private string[] topics;
    private byte[] qosLevels;

    public string[] Topics
    {
      get
      {
        return this.topics;
      }
      set
      {
        this.topics = value;
      }
    }

    public byte[] QoSLevels
    {
      get
      {
        return this.qosLevels;
      }
      set
      {
        this.qosLevels = value;
      }
    }

    public MqttMsgSubscribe()
    {
      this.type = (byte) 8;
    }

    public MqttMsgSubscribe(string[] topics, byte[] qosLevels)
    {
      this.type = (byte) 8;
      this.topics = topics;
      this.qosLevels = qosLevels;
      this.qosLevel = (byte) 1;
    }

    public static MqttMsgSubscribe Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      int num1 = 0;
      MqttMsgSubscribe mqttMsgSubscribe1 = new MqttMsgSubscribe();
      if (protocolVersion == (byte) 4 && ((int) fixedHeaderFirstByte & 15) != 2)
        throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
      int length1 = MqttMsgBase.decodeRemainingLength(channel);
      byte[] buffer = new byte[length1];
      channel.Receive(buffer);
      if (protocolVersion == (byte) 3)
      {
        mqttMsgSubscribe1.qosLevel = checked ((byte) (((int) fixedHeaderFirstByte & 6) >> 1));
        mqttMsgSubscribe1.dupFlag = ((int) fixedHeaderFirstByte & 8) >> 3 == 1;
        mqttMsgSubscribe1.retain = false;
      }
      MqttMsgSubscribe mqttMsgSubscribe2 = mqttMsgSubscribe1;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = checked (index1 + 1);
      int num3 = (int) checked ((ushort) ((int) numArray1[index1] << 8 & 65280));
      mqttMsgSubscribe2.messageId = (ushort) num3;
      MqttMsgSubscribe mqttMsgSubscribe3 = mqttMsgSubscribe1;
      int messageId = (int) mqttMsgSubscribe3.messageId;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = checked (index2 + 1);
      int num5 = (int) numArray2[index2];
      mqttMsgSubscribe3.messageId = (ushort) (messageId | num5);
      IList<string> stringList = (IList<string>) new List<string>();
      IList<byte> byteList1 = (IList<byte>) new List<byte>();
      do
      {
        byte[] numArray3 = buffer;
        int index3 = num4;
        int num6 = checked (index3 + 1);
        int num7 = (int) numArray3[index3] << 8 & 65280;
        byte[] numArray4 = buffer;
        int index4 = num6;
        int sourceIndex = checked (index4 + 1);
        int num8 = (int) numArray4[index4];
        int length2 = num7 | num8;
        byte[] bytes = new byte[length2];
        Array.Copy((Array) buffer, sourceIndex, (Array) bytes, 0, length2);
        int num9 = checked (sourceIndex + length2);
        stringList.Add(new string(Encoding.UTF8.GetChars(bytes)));
        IList<byte> byteList2 = byteList1;
        byte[] numArray5 = buffer;
        int index5 = num9;
        num4 = checked (index5 + 1);
        int num10 = (int) numArray5[index5];
        byteList2.Add((byte) num10);
      }
      while (num4 < length1);
      mqttMsgSubscribe1.topics = new string[stringList.Count];
      mqttMsgSubscribe1.qosLevels = new byte[byteList1.Count];
      int index6 = 0;
      while (index6 < stringList.Count)
      {
        mqttMsgSubscribe1.topics[index6] = stringList[index6];
        mqttMsgSubscribe1.qosLevels[index6] = byteList1[index6];
        checked { ++index6; }
      }
      return mqttMsgSubscribe1;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int index1 = 0;
      if (this.topics == null || this.topics.Length == 0)
        throw new MqttClientException(MqttClientErrorCode.TopicsEmpty);
      if (this.qosLevels == null || this.qosLevels.Length == 0)
        throw new MqttClientException(MqttClientErrorCode.QosLevelsEmpty);
      if (this.topics.Length != this.qosLevels.Length)
        throw new MqttClientException(MqttClientErrorCode.TopicsQosLevelsNotMatch);
      int num4 = checked (num1 + 2);
      int num5 = 0;
      byte[][] numArray1 = new byte[this.topics.Length][];
      int index2 = 0;
      while (index2 < this.topics.Length)
      {
        if (this.topics[index2].Length < 1 || this.topics[index2].Length > (int) ushort.MaxValue)
          throw new MqttClientException(MqttClientErrorCode.TopicLength);
        numArray1[index2] = Encoding.UTF8.GetBytes(this.topics[index2]);
        num2 = checked (num2 + 2 + numArray1[index2].Length + 1);
        checked { ++index2; }
      }
      int remainingLength = checked (num3 + num4 + num2);
      int num6 = 1;
      int num7 = remainingLength;
      do
      {
        checked { ++num6; }
        num7 /= 128;
      }
      while (num7 > 0);
      byte[] buffer = new byte[checked (num6 + num4 + num2)];
      int index3;
      if (protocolVersion == (byte) 4)
      {
        byte[] numArray2 = buffer;
        int index4 = index1;
        index3 = checked (index4 + 1);
        int num8 = 130;
        numArray2[index4] = (byte) num8;
      }
      else
      {
        buffer[index1] = checked ((byte) (128 | (int) this.qosLevel << 1));
        buffer[index1] |= this.dupFlag ? (byte) 8 : (byte) 0;
        index3 = checked (index1 + 1);
      }
      int num9 = this.encodeRemainingLength(remainingLength, buffer, index3);
      if (this.messageId == (ushort) 0)
        throw new MqttClientException(MqttClientErrorCode.WrongMessageId);
      byte[] numArray3 = buffer;
      int index5 = num9;
      int num10 = checked (index5 + 1);
      int num11 = (int) checked ((byte) ((int) this.messageId >> 8 & (int) byte.MaxValue));
      numArray3[index5] = (byte) num11;
      byte[] numArray4 = buffer;
      int index6 = num10;
      int num12 = checked (index6 + 1);
      int num13 = (int) checked ((byte) ((int) this.messageId & (int) byte.MaxValue));
      numArray4[index6] = (byte) num13;
      num5 = 0;
      int index7 = 0;
      while (index7 < this.topics.Length)
      {
        byte[] numArray2 = buffer;
        int index4 = num12;
        int num8 = checked (index4 + 1);
        int num14 = (int) checked ((byte) (numArray1[index7].Length >> 8 & (int) byte.MaxValue));
        numArray2[index4] = (byte) num14;
        byte[] numArray5 = buffer;
        int index8 = num8;
        int destinationIndex = checked (index8 + 1);
        int num15 = (int) checked ((byte) (numArray1[index7].Length & (int) byte.MaxValue));
        numArray5[index8] = (byte) num15;
        Array.Copy((Array) numArray1[index7], 0, (Array) buffer, destinationIndex, numArray1[index7].Length);
        int num16 = checked (destinationIndex + numArray1[index7].Length);
        byte[] numArray6 = buffer;
        int index9 = num16;
        num12 = checked (index9 + 1);
        int qosLevel = (int) this.qosLevels[index7];
        numArray6[index9] = (byte) qosLevel;
        checked { ++index7; }
      }
      return buffer;
    }

    public override string ToString()
    {
      return this.GetTraceString("SUBSCRIBE", new object[3]{ (object) "messageId", (object) "topics", (object) "qosLevels" }, new object[3]{ (object) this.messageId, (object) this.topics, (object) this.qosLevels });
    }
  }
}
