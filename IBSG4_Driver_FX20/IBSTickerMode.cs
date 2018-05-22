// Decompiled with JetBrains decompiler
// Type: PhoenixContact.DDI.IBSTickerMode
// Assembly: IBSG4_Driver_FX20, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3ba9beb416a0ed83
// MVID: 066AFE0C-D702-4CB2-814E-202CA622D4F8
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\IBSG4_Driver_FX20.dll

using System;

namespace PhoenixContact.DDI
{
  [CLSCompliant(true)]
  public enum IBSTickerMode
  {
    Idle = 0,
    Start = 33024, // 0x00008100
    Active = 33025, // 0x00008101
    Stop = 33026, // 0x00008102
    Error = 49152, // 0x0000C000
  }
}
