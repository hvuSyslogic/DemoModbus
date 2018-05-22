// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.InterbusHandlingState
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

namespace PhoenixContact.HFI
{
  public enum InterbusHandlingState
  {
    Idle = 0,
    CreateConfiguration = 33281, // 0x00008201
    ActivateConfiguration = 33282, // 0x00008202
    SvcDownloadActive = 33296, // 0x00008210
    BusActivate = 33536, // 0x00008300
    BusStopped = 33538, // 0x00008302
    SvcDownloadReady = 33552, // 0x00008310
    DoNotChangeState = 33791, // 0x000083FF
    SvcDownloadError = 49680, // 0x0000C210
  }
}
