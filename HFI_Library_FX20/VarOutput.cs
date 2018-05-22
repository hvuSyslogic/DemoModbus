// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.VarOutput
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public class VarOutput : Variable
  {
    private long _outValue;
    private byte[] _outByteArray;

    public VarOutput(int BaseAddress, PD_Length Size, int Length, int BitOffset)
      : base(BaseAddress, Size, Length, BitOffset, "Output")
    {
      this._outValue = 0L;
    }

    public VarOutput(int BaseAddress, PD_Length Size, int Length)
      : base(BaseAddress, Size, Length, "Output")
    {
      this._outValue = 0L;
    }

    public VarOutput(int BaseAddress, int ByteLength)
      : base(BaseAddress, ByteLength, "Output")
    {
      this._outValue = 0L;
      if (ByteLength > 0)
        this._outByteArray = new byte[ByteLength];
      else
        this._outByteArray = new byte[0];
    }

    internal long GetValue
    {
      get
      {
        lock (this)
          return this._outValue;
      }
    }

    internal byte[] GetByteArray
    {
      get
      {
        lock (this)
          return this._outByteArray;
      }
    }

    public long Value
    {
      get
      {
        lock (this)
          return this._outValue;
      }
      set
      {
        if (this._outValue == value)
          return;
        lock (this)
          this._outValue = value <= this.MaxValue ? (value >= this.MinValue ? value : this.MinValue) : this.MaxValue;
        this.CallVarChangeEvent((object) this);
      }
    }

    public bool State
    {
      get
      {
        lock (this)
          return Convert.ToBoolean(this._outValue);
      }
      set
      {
        lock (this)
        {
          if (this._outValue == Convert.ToInt64(value))
            return;
          this._outValue = Convert.ToInt64(value);
          this.CallVarChangeEvent((object) this);
        }
      }
    }

    public byte[] ByteArray
    {
      get
      {
        lock (this)
          return this._outByteArray;
      }
      set
      {
        lock (this)
        {
          if (value != null)
          {
            if (this._outByteArray.Length != value.Length)
              return;
            this._outByteArray = value;
          }
          else
            this._outByteArray = new byte[0];
        }
      }
    }
  }
}
