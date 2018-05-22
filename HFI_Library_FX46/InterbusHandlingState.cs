// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.InterbusHandlingState
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

namespace PhoenixContact.HFI.Inline
{
  public enum InterbusHandlingState
  {
    Idle = 0,
    CreateConfiguration = 33281, // 0x00008201
    ActivateConfiguration = 33282, // 0x00008202
    BusActivate = 33536, // 0x00008300
    BusStopped = 33538, // 0x00008302
    DoNotChangeState = 33791, // 0x000083FF
    MessageClientDiagnostic = 51713, // 0x0000CA01
  }
}
