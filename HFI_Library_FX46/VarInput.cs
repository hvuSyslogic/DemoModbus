// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.VarInput
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using PhoenixContact.PxC_Library.Util;

namespace PhoenixContact.HFI.Inline
{
  public class VarInput : Variable
  {
    private readonly object accessInt = new object();
    private readonly object accessByte = new object();
    private ulong _inpValue;
    private byte[] _inpByteArray;

    public VarInput(int pBaseAddress, PD_Length pSize, int pLength, int pBitOffset)
      : this(pBaseAddress, pSize, pLength, pBitOffset, "Input")
    {
    }

    public VarInput(int pBaseAddress, PD_Length pSize, int pLength, int pBitOffset, string pName)
      : base(pBaseAddress, pSize, pLength, pBitOffset, string.IsNullOrEmpty(pName) ? "Input" : pName)
    {
      this._inpValue = this.MinValue;
    }

    public VarInput(int pBaseAddress, PD_Length pSize, int pLength)
      : this(pBaseAddress, pSize, pLength, "Input")
    {
    }

    public VarInput(int pBaseAddress, PD_Length pSize, int pLength, string pName)
      : base(pBaseAddress, pSize, pLength, string.IsNullOrEmpty(pName) ? "Input" : pName)
    {
      this._inpValue = this.MinValue;
    }

    public VarInput(int pBaseAddress, int pByteLength)
      : this(pBaseAddress, pByteLength, "Input")
    {
    }

    public VarInput(int pBaseAddress, int pByteLength, string pName)
      : base(pBaseAddress, pByteLength, string.IsNullOrEmpty(pName) ? "Input" : pName)
    {
      this._inpValue = this.MinValue;
      if (this.ByteLength > 0)
      {
        this._inpByteArray = new byte[this.ByteLength];
      }
      else
      {
        this._inpByteArray = new byte[0];
        this._varType = VarType.Unknown;
      }
    }

    internal ulong SetValue
    {
      set
      {
        lock (this.accessInt)
        {
          if (this.OnChangeEventValid)
          {
            if ((long) this._inpValue == (long) value)
              return;
            this._inpValue = value;
            this.CallVarChangeEvent((object) this);
          }
          else
            this._inpValue = value;
        }
      }
    }

    internal void SetByteArray(byte[] data)
    {
      if (this._inpByteArray.Length == 0 || data == null)
        return;
      lock (this.accessByte)
      {
        if (this.OnChangeEventValid)
        {
          if (this._inpByteArray.Length < data.Length || this._inpByteArray.Compare(data))
            return;
          this._inpByteArray = data;
          this.CallVarChangeEvent((object) this);
        }
        else
        {
          if (this._inpByteArray.Length < data.Length)
            return;
          this._inpByteArray = data;
        }
      }
    }

    public ulong Value
    {
      get
      {
        lock (this.accessInt)
          return this._inpValue;
      }
    }

    public bool State
    {
      get
      {
        lock (this.accessInt)
          return this._inpValue != 0UL;
      }
    }

    public byte[] ByteArray
    {
      get
      {
        return this.GetByteArray();
      }
    }

    private byte[] GetByteArray()
    {
      lock (this.accessByte)
      {
        if (this._inpByteArray.Length == 0)
          return new byte[0];
        return this._inpByteArray;
      }
    }
  }
}
