// Decompiled with JetBrains decompiler
// Type: EasyModbus.HoldingRegisters
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Threading;

namespace EasyModbus
{
  public class HoldingRegisters
  {
    public short[] localArray = new short[(int) ushort.MaxValue];
    private short[] mqttHoldingRegistersOldValues = new short[(int) ushort.MaxValue];
    private ModbusServer modbusServer;

    public HoldingRegisters(ModbusServer modbusServer)
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
        if (this.modbusServer.MqttBrokerAddress != null && (int) this.localArray[x] != (int) this.mqttHoldingRegistersOldValues[x])
          new Thread(new ParameterizedThreadStart(this.DoWork)).Start((object) x);
        this.mqttHoldingRegistersOldValues[x] = this.localArray[x];
      }
    }

    private void DoWork(object parameter)
    {
      lock (this.modbusServer.lockMQTT)
      {
        int index = (int) parameter;
        try
        {
          this.modbusServer.easyModbus2Mqtt.publish(this.modbusServer.MqttRootTopic + "/holdingregisters" + (object) index, this.localArray[index].ToString(), this.modbusServer.MqttBrokerAddress);
        }
        catch (Exception ex)
        {
        }
        Thread.Sleep(100);
      }
    }
  }
}
