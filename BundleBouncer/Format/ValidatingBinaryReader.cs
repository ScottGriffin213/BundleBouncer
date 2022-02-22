/**
 * BundleBouncer Validating Binary Reader
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BundleBouncer
{
    internal class ValidatingBinaryReader : IDisposable
    {
        private BinaryReader br;
        internal Stream BaseStream { get; private set; }

        internal ValidatingBinaryReader(Stream s)
        {
            BaseStream=s;
            this.br = new BinaryReader(s);
        }

        public void Dispose()
        {
            this.br.Dispose();
        }

        internal string GetCString(string fieldName, ulong minlen = 0UL, ulong maxlen = int.MaxValue, Encoding encoding = null)
        {
            ulong len = 0;
            var origPos = br.BaseStream.Position;
            while (true)
            {
                if (len > maxlen)
                    throw new FailedValidation(fieldName, $"cstring length greater than max: start={origPos}, max={maxlen}");
                len++;
                if (br.ReadByte() == 0)
                    break;
            }
            if (len < minlen)
                throw new FailedValidation(fieldName, $"cstring length less than min: {len} < {minlen}");
            br.BaseStream.Position = origPos;
            return (encoding ?? Encoding.UTF8).GetString(GetBytes(fieldName, (int)len).Take((int)len-1).ToArray());
        }

        internal string GetPascalString(string fieldName, byte minlen = 0, byte maxlen = byte.MaxValue, Encoding encoding = null)
        {
            var len = GetBytes(fieldName, 1)[0];
            if (len < minlen)
                throw new FailedValidation(fieldName, $"length {len} less than min {minlen}");
            if (len > maxlen)
                throw new FailedValidation(fieldName, $"length {len} greater than max {maxlen}");
            byte[] buf = GetBytes(fieldName, len);
            return (encoding ?? Encoding.UTF8).GetString(buf.ToArray());
        }

        internal ushort GetU16(string fieldName, ushort min = ushort.MinValue, ushort max = ushort.MaxValue)
        {
            var value = ToUInt16(GetBytes(fieldName, 2));
            if (value < min)
                throw new FailedValidation(fieldName, $"value less than min: {value} < {min}");
            if (value > max)
                throw new FailedValidation(fieldName, $"value greater than max: {value} > {max}");
            return value;
        }

        internal uint GetU32(string fieldName, uint min = uint.MinValue, uint max = uint.MaxValue)
        {
            var value = ToUInt32(GetBytes(fieldName, 4));
            if (value < min)
                throw new FailedValidation(fieldName, $"value less than min: {value} < {min}");
            if (value > max)
                throw new FailedValidation(fieldName, $"value greater than max: {value} > {max}");
            return value;
        }

        internal ulong GetU64(string fieldName, ulong min = ulong.MinValue, ulong max = ulong.MaxValue)
        {
            var value = ToUInt64(GetBytes(fieldName, 8));
            if (value < min)
                throw new FailedValidation(fieldName, $"value less than min: {value} < {min}");
            if (value > max)
                throw new FailedValidation(fieldName, $"value greater than max: {value} > {max}");
            return value;
        }

        private ushort ToUInt16(byte[] bytes)
        {
            if (bytes.Length != 2)
                throw new InvalidOperationException();
            return (ushort)(((ushort)bytes[1])
                          | ((ushort)bytes[0] << 8));
        }

        private uint ToUInt32(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException();
            return ((uint)bytes[3])
                 | ((uint)bytes[2] << 8)
                 | ((uint)bytes[1] << 16)
                 | ((uint)bytes[0] << 24);
        }

        private int ToInt32(byte[] bytes)
        {
            if (bytes.Length != 4)
                throw new InvalidOperationException();
            return ((int)bytes[3])
                 | ((int)bytes[2] << 8)
                 | ((int)bytes[1] << 16)
                 | ((int)bytes[0] << 24);
        }

        private ulong ToUInt64(byte[] b)
        {
            if (b.Length != 8)
                throw new InvalidOperationException();
            return ((ulong)b[7])
                 | ((ulong)b[6] << 8)
                 | ((ulong)b[5] << 16)
                 | ((ulong)b[4] << 24)
                 | ((ulong)b[3] << 32)
                 | ((ulong)b[2] << 40)
                 | ((ulong)b[1] << 48)
                 | ((ulong)b[0] << 56);
        }

        private long ToInt64(byte[] b)
        {
            if (b.Length != 8)
                throw new InvalidOperationException();
            return ((long)b[7])
                 | ((long)b[6] << 8)
                 | ((long)b[5] << 16)
                 | ((long)b[4] << 24)
                 | ((long)b[3] << 32)
                 | ((long)b[2] << 40)
                 | ((long)b[1] << 48)
                 | ((long)b[0] << 56);
        }

        internal void AlignTo(uint bytes)
        {
            br.BaseStream.Position += bytes - (br.BaseStream.Position % bytes);
        }

        internal int GetS32(string fieldName, int min = int.MinValue, int max = int.MaxValue)
        {
            var value = ToInt32(GetBytes(fieldName, 4));
            if (value < min)
                throw new FailedValidation(fieldName, $"value less than min: {value} < {min}");
            if (value > max)
                throw new FailedValidation(fieldName, $"value greater than max: {value} > {max}");
            return value;
        }

        internal long GetS64(string fieldName, long min = long.MinValue, long max = long.MaxValue)
        {
            var value = ToInt64(GetBytes(fieldName, 8));
            if (value < min)
                throw new FailedValidation(fieldName, $"value less than min: {value} < {min}");
            if (value > max)
                throw new FailedValidation(fieldName, $"value greater than max: {value} > {max}");
            return value;
        }

        internal byte[] GetBytes(string fieldName, int count)
        {
            try
            {
                return br.ReadBytes(count);
            }
            catch (Exception e)
            {
                Logging.Error(e);
                throw new FailedValidation(fieldName, $"Could not read {count} bytes from stream");
            }
        }
    }

    [Serializable]
    internal class FailedValidation : Exception
    {
        public FailedValidation(string fieldName, string why) : base($"{fieldName} - {why}")
        {
        }
    }
}