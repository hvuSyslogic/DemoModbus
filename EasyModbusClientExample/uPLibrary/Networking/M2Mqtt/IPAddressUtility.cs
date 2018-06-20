// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.IPAddressUtility
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System.Net;
using System.Net.Sockets;

namespace uPLibrary.Networking.M2Mqtt
{
  public static class IPAddressUtility
  {
    public static AddressFamily GetAddressFamily(this IPAddress ipAddress)
    {
      return ipAddress.AddressFamily;
    }
  }
}
