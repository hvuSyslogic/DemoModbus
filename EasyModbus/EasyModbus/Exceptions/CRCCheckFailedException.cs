// Decompiled with JetBrains decompiler
// Type: EasyModbus.Exceptions.CRCCheckFailedException
// Assembly: EasyModbus, Version=5.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EBD23A4B-2CA7-473C-89B3-8C8FE0533ECB
// Assembly location: D:\hvu\EasyModbusTCP .NET Package V5.0\EasyModbusTCP .NET Package V5.0\EasyModbusLibrary for .NET (DLL)\EasyModbusLibrary for .NET (DLL)\EasyModbus.dll

using System;
using System.Runtime.Serialization;

namespace EasyModbus.Exceptions
{
  public class CRCCheckFailedException : ModbusException
  {
    public CRCCheckFailedException()
    {
    }

    public CRCCheckFailedException(string message)
      : base(message)
    {
    }

    public CRCCheckFailedException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected CRCCheckFailedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
