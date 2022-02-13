using System;
using System.IO;
using System.Runtime.Serialization;

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

        internal void NextBytesMustEqual(byte[] requiredBytes, string failedDesc)
        {
            var b = br.ReadBytes(requiredBytes.Length);
            if(b.Length!=requiredBytes.Length)
                throw new FailedValidation(failedDesc);
            if(b!=requiredBytes)
                throw new FailedValidation(failedDesc);
        }

        internal uint GetU32(string failedDesc, uint min=uint.MinValue, uint max=uint.MaxValue)
        {
            var value = BitConverter.ToUInt32(br.ReadBytes(4), 0);
            if(value < min)
                throw new FailedValidation(failedDesc);
            if(value > max)
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