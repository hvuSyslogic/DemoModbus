// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.InterbusDiagnostic
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using System.Text;

namespace PhoenixContact.HFI.Inline
{
  public class InterbusDiagnostic
  {
    private object syncAccess = new object();
    private InterbusDiagChangeHandler _hdOnChange;
    private string _name;
    private StatusRegister _statusRegister;
    private int _parameterRegister;
    private int _extendedParameterRegister;

    internal InterbusDiagnostic(string Name)
    {
      if (Name.Length == 0)
        this._name = nameof (InterbusDiagnostic);
      else
        this._name = nameof (InterbusDiagnostic) + Name;
    }

    public StatusRegister StatusRegister
    {
      get
      {
        lock (this.syncAccess)
          return this._statusRegister;
      }
    }

    public int ParameterRegister
    {
      get
      {
        lock (this.syncAccess)
          return this._parameterRegister;
      }
    }

    public int ExtendedParameterRegister
    {
      get
      {
        lock (this.syncAccess)
          return this._extendedParameterRegister;
      }
    }

    internal string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = "InterbusDiagnostic " + value;
      }
    }

    internal void SetRegister(int StatusRegister, int ParameterRegister, int ExtendedParameterRegister)
    {
      lock (this.syncAccess)
      {
        if (StatusRegister == this._statusRegister.Value && ParameterRegister == this._parameterRegister)
          return;
        this._statusRegister.USER = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 0);
        this._statusRegister.PF = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 1);
        this._statusRegister.BUS = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 2);
        this._statusRegister.CTRL = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 3);
        this._statusRegister.DETECT = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 4);
        this._statusRegister.RUN = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 5);
        this._statusRegister.ACTIVE = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 6);
        this._statusRegister.READY = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 7);
        this._statusRegister.BSA = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 8);
        this._statusRegister.BASP = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 9);
        this._statusRegister.RESULT = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 10);
        this._statusRegister.SY_RESULT = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 11);
        this._statusRegister.DC_RESULT = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 12);
        this._statusRegister.WARNING = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 13);
        this._statusRegister.QUALITY = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 14);
        this._statusRegister.SDSI = PhoenixContact.PxC_Library.Util.Util.Int32ToBool(StatusRegister, 15);
        this._statusRegister.Value = StatusRegister;
        this._parameterRegister = ParameterRegister;
        this._extendedParameterRegister = ExtendedParameterRegister;
        if (this._hdOnChange == null)
          return;
        this._hdOnChange((object) this, this._statusRegister, this._parameterRegister, this._extendedParameterRegister);
      }
    }

    public event InterbusDiagChangeHandler OnChange
    {
      add
      {
        this._hdOnChange += value;
      }
      remove
      {
        this._hdOnChange -= value;
      }
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("StateRegister: ");
      stringBuilder.Append(this.StatusRegister.ToString());
      stringBuilder.AppendLine();
      stringBuilder.Append("ParaRegister: ");
      stringBuilder.Append(this.ParameterRegister.ToString("X4"));
      return stringBuilder.ToString();
    }
  }
}
