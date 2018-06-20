// Decompiled with JetBrains decompiler
// Type: EasyModbus.ModbusProtocol
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;

namespace EasyModbus
{
  public class ModbusProtocol
  {
    public DateTime timeStamp;
    public bool request;
    public bool response;
    public ushort transactionIdentifier;
    public ushort protocolIdentifier;
    public ushort length;
    public byte unitIdentifier;
    public byte functionCode;
    public ushort startingAdress;
    public ushort startingAddressRead;
    public ushort startingAddressWrite;
    public ushort quantity;
    public ushort quantityRead;
    public ushort quantityWrite;
    public byte byteCount;
    public byte exceptionCode;
    public byte errorCode;
    public ushort[] receiveCoilValues;
    public ushort[] receiveRegisterValues;
    public short[] sendRegisterValues;
    public bool[] sendCoilValues;
    public ushort crc;

    public enum ProtocolType
    {
      ModbusTCP,
      ModbusUDP,
      ModbusRTU,
    }
  }
}
