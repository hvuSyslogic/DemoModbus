// Decompiled with JetBrains decompiler
// Type: PhoenixContact.Common.ExtensionMethod.DataConvertExtension
// Assembly: Common_FX46, Version=1.3.6053.23249, Culture=neutral, PublicKeyToken=bbf13850d99d956d
// MVID: 8B53F5CC-DB76-4BDD-B641-83311EDC885D
// Assembly location: D:\Program Files (x86)\Phoenix Contact\HFI 3.2\HFI_Tools\Libraries\Common_FX46.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoenixContact.Common.ExtensionMethod
{
  public static class DataConvertExtension
  {
    private static readonly object LockUIntToByteArray = new object();
    private static readonly object LockIntToByteArray = new object();
    private static readonly object LockByteArrayToUInt = new object();
    private static readonly object LockByteArrayToInt = new object();
    private static readonly object LockArrayCompare = new object();
    private static readonly object LockCopyToByteArray = new object();

    public static ushort Swap(this ushort uInt16Value)
    {
      return Convert.ToUInt16(((int) uInt16Value >> 8 | (int) uInt16Value << 8) & (int) ushort.MaxValue);
    }

    public static uint Swap(this uint uInt32Value)
    {
      byte[] bytes = BitConverter.GetBytes(uInt32Value);
      Array.Reverse((Array) bytes);
      return BitConverter.ToUInt32(bytes, 0);
    }

    public static ulong Swap(this ulong uInt64Value)
    {
      byte[] bytes = BitConverter.GetBytes(uInt64Value);
      Array.Reverse((Array) bytes);
      return BitConverter.ToUInt64(bytes, 0);
    }

    public static short Swap(this short int16Value)
    {
      return Convert.ToInt16((int) int16Value >> 8 & (int) byte.MaxValue | ((int) int16Value & (int) byte.MaxValue) << 8);
    }

    public static int Swap16Bytes(this int int32Value)
    {
      return Convert.ToInt32(int32Value >> 8 & (int) byte.MaxValue | (int32Value & (int) byte.MaxValue) << 8);
    }

    public static int Swap(this int int32Value)
    {
      byte[] bytes = BitConverter.GetBytes(int32Value);
      Array.Reverse((Array) bytes);
      return BitConverter.ToInt32(bytes, 0);
    }

    public static long Swap(this long int64Value)
    {
      byte[] bytes = BitConverter.GetBytes(int64Value);
      Array.Reverse((Array) bytes);
      return BitConverter.ToInt64(bytes, 0);
    }

    public static byte[] ToByteArray(this ushort[] uInt16Array)
    {
      return uInt16Array.ToByteArray(0, false);
    }

    public static byte[] ToByteArray(this ushort[] uInt16Array, bool swapValues)
    {
      return uInt16Array.ToByteArray(0, swapValues);
    }

    public static byte[] ToByteArray(this ushort[] uInt16Array, int startAddress)
    {
      return uInt16Array.ToByteArray(startAddress, false);
    }

    public static byte[] ToByteArray(this ushort[] uInt16Array, int startAddress, bool swapValues)
    {
      lock (DataConvertExtension.LockUIntToByteArray)
        return DataConvertExtension.UIntToByteArray(uInt16Array, startAddress, swapValues);
    }

    private static byte[] UIntToByteArray(ushort[] uInt16Array, int startAddress, bool swapValues)
    {
      if (uInt16Array == null)
        return (byte[]) null;
      if (uInt16Array.Length == 0 || startAddress < 0 || startAddress > uInt16Array.Length - 1)
        return new byte[0];
      byte[] byteArray = new byte[uInt16Array.Length * 2 - startAddress * 2];
      int num = 0;
      for (int index = startAddress; index < uInt16Array.Length; ++index)
      {
        if (swapValues)
        {
          byteArray = uInt16Array[index].Swap().CopyIntoByteArray(byteArray, num * 2);
          ++num;
        }
        else
        {
          byteArray = uInt16Array[index].CopyIntoByteArray(byteArray, num * 2);
          ++num;
        }
      }
      return byteArray;
    }

    public static byte[] ToByteArray(this int[] int32Array)
    {
      return int32Array.ToByteArray(0, false);
    }

    public static byte[] ToByteArray(this int[] int32Array, bool swapValues)
    {
      return int32Array.ToByteArray(0, swapValues);
    }

    public static byte[] ToByteArray(this int[] int32Array, int startAddress)
    {
      return int32Array.ToByteArray(startAddress, false);
    }

    public static byte[] ToByteArray(this int[] int32Array, int startAddress, bool swapValues)
    {
      lock (DataConvertExtension.LockIntToByteArray)
        return DataConvertExtension.IntToByteArray(int32Array, startAddress, swapValues);
    }

    private static byte[] IntToByteArray(int[] int32Array, int startAddress, bool swapValues)
    {
      if (int32Array == null)
        return (byte[]) null;
      if (int32Array.Length == 0 || startAddress < 0 || startAddress > int32Array.Length - 1)
        return new byte[0];
      byte[] byteArray = new byte[int32Array.Length * 2 - startAddress * 2];
      int num = 0;
      for (int index = startAddress; index < int32Array.Length; ++index)
      {
        if (swapValues)
        {
          byteArray = int32Array[index].Swap16Bytes().CopyIntoByteArray(byteArray, num * 2);
          ++num;
        }
        else
        {
          byteArray = int32Array[index].CopyIntoByteArray(byteArray, num * 2);
          ++num;
        }
      }
      return byteArray;
    }

    public static ushort[] ToUIntArray(this byte[] byteArray)
    {
      return byteArray.ToUIntArray(false);
    }

    public static ushort[] ToUIntArray(this byte[] byteArray, bool swapBytes)
    {
      lock (DataConvertExtension.LockByteArrayToUInt)
        return DataConvertExtension.ByteToUIntArray(byteArray, 0, swapBytes);
    }

    public static ushort[] ToUIntArray(this byte[] byteArray, int startAddress)
    {
      lock (DataConvertExtension.LockByteArrayToUInt)
        return DataConvertExtension.ByteToUIntArray(byteArray, startAddress, false);
    }

    public static ushort[] ToUIntArray(this byte[] byteArray, int startAddress, bool swapBytes)
    {
      lock (DataConvertExtension.LockByteArrayToUInt)
        return DataConvertExtension.ByteToUIntArray(byteArray, startAddress, swapBytes);
    }

    private static ushort[] ByteToUIntArray(byte[] byteArray, int startAddress, bool swapBytes)
    {
      if (byteArray == null)
        return (ushort[]) null;
      if (byteArray.Length == 0 || startAddress < 0 || startAddress > byteArray.Length - 1)
        return new ushort[0];
      int num = byteArray.Length - startAddress;
      if (num % 2 != 0)
      {
        ++num;
        Array.Resize<byte>(ref byteArray, byteArray.Length + 1);
      }
      ushort[] numArray = new ushort[num / 2];
      for (int index = 0; index < numArray.Length; ++index)
      {
        int startIndex = index * 2 + startAddress;
        numArray[index] = !swapBytes ? BitConverter.ToUInt16(byteArray, startIndex) : BitConverter.ToUInt16(byteArray, startIndex).Swap();
      }
      return numArray;
    }

    public static int[] ToIntArray(this byte[] byteArray)
    {
      return byteArray.ToIntArray(false);
    }

    public static int[] ToIntArray(this byte[] byteArray, bool swapBytes)
    {
      lock (DataConvertExtension.LockByteArrayToInt)
        return DataConvertExtension.ByteToIntArray(byteArray, 0, swapBytes);
    }

    public static int[] ToIntArray(this byte[] byteArray, int startAddress)
    {
      lock (DataConvertExtension.LockByteArrayToInt)
        return DataConvertExtension.ByteToIntArray(byteArray, startAddress, false);
    }

    public static int[] ToIntArray(this byte[] byteArray, int startAddress, bool swapBytes)
    {
      lock (DataConvertExtension.LockByteArrayToInt)
        return DataConvertExtension.ByteToIntArray(byteArray, startAddress, swapBytes);
    }

    private static int[] ByteToIntArray(byte[] byteArray, int startAddress, bool swapBytes)
    {
      if (byteArray == null)
        return (int[]) null;
      if (byteArray.Length == 0 || startAddress < 0 || startAddress > byteArray.Length - 1)
        return new int[0];
      int num = byteArray.Length - startAddress;
      if (num % 2 != 0)
      {
        ++num;
        Array.Resize<byte>(ref byteArray, byteArray.Length + 1);
      }
      int[] numArray = new int[num / 2];
      for (int index = 0; index < numArray.Length; ++index)
      {
        int startIndex = index * 2 + startAddress;
        numArray[index] = !swapBytes ? (int) BitConverter.ToUInt16(byteArray, startIndex) : (int) BitConverter.ToUInt16(byteArray, startIndex).Swap();
      }
      return numArray;
    }

    public static ushort ToUInt16(this double doubleValue, bool signedFormat = false)
    {
      if (signedFormat)
        return BitConverter.ToUInt16(BitConverter.GetBytes(Convert.ToInt16(Math.Round(doubleValue))), 0);
      return Convert.ToUInt16(Math.Round(doubleValue));
    }

    public static ushort ToUInt16(this short int16Value)
    {
      return BitConverter.ToUInt16(BitConverter.GetBytes(int16Value), 0);
    }

    public static uint ToUInt32(this int int32Value)
    {
      return BitConverter.ToUInt32(BitConverter.GetBytes(int32Value), 0);
    }

    public static ulong ToUInt64(this long int64Value)
    {
      return BitConverter.ToUInt64(BitConverter.GetBytes(int64Value), 0);
    }

    public static short ToInt16(this ushort uint16Value)
    {
      return BitConverter.ToInt16(BitConverter.GetBytes(uint16Value), 0);
    }

    public static int ToInt32(this uint uint32Value)
    {
      return BitConverter.ToInt32(BitConverter.GetBytes(uint32Value), 0);
    }

    public static long ToInt64(this ulong uint64Value)
    {
      return BitConverter.ToInt64(BitConverter.GetBytes(uint64Value), 0);
    }

    public static bool Compare(this byte[] array1, byte[] array2)
    {
      lock (DataConvertExtension.LockArrayCompare)
      {
        if (array1 != null && array2 != null && (array1.Length == array2.Length && array2.Length != 0))
          return !((IEnumerable<byte>) array2).Where<byte>((Func<byte, int, bool>) ((t, i) => (int) array1[i] != (int) t)).Any<byte>();
        return false;
      }
    }

    public static bool Compare(this ushort[] array1, ushort[] array2)
    {
      lock (DataConvertExtension.LockArrayCompare)
      {
        if (array1 != null && array2 != null && (array1.Length == array2.Length && array2.Length != 0))
          return !((IEnumerable<ushort>) array2).Where<ushort>((Func<ushort, int, bool>) ((t, i) => (int) array1[i] != (int) t)).Any<ushort>();
        return false;
      }
    }

    public static byte[] CopyIntoByteArray(this int intValue, byte[] byteArray, int startAddress)
    {
      if (byteArray == null || startAddress < 0 || (startAddress > byteArray.Length - 1 || byteArray.Length == 0))
        throw new ArgumentException("The parameter byteArray is null, lengt or startAddress not possible.");
      lock (DataConvertExtension.LockCopyToByteArray)
      {
        byteArray[startAddress] = Convert.ToByte(intValue & (int) byte.MaxValue);
        byteArray[startAddress + 1] = Convert.ToByte(intValue >> 8 & (int) byte.MaxValue);
        return byteArray;
      }
    }

    public static byte[] CopyIntoByteArray(this ushort uIntValue, byte[] byteArray, int startAddress)
    {
      if (byteArray == null || startAddress < 0 || (startAddress > byteArray.Length - 1 || byteArray.Length == 0))
        throw new ArgumentException("The parameter byteArray is null, lengt or start address not possible.");
      lock (DataConvertExtension.LockCopyToByteArray)
      {
        byteArray[startAddress] = Convert.ToByte((int) uIntValue & (int) byte.MaxValue);
        byteArray[startAddress + 1] = Convert.ToByte((int) uIntValue >> 8);
        return byteArray;
      }
    }
  }
}
