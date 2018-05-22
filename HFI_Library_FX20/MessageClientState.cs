// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.MessageClientState
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

namespace PhoenixContact.HFI
{
  public enum MessageClientState
  {
    Idle = 0,
    SendRequest = 33537, // 0x00008301
    SendRequestOnly = 33538, // 0x00008302
    WaitingForConfirmation = 33546, // 0x0000830A
    ConfirmationReceived = 33556, // 0x00008314
    SendRequestOnlyDone = 33557, // 0x00008315
    Error = 49251, // 0x0000C063
  }
}
