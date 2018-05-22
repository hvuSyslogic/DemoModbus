// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.I_IBS_G4
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public interface I_IBS_G4
  {
    IBS_Diagnostic IBS_Diag { get; }

    event OnIBS_DiagChangeHandler OnIBS_DiagnosticChange;

    InterbusHandling Bus { get; set; }

    event DiagnosticHandler OnDiagnostic;
  }
}
