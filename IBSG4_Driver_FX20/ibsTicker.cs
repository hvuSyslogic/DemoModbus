// Decompiled with JetBrains decompiler
// Type: PhoenixContact.DDI.ibsTicker
// Assembly: IBSG4_Driver_FX20, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3ba9beb416a0ed83
// MVID: 066AFE0C-D702-4CB2-814E-202CA622D4F8
// Assembly location: D:\DotNet Framework 2.0\HFI\Libraries\IBSG4_Driver_FX20.dll

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PhoenixContact.DDI
{
  public class ibsTicker : IDisposable
  {
    private static int _tickerCnt = 0;
    private static int _instanceCounter = 0;
    private static ManualResetEvent _locEvent = new ManualResetEvent(false);
    private int _tickerTime = 10;
    private const int MainTick = 5;
    private const int GarbageCollectionTrigger = 100;
    private int _locInstNumber;
    private Thread _thrHdlCallBack;
    private bool _tickerEnable;
    private IBSTickerMode _tickerState;
    private IBSTickerHandler _chOnTimerTick;
    private static int timerID;
    private static ibsTicker.TimeProc timeProcPeriodic;

    [DllImport("kernel32.dll")]
    private static extern int GetIdleTime();

    [DllImport("kernel32.dll")]
    private static extern int GetTickCount();

    [DllImport("winmm.dll")]
    private static extern int timeGetDevCaps(ref TimerCaps caps, int sizeOfTimerCaps);

    [DllImport("winmm.dll")]
    private static extern int timeSetEvent(int delay, int resolution, ibsTicker.TimeProc timeProc, int user, int mode);

    [DllImport("winmm.dll")]
    private static extern int timeKillEvent(int id);

    public ibsTicker(int para_tickerTime, ThreadPriority threadPrio)
    {
      if (ibsTicker._instanceCounter == 0)
      {
        ++ibsTicker._instanceCounter;
        this._locInstNumber = ibsTicker._instanceCounter;
        ibsTicker.timeProcPeriodic = new ibsTicker.TimeProc(ibsTicker.OnTimerPeriodicEvent);
        ibsTicker.timerID = ibsTicker.timeSetEvent(5, 0, ibsTicker.timeProcPeriodic, 0, 1);
      }
      else
      {
        ++ibsTicker._instanceCounter;
        this._locInstNumber = ibsTicker._instanceCounter;
      }
      this._tickerState = IBSTickerMode.Idle;
      this._tickerTime = para_tickerTime;
      if (this._tickerTime < 5)
        this._tickerTime = 5;
      this._thrHdlCallBack = new Thread(new ThreadStart(this.workerThreadCallback));
      this._thrHdlCallBack.Start();
      this._thrHdlCallBack.Priority = threadPrio;
    }

    public int IBS_TickTime
    {
      get
      {
        return this._tickerTime;
      }
      set
      {
        this._tickerTime = value;
      }
    }

    public bool Enable
    {
      get
      {
        return this._tickerEnable;
      }
      set
      {
        if (this._tickerEnable && !value)
          this._tickerState = IBSTickerMode.Stop;
        if (!this._tickerEnable && value)
          this._tickerState = IBSTickerMode.Start;
        this._tickerEnable = value;
      }
    }

    public event IBSTickerHandler OnTimerTick
    {
      add
      {
        this._chOnTimerTick += value;
      }
      remove
      {
        this._chOnTimerTick -= value;
      }
    }

    public void Dispose()
    {
      this.Enable = false;
      Thread.Sleep(this._tickerTime * 2);
      if (this._thrHdlCallBack != null)
      {
        this._thrHdlCallBack.Abort();
        this._thrHdlCallBack.Join();
      }
      --ibsTicker._instanceCounter;
      if (ibsTicker._instanceCounter > 0)
        return;
      ibsTicker.timeKillEvent(ibsTicker.timerID);
      Thread.Sleep(20);
      ibsTicker._locEvent.Reset();
    }

    private static void OnTimerPeriodicEvent(int ID, int Msg, int User, int Param1, int Param2)
    {
      if (ibsTicker._tickerCnt < int.MaxValue)
        ibsTicker._tickerCnt += 5;
      else
        ibsTicker._tickerCnt = 0;
      ibsTicker._locEvent.Set();
      if (ibsTicker._tickerCnt % 100 == 0)
        GC.Collect();
      Thread.Sleep(0);
    }

    private void workerThreadCallback()
    {
      int num = 0;
      Thread.Sleep(10);
      try
      {
        while (true)
        {
          do
          {
            ibsTicker._locEvent.WaitOne();
            Thread.Sleep(0);
            ibsTicker._locEvent.Reset();
            if (this._tickerTime == 5)
              this.OnTick();
            else if (ibsTicker._tickerCnt < num)
              num -= int.MaxValue;
          }
          while (ibsTicker._tickerCnt - num < this._tickerTime);
          num = ibsTicker._tickerCnt;
          this.OnTick();
        }
      }
      catch (ThreadAbortException ex)
      {
        Thread.ResetAbort();
      }
    }

    private void OnTick()
    {
      if (this._tickerState == IBSTickerMode.Start)
      {
        if (this._chOnTimerTick != null)
          this._chOnTimerTick(this._tickerState);
        this._tickerState = IBSTickerMode.Active;
      }
      if (this._tickerState == IBSTickerMode.Active && this._chOnTimerTick != null)
        this._chOnTimerTick(this._tickerState);
      if (this._tickerState != IBSTickerMode.Stop)
        return;
      if (this._chOnTimerTick != null)
        this._chOnTimerTick(this._tickerState);
      this._tickerState = IBSTickerMode.Idle;
    }

    private delegate void TimeProc(int ID, int Msg, int User, int Param1, int Param2);
  }
}
