//************************************************************************
//
//    This work contains valuable confidential and proprietary
//    information.  Disclosure, use or reproduction without the
//    written authorization of the company is prohibited.  This
//    unpublished work by the company is protected by the laws of the
//    United States and other countries.  If publication of the work
//    should occur the following notice shall apply:
//    Copyright Baldwin Technology Company, Inc. 1998/2018 All Rights Reserved
//
//    Creation Date: 06/25/2018
//    Description: enum VarType to work with Variable Input and Output class
//
//************************************************************************

namespace Qti.Autotron.ModbusAutotronAPI
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
                        if ((long)this._inpValue == (long)value)
                            return;
                        this._inpValue = value;
                        this.CallVarChangeEvent((object)this);
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
                    this.CallVarChangeEvent((object)this);
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
