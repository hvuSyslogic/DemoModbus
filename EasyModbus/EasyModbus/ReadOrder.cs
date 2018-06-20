// Decompiled with JetBrains decompiler
// Type: EasyModbus.ReadOrder
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

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
