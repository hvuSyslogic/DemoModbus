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
//    Creation Date: 07/5/2018
//    Description: EasyModbusItem implement IEasyModbusItem interface 
//    should be in a collection resides in the EasyModbusWrapper
//
//************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public class EasyModbusItem : IEasyModbusItem
    {
        public EasyModbusItem(int functionCode, int startingAddress, int quantity)
        {
            FunctionCode = functionCode;
            StartingAddress = startingAddress;
            Quantity = quantity;
        }
        public EasyModbusItem(int functionCode, int startingAddress, int quantity, int[] dataSource)
        {
            FunctionCode = functionCode;
            StartingAddress = startingAddress;
            Quantity = quantity;
            DataSource = dataSource;
        }
        public EasyModbusItem(byte[] easyModbusFormatTxBuffer)
        {
            if (easyModbusFormatTxBuffer == null) throw new ArgumentNullException("easyModbusFormatTxBuffer for ModbusItem");
            if (easyModbusFormatTxBuffer.Length < 12) throw new InvalidCastException();
            FunctionCode = Convert.ToInt32( easyModbusFormatTxBuffer[7]);
            StartingAddress = easyModbusFormatTxBuffer[8] * 256;// shift left 8bit.
            StartingAddress += easyModbusFormatTxBuffer[9];
            Quantity = easyModbusFormatTxBuffer[10] * 256;// shift left 8bit.
            Quantity = +easyModbusFormatTxBuffer[11];
            TxBuffer = easyModbusFormatTxBuffer;
        }
        public EasyModbusItem(EasyModbusItem rhs): this( rhs.FunctionCode, rhs.StartingAddress, rhs.Quantity, rhs.DataSource)
        {
            this.RxBuffer = rhs.RxBuffer;
            this.TxBuffer = rhs.TxBuffer;
        }
        public int FunctionCode
        {
            get;
            private set;
        }

        public int Quantity
        {
            get;
            private set;
        }

        public bool ShouldRetry
        {
            get
            {
                return --_retry == 0 ? false : true; 
            }
        }
        public byte[] RxBuffer
        {
            get
            {
                return _abyRxBuffer.ToArray();
            }
            set
            {
                if (value != null)
                {
                    _abyRxBuffer.Clear();
                    foreach (byte b in value) _abyRxBuffer.Add(b);
                }
            }
        }
        public byte[] TxBuffer
        {
            get
            {
                return _abyTxBuffer.ToArray();
            }
            set
            {
                if (value != null)
                {
                    _abyTxBuffer.Clear();
                    foreach (byte b in value) _abyTxBuffer.Add(b);
                }
            }
        }

        public int[] DataSource
        {
            get
            {
                return _aiDataSource.ToArray();
            }
            set
            {
                if (value != null)
                {
                    _aiDataSource.Clear();
                    foreach (int i in value) _aiDataSource.Add(i);
                }
            }
        }

        public int StartingAddress
        {
            get;
            private set;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return this.CompareTo(obj) == 0;
        }
        public override int GetHashCode()
        {
            // first 2 byte of SendData is transaction ID.
            return base.GetHashCode() + FunctionCode + StartingAddress + Quantity + (TxBuffer[0] << 8 + TxBuffer[1]);
        }
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(base.ToString());
            str.AppendFormat(" FC: {0} Address:{1} Quantity: {2}\n", FunctionCode, StartingAddress, Quantity);
            return str.ToString() ;
        }
        const int MAX_RETRY = 3;
        const int MAX_BUFFER_SIZE = 128;
        private int _retry = MAX_RETRY;
        IList<byte> _abyRxBuffer = new List<byte>();
        IList<byte> _abyTxBuffer = new List<byte>();
        IList<int> _aiDataSource = new List<int>();
        #region IComparable 
        public int CompareTo(object obj)
        {
            var LeftItem = obj as IEasyModbusItem;
            if (LeftItem != null)
            {
                if (this.FunctionCode == LeftItem.FunctionCode)
                {
                    if (this.StartingAddress == LeftItem.StartingAddress)
                    {
                        if (this.Quantity == LeftItem.Quantity)
                        {
                            return this.TxBuffer.SequenceEqual(LeftItem.TxBuffer) ? 0: 1 ;
                        }
                        else
                            return this.Quantity > LeftItem.Quantity ? 1 : -1;
                    }
                    else
                        return this.StartingAddress > LeftItem.StartingAddress ? 1 : -1;
                }
                else
                    return this.FunctionCode > LeftItem.FunctionCode ? 1 : -1;
            }
            return 1;
        }
        #endregion
    }
}
