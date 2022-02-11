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
        public static unsafe string CoreBasicString2String(IntPtr @this)
        {
            if (@this == IntPtr.Zero)
                return null;
            //Logging.Info("CSTR");
            var charstr = origNATIVECoreBasicString_CStr(@this);
            if (charstr == IntPtr.Zero)
                return null;
            return Marshal.PtrToStringUTF8(charstr);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate char CoreStringStorageDefault_Char_Assign_Delegate(ref IntPtr ustr, string input, ulong len);
        internal static CoreStringStorageDefault_Char_Assign_Delegate origNATIVECoreStringStorageDefault_Char_Assign;
        public static unsafe CoreStringChar String2CoreBasicString(string input)
        {
            return new CoreStringChar(input);
        }
    }

    public sealed class CoreStringChar// : IDisposable
    {
        private IntPtr ptr;
        //private bool disposedValue;

        public IntPtr Pointer
        {
            get
            { return ptr; }
        }

        internal CoreStringChar(IntPtr ptr)
        {
            this.ptr = ptr;
        }
        public CoreStringChar(string value)
        {
            UnityCoreUtils.origNATIVECoreStringStorageDefault_Char_Assign(ref ptr, value, (ulong)value.Length);
        }
        /*

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                UnityCoreUtils.origNATIVECoreStringStorageDefault_Char_Deallocate(this.ptr);
                disposedValue = true;
            }
        }

        ~CoreStringChar()
        {
             // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
             Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        */
    }
}