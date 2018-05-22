// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Variable
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public abstract class Variable
  {
    private int _maxVariableLength = 63;
    private string _name;
    private int _baseAddress;
    private int _length;
    private int _byteLength;
    private int _bitOffset;
    internal VarType _varType;
    private long _maxValue;
    private long _minValue;
    private VarChangeHandler _inpChange;

    public Variable(int BaseAddress, PD_Length Size, int Length, int BitOffset, string Name)
    {
      if (BaseAddress < 0 || Length <= 0 || (Length > (int) Size * 8 || Length > this._maxVariableLength) || (BitOffset < 0 || BitOffset > (int) Size * 8 - 1 || BitOffset + Length > (int) Size * 8))
      {
        this._baseAddress = 0;
        this._length = 0;
        this._bitOffset = 0;
        this._varType = VarType.Unknown;
      }
      else
      {
        this._baseAddress = BaseAddress;
        this._length = Length;
        this._bitOffset = BitOffset;
        this._varType = this._length != 1 ? VarType.UInt63 : VarType.Boolean;
        this.calcMinMaxRange();
        this._byteLength = (int) Size;
        this._name = Name + " " + this._baseAddress.ToString() + "." + this._bitOffset.ToString() + " (" + this._varType.ToString() + ")";
      }
    }

    public Variable(int BaseAddress, PD_Length Size, int Length, string Name)
    {
      if (BaseAddress < 0 || Length <= 0 || (Length > (int) Size * 8 || Length > this._maxVariableLength))
      {
        this._baseAddress = 0;
        this._length = 0;
        this._bitOffset = 0;
        this._varType = VarType.Unknown;
      }
      else
      {
        this._baseAddress = BaseAddress;
        this._length = Length;
        this._bitOffset = 0;
        this._varType = VarType.UInt63;
        this.calcMinMaxRange();
        this._byteLength = (int) Size;
        this._name = Name + " " + this._baseAddress.ToString() + "." + this._bitOffset.ToString() + " (" + this._varType.ToString() + ")";
      }
    }

    public Variable(int BaseAddress, int ByteLength, string Name)
    {
      if (BaseAddress < 0 || ByteLength < 0)
      {
        this._baseAddress = 0;
        this._length = 0;
        this._bitOffset = 0;
        this._varType = VarType.Unknown;
      }
      else
      {
        this._baseAddress = BaseAddress;
        this._length = 0;
        this._bitOffset = 0;
        this._byteLength = ByteLength;
        this._varType = VarType.ByteArray;
      }
      this._name = Name + " " + this._baseAddress.ToString() + " (" + this._varType.ToString() + ")";
    }

    public override string ToString()
    {
      return this._name;
    }

    private void calcMinMaxRange()
    {
      this._maxValue = (long) (Math.Pow(2.0, (double) this._length) - 1.0);
      this._minValue = 0L;
    }

    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    public long MaxValue
    {
      get
      {
        return this._maxValue;
      }
    }

    public long MinValue
    {
      get
      {
        return this._minValue;
      }
    }

    public int BaseAddress
    {
      get
      {
        return this._baseAddress;
      }
    }

    public int Length
    {
      get
      {
        return this._length;
      }
    }

    public int BitOffset
    {
      get
      {
        return this._bitOffset;
      }
    }

    public int ByteLength
    {
      get
      {
        return this._byteLength;
      }
    }

    public VarType VarType
    {
      get
      {
        return this._varType;
      }
    }

    public event VarChangeHandler OnChange
    {
      add
      {
        this._inpChange += value;
      }
      remove
      {
        this._inpChange -= value;
      }
    }

    internal void CallVarChangeEvent(object Sender)
    {
      if (this._inpChange == null)
        return;
      this._inpChange(Sender);
    }

    internal bool VarChangeHandlerDelegateValid
    {
      get
      {
        return this._inpChange != null;
      }
    }
  }
}
