// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.IMqttNetworkChannel
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

namespace uPLibrary.Networking.M2Mqtt
{
  public interface IMqttNetworkChannel
  {
    bool DataAvailable { get; }

    int Receive(byte[] buffer);

    int Receive(byte[] buffer, int timeout);

    int Send(byte[] buffer);

    void Close();

    void Connect();

    void Accept();
  }
}
