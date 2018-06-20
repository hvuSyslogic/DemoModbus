// Decompiled with JetBrains decompiler
// Type: EasyModbus.NetworkConnectionParameter
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System.Net;
using System.Net.Sockets;

namespace EasyModbus
{
  internal struct NetworkConnectionParameter
  {
    public NetworkStream stream;
    public byte[] bytes;
    public int portIn;
    public IPAddress ipAddressIn;
  }
}
