// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.SvcWriterResult
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

namespace PhoenixContact.HFI
{
  public enum SvcWriterResult
  {
    Inactive = 0,
    NoError = 33536, // 0x00008300
    ConfirmationTimeout = 49156, // 0x0000C004
    WritingSuccess = 49162, // 0x0000C00A
    WritingFailed = 49163, // 0x0000C00B
    ParseError = 49164, // 0x0000C00C
    NoSvcFilename = 49165, // 0x0000C00D
    FileNotFoundError = 49167, // 0x0000C00F
  }
}
