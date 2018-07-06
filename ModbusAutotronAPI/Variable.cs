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
//    Description: Variable to work with Variable Input and Output class
//
//************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public abstract class Variable : IComparable, IVariable
    {
        private int _maxVariableLength = 64;
        private string _name;
        private string _controllerName;
        private int _baseAddress;
        private int _length;
        private int _byteLength;
        private int _bitOffset;
        internal VarType _varType;
        private ulong _maxValue;
        private ulong _minValue;
        private VarChangeHandler _hdVarChange;

        public Variable(int pBaseAddress, PD_Length pSize, int pLength, int pBitOffset, string pName)
        {
            if (pBaseAddress < 0 || pLength <= 0 || (pLength > (int)pSize * 8 || pLength > this._maxVariableLength) || (pBitOffset < 0 || pBitOffset > (int)pSize * 8 - 1 || pBitOffset + pLength > (int)pSize * 8))
            {
                this.BaseAddress = 0;
                this.Length = 0;
                this.BitOffset = 0;
                this.VarType = VarType.Unknown;
            }
            else
            {
                this.BaseAddress = pBaseAddress;
                this.Length = pLength;
                this.BitOffset = pBitOffset;
                this._varType = this.Length != 1 ? VarType.UInt64 : VarType.Boolean;
                this.CalcMinMaxRange();
                this.ByteLength = (int)pSize;
                if (pName != "Input" && pName != "Output")
                    this.Name = pName;
                else
                    this.Name = string.Format("{0} {1}.{2}", (object)pName, (object)this.BaseAddress.ToString(), (object)this.BitOffset.ToString());
            }
        }

        public Variable(int pBaseAddress, PD_Length pSize, int pLength, string pName)
        {
            if (pBaseAddress < 0 || pLength <= 0 || (pLength > (int)pSize * 8 || pLength > this._maxVariableLength))
            {
                this.BaseAddress = 0;
                this.Length = 0;
                this.BitOffset = 0;
                this.VarType = VarType.Unknown;
            }
            else
            {
                this.BaseAddress = pBaseAddress;
                this.Length = pLength;
                this.BitOffset = 0;
                this.VarType = VarType.UInt64;
                this.CalcMinMaxRange();
                this.ByteLength = (int)pSize;
                if (pName != "Input" && pName != "Output")
                    this.Name = pName;
                else
                    this.Name = string.Format("{0} {1}.{2}", (object)pName, (object)this.BaseAddress.ToString(), (object)this.BitOffset.ToString());
            }
        }

        public Variable(int pBaseAddress, int pByteLength, string pName)
        {
            if (pBaseAddress < 0 || pByteLength <= 0)
            {
                this.BaseAddress = 0;
                this.Length = 0;
                this.BitOffset = 0;
                this.VarType = VarType.Unknown;
            }
            else
            {
                this.BaseAddress = pBaseAddress;
                this.Length = 0;
                this.BitOffset = 0;
                this.ByteLength = pByteLength;
                this.VarType = VarType.ByteArray;
            }
            if (pName != "Input" && pName != "Output")
                this.Name = pName;
            else
                this.Name = string.Format("{0} {1}.{2}", (object)pName, (object)this.BaseAddress.ToString(), (object)this.BitOffset.ToString());
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", (object)this.Name, (object)this.VarType.ToString());
        }

        internal void AssignController(string ControllerName)
        {
            this.ControllerAssigned = !string.IsNullOrEmpty(ControllerName);
            this._controllerName = ControllerName;
        }

        private void CalcMinMaxRange()
        {
            this._maxValue = this._length > 63 ? ulong.MaxValue : Convert.ToUInt64(Math.Pow(2.0, (double)this._length) - 1.0);
            this._minValue = 0UL;
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            internal set
            {
                this._name = value;
            }
        }

        public string ControllerName
        {
            get
            {
                return this._controllerName;
            }
        }

        internal bool ControllerAssigned { get; private set; }

        public ulong MaxValue
        {
            get
            {
                return this._maxValue;
            }
        }

        public ulong MinValue
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
            internal set
            {
                this._baseAddress = value;
            }
        }

        public int Length
        {
            get
            {
                return this._length;
            }
            internal set
            {
                this._length = value;
            }
        }

        public int BitOffset
        {
            get
            {
                return this._bitOffset;
            }
            internal set
            {
                this._bitOffset = value;
            }
        }

        public int ByteLength
        {
            get
            {
                return this._byteLength;
            }
            internal set
            {
                this._byteLength = value;
            }
        }

        public VarType VarType
        {
            get
            {
                return this._varType;
            }
            internal set
            {
                this._varType = value;
            }
        }

        public event VarChangeHandler OnChange
        {
            add
            {
                this._hdVarChange += value;
            }
            remove
            {
                this._hdVarChange -= value;
            }
        }

        internal void CallVarChangeEvent(object Sender)
        {
            if (this._hdVarChange == null)
                return;
            this._hdVarChange(Sender);
        }

        internal bool OnChangeEventValid
        {
            get
            {
                return this._hdVarChange != null;
            }
        }

        public int CompareTo(object obj)
        {
            Variable var = obj as Variable;
            if (this.BaseAddress > var.BaseAddress)
                return 1;
            if (this.BaseAddress < var.BaseAddress)
                return -1;
            return this.CompareBitOffset(var);
        }

        private int CompareBitOffset(Variable var)
        {
            if (this.BitOffset > var.BitOffset)
                return 1;
            if (this.BitOffset < var.BitOffset)
                return -1;
            return this.CompareVarName(var);
        }

        private int CompareVarName(Variable var)
        {
            int num = var.Name.CompareTo(var.Name);
            if (num == 0)
                return this.CompareVarType(var);
            return num;
        }

        private int CompareVarType(Variable var)
        {
            int num = this.VarType.CompareTo((object)var.VarType);
            if (num == 0)
                return this.CompareByteLenght(var);
            return num;
        }

        private int CompareByteLenght(Variable var)
        {
            if (this.ByteLength > var.ByteLength)
                return 1;
            if (this.ByteLength < var.ByteLength)
                return -1;
            return this.CompareLenght(var);
        }

        private int CompareLenght(Variable var)
        {
            if (this.Length > var.Length)
                return 1;
            return this.Length < var.Length ? -1 : 0;
        }
    }

}
