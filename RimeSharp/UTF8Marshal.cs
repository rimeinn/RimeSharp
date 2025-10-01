using System.Runtime.InteropServices;
using System.Text;

namespace RimeSharp
{
    internal static class UTF8Marshal
    {
        internal static string PtrToStringUTF8(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero) return string.Empty;
            var len = 0;
            while (Marshal.ReadByte(ptr, len) != 0)
                len++;
            if (len <= 0) return string.Empty;
            var bytes = new byte[len];
            Marshal.Copy(ptr, bytes, 0, len);
            return Encoding.UTF8.GetString(bytes);
        }

        internal static string PtrToStringUTF8(IntPtr ptr, int length)
        {
            if (ptr == IntPtr.Zero || length <= 0) return string.Empty;
            var bytes = new byte[length];
            Marshal.Copy(ptr, bytes, 0, length);
            return Encoding.UTF8.GetString(bytes);
        }

        internal static IntPtr StringToHGlobalUTF8(string? s)
        {
            if (s is null) return IntPtr.Zero;
            var bytes = Encoding.UTF8.GetBytes(s);
            Array.Resize(ref bytes, bytes.Length + 1);
            bytes[bytes.Length - 1] = 0; // tailing zero
            var ptr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            return ptr;
        }
    }
}
