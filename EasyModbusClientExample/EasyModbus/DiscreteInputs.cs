// Decompiled with JetBrains decompiler
// Type: EasyModbus.DiscreteInputs
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;
using System.Threading;

namespace EasyModbus
{
  public class DiscreteInputs
  {
    public bool[] localArray = new bool[(int) ushort.MaxValue];
    private bool[] mqttDiscreteInputsOldValues = new bool[(int) ushort.MaxValue];
    private ModbusServer modbusServer;

    public DiscreteInputs(ModbusServer modbusServer)
    {
      this.modbusServer = modbusServer;
    }

    public bool this[int x]
    {
      get
      {
        return this.localArray[x];
      }
      set
      {
        this.localArray[x] = value;
        if (this.modbusServer.MqttBrokerAddress == null || this.localArray[x] == this.mqttDiscreteInputsOldValues[x])
          return;
        this.mqttDiscreteInputsOldValues[x] = this.localArray[x];
        new Thread(new ParameterizedThreadStart(this.DoWork)).Start((object) x);
      }
    }

    private void DoWork(object parameter)
    {
      lock (this.modbusServer.lockMQTT)
      {
        int index = (int) parameter;
        try
        {
          this.modbusServer.easyModbus2Mqtt.publish(this.modbusServer.MqttRootTopic + "/discreteinputs" + (object) index, this.localArray[index].ToString(), this.modbusServer.MqttBrokerAddress);
        }
        catch (Exception ex)
        {
        }
        Thread.Sleep(100);
      }
    }
  }
}
