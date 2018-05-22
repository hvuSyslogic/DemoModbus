// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.Ticker.TickerFactory
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace PhoenixContact.Common.Ticker
{
  public static class TickerFactory
  {
    private static uint timerId = 0;
    private static int mainTick = 1;
    private static int garbageCollectionTick = 500;
    private static readonly object extAccess = new object();
    private static readonly List<PhoenixContact.Common.Ticker.Ticker> TickerList = new List<PhoenixContact.Common.Ticker.Ticker>();
    private static TickerFactory.TimeProc timeProcPeriodic;
    private static int tickCount;

    [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint timeSetEvent(uint delay, uint resolution, TickerFactory.TimeProc timeProc, uint user, uint mode);

    [DllImport("winmm.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern uint timeKillEvent(uint id);

    public static int MainTick
    {
      get
      {
        return TickerFactory.mainTick;
      }
      set
      {
        if (TickerFactory.TickerList.Count != 0 || value <= 0)
          return;
        TickerFactory.mainTick = value;
      }
    }

    public static int GarbageCollectionTick
    {
      get
      {
        return TickerFactory.garbageCollectionTick;
      }
      set
      {
        if (TickerFactory.TickerList.Count != 0 || value < 0)
          return;
        TickerFactory.garbageCollectionTick = value;
      }
    }

    public static int Count
    {
      get
      {
        return TickerFactory.TickerList.Count;
      }
    }

    public static PhoenixContact.Common.Ticker.Ticker Create(string name, int intervall, ThreadPriority priority)
    {
      lock (TickerFactory.extAccess)
      {
        PhoenixContact.Common.Ticker.Ticker ticker = new PhoenixContact.Common.Ticker.Ticker(name, intervall, priority);
        TickerFactory.TickerList.Add(ticker);
        if (TickerFactory.timerId == 0U && TickerFactory.TickerList.Count > 0)
        {
          TickerFactory.timeProcPeriodic = new TickerFactory.TimeProc(TickerFactory.OnTimerPeriodicEvent);
          TickerFactory.timerId = TickerFactory.timeSetEvent((uint) TickerFactory.MainTick, 0U, TickerFactory.timeProcPeriodic, 0U, 1U);
        }
        return ticker;
      }
    }

    private static void OnTimerPeriodicEvent(uint id, uint msg, uint user, uint param1, uint param2)
    {
      lock (TickerFactory.extAccess)
      {
        List<PhoenixContact.Common.Ticker.Ticker> remove = (List<PhoenixContact.Common.Ticker.Ticker>) null;
        foreach (PhoenixContact.Common.Ticker.Ticker ticker in TickerFactory.TickerList)
        {
          if (ticker.Remove)
          {
            if (remove == null)
              remove = new List<PhoenixContact.Common.Ticker.Ticker>();
            remove.Add(ticker);
          }
          else
            ticker.Update(TickerFactory.tickCount);
        }
        if (TickerFactory.tickCount < int.MaxValue)
          TickerFactory.tickCount += TickerFactory.MainTick;
        else
          TickerFactory.tickCount = 0;
        if (remove != null)
          TickerFactory.RemoveTicker(remove);
        if (TickerFactory.garbageCollectionTick <= 0 || TickerFactory.TickerList.Count <= 0 || TickerFactory.tickCount % TickerFactory.garbageCollectionTick != 0)
          return;
        GC.Collect();
      }
    }

    private static void RemoveTicker(List<PhoenixContact.Common.Ticker.Ticker> remove)
    {
      foreach (PhoenixContact.Common.Ticker.Ticker ticker in remove)
        TickerFactory.TickerList.Remove(ticker);
      if (TickerFactory.TickerList.Count > 0)
        return;
      TickerFactory.KillTimerId();
      Thread.Sleep(Convert.ToInt32(TickerFactory.MainTick * 4));
    }

    public static void DeleteAllCreatedTicker()
    {
      lock (TickerFactory.extAccess)
      {
        TickerFactory.KillTimerId();
        if (TickerFactory.TickerList == null)
          return;
        TickerFactory.TickerList.Clear();
      }
    }

    private static void KillTimerId()
    {
      if (TickerFactory.timerId == 0U)
        return;
      int num = (int) TickerFactory.timeKillEvent(TickerFactory.timerId);
      TickerFactory.timerId = 0U;
    }

    private enum TimerMode
    {
      OneShot = 0,
      Periodic = 1,
      KillTimer = 256, // 0x00000100
    }

    private delegate void TimeProc(uint id, uint msg, uint user, uint param1, uint param2);
  }
}
