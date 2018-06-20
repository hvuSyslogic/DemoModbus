// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System.Text;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public abstract class MqttMsgBase
  {
    internal const byte MSG_TYPE_MASK = 240;
    internal const byte MSG_TYPE_OFFSET = 4;
    internal const byte MSG_TYPE_SIZE = 4;
    internal const byte MSG_FLAG_BITS_MASK = 15;
    internal const byte MSG_FLAG_BITS_OFFSET = 0;
    internal const byte MSG_FLAG_BITS_SIZE = 4;
    internal const byte DUP_FLAG_MASK = 8;
    internal const byte DUP_FLAG_OFFSET = 3;
    internal const byte DUP_FLAG_SIZE = 1;
    internal const byte QOS_LEVEL_MASK = 6;
    internal const byte QOS_LEVEL_OFFSET = 1;
    internal const byte QOS_LEVEL_SIZE = 2;
    internal const byte RETAIN_FLAG_MASK = 1;
    internal const byte RETAIN_FLAG_OFFSET = 0;
    internal const byte RETAIN_FLAG_SIZE = 1;
    internal const byte MQTT_MSG_CONNECT_TYPE = 1;
    internal const byte MQTT_MSG_CONNACK_TYPE = 2;
    internal const byte MQTT_MSG_PUBLISH_TYPE = 3;
    internal const byte MQTT_MSG_PUBACK_TYPE = 4;
    internal const byte MQTT_MSG_PUBREC_TYPE = 5;
    internal const byte MQTT_MSG_PUBREL_TYPE = 6;
    internal const byte MQTT_MSG_PUBCOMP_TYPE = 7;
    internal const byte MQTT_MSG_SUBSCRIBE_TYPE = 8;
    internal const byte MQTT_MSG_SUBACK_TYPE = 9;
    internal const byte MQTT_MSG_UNSUBSCRIBE_TYPE = 10;
    internal const byte MQTT_MSG_UNSUBACK_TYPE = 11;
    internal const byte MQTT_MSG_PINGREQ_TYPE = 12;
    internal const byte MQTT_MSG_PINGRESP_TYPE = 13;
    internal const byte MQTT_MSG_DISCONNECT_TYPE = 14;
    internal const byte MQTT_MSG_CONNECT_FLAG_BITS = 0;
    internal const byte MQTT_MSG_CONNACK_FLAG_BITS = 0;
    internal const byte MQTT_MSG_PUBLISH_FLAG_BITS = 0;
    internal const byte MQTT_MSG_PUBACK_FLAG_BITS = 0;
    internal const byte MQTT_MSG_PUBREC_FLAG_BITS = 0;
    internal const byte MQTT_MSG_PUBREL_FLAG_BITS = 2;
    internal const byte MQTT_MSG_PUBCOMP_FLAG_BITS = 0;
    internal const byte MQTT_MSG_SUBSCRIBE_FLAG_BITS = 2;
    internal const byte MQTT_MSG_SUBACK_FLAG_BITS = 0;
    internal const byte MQTT_MSG_UNSUBSCRIBE_FLAG_BITS = 2;
    internal const byte MQTT_MSG_UNSUBACK_FLAG_BITS = 0;
    internal const byte MQTT_MSG_PINGREQ_FLAG_BITS = 0;
    internal const byte MQTT_MSG_PINGRESP_FLAG_BITS = 0;
    internal const byte MQTT_MSG_DISCONNECT_FLAG_BITS = 0;
    public const byte QOS_LEVEL_AT_MOST_ONCE = 0;
    public const byte QOS_LEVEL_AT_LEAST_ONCE = 1;
    public const byte QOS_LEVEL_EXACTLY_ONCE = 2;
    public const byte QOS_LEVEL_GRANTED_FAILURE = 128;
    internal const ushort MAX_TOPIC_LENGTH = 65535;
    internal const ushort MIN_TOPIC_LENGTH = 1;
    internal const byte MESSAGE_ID_SIZE = 2;
    protected byte type;
    protected bool dupFlag;
    protected byte qosLevel;
    protected bool retain;
    protected ushort messageId;

    public byte Type
    {
      get
      {
        return this.type;
      }
      set
      {
        this.type = value;
      }
    }

    public bool DupFlag
    {
      get
      {
        return this.dupFlag;
      }
      set
      {
        this.dupFlag = value;
      }
    }

    public byte QosLevel
    {
      get
      {
        return this.qosLevel;
      }
      set
      {
        this.qosLevel = value;
      }
    }

    public bool Retain
    {
      get
      {
        return this.retain;
      }
      set
      {
        this.retain = value;
      }
    }

    public ushort MessageId
    {
      get
      {
        return this.messageId;
      }
      set
      {
        this.messageId = value;
      }
    }

    public abstract byte[] GetBytes(byte protocolVersion);

    protected int encodeRemainingLength(int remainingLength, byte[] buffer, int index)
    {
      do
      {
        int num = remainingLength % 128;
        remainingLength /= 128;
        if (remainingLength > 0)
          num |= 128;
        buffer[checked (index++)] = checked ((byte) num);
      }
      while (remainingLength > 0);
      return index;
    }

    protected static int decodeRemainingLength(IMqttNetworkChannel channel)
    {
      int num1 = 1;
      int num2 = 0;
      byte[] buffer = new byte[1];
      int num3;
      do
      {
        channel.Receive(buffer);
        num3 = (int) buffer[0];
        checked { num2 += (num3 & (int) sbyte.MaxValue) * num1; }
        checked { num1 *= 128; }
      }
      while ((uint) (num3 & 128) > 0U);
      return num2;
    }

    protected string GetTraceString(string name, object[] fieldNames, object[] fieldValues)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(name);
      if (fieldNames != null && fieldValues != null)
      {
        stringBuilder.Append("(");
        bool flag = false;
        int index = 0;
        while (index < fieldValues.Length)
        {
          if (fieldValues[index] != null)
          {
            if (flag)
              stringBuilder.Append(",");
            stringBuilder.Append(fieldNames[index]);
            stringBuilder.Append(":");
            stringBuilder.Append(this.GetStringObject(fieldValues[index]));
            flag = true;
          }
          checked { ++index; }
        }
        stringBuilder.Append(")");
      }
      return stringBuilder.ToString();
    }

    private object GetStringObject(object value)
    {
      byte[] numArray = value as byte[];
      if (numArray != null)
      {
        string str = "0123456789ABCDEF";
        StringBuilder stringBuilder = new StringBuilder(checked (numArray.Length * 2));
        int index = 0;
        while (index < numArray.Length)
        {
          stringBuilder.Append(str[(int) numArray[index] >> 4]);
          stringBuilder.Append(str[(int) numArray[index] & 15]);
          checked { ++index; }
        }
        return (object) stringBuilder.ToString();
      }
      object[] objArray = value as object[];
      if (objArray == null)
        return value;
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append('[');
      int index1 = 0;
      while (index1 < objArray.Length)
      {
        if (index1 > 0)
          stringBuilder1.Append(',');
        stringBuilder1.Append(objArray[index1]);
        checked { ++index1; }
      }
      stringBuilder1.Append(']');
      return (object) stringBuilder1.ToString();
    }
  }
}
