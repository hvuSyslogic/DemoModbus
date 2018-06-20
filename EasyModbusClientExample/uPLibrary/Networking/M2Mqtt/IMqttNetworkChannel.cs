// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.IMqttNetworkChannel
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

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
