// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.Progress
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;

namespace PhoenixContact.PxC_Library.Util
{
  public class Progress : IProgress
  {
    private readonly object _extAccess = new object();
    private static int _count;
    private Progress _innerProg;
    private Progress.ChangeHandler _hdChange;

    public Progress()
    {
      ++Progress._count;
      this.Name = string.Format("Progress_{0}", (object) Progress._count);
      this.CountMax = 100L;
      this.Parameter = string.Empty;
      this.State = Progress.States.Idle;
      this.CallChangeEvent();
    }

    public Progress(string name, long countMax)
    {
      this.Name = name;
      this.CountMax = countMax;
      this.Parameter = string.Empty;
      this.State = Progress.States.Idle;
      this.CallChangeEvent();
    }

    public Progress(string name, long countMax, Progress innerProgress)
    {
      this.Name = name;
      this.CountMax = countMax;
      this.State = Progress.States.Idle;
      this.Parameter = string.Empty;
      this.InnerProgress = innerProgress;
      this.CallChangeEvent();
    }

    public string Name { get; set; }

    public Progress InnerProgress
    {
      get
      {
        return this._innerProg;
      }
      set
      {
        this._innerProg = value;
        if (this._innerProg == null)
          return;
        this._innerProg.OnChange += new Progress.ChangeHandler(this._innerProg_OnChange);
      }
    }

    public long CountActual { get; private set; }

    public long CountMax { get; private set; }

    public int Percent { get; private set; }

    public Progress.States State { get; private set; }

    public string Parameter { get; private set; }

    public bool SetCoutMax(long countMax)
    {
      lock (this._extAccess)
      {
        if (this.State == Progress.States.Idle)
        {
          this.CountMax = countMax;
          return true;
        }
      }
      return false;
    }

    public void SetProgress(long countActual)
    {
      this.IntSetProgress(countActual, string.Empty);
    }

    public void SetProgress(long countActual, string parameter)
    {
      this.IntSetProgress(countActual, parameter);
    }

    public void SetProgress(long countActual, long countMax, string parameter)
    {
      if (countMax <= 0L)
        return;
      this.CountMax = countMax;
      this.IntSetProgress(countActual, parameter);
    }

    public void SetParameter(string parameter)
    {
      lock (this._extAccess)
      {
        if (this.CountMax <= 0L || this.State != Progress.States.Idle || string.IsNullOrEmpty(parameter))
          return;
        this.Parameter = parameter;
      }
    }

    public bool AbortProgress(string parameter)
    {
      lock (this._extAccess)
      {
        if (!this.AbortInnerProgress(this.InnerProgress, parameter) || this.State != Progress.States.Active && this.State != Progress.States.Suspend)
          return false;
        this.State = Progress.States.Abort;
        this.Parameter = parameter;
        this.CallChangeEvent();
        return true;
      }
    }

    public bool Reset()
    {
      lock (this._extAccess)
      {
        if (!Progress.ResetInnerProgress(this.InnerProgress) || this.State == Progress.States.Active || this.State == Progress.States.Suspend)
          return false;
        this.State = Progress.States.Idle;
        this.CountActual = 0L;
        this.Percent = 0;
        this.Parameter = string.Empty;
        this.CallChangeEvent();
        return true;
      }
    }

    private void IntSetProgress(long countActual, string parameter)
    {
      lock (this._extAccess)
      {
        if (this.CountMax <= 0L)
          return;
        if (this.State == Progress.States.Idle && countActual > 0L)
          this.State = Progress.States.Active;
        if (this.State != Progress.States.Active)
          return;
        if (!string.IsNullOrEmpty(parameter))
          this.Parameter = parameter;
        this.CountActual = countActual <= this.CountMax ? countActual : this.CountMax;
        this.Percent = Convert.ToInt32(100L * this.CountActual / this.CountMax);
        if (this.CountActual >= this.CountMax)
        {
          if (this.InnerProgress == null)
            this.State = Progress.States.Finished;
        }
      }
      this.CallChangeEvent();
    }

    public event Progress.ChangeHandler OnChange
    {
      add
      {
        this._hdChange += value;
      }
      remove
      {
        if (this._hdChange == null)
          return;
        this._hdChange -= value;
      }
    }

    public override string ToString()
    {
      return string.Format("{0}, {1}, {2} from {3}, {4}%, {5}", (object) this.Name, (object) this.State, (object) this.CountActual, (object) this.CountMax, (object) this.Percent, (object) this.Parameter);
    }

    private void _innerProg_OnChange(object sender)
    {
      if (this.InnerProgress != null && this.InnerProgress.State == Progress.States.Finished && (this.State == Progress.States.Active && this.CountActual >= this.CountMax))
        this.State = Progress.States.Finished;
      this.CallChangeEvent();
    }

    private bool AbortInnerProgress(Progress pData, string pParameter)
    {
      if (pData != null)
      {
        if (pData.InnerProgress != null && (!this.AbortInnerProgress(pData.InnerProgress, pParameter) || !pData.InnerProgress.AbortProgress(pParameter)) || this.State != Progress.States.Active && this.State != Progress.States.Suspend)
          return false;
        pData.State = Progress.States.Abort;
        pData.Parameter = pParameter;
      }
      return true;
    }

    private static bool ResetInnerProgress(Progress pInnerData)
    {
      if (pInnerData == null)
        return true;
      if (pInnerData.InnerProgress != null && !Progress.ResetInnerProgress(pInnerData.InnerProgress))
        return false;
      return pInnerData.Reset();
    }

    private void CallChangeEvent()
    {
      if (this._hdChange == null)
        return;
      this._hdChange((object) this);
    }

    public delegate void ChangeHandler(object sender);

    public enum States
    {
      Idle,
      Active,
      Suspend,
      Finished,
      Abort,
    }
  }
}
