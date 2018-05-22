// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.Progress_o
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;

namespace PhoenixContact.PxC_Library.Util
{
  public class Progress_o : IProgress_o
  {
    private static readonly object m_lock = new object();
    private static volatile Progress_o instance = (Progress_o) null;
    private string pbMainText = string.Empty;
    private string pbText = string.Empty;
    private UpdateProgressBar _hdUpdateProgressStatus;
    private int pbProcessPercentage;
    private int pbTotalPercentage;
    private bool showProcessProgressBar;

    private Progress_o()
    {
    }

    public static Progress_o GetInstance()
    {
      if (Progress_o.instance == null)
      {
        lock (Progress_o.m_lock)
        {
          if (Progress_o.instance == null)
            Progress_o.instance = new Progress_o();
        }
      }
      return Progress_o.instance;
    }

    public void InitProcessWindow(string StateText, bool ShowProcessProgressBar)
    {
      this.pbMainText = StateText;
      this.showProcessProgressBar = ShowProcessProgressBar;
    }

    public bool SetTotalValue(int MaxValue, int ActualValue)
    {
      return this.SetTotalValue((long) MaxValue, (long) ActualValue);
    }

    public bool SetTotalValue(long MaxValue, long ActualValue)
    {
      if (this._hdUpdateProgressStatus == null)
        return true;
      this.pbText = string.Format(this.pbMainText, (object) ActualValue.ToString(), (object) MaxValue.ToString());
      if (ActualValue > 0L)
      {
        this.pbTotalPercentage = Convert.ToInt32((double) ActualValue * 100.0 / (double) MaxValue);
        if (this.pbTotalPercentage > 100)
          this.pbTotalPercentage = 100;
        this.pbProcessPercentage = !this.showProcessProgressBar ? -1 : 0;
      }
      return !this._hdUpdateProgressStatus(this.pbText, this.pbTotalPercentage, this.pbProcessPercentage);
    }

    public bool SetProcessValue(int MaxValue, int ActualValue)
    {
      return this.SetProcessValue((long) MaxValue, (long) ActualValue);
    }

    public bool SetProcessValue(long MaxValue, long ActualValue)
    {
      if (this._hdUpdateProgressStatus == null || !this.showProcessProgressBar)
        return true;
      if (ActualValue > 0L)
      {
        this.pbProcessPercentage = Convert.ToInt32((double) ActualValue * 100.0 / (double) MaxValue);
        if (this.pbProcessPercentage > 100)
          this.pbProcessPercentage = 100;
      }
      return this._hdUpdateProgressStatus(this.pbText, this.pbTotalPercentage, this.pbProcessPercentage);
    }

    public event UpdateProgressBar OnUpdateProgressStatus
    {
      add
      {
        this._hdUpdateProgressStatus = value;
      }
      remove
      {
        this._hdUpdateProgressStatus = value;
      }
    }
  }
}
