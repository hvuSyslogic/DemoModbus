// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.Ticker.Ticker
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Globalization;
using System.Threading;

namespace PhoenixContact.Common.Ticker
{
  public class Ticker : ITick, IDisposable
  {
    private readonly ManualResetEvent cyclicMrEvent = new ManualResetEvent(false);
    private readonly object syncObject = new object();
    private ThreadPriority priority = ThreadPriority.Normal;
    private static uint sequentialNumber;
    private bool disposed;
    private Exception exception;
    private TickerDiagnosticHandler hdOnDiagnostic;
    private TickerHandler hdOnDisable;
    private TickerHandler hdOnEnable;
    private TickerHandler hdOnTick;
    private int intervall;
    private bool runWorker;
    private Thread thrHdlCallBack;
    private PhoenixContact.Common.Ticker.Ticker.TickerMode tickerState;

    public override string ToString()
    {
      return this.Name;
    }

    internal Ticker(string name, int intervall, ThreadPriority threadPrio)
    {
      this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle;
      this.Name = name;
      this.Intervall = intervall;
      this.Priority = threadPrio;
      this.Error = false;
      this.Ready = false;
      ++PhoenixContact.Common.Ticker.Ticker.sequentialNumber;
      if (!string.IsNullOrEmpty(name))
        return;
      this.Name = string.Format("Ticker_{0}", (object) PhoenixContact.Common.Ticker.Ticker.sequentialNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    internal bool Remove { get; private set; }

    internal int ActualTickCount { get; private set; }

    public string Name { get; }

    public bool Ready { get; private set; }

    public bool Error { get; private set; }

    public int Intervall
    {
      get
      {
        return this.intervall;
      }
      set
      {
        if (this.tickerState != PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle)
          return;
        this.intervall = value < 1 ? 1 : value;
      }
    }

    public ThreadPriority Priority
    {
      get
      {
        return this.priority;
      }
      set
      {
        if (this.tickerState != PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle)
          return;
        this.priority = value;
      }
    }

    public bool Enable()
    {
      lock (this.syncObject)
      {
        if (this.Remove || this.tickerState != PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle)
          return false;
        this.Ready = false;
        this.Error = false;
        this.runWorker = true;
        if (this.thrHdlCallBack != null)
        {
          this.thrHdlCallBack.Abort();
          this.thrHdlCallBack = (Thread) null;
        }
        this.thrHdlCallBack = new Thread(new ThreadStart(this.WorkerThreadCallback))
        {
          Priority = this.Priority
        };
        this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Enable;
        this.thrHdlCallBack.Start();
        return true;
      }
    }

    public bool Disable()
    {
      lock (this.syncObject)
      {
        if (this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Active)
        {
          this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Disable;
          this.TerminateThread();
          this.Ready = false;
          return true;
        }
        if (this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Error)
        {
          this.TerminateThread();
          this.Ready = false;
          this.Error = false;
          this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle;
          return true;
        }
      }
      return false;
    }

    public bool DisableAsync()
    {
      lock (this.syncObject)
      {
        if (this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Enable || this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Active)
          this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Disable;
        new Thread(new ThreadStart(this.ExecuteDisable))
        {
          Priority = this.Priority
        }.Start();
        return true;
      }
    }

    public event TickerHandler OnEnable
    {
      add
      {
        this.hdOnEnable += value;
      }
      remove
      {
        if (this.hdOnEnable == null)
          return;
        this.hdOnEnable -= value;
      }
    }

    public event TickerHandler OnDisable
    {
      add
      {
        this.hdOnDisable += value;
      }
      remove
      {
        if (this.hdOnDisable == null)
          return;
        this.hdOnDisable -= value;
      }
    }

    public event TickerHandler OnTick
    {
      add
      {
        this.hdOnTick += value;
      }
      remove
      {
        if (this.hdOnTick == null)
          return;
        this.hdOnTick -= value;
      }
    }

    public event TickerDiagnosticHandler OnDiagnostic
    {
      add
      {
        this.hdOnDiagnostic += value;
      }
      remove
      {
        if (this.hdOnDiagnostic == null)
          return;
        this.hdOnDiagnostic -= value;
      }
    }

    private void ExecuteDisable()
    {
      lock (this.syncObject)
      {
        this.TerminateThread();
        this.Ready = false;
        this.Error = false;
        this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle;
      }
    }

    internal void Update(int millisecondCount)
    {
      if (this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle || this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Disable || this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Error)
        return;
      if (millisecondCount < this.ActualTickCount)
        this.ActualTickCount -= int.MaxValue;
      if (millisecondCount - this.ActualTickCount < this.Intervall)
        return;
      this.ActualTickCount = millisecondCount;
      this.cyclicMrEvent.Set();
    }

    private void WorkerThreadCallback()
    {
      do
      {
        try
        {
          if (this.cyclicMrEvent.WaitOne(this.Intervall * 10))
          {
            this.cyclicMrEvent.Reset();
            this.CallOnUpdateTick();
          }
          else
            Thread.Sleep(10);
        }
        catch (ThreadAbortException ex)
        {
          Thread.ResetAbort();
          this.runWorker = false;
        }
      }
      while (this.runWorker);
      if (this.CallEvent(this.hdOnDisable))
      {
        this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle;
      }
      else
      {
        this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Idle;
        this.SetError();
      }
    }

    private void CallOnUpdateTick()
    {
      if (this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Active)
      {
        if (this.CallEvent(this.hdOnTick))
          return;
        lock (this.syncObject)
        {
          this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Error;
          this.SetError();
        }
      }
      else
      {
        if (this.tickerState != PhoenixContact.Common.Ticker.Ticker.TickerMode.Enable)
          return;
        if (this.CallEvent(this.hdOnEnable))
        {
          lock (this.syncObject)
          {
            if (this.tickerState == PhoenixContact.Common.Ticker.Ticker.TickerMode.Disable)
              return;
            this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Active;
            this.Ready = true;
          }
        }
        else
        {
          lock (this.syncObject)
          {
            this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Error;
            this.SetError();
          }
        }
      }
    }

    internal bool CallEvent(TickerHandler tickerHandler)
    {
      if (tickerHandler == null)
        return true;
      try
      {
        tickerHandler((object) this);
        return true;
      }
      catch (Exception ex)
      {
        this.exception = ex;
        return false;
      }
    }

    private void SetError()
    {
      this.Ready = false;
      this.Error = true;
      if (this.hdOnDiagnostic == null)
        return;
      try
      {
        if (this.exception != null)
          this.hdOnDiagnostic(this.exception);
      }
      catch
      {
      }
      this.exception = (Exception) null;
    }

    private void TerminateThread()
    {
      try
      {
        if (this.thrHdlCallBack == null || !this.thrHdlCallBack.IsAlive || this.thrHdlCallBack.ThreadState == ThreadState.AbortRequested)
          return;
        this.thrHdlCallBack.Abort();
        Thread.Sleep(this.Intervall);
      }
      catch
      {
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        this.Remove = true;
        this.Ready = false;
        this.Error = false;
        this.disposed = true;
        this.tickerState = PhoenixContact.Common.Ticker.Ticker.TickerMode.Disposed;
        this.TerminateThread();
        this.cyclicMrEvent.Reset();
      }
      this.disposed = true;
    }

    private enum TickerMode
    {
      Idle = 33024, // 0x00008100
      Enable = 33025, // 0x00008101
      Active = 33026, // 0x00008102
      Disable = 33028, // 0x00008104
      Error = 49152, // 0x0000C000
      Disposed = 57344, // 0x0000E000
    }
  }
}
