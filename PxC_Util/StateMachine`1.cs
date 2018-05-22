// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.StateMachine`1
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;

namespace PhoenixContact.PxC_Library.Util
{
  public class StateMachine<EnumType>
  {
    private readonly string name;
    private readonly object syncObj;
    private EnumType aktState;
    private StateMachine<EnumType>.StateChange hdStateChange;
    private EnumType oldState;

    public StateMachine(string Name)
    {
      this.name = Name == null || Name.Length == 0 ? "StateMashine" : Name;
      this.syncObj = new object();
    }

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    public bool Active
    {
      get
      {
        return Convert.ToBoolean((object) this.aktState);
      }
    }

    public EnumType GetState()
    {
      return this.aktState;
    }

    public EnumType GetLastState()
    {
      return this.oldState;
    }

    public void ChangeState(EnumType NewState)
    {
      lock (this.syncObj)
      {
        if (Convert.ToInt32((object) this.aktState) == Convert.ToInt32((object) NewState))
          return;
        this.oldState = this.aktState;
        this.aktState = NewState;
        if (this.hdStateChange == null)
          return;
        this.hdStateChange(this.aktState);
      }
    }

    public void CallChangeState()
    {
      lock (this.syncObj)
      {
        if (this.hdStateChange == null)
          return;
        this.hdStateChange(this.aktState);
      }
    }

    public event StateMachine<EnumType>.StateChange OnStateChange
    {
      add
      {
        this.hdStateChange += value;
      }
      remove
      {
        this.hdStateChange -= value;
      }
    }

    public override string ToString()
    {
      return this.name + ": " + this.aktState.ToString();
    }

    public delegate void StateChange(EnumType State);
  }
}
