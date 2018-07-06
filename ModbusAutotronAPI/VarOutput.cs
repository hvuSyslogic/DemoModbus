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
//    Creation Date: 06/27/2018
//    Description: Utility Extensions class to work with the assembly 
//
//************************************************************************
using System;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public class VarOutput : Variable
    {
        private readonly object accessInt = new object();
        private readonly object accessByte = new object();
        private ulong _outValue;
        private byte[] _outByteArray;

        public VarOutput(int pBaseAddress, PD_Length pSize, int pLength, int pBitOffset)
          : this(pBaseAddress, pSize, pLength, pBitOffset, "Output")
        {
        }

        public VarOutput(int pBaseAddress, PD_Length pSize, int pLength, int pBitOffset, string pName)
          : base(pBaseAddress, pSize, pLength, pBitOffset, string.IsNullOrEmpty(pName) ? "Output" : pName)
        {
            this._outValue = this.MinValue;
        }

        public VarOutput(int pBaseAddress, PD_Length pSize, int pLength)
          : this(pBaseAddress, pSize, pLength, "Output")
        {
        }

        public VarOutput(int pBaseAddress, PD_Length pSize, int pLength, string pName)
          : base(pBaseAddress, pSize, pLength, string.IsNullOrEmpty(pName) ? "Output" : pName)
        {
            this._outValue = this.MinValue;
        }

        public VarOutput(int pBaseAddress, int pByteLength)
          : this(pBaseAddress, pByteLength, "Output")
        {
        }

        public VarOutput(int pBaseAddress, int pByteLength, string pName)
          : base(pBaseAddress, pByteLength, string.IsNullOrEmpty(pName) ? "Output" : pName)
        {
            this._outValue = this.MinValue;
            if (this.ByteLength > 0)
                this._outByteArray = new byte[this.ByteLength];
            else
                this._outByteArray = new byte[0];
        }

        internal ulong GetValue()
        {
            lock (this.accessInt)
                return this._outValue;
        }

        internal byte[] GetByteArray()
        {
            lock (this.accessByte)
                return this._outByteArray;
        }

        public ulong Value
        {
            get
            {
                lock (this.accessInt)
                    return this._outValue;
            }
            set
            {
                lock (this.accessInt)
                {
                    if (value > this.MaxValue || (long)this._outValue == (long)value)
                        return;
                    this._outValue = value;
                    this.CallVarChangeEvent((object)this);
                }
            }
        }

        public bool State
        {
            get
            {
                lock (this.accessInt)
                    return Convert.ToBoolean(this._outValue);
            }
            set
            {
                lock (this.accessInt)
                {
                    if ((long)this._outValue == (long)Convert.ToUInt64(value))
                        return;
                    this._outValue = Convert.ToUInt64(value);
                    this.CallVarChangeEvent((object)this);
                }
            }
        }

        public byte[] ByteArray
        {
            get
            {
                return this.GetByteArray();
            }
            set
            {
                this.SetByteArray(value);
            }
        }

        private void SetByteArray(byte[] data)
        {
            lock (this.accessByte)
            {
                if (data == null)
                    return;
                if (this._outByteArray.Length != data.Length)
                    throw new ArgumentException("Wrong byte array length.");
                this._outByteArray = data;
            }
        }
    }

}
