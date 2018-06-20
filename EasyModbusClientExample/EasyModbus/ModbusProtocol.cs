// Decompiled with JetBrains decompiler
// Type: EasyModbus.ModbusProtocol
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

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
