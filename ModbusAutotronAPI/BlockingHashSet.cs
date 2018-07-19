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
//    Creation Date: 07/13/2018
//    Description: BlockingHashSet for EasyModbusItem, instead of trying to 
//    dequeued an empty HashSet, it hold the thread, wait until item is enqueued.
//
//************************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Qti.Autotron.ModbusAutotronAPI
{
    public class BlockingHashSet
    {
        private int _iCount = 0;
        private HashSet<EasyModbusItem> _HashSet;

        public BlockingHashSet()
        {
            _iCount = 0;
            _HashSet = new HashSet<EasyModbusItem>();
        }
        public int Count()
        {
            lock (_HashSet)
            {
                return _HashSet.Count;
            }
        }
        public EasyModbusItem RemoveFirstItem()
        {
            lock (_HashSet)
            {
                while (_iCount <= 0) Monitor.Wait(_HashSet);

                _iCount--;
                var currentItem = _HashSet.First();
                var item = new EasyModbusItem(currentItem.TxBuffer);
                item.RxBuffer = currentItem.RxBuffer;
                _HashSet.Remove(currentItem);
                Trace.WriteLine(string.Format("_HashSet Remove First {0} {1}", currentItem.FunctionCode, currentItem.StartingAddress));
                return item;
            }
        }
        public void Add(EasyModbusItem data)
        {
            if (data == null) throw new ArgumentNullException("data");
            lock (_HashSet)
            {
                _HashSet.Add(data);
                _iCount++;
                Monitor.Pulse(_HashSet);
                Trace.WriteLine(string.Format("_HashSet Rx Add {0}", data));
            }
        }
        public void UpdateWithRxData(EasyModbusItem currentEasyModbusItem, byte[] rxData)
        {
            if (currentEasyModbusItem == null) throw new ArgumentNullException("CurrentEasyModbusItem");
            if (rxData == null) throw new ArgumentNullException("RxData");
            lock (_HashSet)
            {
                _HashSet.Remove(currentEasyModbusItem);
                Trace.WriteLine(string.Format("_HashSet Rx Remove {0} {1}", currentEasyModbusItem.FunctionCode, currentEasyModbusItem.StartingAddress));
                var itemToAdd = new EasyModbusItem(currentEasyModbusItem.TxBuffer);
                itemToAdd.RxBuffer = rxData;
                _HashSet.Add(itemToAdd);
                Trace.WriteLine(string.Format("_HashSet Rx Add {0} {1} -{2}{3}", currentEasyModbusItem.FunctionCode, currentEasyModbusItem.StartingAddress, rxData[0], rxData[1]));
                Monitor.Pulse(_HashSet);
            }
        }
        public void UpdateWithReturnedValues(EasyModbusItem currentEasyModbusItem, int[] ReturnedData)
        {
            if (currentEasyModbusItem == null) throw new ArgumentNullException("CurrentEasyModbusItem");
            if (ReturnedData == null) throw new ArgumentNullException("ReturnedData");
            lock (_HashSet)
            {
                _HashSet.Remove(currentEasyModbusItem);
                Trace.WriteLine(string.Format("_HashSet Remove {0} {1}", currentEasyModbusItem.FunctionCode, currentEasyModbusItem.StartingAddress));
                var itemToAdd = new EasyModbusItem(currentEasyModbusItem.TxBuffer);
                itemToAdd.DataSource = ReturnedData;
                _HashSet.Add(itemToAdd);
                Trace.WriteLine(string.Format("_HashSet Add {0} {1} -{2}", currentEasyModbusItem.FunctionCode, currentEasyModbusItem.StartingAddress, ReturnedData[0]));
                Monitor.Pulse(_HashSet);
            }
        }
    }
}
