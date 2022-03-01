/**
 * BundleBouncer Unity Native Interface Utilities
 * 
 * Copyright (c) 2022 BundleBouncer Contributors
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

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

        // core::StringStorageDefault<wchar_t>::deallocate
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate char CoreStringStorageDefault_WCharT_Deallocate_Delegate(ref IntPtr ustr);
        internal static CoreStringStorageDefault_WCharT_Deallocate_Delegate origNATIVECoreStringStorageDefault_WCharT_Deallocate;

        public static UnityString String2CoreBasicString(string input)
        {
            return new UnityString(input);
        }
    }

    /// <summary>
    /// Unity has a string table for some reason.  We need to add and remove strings to it.
    /// 
    /// Use this class if you are making such a string from scratch.
    /// If you are using a pre-existing string that is cleaned up in Unity, use ExistingUnityString.
    /// </summary>
    public sealed class UnityString : IDisposable
    {
        private IntPtr ptr;
        private bool disposedValue;
        public UnityString(string value)
        {
            UnityCoreUtils.origNATIVECoreStringStorageDefault_Char_Assign(ref ptr, value, (ulong)value.Length);
        }

        public string Value
        {
            get
            {
                return UnityCoreUtils.CoreBasicString2String(this.ptr);
            }
            set
            {
                // TODO: Realloc?
                UnityCoreUtils.origNATIVECoreStringStorageDefault_WCharT_Deallocate(ref this.ptr);
                UnityCoreUtils.origNATIVECoreStringStorageDefault_Char_Assign(ref this.ptr, value, (ulong)value.Length);
            }
        }

        public IntPtr Pointer
        {
            get
            {
                return this.ptr;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                UnityCoreUtils.origNATIVECoreStringStorageDefault_WCharT_Deallocate(ref this.ptr);
                disposedValue = true;
            }
        }

        ~UnityString()
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

    }
    public sealed class ExistingUnityString// : IDisposable
    {
        private IntPtr ptr;
        //private bool disposedValue;

        public IntPtr Pointer
        {
            get
            { return ptr; }
        }

        internal ExistingUnityString(IntPtr ptr)
        {
            this.ptr = ptr;
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