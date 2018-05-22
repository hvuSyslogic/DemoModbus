// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.SortableBindingList`1
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace PhoenixContact.PxC_Library.Util
{
  public class SortableBindingList<T> : BindingList<T>
  {
    private bool _Sorted;
    private ListSortDirection _sortDirection;
    private PropertyDescriptor _sortProperty;

    public SortableBindingList()
    {
    }

    public SortableBindingList(IList<T> list)
      : base(list)
    {
    }

    protected override bool SupportsSortingCore
    {
      get
      {
        return true;
      }
    }

    protected override bool IsSortedCore
    {
      get
      {
        return this._Sorted;
      }
    }

    protected override ListSortDirection SortDirectionCore
    {
      get
      {
        return this._sortDirection;
      }
    }

    protected override PropertyDescriptor SortPropertyCore
    {
      get
      {
        return this._sortProperty;
      }
    }

    protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
    {
      List<T> items = this.Items as List<T>;
      if (items != null)
      {
        PropertyComparer<T> propertyComparer = new PropertyComparer<T>(prop, direction);
        items.Sort((IComparer<T>) propertyComparer);
        this._Sorted = true;
        this._sortProperty = prop;
        this._sortDirection = direction;
        this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
      }
      else
        this._Sorted = false;
    }

    protected override void RemoveSortCore()
    {
      this._Sorted = false;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    protected override bool SupportsSearchingCore
    {
      get
      {
        return true;
      }
    }

    public int Find(string property, object key)
    {
      PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof (T)).Find(property, true);
      if (prop == null)
        return -1;
      return this.FindCore(prop, key);
    }

    protected override int FindCore(PropertyDescriptor prop, object key)
    {
      PropertyInfo property = typeof (T).GetProperty(prop.Name);
      if (key != null)
      {
        for (int index = 0; index < this.Count; ++index)
        {
          T obj = this.Items[index];
          if (property.GetValue((object) obj, (object[]) null).Equals(key))
            return index;
        }
      }
      return -1;
    }
  }
}
