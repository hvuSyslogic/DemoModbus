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
//    Description: BlockingQueue, instead of throwing exception when trying to dequeued 
//    and empty queue, it hold the thread, wait until item is enqueued.
//
//************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

/// <summary>
/// https://blogs.msdn.microsoft.com/toub/2006/04/12/blocking-queues/
/// </summary>
namespace Qti.Autotron.ModbusAutotronAPI
{
    class BlockingQueue<T> : IEnumerable<T>
    {

        private int _iCount = 0;

        private Queue<T> _Queue = new Queue<T>();
        public int Count()
        {
            lock (_Queue)
            {
                return _Queue.Count;
            }
        }
        public T Dequeue()
        {
            lock (_Queue)
            {
                while (_iCount <= 0) Monitor.Wait(_Queue);
                _iCount--;
                Trace.WriteLine(string.Format("Dequeue {0}", _Queue.Peek()));
                return _Queue.Dequeue();
            }

        }
        public void Enqueue(T data)
        {
            if (data == null) throw new ArgumentNullException("data");
            lock (_Queue)
            {
                _Queue.Enqueue(data);
                _iCount++;
                Trace.WriteLine(string.Format("Enqueue {0}", _Queue.Peek()));
                Monitor.Pulse(_Queue);

            }

        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            while (true) yield return Dequeue();
        }
        //for using with foreach method...
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

    }

}
