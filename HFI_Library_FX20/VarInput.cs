// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.VarInput
// Assembly: HFI_Library_FX20, Version=2.1.0.0, Culture=neutral, PublicKeyToken=61dd274f0cd79c49
// MVID: BBEABD5D-3D47-474E-899D-9A7AB31C38F6
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\HFI_Library_FX20.dll

using System;

namespace PhoenixContact.HFI
{
  [CLSCompliant(true)]
  public class VarInput : Variable
  {
    private long _inpValue;
    private byte[] _inpByteArray;

    public VarInput(int BaseAddress, PD_Length Size, int Length, int BitOffset)
      : base(BaseAddress, Size, Length, BitOffset, "Input")
    {
      this._inpValue = 0L;
    }

    public VarInput(int BaseAddress, PD_Length Size, int Length)
      : base(BaseAddress, Size, Length, "Input")
    {
      this._inpValue = 0L;
    }

    public VarInput(int BaseAddress, int ByteLength)
      : base(BaseAddress, ByteLength, "Input")
    {
      this._inpValue = 0L;
      if (ByteLength > 0)
      {
        this._inpByteArray = new byte[ByteLength];
      }
      else
      {
        this._inpByteArray = new byte[0];
        this._varType = VarType.Unknown;
      }
    }

    internal long SetValue
    {
      set
      {
        lock (this)
        {
          if (this.VarChangeHandlerDelegateValid)
          {
            if (this._inpValue == value)
              return;
            this._inpValue = value;
            this.CallVarChangeEvent((object) this);
          }
          else
            this._inpValue = value;
        }
      }
    }

    internal byte[] SetByteArray
    {
      set
      {
        if (this._inpByteArray.Length <= 0 || value == null)
          return;
        lock (this)
        {
          if (this.VarChangeHandlerDelegateValid)
          {
            if (this._inpByteArray.Length < value.Length)
              return;
            for (int index = 0; index < this._inpByteArray.Length; ++index)
            {
              if ((int) this._inpByteArray[index] != (int) value[index])
              {
                this._inpByteArray = value;
                this.CallVarChangeEvent((object) this);
                break;
              }
            }
          }
          else
          {
            if (this._inpByteArray.Length < value.Length)
              return;
            this._inpByteArray = value;
          }
        }
      }
    }

    public long Value
    {
      get
      {
        lock (this)
          return this._inpValue;
      }
    }

    public bool State
    {
      get
      {
        lock (this)
          return this._inpValue != 0L;
      }
    }

    public byte[] ByteArray
    {
      get
      {
        lock (this)
        {
          if (this._inpByteArray.Length == 0)
            return new byte[0];
          return this._inpByteArray;
        }
      }
    }
  }
}
