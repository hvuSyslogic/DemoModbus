// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgUnsubscribe
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgUnsubscribe : MqttMsgBase
  {
    private string[] topics;

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

    public MqttMsgUnsubscribe()
    {
      this.type = (byte) 10;
    }

    public MqttMsgUnsubscribe(string[] topics)
    {
      this.type = (byte) 10;
      this.topics = topics;
      this.qosLevel = (byte) 1;
    }

    public static MqttMsgUnsubscribe Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      int num1 = 0;
      MqttMsgUnsubscribe mqttMsgUnsubscribe1 = new MqttMsgUnsubscribe();
      if (protocolVersion == (byte) 4 && ((int) fixedHeaderFirstByte & 15) != 2)
        throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
      int length1 = MqttMsgBase.decodeRemainingLength(channel);
      byte[] buffer = new byte[length1];
      channel.Receive(buffer);
      if (protocolVersion == (byte) 3)
      {
        mqttMsgUnsubscribe1.qosLevel = checked ((byte) (((int) fixedHeaderFirstByte & 6) >> 1));
        mqttMsgUnsubscribe1.dupFlag = ((int) fixedHeaderFirstByte & 8) >> 3 == 1;
        mqttMsgUnsubscribe1.retain = false;
      }
      MqttMsgUnsubscribe mqttMsgUnsubscribe2 = mqttMsgUnsubscribe1;
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = checked (index1 + 1);
      int num3 = (int) checked ((ushort) ((int) numArray1[index1] << 8 & 65280));
      mqttMsgUnsubscribe2.messageId = (ushort) num3;
      MqttMsgUnsubscribe mqttMsgUnsubscribe3 = mqttMsgUnsubscribe1;
      int messageId = (int) mqttMsgUnsubscribe3.messageId;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int num4 = checked (index2 + 1);
      int num5 = (int) numArray2[index2];
      mqttMsgUnsubscribe3.messageId = (ushort) (messageId | num5);
      IList<string> stringList = (IList<string>) new List<string>();
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
        num4 = checked (sourceIndex + length2);
        stringList.Add(new string(Encoding.UTF8.GetChars(bytes)));
      }
      while (num4 < length1);
      mqttMsgUnsubscribe1.topics = new string[stringList.Count];
      int index5 = 0;
      while (index5 < stringList.Count)
      {
        mqttMsgUnsubscribe1.topics[index5] = stringList[index5];
        checked { ++index5; }
      }
      return mqttMsgUnsubscribe1;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int index1 = 0;
      if (this.topics == null || this.topics.Length == 0)
        throw new MqttClientException(MqttClientErrorCode.TopicsEmpty);
      int num4 = checked (num1 + 2);
      int num5 = 0;
      byte[][] numArray1 = new byte[this.topics.Length][];
      int index2 = 0;
      while (index2 < this.topics.Length)
      {
        if (this.topics[index2].Length < 1 || this.topics[index2].Length > (int) ushort.MaxValue)
          throw new MqttClientException(MqttClientErrorCode.TopicLength);
        numArray1[index2] = Encoding.UTF8.GetBytes(this.topics[index2]);
        num2 = checked (num2 + 2 + numArray1[index2].Length);
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
        int num8 = 162;
        numArray2[index4] = (byte) num8;
      }
      else
      {
        buffer[index1] = checked ((byte) (160 | (int) this.qosLevel << 1));
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
        num12 = checked (destinationIndex + numArray1[index7].Length);
        checked { ++index7; }
      }
      return buffer;
    }

    public override string ToString()
    {
      return this.GetTraceString("UNSUBSCRIBE", new object[2]{ (object) "messageId", (object) "topics" }, new object[2]{ (object) this.messageId, (object) this.topics });
    }
  }
}
