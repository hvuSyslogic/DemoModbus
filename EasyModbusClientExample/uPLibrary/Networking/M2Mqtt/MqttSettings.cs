// Decompiled with JetBrains decompiler
// Type: uPLibrary.Networking.M2Mqtt.MqttSettings
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

namespace uPLibrary.Networking.M2Mqtt
{
  public class MqttSettings
  {
    public const int MQTT_BROKER_DEFAULT_PORT = 1883;
    public const int MQTT_BROKER_DEFAULT_SSL_PORT = 8883;
    public const int MQTT_DEFAULT_TIMEOUT = 30000;
    public const int MQTT_ATTEMPTS_RETRY = 3;
    public const int MQTT_DELAY_RETRY = 10000;
    public const int MQTT_CONNECT_TIMEOUT = 30000;
    public const int MQTT_MAX_INFLIGHT_QUEUE_SIZE = 2147483647;
    private static MqttSettings instance;

    public int Port { get; internal set; }

    public int SslPort { get; internal set; }

    public int TimeoutOnConnection { get; internal set; }

    public int TimeoutOnReceiving { get; internal set; }

    public int AttemptsOnRetry { get; internal set; }

    public int DelayOnRetry { get; internal set; }

    public int InflightQueueSize { get; set; }

    public static MqttSettings Instance
    {
      get
      {
        if (MqttSettings.instance == null)
          MqttSettings.instance = new MqttSettings();
        return MqttSettings.instance;
      }
    }

    private MqttSettings()
    {
      this.Port = 1883;
      this.SslPort = 8883;
      this.TimeoutOnReceiving = 30000;
      this.AttemptsOnRetry = 3;
      this.DelayOnRetry = 10000;
      this.TimeoutOnConnection = 30000;
      this.InflightQueueSize = int.MaxValue;
    }
  }
}
