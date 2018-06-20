// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;

namespace uPLibrary.Networking.M2Mqtt.Messages
{
  public class MqttMsgSubscribedEventArgs : EventArgs
  {
    private ushort messageId;
    private byte[] grantedQosLevels;

    public ushort MessageId
    {
      get
      {
        return this.messageId;
      }
      internal set
      {
        this.messageId = value;
      }
    }

    public byte[] GrantedQoSLevels
    {
      get
      {
        return this.grantedQosLevels;
      }
      internal set
      {
        this.grantedQosLevels = value;
      }
    }

    public MqttMsgSubscribedEventArgs(ushort messageId, byte[] grantedQosLevels)
    {
      this.messageId = messageId;
      this.grantedQosLevels = grantedQosLevels;
    }
  }
}
