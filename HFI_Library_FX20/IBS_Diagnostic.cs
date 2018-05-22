// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.IBS_Diagnostic
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

namespace PhoenixContact.HFI
{
  public class IBS_Diagnostic
  {
    private StatusRegister _statusRegister;
    private int _parameterRegister;
    private int _extendedParameterRegister;

    public StatusRegister StatusRegister
    {
      get
      {
        return this._statusRegister;
      }
    }

    public int ParameterRegister
    {
      get
      {
        return this._parameterRegister;
      }
    }

    public int ExtendedParameterRegister
    {
      get
      {
        return this._extendedParameterRegister;
      }
    }

    internal int SetStatusRegister
    {
      set
      {
        this._statusRegister.USER = Util.Int32ToBool(value, 0);
        this._statusRegister.PF = Util.Int32ToBool(value, 1);
        this._statusRegister.BUS = Util.Int32ToBool(value, 2);
        this._statusRegister.CTRL = Util.Int32ToBool(value, 3);
        this._statusRegister.DETECT = Util.Int32ToBool(value, 4);
        this._statusRegister.RUN = Util.Int32ToBool(value, 5);
        this._statusRegister.ACTIVE = Util.Int32ToBool(value, 6);
        this._statusRegister.READY = Util.Int32ToBool(value, 7);
        this._statusRegister.BSA = Util.Int32ToBool(value, 8);
        this._statusRegister.BASP = Util.Int32ToBool(value, 9);
        this._statusRegister.RESULT = Util.Int32ToBool(value, 10);
        this._statusRegister.SY_RESULT = Util.Int32ToBool(value, 11);
        this._statusRegister.DC_RESULT = Util.Int32ToBool(value, 12);
        this._statusRegister.WARNING = Util.Int32ToBool(value, 13);
        this._statusRegister.QUALITY = Util.Int32ToBool(value, 14);
        this._statusRegister.SDSI = Util.Int32ToBool(value, 15);
        this._statusRegister.Value = value;
      }
    }

    internal int SetParameterRegister
    {
      set
      {
        this._parameterRegister = value;
      }
      get
      {
        return this._parameterRegister;
      }
    }

    internal int SetExtendedParameterRegister
    {
      set
      {
        this._extendedParameterRegister = value;
      }
    }
  }
}
