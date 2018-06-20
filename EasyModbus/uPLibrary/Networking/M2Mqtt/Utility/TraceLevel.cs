// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.Utility.TraceLevel
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

namespace uPLibrary.Networking.M2Mqtt.Utility
{
  public enum TraceLevel
  {
    Error = 1,
    Warning = 2,
    Information = 4,
    Verbose = 15, // 0x0000000F
    Frame = 16, // 0x00000010
    Queuing = 32, // 0x00000020
  }
}
