using System;
using System.Security.Cryptography;

namespace AES
{
    class AESDecryptor : ICryptoTransform
    {
        public bool CanReuseTransform => throw new NotImplementedException();

        public bool CanTransformMultipleBlocks => throw new NotImplementedException();

        public int InputBlockSize => throw new NotImplementedException();

        public int OutputBlockSize => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            throw new NotImplementedException();
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            throw new NotImplementedException();
        }
    }
}
