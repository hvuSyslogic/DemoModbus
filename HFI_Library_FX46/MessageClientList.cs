// Decompiled with JetBrains decompiler
// Type: PhoenixContact.HFI.Inline.MessageClientList
// Assembly: HFI_Library_FX46, Version=3.2.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 42FFD0DD-74E7-4B38-A116-483C52C5F352
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\HFI_Library_FX46.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PhoenixContact.HFI.Inline
{
  internal class MessageClientList
  {
    private readonly List<MessageClient> messageClientList;
    private readonly object accessLock;

    internal MessageClientList()
    {
      this.messageClientList = new List<MessageClient>();
      this.accessLock = new object();
    }

    internal int Count
    {
      get
      {
        return this.messageClientList.Count;
      }
    }

    internal void AddService(MessageClient service)
    {
      lock (this.accessLock)
      {
        if (service == null)
          return;
        this.messageClientList.Add(service);
      }
    }

    internal void Remove(MessageClient service)
    {
      lock (this.accessLock)
      {
        if (service == null)
          return;
        this.messageClientList.Remove(service);
      }
    }

    internal void DeleteAll()
    {
      lock (this.accessLock)
        this.messageClientList.Clear();
    }

    internal ICollection<MessageClient> GetCollection()
    {
      lock (this.accessLock)
        return (ICollection<MessageClient>) this.messageClientList;
    }

    internal ReadOnlyCollection<MessageClient> GetReadOnlyCollection()
    {
      lock (this.accessLock)
        return this.messageClientList.AsReadOnly();
    }

    internal void UpdateReceiveConfirmations(int receiveResult, int receiveUserId, int receiveLength, byte[] receiveData)
    {
      lock (this.accessLock)
      {
        if (this.GetWaitingConfirmations().Count <= 0)
          return;
        MessageClient waitConfirmation = this.GetClientWithWaitConfirmation(receiveUserId);
        if (waitConfirmation != null)
        {
          waitConfirmation.SetReceiveData(receiveData, receiveLength);
          waitConfirmation.CallConfirmationReceivedEvent((object) this);
        }
        foreach (MessageClient waitingConfirmation in (IEnumerable<MessageClient>) this.GetWaitingConfirmations())
          waitingConfirmation.CheckTimeout();
      }
    }

    internal IEnumerable<MessageClient> GetClientsWithSendData()
    {
      lock (this.accessLock)
        return (IEnumerable<MessageClient>) this.messageClientList.FindAll((Predicate<MessageClient>) (i => i.IsDataToSend));
    }

    private ICollection<MessageClient> GetWaitingConfirmations()
    {
      lock (this.accessLock)
        return (ICollection<MessageClient>) this.messageClientList.FindAll((Predicate<MessageClient>) (i => i.State == MessageClientState.WaitingForConfirmation));
    }

    private MessageClient GetClientWithWaitConfirmation(int userId)
    {
      lock (this.accessLock)
        return this.messageClientList.Find((Predicate<MessageClient>) (i =>
        {
          if (i.State == MessageClientState.WaitingForConfirmation)
            return i.ControllerId == userId;
          return false;
        }));
    }

    internal void ClearSendReceiveData()
    {
      lock (this.accessLock)
      {
        foreach (MessageClient messageClient in this.messageClientList)
        {
          messageClient.ClearSendData();
          messageClient.ClearReceiveData();
        }
      }
    }
  }
}
