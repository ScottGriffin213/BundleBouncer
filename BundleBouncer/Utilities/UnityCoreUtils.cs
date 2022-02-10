using System;
using System.Runtime.InteropServices;

namespace BundleBouncer
{
    internal class UnityCoreUtils
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate IntPtr CoreBasicString_CStr_Delegate(IntPtr @this);
        internal static CoreBasicString_CStr_Delegate origNATIVECoreBasicString_CStr;

        /// <summary>
        /// Converts core::basic_string<char,core::StringStorageDefault<char>> to mono string
        /// </summary>
        /// <param name="a"></param>
        internal static unsafe string CoreBasicString2String(IntPtr @this)
        {
            if (@this == IntPtr.Zero)
                return null;
            //Logging.Info("CSTR");
            var charstr = origNATIVECoreBasicString_CStr(@this);
            if (charstr == IntPtr.Zero)
                return null;
            return Marshal.PtrToStringUTF8(charstr);
        }
    }
}