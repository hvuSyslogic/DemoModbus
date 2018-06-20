// Decompiled with JetBrains decompiler
// Type: EasyModbus.InputRegisters
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Threading;

namespace EasyModbus
{
  public class InputRegisters
  {
    public short[] localArray = new short[(int) ushort.MaxValue];
    private short[] mqttInputRegistersOldValues = new short[(int) ushort.MaxValue];
    private ModbusServer modbusServer;

    public InputRegisters(ModbusServer modbusServer)
    {
      this.modbusServer = modbusServer;
    }

    public short this[int x]
    {
      get
      {
        return this.localArray[x];
      }
      set
      {
        this.localArray[x] = value;
        if (this.modbusServer.MqttBrokerAddress == null || (int) this.localArray[x] == (int) this.mqttInputRegistersOldValues[x])
          return;
        this.mqttInputRegistersOldValues[x] = this.localArray[x];
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
          this.modbusServer.easyModbus2Mqtt.publish(this.modbusServer.MqttRootTopic + "/inputregisters" + (object) index, this.localArray[index].ToString(), this.modbusServer.MqttBrokerAddress);
        }
        catch (Exception ex)
        {
        }
        Thread.Sleep(100);
      }
    }
  }
}
