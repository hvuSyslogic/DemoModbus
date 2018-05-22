// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.StatusRegister
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;

namespace PhoenixContact.HFI
{
  public struct StatusRegister
  {
    public bool USER;
    public bool PF;
    public bool BUS;
    public bool CTRL;
    public bool DETECT;
    public bool RUN;
    public bool ACTIVE;
    public bool READY;
    public bool BSA;
    public bool BASP;
    public bool RESULT;
    public bool SY_RESULT;
    public bool DC_RESULT;
    public bool WARNING;
    public bool QUALITY;
    public bool SDSI;
    public int Value;

    internal string ToString(string p)
    {
      throw new Exception("The method or operation is not implemented.");
    }
  }
}
