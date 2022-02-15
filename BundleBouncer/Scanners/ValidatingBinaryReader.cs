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

        internal ValidatingBinaryReader(Stream s)
        {
            this.br = new BinaryReader(s);
        }

        public void Dispose()
        {
            this.br.Dispose();
        }

        internal string GetCString(string failedDesc, ulong minlen=0UL, ulong maxlen=ulong.MaxValue, Encoding encoding=null)
        {
            List<byte> buf = new List<byte>();
            byte b;
            while(true)
            {
                if((ulong)buf.LongCount() > maxlen)
                    throw new FailedValidation(failedDesc);
                b = br.ReadByte();
                if(b == 0x00)
                    break;
                buf.Add(b);
            }
            if((ulong)buf.LongCount() < minlen)
                throw new FailedValidation(failedDesc);
            return (encoding ?? Encoding.UTF8).GetString(buf.ToArray());
        }

        internal string GetPascalString(string failedDesc, byte minlen=0, byte maxlen=byte.MaxValue, Encoding encoding=null)
        {
            byte[] buf = br.ReadBytes(br.ReadByte());
            if((byte)buf.Length < minlen)
                throw new FailedValidation(failedDesc);
            if((byte)buf.Length > maxlen)
                throw new FailedValidation(failedDesc);
            return (encoding ?? Encoding.UTF8).GetString(buf.ToArray());
        }

        internal void NextBytesMustEqual(byte[] requiredBytes, string failedDesc)
        {
            var b = br.ReadBytes(requiredBytes.Length);
            if (b.Length != requiredBytes.Length)
                throw new FailedValidation(failedDesc);
            if (b != requiredBytes)
                throw new FailedValidation(failedDesc);
        }

        internal uint GetU32(string failedDesc, uint min = uint.MinValue, uint max = uint.MaxValue)
        {
            var value = BitConverter.ToUInt32(br.ReadBytes(4), 0);
            if (value < min)
                throw new FailedValidation(failedDesc);
            if (value > max)
                throw new FailedValidation(failedDesc);
            return value;
        }
    }

    [Serializable]
    internal class FailedValidation : Exception
    {
        public FailedValidation()
        {
        }

        public FailedValidation(string message) : base(message)
        {
        }

        public FailedValidation(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FailedValidation(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}