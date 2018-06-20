// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgConnack
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgConnack : MqttMsgBase
  {
    public const byte CONN_ACCEPTED = 0;
    public const byte CONN_REFUSED_PROT_VERS = 1;
    public const byte CONN_REFUSED_IDENT_REJECTED = 2;
    public const byte CONN_REFUSED_SERVER_UNAVAILABLE = 3;
    public const byte CONN_REFUSED_USERNAME_PASSWORD = 4;
    public const byte CONN_REFUSED_NOT_AUTHORIZED = 5;
    private const byte TOPIC_NAME_COMP_RESP_BYTE_OFFSET = 0;
    private const byte TOPIC_NAME_COMP_RESP_BYTE_SIZE = 1;
    private const byte CONN_ACK_FLAGS_BYTE_OFFSET = 0;
    private const byte CONN_ACK_FLAGS_BYTE_SIZE = 1;
    private const byte SESSION_PRESENT_FLAG_MASK = 1;
    private const byte SESSION_PRESENT_FLAG_OFFSET = 0;
    private const byte SESSION_PRESENT_FLAG_SIZE = 1;
    private const byte CONN_RETURN_CODE_BYTE_OFFSET = 1;
    private const byte CONN_RETURN_CODE_BYTE_SIZE = 1;
    private bool sessionPresent;
    private byte returnCode;

    public bool SessionPresent
    {
      get
      {
        return this.sessionPresent;
      }
      set
      {
        this.sessionPresent = value;
      }
    }

    public byte ReturnCode
    {
      get
      {
        return this.returnCode;
      }
      set
      {
        this.returnCode = value;
      }
    }

    public MqttMsgConnack()
    {
      this.type = (byte) 2;
    }

    public static MqttMsgConnack Parse(byte fixedHeaderFirstByte, byte protocolVersion, IMqttNetworkChannel channel)
    {
      MqttMsgConnack mqttMsgConnack = new MqttMsgConnack();
      if (protocolVersion == (byte) 4 && ((uint) fixedHeaderFirstByte & 15U) > 0U)
        throw new MqttClientException(MqttClientErrorCode.InvalidFlagBits);
      byte[] buffer = new byte[MqttMsgBase.decodeRemainingLength(channel)];
      channel.Receive(buffer);
      if (protocolVersion == (byte) 4)
        mqttMsgConnack.sessionPresent = ((uint) buffer[0] & 1U) > 0U;
      mqttMsgConnack.returnCode = buffer[1];
      return mqttMsgConnack;
    }

    public override byte[] GetBytes(byte ProtocolVersion)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int num5 = ProtocolVersion != (byte) 4 ? checked (num1 + 2) : checked (num1 + 2);
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
      if (ProtocolVersion == (byte) 4)
      {
        byte[] numArray = buffer;
        int index2 = num4;
        index1 = checked (index2 + 1);
        int num8 = 32;
        numArray[index2] = (byte) num8;
      }
      else
      {
        byte[] numArray = buffer;
        int index2 = num4;
        index1 = checked (index2 + 1);
        int num8 = 32;
        numArray[index2] = (byte) num8;
      }
      int num9 = this.encodeRemainingLength(remainingLength, buffer, index1);
      int num10;
      if (ProtocolVersion == (byte) 4)
      {
        byte[] numArray = buffer;
        int index2 = num9;
        num10 = checked (index2 + 1);
        int num8 = this.sessionPresent ? 1 : 0;
        numArray[index2] = (byte) num8;
      }
      else
      {
        byte[] numArray = buffer;
        int index2 = num9;
        num10 = checked (index2 + 1);
        int num8 = 0;
        numArray[index2] = (byte) num8;
      }
      byte[] numArray1 = buffer;
      int index3 = num10;
      int num11 = checked (index3 + 1);
      int returnCode = (int) this.returnCode;
      numArray1[index3] = (byte) returnCode;
      return buffer;
    }

    public override string ToString()
    {
      return this.GetTraceString("CONNACK", new object[1]{ (object) "returnCode" }, new object[1]{ (object) this.returnCode });
    }
  }
}
