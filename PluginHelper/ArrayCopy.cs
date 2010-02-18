using System;

namespace mkNETtools.PluginHelper
{
	/// <summary>
	/// Summary description for ArrayCopy.
	/// </summary>
	public abstract class ArrayCopy
	{
    public static byte [] SByteToByte(sbyte [] data)
    {
      byte [] array = new byte[data.Length];
      
      for (int b = 0; b < data.Length; b++)
        array[b] = (byte)data[b];

      return array;
    }

    public static sbyte [] ByteToSByte(byte [] data)
    {
      sbyte [] array = new sbyte[data.Length];
      
      for (int b = 0; b < data.Length; b++)
        array[b] = (sbyte)data[b];

      return array;
    }
	}
}
