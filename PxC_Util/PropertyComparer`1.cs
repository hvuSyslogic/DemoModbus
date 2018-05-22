// Decompiled with JetBrains decompiler
// Type: PhoenixContact.PxC_Library.Util.PropertyComparer`1
// Assembly: PxC_Util, Version=1.4.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 18D5BDF8-0D3D-4138-A479-03DED5E34959
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\PxC_Util.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PhoenixContact.PxC_Library.Util
{
  public class PropertyComparer<T> : IComparer<T>
  {
    private readonly ListSortDirection _direction;
    private readonly PropertyDescriptor _property;

    public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
    {
      this._property = property;
      this._direction = direction;
    }

    public int Compare(T xWord, T yWord)
    {
      object propertyValue1 = this.GetPropertyValue(xWord, this._property.Name);
      object propertyValue2 = this.GetPropertyValue(yWord, this._property.Name);
      if (this._direction == ListSortDirection.Ascending)
        return this.CompareAscending(propertyValue1, propertyValue2);
      return this.CompareDescending(propertyValue1, propertyValue2);
    }

    public bool Equals(T xWord, T yWord)
    {
      return xWord.Equals((object) yWord);
    }

    public int GetHashCode(T obj)
    {
      return obj.GetHashCode();
    }

    private int CompareAscending(object xValue, object yValue)
    {
      if (xValue == null && yValue == null)
        return 0;
      if (xValue == null)
        return -1;
      if (yValue == null)
        return 1;
      return !(xValue is IComparable) ? (!xValue.Equals(yValue) ? xValue.ToString().CompareTo(yValue.ToString()) : 0) : ((IComparable) xValue).CompareTo(yValue);
    }

    private int CompareDescending(object xValue, object yValue)
    {
      return this.CompareAscending(xValue, yValue) * -1;
    }

    private object GetPropertyValue(T value, string property)
    {
      return value.GetType().GetProperty(property).GetValue((object) value, (object[]) null);
    }
  }
}
