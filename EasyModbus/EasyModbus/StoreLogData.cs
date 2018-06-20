// Decompiled with JetBrains decompiler
// Type: EasyModbus.StoreLogData
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

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
