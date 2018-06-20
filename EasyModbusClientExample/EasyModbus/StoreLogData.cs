// Decompiled with JetBrains decompiler
// Type: EasyModbus.StoreLogData
// Assembly: EasyModbusClientExample, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92ADC808-D80B-41C0-B9AB-216E9E70F2AD
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusClient (.NET Version)\EasyModbusClient (.NET Version)\EasyModbusClientExample.exe

using System;
using System.IO;

namespace EasyModbus
{
  public sealed class StoreLogData
  {
    private static object syncObject = new object();
    private string filename = (string) null;
    private static volatile StoreLogData instance;

    private StoreLogData()
    {
    }

    public static StoreLogData Instance
    {
      get
      {
        if (StoreLogData.instance == null)
        {
          lock (StoreLogData.syncObject)
          {
            if (StoreLogData.instance == null)
              StoreLogData.instance = new StoreLogData();
          }
        }
        return StoreLogData.instance;
      }
    }

    public void Store(string message)
    {
      if (this.filename == null)
        return;
      using (StreamWriter streamWriter = new StreamWriter(this.Filename, true))
        streamWriter.WriteLine(message);
    }

    public void Store(string message, DateTime timestamp)
    {
      try
      {
        using (StreamWriter streamWriter = new StreamWriter(this.Filename, true))
          streamWriter.WriteLine(timestamp.ToString("dd.MM.yyyy H:mm:ss.ff ") + message);
      }
      catch (Exception ex)
      {
      }
    }

    public string Filename
    {
      get
      {
        return this.filename;
      }
      set
      {
        this.filename = value;
      }
    }
  }
}
