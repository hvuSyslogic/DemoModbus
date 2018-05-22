// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.ExtensionMethod.BitExtensions
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Collections;

namespace PhoenixContact.Common.ExtensionMethod
{
  public static class BitExtensions
  {
    private static readonly object lockBitArrayToUInt = new object();
    private static readonly object lockBitArrayToInt = new object();

    public static BitArray ToBitArray(this ushort value)
    {
      return new BitArray(BitConverter.GetBytes(value));
    }

    public static BitArray ToBitArray(this uint value)
    {
      return new BitArray(BitConverter.GetBytes(value));
    }

    public static BitArray ToBitArray(this ulong value)
    {
      return new BitArray(BitConverter.GetBytes(value));
    }

    public static BitArray ToBitArray(this ulong value, int byteLength)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      Array.Resize<byte>(ref bytes, byteLength);
      return new BitArray(bytes);
    }

    public static BitArray ToBitArray(this int value)
    {
      return new BitArray(BitConverter.GetBytes(value));
    }

    public static BitArray ToBitArray(this long value)
    {
      return new BitArray(BitConverter.GetBytes(value));
    }

    public static ushort ToUInt16(this BitArray array)
    {
      if (array.Length > 16)
        throw new ArgumentException("The array is too long.");
      lock (BitExtensions.lockBitArrayToUInt)
      {
        byte[] numArray = new byte[2];
        array.CopyTo((Array) numArray, 0);
        return BitConverter.ToUInt16(numArray, 0);
      }
    }

    public static uint ToUInt32(this BitArray array)
    {
      if (array.Length > 32)
        throw new ArgumentException("The array is too long.");
      lock (BitExtensions.lockBitArrayToUInt)
      {
        byte[] numArray = new byte[4];
        array.CopyTo((Array) numArray, 0);
        return BitConverter.ToUInt32(numArray, 0);
      }
    }

    public static ulong ToUInt64(this BitArray array)
    {
      if (array.Length > 64)
        throw new ArgumentException("The array is too long.");
      lock (BitExtensions.lockBitArrayToUInt)
      {
        byte[] numArray = new byte[8];
        array.CopyTo((Array) numArray, 0);
        return BitConverter.ToUInt64(numArray, 0);
      }
    }

    public static int ToInt32(this BitArray array)
    {
      if (array.Length > 32)
        throw new ArgumentException("The array is too long.");
      lock (BitExtensions.lockBitArrayToInt)
      {
        int[] numArray = new int[1];
        array.CopyTo((Array) numArray, 0);
        return numArray[0];
      }
    }

    public static long ToInt64(this BitArray array)
    {
      if (array.Length > 64)
        throw new ArgumentException("The array is too long.");
      lock (BitExtensions.lockBitArrayToInt)
      {
        byte[] numArray = new byte[8];
        array.CopyTo((Array) numArray, 0);
        return BitConverter.ToInt64(numArray, 0);
      }
    }
  }
}
