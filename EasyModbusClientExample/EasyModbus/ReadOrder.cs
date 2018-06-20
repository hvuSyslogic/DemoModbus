// Decompiled with JetBrains decompiler
// Type: EasyModbus.ReadOrder
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System.Threading;

namespace EasyModbus
{
  public class ReadOrder
  {
    public int CylceTime = 500;
    public FunctionCode FunctionCode;
    public int StartingAddress;
    public int Quantity;
    public string[] Topic;
    public int[] Hysteresis;
    public string[] Unit;
    public float[] Scale;
    public bool[] Retain;
    internal Thread thread;
    internal object[] oldvalue;
  }
}
