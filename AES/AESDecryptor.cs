using System;
using System.Security.Cryptography;

namespace AES
{
    class AESDecryptor : ICryptoTransform
    {
        private byte[] roundKey;
        private byte[] iv;
        private int numberOfRounds;
        public bool CanReuseTransform => throw new NotImplementedException();

        public bool CanTransformMultipleBlocks => throw new NotImplementedException();

        public int InputBlockSize => throw new NotImplementedException();

        public int OutputBlockSize => throw new NotImplementedException();

        public AESDecryptor(byte[] key, byte[] iv)
        {
            AES.ThrowIfKeyInvalid(key);
            AES.ThrowIfIvInvalid(iv);

            this.roundKey = KeySchedule.GenerateSchedule(key);
            this.iv = new byte[iv.Length];
            Array.Copy(iv, this.iv, iv.Length);
            this.numberOfRounds = roundKey.Length - 16;
        }

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
