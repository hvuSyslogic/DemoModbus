// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgConnect
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgConnect : MqttMsgBase
  {
    internal const string PROTOCOL_NAME_V3_1 = "MQIsdp";
    internal const string PROTOCOL_NAME_V3_1_1 = "MQTT";
    internal const int CLIENT_ID_MAX_LENGTH = 23;
    internal const byte PROTOCOL_NAME_LEN_SIZE = 2;
    internal const byte PROTOCOL_NAME_V3_1_SIZE = 6;
    internal const byte PROTOCOL_NAME_V3_1_1_SIZE = 4;
    internal const byte PROTOCOL_VERSION_SIZE = 1;
    internal const byte CONNECT_FLAGS_SIZE = 1;
    internal const byte KEEP_ALIVE_TIME_SIZE = 2;
    internal const byte PROTOCOL_VERSION_V3_1 = 3;
    internal const byte PROTOCOL_VERSION_V3_1_1 = 4;
    internal const ushort KEEP_ALIVE_PERIOD_DEFAULT = 60;
    internal const ushort MAX_KEEP_ALIVE = 65535;
    internal const byte USERNAME_FLAG_MASK = 128;
    internal const byte USERNAME_FLAG_OFFSET = 7;
    internal const byte USERNAME_FLAG_SIZE = 1;
    internal const byte PASSWORD_FLAG_MASK = 64;
    internal const byte PASSWORD_FLAG_OFFSET = 6;
    internal const byte PASSWORD_FLAG_SIZE = 1;
    internal const byte WILL_RETAIN_FLAG_MASK = 32;
    internal const byte WILL_RETAIN_FLAG_OFFSET = 5;
    internal const byte WILL_RETAIN_FLAG_SIZE = 1;
    internal const byte WILL_QOS_FLAG_MASK = 24;
    internal const byte WILL_QOS_FLAG_OFFSET = 3;
    internal const byte WILL_QOS_FLAG_SIZE = 2;
    internal const byte WILL_FLAG_MASK = 4;
    internal const byte WILL_FLAG_OFFSET = 2;
    internal const byte WILL_FLAG_SIZE = 1;
    internal const byte CLEAN_SESSION_FLAG_MASK = 2;
    internal const byte CLEAN_SESSION_FLAG_OFFSET = 1;
    internal const byte CLEAN_SESSION_FLAG_SIZE = 1;
    internal const byte RESERVED_FLAG_MASK = 1;
    internal const byte RESERVED_FLAG_OFFSET = 0;
    internal const byte RESERVED_FLAG_SIZE = 1;
    private string protocolName;
    private byte protocolVersion;
    private string clientId;
    protected bool willRetain;
    protected byte willQosLevel;
    private bool willFlag;
    private string willTopic;
    private string willMessage;
    private string username;
    private string password;
    private bool cleanSession;
    private ushort keepAlivePeriod;

    public string ProtocolName
    {
      get
      {
        return this.protocolName;
      }
      set
      {
        this.protocolName = value;
      }
    }

    public byte ProtocolVersion
    {
      get
      {
        return this.protocolVersion;
      }
      set
      {
        this.protocolVersion = value;
      }
    }

    public string ClientId
    {
      get
      {
        return this.clientId;
      }
      set
      {
        this.clientId = value;
      }
    }

    public bool WillRetain
    {
      get
      {
        return this.willRetain;
      }
      set
      {
        this.willRetain = value;
      }
    }

    public byte WillQosLevel
    {
      get
      {
        return this.willQosLevel;
      }
      set
      {
        this.willQosLevel = value;
      }
    }

    public bool WillFlag
    {
      get
      {
        return this.willFlag;
      }
      set
      {
        this.willFlag = value;
      }
    }

    public string WillTopic
    {
      get
      {
        return this.willTopic;
      }
      set
      {
        this.willTopic = value;
      }
    }

    public string WillMessage
    {
      get
      {
        return this.willMessage;
      }
      set
      {
        this.willMessage = value;
      }
    }

    public string Username
    {
      get
      {
        return this.username;
      }
      set
      {
        this.username = value;
      }
    }

    public string Password
    {
      get
      {
        return this.password;
      }
      set
      {
        this.password = value;
      }
    }

    public bool CleanSession
    {
      get
      {
        return this.cleanSession;
      }
      set
      {
        this.cleanSession = value;
      }
    }

    public ushort KeepAlivePeriod
    {
      get
      {
        return this.keepAlivePeriod;
      }
      set
      {
        this.keepAlivePeriod = value;
      }
    }

    public MqttMsgConnect()
    {
      this.type = (byte) 1;
    }

    public MqttMsgConnect(string clientId)
      : this(clientId, (string) null, (string) null, false, (byte) 1, false, (string) null, (string) null, true, (ushort) 60, (byte) 4)
    {
    }

    public MqttMsgConnect(string clientId, string username, string password, bool willRetain, byte willQosLevel, bool willFlag, string willTopic, string willMessage, bool cleanSession, ushort keepAlivePeriod, byte protocolVersion)
    {
      this.type = (byte) 1;
      this.clientId = clientId;
      this.username = username;
      this.password = password;
      this.willRetain = willRetain;
      this.willQosLevel = willQosLevel;
      this.willFlag = willFlag;
      this.willTopic = willTopic;
      this.willMessage = willMessage;
      this.cleanSession = cleanSession;
      this.keepAlivePeriod = keepAlivePeriod;
      this.protocolVersion = protocolVersion;
      this.protocolName = this.protocolVersion == (byte) 4 ? "MQTT" : "MQIsdp";
    }

    public static MqttMsgConnect Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      int num1 = 0;
      MqttMsgConnect mqttMsgConnect1 = new MqttMsgConnect();
      byte[] buffer = new byte[MqttMsgBase.decodeRemainingLength(channel)];
      channel.Receive(buffer);
      byte[] numArray1 = buffer;
      int index1 = num1;
      int num2 = checked (index1 + 1);
      int num3 = (int) numArray1[index1] << 8 & 65280;
      byte[] numArray2 = buffer;
      int index2 = num2;
      int sourceIndex1 = checked (index2 + 1);
      int num4 = (int) numArray2[index2];
      int length1 = num3 | num4;
      byte[] bytes1 = new byte[length1];
      Array.Copy((Array) buffer, sourceIndex1, (Array) bytes1, 0, length1);
      int index3 = checked (sourceIndex1 + length1);
      mqttMsgConnect1.protocolName = new string(Encoding.UTF8.GetChars(bytes1));
      if (!mqttMsgConnect1.protocolName.Equals("MQIsdp") && !mqttMsgConnect1.protocolName.Equals("MQTT"))
        throw new MqttClientException(MqttClientErrorCode.InvalidProtocolName);
      mqttMsgConnect1.protocolVersion = buffer[index3];
      int index4 = checked (index3 + 1);
      if (mqttMsgConnect1.protocolVersion == (byte) 4 && ((uint) buffer[index4] & 1U) > 0U)
        throw new MqttClientException(MqttClientErrorCode.InvalidConnectFlags);
      bool flag1 = ((uint) buffer[index4] & 128U) > 0U;
      bool flag2 = ((uint) buffer[index4] & 64U) > 0U;
      mqttMsgConnect1.willRetain = ((uint) buffer[index4] & 32U) > 0U;
      mqttMsgConnect1.willQosLevel = checked ((byte) (((int) buffer[index4] & 24) >> 3));
      mqttMsgConnect1.willFlag = ((uint) buffer[index4] & 4U) > 0U;
      mqttMsgConnect1.cleanSession = ((uint) buffer[index4] & 2U) > 0U;
      int num5 = checked (index4 + 1);
      MqttMsgConnect mqttMsgConnect2 = mqttMsgConnect1;
      byte[] numArray3 = buffer;
      int index5 = num5;
      int num6 = checked (index5 + 1);
      int num7 = (int) checked ((ushort) ((int) numArray3[index5] << 8 & 65280));
      mqttMsgConnect2.keepAlivePeriod = (ushort) num7;
      MqttMsgConnect mqttMsgConnect3 = mqttMsgConnect1;
      int keepAlivePeriod = (int) mqttMsgConnect3.keepAlivePeriod;
      byte[] numArray4 = buffer;
      int index6 = num6;
      int num8 = checked (index6 + 1);
      int num9 = (int) numArray4[index6];
      mqttMsgConnect3.keepAlivePeriod = (ushort) (keepAlivePeriod | num9);
      byte[] numArray5 = buffer;
      int index7 = num8;
      int num10 = checked (index7 + 1);
      int num11 = (int) numArray5[index7] << 8 & 65280;
      byte[] numArray6 = buffer;
      int index8 = num10;
      int sourceIndex2 = checked (index8 + 1);
      int num12 = (int) numArray6[index8];
      int length2 = num11 | num12;
      byte[] bytes2 = new byte[length2];
      Array.Copy((Array) buffer, sourceIndex2, (Array) bytes2, 0, length2);
      int num13 = checked (sourceIndex2 + length2);
      mqttMsgConnect1.clientId = new string(Encoding.UTF8.GetChars(bytes2));
      if (mqttMsgConnect1.protocolVersion == (byte) 4 && length2 == 0 && !mqttMsgConnect1.cleanSession)
        throw new MqttClientException(MqttClientErrorCode.InvalidClientId);
      if (mqttMsgConnect1.willFlag)
      {
        byte[] numArray7 = buffer;
        int index9 = num13;
        int num14 = checked (index9 + 1);
        int num15 = (int) numArray7[index9] << 8 & 65280;
        byte[] numArray8 = buffer;
        int index10 = num14;
        int sourceIndex3 = checked (index10 + 1);
        int num16 = (int) numArray8[index10];
        int length3 = num15 | num16;
        byte[] bytes3 = new byte[length3];
        Array.Copy((Array) buffer, sourceIndex3, (Array) bytes3, 0, length3);
        int num17 = checked (sourceIndex3 + length3);
        mqttMsgConnect1.willTopic = new string(Encoding.UTF8.GetChars(bytes3));
        byte[] numArray9 = buffer;
        int index11 = num17;
        int num18 = checked (index11 + 1);
        int num19 = (int) numArray9[index11] << 8 & 65280;
        byte[] numArray10 = buffer;
        int index12 = num18;
        int sourceIndex4 = checked (index12 + 1);
        int num20 = (int) numArray10[index12];
        int length4 = num19 | num20;
        byte[] bytes4 = new byte[length4];
        Array.Copy((Array) buffer, sourceIndex4, (Array) bytes4, 0, length4);
        num13 = checked (sourceIndex4 + length4);
        mqttMsgConnect1.willMessage = new string(Encoding.UTF8.GetChars(bytes4));
      }
      if (flag1)
      {
        byte[] numArray7 = buffer;
        int index9 = num13;
        int num14 = checked (index9 + 1);
        int num15 = (int) numArray7[index9] << 8 & 65280;
        byte[] numArray8 = buffer;
        int index10 = num14;
        int sourceIndex3 = checked (index10 + 1);
        int num16 = (int) numArray8[index10];
        int length3 = num15 | num16;
        byte[] bytes3 = new byte[length3];
        Array.Copy((Array) buffer, sourceIndex3, (Array) bytes3, 0, length3);
        num13 = checked (sourceIndex3 + length3);
        mqttMsgConnect1.username = new string(Encoding.UTF8.GetChars(bytes3));
      }
      if (flag2)
      {
        byte[] numArray7 = buffer;
        int index9 = num13;
        int num14 = checked (index9 + 1);
        int num15 = (int) numArray7[index9] << 8 & 65280;
        byte[] numArray8 = buffer;
        int index10 = num14;
        int sourceIndex3 = checked (index10 + 1);
        int num16 = (int) numArray8[index10];
        int length3 = num15 | num16;
        byte[] bytes3 = new byte[length3];
        Array.Copy((Array) buffer, sourceIndex3, (Array) bytes3, 0, length3);
        int num17 = checked (sourceIndex3 + length3);
        mqttMsgConnect1.password = new string(Encoding.UTF8.GetChars(bytes3));
      }
      return mqttMsgConnect1;
    }

    public override byte[] GetBytes(byte protocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      byte[] bytes = Encoding.UTF8.GetBytes(this.clientId);
      byte[] numArray1 = !this.willFlag || this.willTopic == null ? (byte[]) null : Encoding.UTF8.GetBytes(this.willTopic);
      byte[] numArray2 = !this.willFlag || this.willMessage == null ? (byte[]) null : Encoding.UTF8.GetBytes(this.willMessage);
      byte[] numArray3 = this.username == null || this.username.Length <= 0 ? (byte[]) null : Encoding.UTF8.GetBytes(this.username);
      byte[] numArray4 = this.password == null || this.password.Length <= 0 ? (byte[]) null : Encoding.UTF8.GetBytes(this.password);
      if (this.protocolVersion == (byte) 4)
      {
        if (this.willFlag && (this.willQosLevel >= (byte) 3 || numArray1 == null || numArray2 == null || numArray1 != null && numArray1.Length == 0 || numArray2 != null && numArray2.Length == 0))
          throw new MqttClientException(MqttClientErrorCode.WillWrong);
        if (!this.willFlag && (this.willRetain || numArray1 != null || numArray2 != null || numArray1 != null && numArray1.Length != 0 || numArray2 != null && (uint) numArray2.Length > 0U))
          throw new MqttClientException(MqttClientErrorCode.WillWrong);
      }
      if (this.keepAlivePeriod > ushort.MaxValue)
        throw new MqttClientException(MqttClientErrorCode.KeepAliveWrong);
      if (this.willQosLevel < (byte) 0 || this.willQosLevel > (byte) 2)
        throw new MqttClientException(MqttClientErrorCode.WillWrong);
      int num5 = checked ((unchecked (this.protocolVersion != (byte) 3) ? num1 + 6 : num1 + 8) + 1 + 1 + 2);
            //int num6 = checked (num2 + (bytes.Length + 2) + (unchecked (numArray1 != null) ? numArray1.Length + 2 : 0) + (unchecked (numArray2 != null) ? numArray2.Length + 2 : 0) + (unchecked (numArray3 != null) ? numArray3.Length + 2 : 0) + unchecked (numArray4 != null) ? numArray4.Length + 2 : 0);
            int num6 = checked(num2 + (bytes.Length + 2));
            if (unchecked(numArray1 != null))
            {
                num6 = checked(num6 + numArray1.Length + 2);
            }
            if (unchecked(numArray2 != null))
            {
                num6 = checked(num6 + numArray2.Length + 2);
            }
            if (unchecked(numArray3 != null))
            {
                num6 = checked(num6 + numArray3.Length + 2);
            }
            if (unchecked(numArray4 != null))
            {
                num6 = checked(num6 + numArray4.Length + 2);
            }
            int remainingLength = checked (num3 + num5 + num6);
      int num7 = 1;
      int num8 = remainingLength;
      do
      {
        checked { ++num7; }
        num8 /= 128;
      }
      while (num8 > 0);
      byte[] buffer = new byte[checked (num7 + num5 + num6)];
      byte[] numArray5 = buffer;
      int index1 = num4;
      int index2 = checked (index1 + 1);
      int num9 = 16;
      numArray5[index1] = (byte) num9;
      int num10 = this.encodeRemainingLength(remainingLength, buffer, index2);
      byte[] numArray6 = buffer;
      int index3 = num10;
      int num11 = checked (index3 + 1);
      int num12 = 0;
      numArray6[index3] = (byte) num12;
      int num13;
      if (this.protocolVersion == (byte) 3)
      {
        byte[] numArray7 = buffer;
        int index4 = num11;
        int destinationIndex = checked (index4 + 1);
        int num14 = 6;
        numArray7[index4] = (byte) num14;
        Array.Copy((Array) Encoding.UTF8.GetBytes("MQIsdp"), 0, (Array) buffer, destinationIndex, 6);
        int num15 = checked (destinationIndex + 6);
        byte[] numArray8 = buffer;
        int index5 = num15;
        num13 = checked (index5 + 1);
        int num16 = 3;
        numArray8[index5] = (byte) num16;
      }
      else
      {
        byte[] numArray7 = buffer;
        int index4 = num11;
        int destinationIndex = checked (index4 + 1);
        int num14 = 4;
        numArray7[index4] = (byte) num14;
        Array.Copy((Array) Encoding.UTF8.GetBytes("MQTT"), 0, (Array) buffer, destinationIndex, 4);
        int num15 = checked (destinationIndex + 4);
        byte[] numArray8 = buffer;
        int index5 = num15;
        num13 = checked (index5 + 1);
        int num16 = 4;
        numArray8[index5] = (byte) num16;
      }
      byte num17 = (byte) ((int) (byte) ((int) (byte) (0 | (numArray3 != null ? 128 : 0)) | (numArray4 != null ? 64 : 0)) | (this.willRetain ? 32 : 0));
      if (this.willFlag)
        num17 |= checked ((byte) ((int) this.willQosLevel << 3));
      byte num18 = (byte) ((int) (byte) ((int) num17 | (this.willFlag ? 4 : 0)) | (this.cleanSession ? 2 : 0));
      byte[] numArray9 = buffer;
      int index6 = num13;
      int num19 = checked (index6 + 1);
      int num20 = (int) num18;
      numArray9[index6] = (byte) num20;
      byte[] numArray10 = buffer;
      int index7 = num19;
      int num21 = checked (index7 + 1);
      int num22 = (int) checked ((byte) ((int) this.keepAlivePeriod >> 8 & (int) byte.MaxValue));
      numArray10[index7] = (byte) num22;
      byte[] numArray11 = buffer;
      int index8 = num21;
      int num23 = checked (index8 + 1);
      int num24 = (int) checked ((byte) ((int) this.keepAlivePeriod & (int) byte.MaxValue));
      numArray11[index8] = (byte) num24;
      byte[] numArray12 = buffer;
      int index9 = num23;
      int num25 = checked (index9 + 1);
      int num26 = (int) checked ((byte) (bytes.Length >> 8 & (int) byte.MaxValue));
      numArray12[index9] = (byte) num26;
      byte[] numArray13 = buffer;
      int index10 = num25;
      int destinationIndex1 = checked (index10 + 1);
      int num27 = (int) checked ((byte) (bytes.Length & (int) byte.MaxValue));
      numArray13[index10] = (byte) num27;
      Array.Copy((Array) bytes, 0, (Array) buffer, destinationIndex1, bytes.Length);
      int num28 = checked (destinationIndex1 + bytes.Length);
      if (this.willFlag && numArray1 != null)
      {
        byte[] numArray7 = buffer;
        int index4 = num28;
        int num14 = checked (index4 + 1);
        int num15 = (int) checked ((byte) (numArray1.Length >> 8 & (int) byte.MaxValue));
        numArray7[index4] = (byte) num15;
        byte[] numArray8 = buffer;
        int index5 = num14;
        int destinationIndex2 = checked (index5 + 1);
        int num16 = (int) checked ((byte) (numArray1.Length & (int) byte.MaxValue));
        numArray8[index5] = (byte) num16;
        Array.Copy((Array) numArray1, 0, (Array) buffer, destinationIndex2, numArray1.Length);
        num28 = checked (destinationIndex2 + numArray1.Length);
      }
      if (this.willFlag && numArray2 != null)
      {
        byte[] numArray7 = buffer;
        int index4 = num28;
        int num14 = checked (index4 + 1);
        int num15 = (int) checked ((byte) (numArray2.Length >> 8 & (int) byte.MaxValue));
        numArray7[index4] = (byte) num15;
        byte[] numArray8 = buffer;
        int index5 = num14;
        int destinationIndex2 = checked (index5 + 1);
        int num16 = (int) checked ((byte) (numArray2.Length & (int) byte.MaxValue));
        numArray8[index5] = (byte) num16;
        Array.Copy((Array) numArray2, 0, (Array) buffer, destinationIndex2, numArray2.Length);
        num28 = checked (destinationIndex2 + numArray2.Length);
      }
      if (numArray3 != null)
      {
        byte[] numArray7 = buffer;
        int index4 = num28;
        int num14 = checked (index4 + 1);
        int num15 = (int) checked ((byte) (numArray3.Length >> 8 & (int) byte.MaxValue));
        numArray7[index4] = (byte) num15;
        byte[] numArray8 = buffer;
        int index5 = num14;
        int destinationIndex2 = checked (index5 + 1);
        int num16 = (int) checked ((byte) (numArray3.Length & (int) byte.MaxValue));
        numArray8[index5] = (byte) num16;
        Array.Copy((Array) numArray3, 0, (Array) buffer, destinationIndex2, numArray3.Length);
        num28 = checked (destinationIndex2 + numArray3.Length);
      }
      if (numArray4 != null)
      {
        byte[] numArray7 = buffer;
        int index4 = num28;
        int num14 = checked (index4 + 1);
        int num15 = (int) checked ((byte) (numArray4.Length >> 8 & (int) byte.MaxValue));
        numArray7[index4] = (byte) num15;
        byte[] numArray8 = buffer;
        int index5 = num14;
        int destinationIndex2 = checked (index5 + 1);
        int num16 = (int) checked ((byte) (numArray4.Length & (int) byte.MaxValue));
        numArray8[index5] = (byte) num16;
        Array.Copy((Array) numArray4, 0, (Array) buffer, destinationIndex2, numArray4.Length);
        int num29 = checked (destinationIndex2 + numArray4.Length);
      }
      return buffer;
    }

    public override string ToString()
    {
      return this.GetTraceString("CONNECT", new object[12]{ (object) "protocolName", (object) "protocolVersion", (object) "clientId", (object) "willFlag", (object) "willRetain", (object) "willQosLevel", (object) "willTopic", (object) "willMessage", (object) "username", (object) "password", (object) "cleanSession", (object) "keepAlivePeriod" }, new object[12]{ (object) this.protocolName, (object) this.protocolVersion, (object) this.clientId, (object) this.willFlag, (object) this.willRetain, (object) this.willQosLevel, (object) this.willTopic, (object) this.willMessage, (object) this.username, (object) this.password, (object) this.cleanSession, (object) this.keepAlivePeriod });
    }
  }
}
