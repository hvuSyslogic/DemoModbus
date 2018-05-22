// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.MessageClientState
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

namespace PhoenixContact.HFI.Inline
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
